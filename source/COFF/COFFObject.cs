// Copyright (c) 2023, Johan Nyvaller
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
//
// 3. Neither the name of the copyright holder nor the names of its
//    contributors may be used to endorse or promote products derived from
//    this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace LibPENUT
{
    /// <summary>
    /// Represents a COFF object image
    /// </summary>
    public class COFFObject
    {
        #region Private Members

        private bool m_layoutSuspended;
        private COFFSection m_symtabSection;

        private COFFFileHeader m_header;
        private List<COFFSection> m_sections;
        private List<COFFSymbol> m_symbols;
        private SortedList<UInt32, string> m_stringTable;
        #endregion


        #region Public Properties

        /// <summary>
        /// A COFFFileHeader object containing header information for the image
        /// </summary>
        /// <see cref="COFFFileHeader"/>
        public COFFFileHeader Header
        {
            get { return m_header; }
            set { m_header = value; m_header.Image = this; }
        }

        /// <summary>
        /// An object derived from the abstract baseclass COFFOptionalHeader containing additional header data.
        /// </summary>
        /// <see cref="COFFOptionalHeader"/>
        public COFFOptionalHeader OptionalHeader
        {
            get; set;
        }

        /// <summary>
        /// A list of COFFSection objects representing all the sections in the image
        /// </summary>
        /// <see cref="COFFSection"/>
        public IEnumerable<COFFSection> Sections
        {
            get { return m_sections; }
        }

        /// <summary>
        /// A list of COFFSymbolTableEntry objects representing all symbols contained in the image
        /// </summary>
        /// <see cref="COFFSymbol"/>
        public IEnumerable<COFFSymbol> Symbols
        {
            get { return m_symbols; }
        }

        /// <summary>
        /// The image string table represented as an ordered dictionary of strings indexed by the relative byte offset of each string in the table.
        /// </summary>
        public IDictionary<UInt32,string> StringTable
        {
            get { return m_stringTable; }
        }

        /// <summary>
        /// The size of this object in bytes
        /// </summary>
        public UInt32 Size
        {
            get
            {
                UInt32 size = COFFFileHeader.Size;

                if (OptionalHeader != null)
                    size += OptionalHeader.Size;

                foreach (COFFSection Section in m_sections)
                    size += Section.Size;

                size += Header.NumberOfSymbols * COFFSymbol.Size;

                size += 4;	// SizeOfStringTable
                foreach(string s in StringTable.Values)
                    size += Convert.ToUInt32(Encoding.ASCII.GetByteCount(s) + 1);	// Length of string plus null byte

                return size;
            }
        }
        #endregion

        /// <summary>
        /// Creates a new COFFObject
        /// </summary>
        public COFFObject()
        {
            Header = new COFFFileHeader();
            OptionalHeader = new COFFBaseOptionalHeader();
            m_sections = new List<COFFSection>();
            m_symbols = new List<COFFSymbol>();
            m_stringTable = new SortedList<UInt32, string>();
            m_layoutSuspended = false;
            m_symtabSection = null;
        }

        /// <summary>
        /// Creates a new COFFObject object from the specified file
        /// </summary>
        /// <param name="fileName">The name of the file to load the object from</param>
        public COFFObject(string fileName)
            : this()
        {
            Read(fileName);
        }

        /// <summary>
        /// Disables the automatic recalculation of data offsets and other fields in the file and header sections when the image is modified. This may be desired before making a large number of modifications to the image.
        /// Note that calling the Write() method will always perform a layout even if this method has been called.
        /// </summary>
        public void SuspendLayout()
        {
            m_layoutSuspended = true;
        }

        /// <summary>
        /// Resumes automatic recalculation of data offsets and other fields in the file and header sections after SuspendLayout() has been called.
        /// </summary>
        public void ResumeLayout()
        {
            m_layoutSuspended = false;
            UpdateLayout();
        }

        /// <summary>
        /// Update all offsets and sizes in the fileheader and sectionheaders based on the current content of the image
        /// </summary>
        public virtual void UpdateLayout()
        {
            UpdateLayout(0, COFFFileHeader.Size + Header.SizeOfOptionalHeader + Header.NrOfSections * COFFSectionHeader.Size);
        }

        /// <summary>
        /// Update all offsets and size fields in the file header and section headers based on the current content of the image
        /// </summary>
        /// <param name="sectionFileAlignment">The alignment required for the sections when written to file. N/A for object files and typically 512 for exectuable images </param>
        /// <param name="sizeOfHeaders">The size of all headers up until the end of the last section header, adjusted for sectionFileAlignment. I.e. this is the start of the raw section data</param>
        public virtual void UpdateLayout(UInt32 sectionFileAlignment, UInt32 sizeOfHeaders)
        {
            // Sort sections by virtual address so they end up in correct order
            lock(m_sections)
            { 
                m_sections.Sort((COFFSection a, COFFSection b) => a.Header.VirtualAddress.CompareTo(b.Header.VirtualAddress));
            }

            Header.NrOfSections = Convert.ToUInt16(m_sections.Count);
            Header.NumberOfSymbols = Convert.ToUInt32(m_symbols.Count);
            foreach (COFFSymbol Symbol in m_symbols)
                Header.NumberOfSymbols += Symbol.NumberOfAuxSymbols;

            // Update optional header fields
            if (OptionalHeader != null)
            {
                Header.SizeOfOptionalHeader = Convert.ToUInt16(OptionalHeader.Size);

                // Find the first code section and set as BaseOfCode
                COFFSection firstCodeSection = Sections.FirstOrDefault(s => (s.Header.Characteristics & COFFSectionCharacteristics.IMAGE_SCN_CNT_CODE) != 0);
                if (firstCodeSection != null)
                {
                    OptionalHeader.BaseOfCode = firstCodeSection.Header.VirtualAddress;
                }
                else if (m_sections.Count > 0)
                {
                    // Fallback in case there are no code sections (resource only DLLs and similar). At least Microsofts tools set this to the VA of the first section regardless of section type in this case so lets just stay with that
                    OptionalHeader.BaseOfCode = m_sections[0].Header.VirtualAddress;
                }
            }

            // currentDataPointer is initialized to the start of the section data (directly after the table of section headers) and advanced below for each data pointer to be updated
            UInt32 currentDataPointer = sizeOfHeaders;

            // Adjust the various offsets in each section header
            foreach (COFFSection section in Sections)
            {
                // Update raw data pointer unless the IMAGE_SCN_CNT_UNINITIALIZED_DATA flag is set
                if (section.Header.SizeOfRawData > 0 && (section.Header.Characteristics & COFFSectionCharacteristics.IMAGE_SCN_CNT_UNINITIALIZED_DATA) == 0)
                {
                    // Adjust start of section data according to specified file alignment
                    currentDataPointer = currentDataPointer.AlignTo(sectionFileAlignment);

                    section.Header.PointerToRawData = currentDataPointer;
                    // Advance data pointer past the raw section data
                    currentDataPointer += section.Header.SizeOfRawData;
                }
                else
                {
                    section.Header.PointerToRawData = 0;
                }

                // Adjust relocation pointer
                if (section.Header.NumberOfRelocations > 0)
                {
                    section.Header.PointerToRelocations = currentDataPointer;
                    // Advance data pointer past the relocation entries
                    currentDataPointer += (section.Header.NumberOfRelocations * COFFRelocation.Size);
                }
                else
                {
                    section.Header.PointerToRelocations = 0;
                }

                // Adjust linenumber pointer
                if (section.Header.NumberOfLineNumbers > 0)
                {
                    section.Header.PointerToLineNumbers = currentDataPointer;
                    // Advance data pointer past the linenumber entries
                    currentDataPointer += (section.Header.NumberOfLineNumbers * COFFLineNumber.Size);
                }
                else
                {
                    section.Header.PointerToLineNumbers = 0;
                }
            } // For each section

            // Finally after updating all section headers update the symboltable pointer. currentDataPointer should be positioned after the last section at this point
            // Note that it's possible to have a string table even if the symbol table is empty and since the symbol table pointer is used to locate the string table we need to keep this updated
            if (m_symtabSection != null && Sections.Contains(m_symtabSection))
            {
                // Image is using the ELF convention with the symboltable in a .symtab section
                Header.PointerToSymbolTable = m_symtabSection.Header.PointerToRawData;
            }
            else if (Header.NumberOfSymbols > 0 || m_stringTable.Count > 0)
            {
                Header.PointerToSymbolTable = currentDataPointer;
            }
            else
            {
                Header.PointerToSymbolTable = 0;
            }
        }

        /// <summary>
        /// Returns the section that contains the specified relative virtual address, or null if no section matched the RVA
        /// </summary>
        public COFFSection GetSectionFromRva(UInt32 rva)
        {
            return Sections.FirstOrDefault(s => rva >= s.Header.VirtualAddress && rva < (s.Header.VirtualAddress + s.Header.VirtualSize));
        }

        /// <summary>
        /// Attempts to find the section that contains the specified relative virtual address. Returns true if the section was found, otherwise false and the output variable is set to null.
        /// </summary>
        public bool TryGetSectionFromRva(UInt32 rva, out COFFSection section)
        {
            section = GetSectionFromRva(rva);
            return section != null;
        }

        /// <summary>
        /// Convert a relative virtual address in the image into a byte offset from the start of the image
        /// </summary>
        /// <param name="rva">The virtual address to convert</param>
        /// <returns>An byte offset from the beginning of the image into the section matching the supplied RVA or -1 if no section matched the RVA</returns>
        public long GetImageOffsetFromRva(uint rva)
        {
            long offset = -1;

            // Find the section that the RVA belongs to
            COFFSection section = GetSectionFromRva(rva);

            // If we found a matching section, compute the delta between the sections RVA and raw data offset and add that to our RVA to find the offset
            if (section != null)
            {
                long delta = Convert.ToInt64(section.Header.PointerToRawData) - Convert.ToInt64(section.Header.VirtualAddress);
                offset = rva + delta;
            }

            return offset;
        }

        /// <summary>
        /// Reads a COFF object image from the specified file
        /// </summary>
        /// <param name="fileName">The name of the file to load the object from</param>
        public virtual void Read(string fileName)
        {
            using (FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Read(inputStream);
            }
        }

        /// <summary>
        /// Read a COFF object image from a stream
        /// </summary>
        /// <param name="inputStream">The stream to read data from. The specified stream must support random access, a.k.a seeking</param>
        /// <param name="imageStreamOffset">Offset to the position in the stream to be considered as the starting point of this image. This value is added to all data pointers in the image when seeking in the stream</param>
        public virtual void Read(Stream inputStream, long imageStreamOffset = 0)
        {
            if (!inputStream.CanSeek)
                throw new ArgumentException("The specified stream doesn't support seeking", "inputStream");

            // Ensure we clear everything in case of multiple calls to read on the same image
            m_symbols.Clear();
            m_stringTable.Clear();
            m_sections.Clear();

            // Read header
            Header.Read(inputStream);

            // Read optional header (if any)
            if (Header.SizeOfOptionalHeader != 0)
            {
                OptionalHeader.Read(inputStream, Header.SizeOfOptionalHeader);
                OptionalHeader.Image = this;
            }
            else
            {
                OptionalHeader = null;
            }

            // Read all sections
            for (int i = 0; i < Header.NrOfSections; i++)
            {
                COFFSection section = new COFFSection();
                section.Read(inputStream, imageStreamOffset);
                section.Image = this;
                m_sections.Add(section);
            }

            // Seek to start of symboltable (if present)
            if (Header.PointerToSymbolTable != 0)
            {
                // The GO compiler (and maybe other tools?) uses the ELF convention of putting the symbol table (and string table) in a .symtab section
                // instead of the COFF standard of having them as a datastructure and we need to remember that when updating our layout.
                // TODO: Look at if we can support symbol table editing in this scenario or if that should be blocked
                m_symtabSection = Sections.FirstOrDefault(s => s.Header.PointerToRawData == Header.PointerToSymbolTable);

                inputStream.Seek(imageStreamOffset + Header.PointerToSymbolTable, SeekOrigin.Begin);

                // Read symbol table
                for (int i = 0; i < Header.NumberOfSymbols; i++)
                {
                    COFFSymbol symbol = new COFFSymbol();
                    symbol.Read(inputStream);
                    symbol.Image = this;
                    i += symbol.NumberOfAuxSymbols;
                    m_symbols.Add(symbol);
                }
            }

            long pointerToStringTable = Header.PointerToSymbolTable + Header.NumberOfSymbols * COFFSymbol.Size;

            if (pointerToStringTable > 0)
            {
                inputStream.Seek(imageStreamOffset + pointerToStringTable, SeekOrigin.Begin);

                using (PENUTBinaryReader reader = new PENUTBinaryReader(inputStream, Encoding.ASCII, true))
                {
                    UInt32 sizeOfStringTable = reader.ReadUInt32();

                    // Subtract the The 4 bytes specifying the size since those are supposed to be included in the size field but we also need to guard agains a zero size since at least one case has been observed (a .res file) where the spec was violated and the size was set to zero.
                    if (sizeOfStringTable >= 4)
                        sizeOfStringTable -= 4;

                    // Get the raw null separated string data and parse into our list of strings
                    byte[] rawStringData = reader.ReadBytes(Convert.ToInt32(sizeOfStringTable));

                    int currentStringOffset = 0;
                    for (int i = 0; i < sizeOfStringTable; i++)
                    {
                        if (rawStringData[i] == 0)
                        {
                            // Note that the offset we record as the key into the sorted list must be from the actual beginning of the stringtable so we need add back the 4 bytes for the size field here
                            m_stringTable.Add((UInt32)(currentStringOffset + 4), Encoding.ASCII.GetString(rawStringData, currentStringOffset, i - currentStringOffset));
                            currentStringOffset = i + 1;
                        }
                    }
                }

                // If symbol and string table is contained in a .symtab section we need to ensure we explicitly position at the end of the section before exiting (string table may not extend all the way to the end)
                if (m_symtabSection != null)
                {
                    inputStream.Seek(imageStreamOffset + m_symtabSection.Header.PointerToRawData + m_symtabSection.Header.SizeOfRawData, SeekOrigin.Begin);
                }
            }
            else
            {
                // Ensure we always position at the end of the last sectiondata before returning so derived classes can keep reading from there
                COFFSection lastSection = Sections.Last();
                inputStream.Seek(imageStreamOffset + lastSection.Header.PointerToRawData + lastSection.Header.SizeOfRawData, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Writes the image to a file
        /// </summary>
        /// <param name="fileName">Name of the file to save the image to</param>
        public virtual void Write(string fileName)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                Write(outputStream);
            }
        }

        /// <summary>
        /// Write this image to the specified Stream
        /// </summary>
        /// <param name="outputStream">The Stream to write to. The stream must support seeking</param>
        /// <param name="imageStreamOffset">Offset to the position in the stream to be considered as the starting point of this image. This value is added to all filepointers in the image when seeking in the stream</param>
        public virtual void Write(Stream outputStream, long imageStreamOffset = 0)
        {
            if (!outputStream.CanSeek)
                throw new ArgumentException("The specified stream doesn't support seeking", "outputStream");

            // Update all the offsets in the fileheader and sectionheaders
            UpdateLayout();

            Header.Write(outputStream);		// Write headers
            if (OptionalHeader != null)
                OptionalHeader.Write(outputStream);

            // Write all sections to stream
            foreach (COFFSection Section in m_sections)
                Section.Write(outputStream, imageStreamOffset);

            // Write symboltable if there is one and if it's not contained in a .symtab section (ELF style) in which case we have already written it
            if (Header.PointerToSymbolTable != 0 && (m_symtabSection == null || !Sections.Contains(m_symtabSection)) )
            {
                outputStream.Seek(Header.PointerToSymbolTable + imageStreamOffset, SeekOrigin.Begin);
                foreach (COFFSymbol Symbol in m_symbols)
                    Symbol.Write(outputStream);

                //Write string table
                // Count number of bytes 
                UInt32 sizeOfStringTable = 4;		// The four bytes describing the size should be included in the size
                foreach (string s in m_stringTable.Values)
                    sizeOfStringTable += Convert.ToUInt32(Encoding.ASCII.GetByteCount(s) + 1);  // Length of current string + terminating null

                using (PENUTBinaryWriter writer = new PENUTBinaryWriter(outputStream, Encoding.ASCII, true))
                {
                    // Write size
                    writer.Write(sizeOfStringTable);

                    // Write the strings
                    foreach (string s in m_stringTable.Values)
                    {
                        writer.Write(Encoding.ASCII.GetBytes(s));
                        writer.Write((byte)0);
                    }
                }
            }
            else
            {
                // Ensure we position at the end of the image for consistency even if there is no symbol/string table
                COFFSection lastSection = Sections.Last();
                outputStream.Seek(imageStreamOffset + lastSection.Header.PointerToRawData + lastSection.Header.SizeOfRawData, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Adds a new section to the object
        /// </summary>
        /// <param name="section">The COFFSection object to add</param>
        public void AddSection(COFFSection section)
        {
            lock (m_sections)
            {
                section.Image = this;
                m_sections.Add(section);
            }

            if (! m_layoutSuspended)
                UpdateLayout();
        }

        /// <summary>
        /// Removes the specified section from the object
        /// </summary>
        /// <param name="section">The COFFSection object to remove</param>
        public void RemoveSection(COFFSection section)
        {
            lock (m_sections)
            {
                m_sections.Remove(section);
            }
            if (!m_layoutSuspended)
                UpdateLayout();
        }

        /// <summary>
        /// Adds a new symbol to the symboltable
        /// </summary>
        /// <param name="symbol">The COFFSymbolTableEntry object to add</param>
        public void AddSymbol(COFFSymbol symbol)
        {
            symbol.Image = this;

            lock (m_symbols)
            {
                m_symbols.Add(symbol);
            }
            
            if (!m_layoutSuspended)
                UpdateLayout();
        }

        /// <summary>
        /// Removes the specified symbol from the symboltable
        /// </summary>
        /// <param name="symbol">The COFFSymbolTableEntry object to remove</param>
        public void RemoveSymbol(COFFSymbol symbol)
        {
            lock (m_symbols)
            {
                m_symbols.Remove(symbol);
            }

            if (!m_layoutSuspended)
                UpdateLayout();
        }

        /// <summary>
        /// Adds a new string to the stringtable
        /// </summary>
        /// <param name="stringData">The string to add</param>
        /// <returns>The relative byte offset within the stringtable for the new string</returns>
        public uint AddString(string stringData)
        {
            UInt32 nextOffset;
            lock(m_stringTable)
            {
                // Calculate the byte offset for the new string based on the current last entry in the string table
                nextOffset = m_stringTable.Count == 0 ? 0 :
                    (UInt32)(m_stringTable.Keys[m_stringTable.Count - 1] + Encoding.ASCII.GetByteCount(m_stringTable.Values[m_stringTable.Count - 1]) + 1);

                m_stringTable.Add(nextOffset, stringData);
            }
            return nextOffset;
        }

        /// <summary>
        /// Removes the first instance of the specified string from the stringtable
        /// </summary>
        /// <param name="stringData">The string to remove</param>
        /// <returns>true if the string was found and removed, false otherwise</returns>
        public bool RemoveString(string stringData)
        {
            lock (m_stringTable)
            {
                int index = m_stringTable.IndexOfValue(stringData);
                if (index >= 0)
                    m_stringTable.RemoveAt(index);
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Removes the string at the specified byte offset from the string table
        /// </summary>
        /// <param name="stringOffset">Offset in bytes of the string to remove</param>
        /// <returns>true if the string was found and removed, false otherwise</returns>
        public bool RemoveString(UInt32 stringOffset)
        {
            lock (m_stringTable)
            {
                if (m_stringTable.ContainsKey(stringOffset))
                    m_stringTable.Remove(stringOffset);
                else
                    return false;
            }

            return true;
        }

    }

}
