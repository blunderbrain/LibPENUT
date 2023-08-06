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

namespace LibPENUT
{
    /// <summary>
    /// Represents a section in a COFF image
    /// </summary>
    public class COFFSection
    {
        private byte[] m_rawData;
        private List<COFFRelocation> m_relocations;
        private List<COFFLineNumber> m_lineNumbers;

        #region Public Properties

        /// <summary>
        /// A COFFSectionHeader object containing information about the section
        /// </summary>
        public COFFSectionHeader Header
        {
            get;set;
        }

        /// <summary>
        /// The size in bytes of this section (including size of the header)
        /// </summary>
        public UInt32 Size
        {
            get
            {
                UInt32 size = COFFSectionHeader.Size;
                size += Convert.ToUInt32(
                    (m_relocations.Count * COFFRelocation.Size) +
                    (m_lineNumbers.Count * COFFLineNumber.Size)
                    );

                if ((Header.Characteristics & COFFSectionCharacteristics.IMAGE_SCN_CNT_UNINITIALIZED_DATA) == 0)
                    size += Header.SizeOfRawData;

                return size;
            }
        }

        /// <summary>
        /// Name of the section
        /// </summary>
        public string Name
        {
            get { return Header.Name; }
            set { Header.Name = value; }
        }

        /// <summary>
        /// Raw byte representation of the data contained in the section
        /// </summary>
        public byte[] RawData
        {
            get { return m_rawData; }
            set
            {
                m_rawData = value;
                Header.SizeOfRawData = Convert.ToUInt32(m_rawData.Length);
            }
        }

        /// <summary>
        /// A list of COFFRelocationEntry representing all relocations for the section
        /// </summary>
        public IEnumerable<COFFRelocation> Relocations
        {
            get { return m_relocations; }
        }

        /// <summary>
        /// A list of COFFLinenumberEntry representing all linenumbers for the section
        /// </summary>
        public IEnumerable<COFFLineNumber> Linenumbers
        {
            get { return m_lineNumbers; }
        }

        /// <summary>
        /// Reference to the COFF image that this section belongs to
        /// </summary>
        public COFFObject Image
        {
            get;set;
        }

        #endregion

        /// <summary>
        /// Creates a new COFFSection object with a default name of ".section"
        /// </summary>
        public COFFSection()
            : this(".section")
        { }

        /// <summary>
        /// Creates a new COFFSection object with the specified name
        /// </summary>
        /// <param name="name">The name of the new section</param>
        public COFFSection(string name)
        {
            Header = new COFFSectionHeader(name);
            m_rawData = new byte[0];
            m_relocations = new List<COFFRelocation>();
            m_lineNumbers = new List<COFFLineNumber>();
        }

        /// <summary>
        /// Read a COFF section from the specified stream. The stream should be positioned at the start of the section header before calling this method. If the method succeeds the stream will be positioned at the end of the section header
        /// </summary>
        /// <param name="inputStream">The stream to read data from. The stream must support random access, a.k.a seeking</param>
        /// <param name="imageStreamOffset">Offset in the stream to be considered as the starting point for the COFF image that this section belongs to. This parameter must be specified if the image is not located at the start of the stream</param>
        public void Read(Stream inputStream, long imageStreamOffset = 0)
        {
            if (!inputStream.CanSeek)
                throw new ArgumentException("The specified stream doesn't support seeking", "inputStream");

            m_relocations.Clear();
            m_lineNumbers.Clear();

            // Read Header data
            Header.Read(inputStream);
            long returnPosition = inputStream.Position;

            // Read raw data
            if (Header.SizeOfRawData != 0 && Header.PointerToRawData != 0)
            {
                m_rawData = new byte[Header.SizeOfRawData];
                inputStream.Seek(imageStreamOffset + Header.PointerToRawData, SeekOrigin.Begin);
                inputStream.Read(m_rawData, 0, Convert.ToInt32(Header.SizeOfRawData));
            }

            using (PENUTBinaryReader reader = new PENUTBinaryReader(inputStream, Encoding.ASCII, true))
            {
                // Read relocations
                if (Header.NumberOfRelocations != 0 && Header.PointerToRelocations != 0)
                {
                    inputStream.Seek(imageStreamOffset + Header.PointerToRelocations, SeekOrigin.Begin);

                    for (int i = 0; i < Header.NumberOfRelocations; i++)
                    {
                        COFFRelocation Relocation = new COFFRelocation();
                        Relocation.VirtualAddress = reader.ReadUInt32();
                        Relocation.SymbolTableIndex = reader.ReadUInt32();
                        Relocation.Type = reader.ReadUInt16();
                        m_relocations.Add(Relocation);
                    }
                }
                // Read linenumbers
                if (Header.NumberOfLineNumbers != 0 && Header.PointerToLineNumbers != 0)
                {
                    inputStream.Seek(imageStreamOffset + Header.PointerToLineNumbers, SeekOrigin.Begin);
                    for (int i = 0; i < Header.NumberOfLineNumbers; i++)
                    {
                        COFFLineNumber Linenumber = new COFFLineNumber();
                        Linenumber.Type = reader.ReadUInt32();
                        Linenumber.LineNumber = reader.ReadUInt16();
                        m_lineNumbers.Add(Linenumber);
                    }
                }
            }

            // Restore stream position to after header.
            inputStream.Seek(returnPosition, SeekOrigin.Begin);
        }


        /// <summary>
        /// Write this section to the specified stream.  The stream should be positioned at the start of the section header before calling this method. If the method succeeds the stream will be positioned at the end of the section header
        /// The pointer members in the header must contain valid pointers before this method is called
        /// </summary>
        /// <param name="outputStream">The Stream object to write to. The stream must support random access, a.k.a seeking</param>
        /// <param name="imageStreamOffset">Offset to add to all filepointers when writing the data. This parameter must be specified if the image is not located at the start of the stream</param>
        public void Write(Stream outputStream, long imageStreamOffset = 0)
        {
            if (!outputStream.CanSeek)
                throw new ArgumentException("The specified stream doesn't support seeking", "outputStream");

            Header.Write(outputStream);
            long returnPosition = outputStream.Position;

            // Write raw data (if any)
            if (Header.SizeOfRawData != 0 && Header.PointerToRawData != 0)
            {
                EnsureStreamLength(outputStream, imageStreamOffset + Header.PointerToRawData);  // Ensure target stream is big enough to seek to this location
                outputStream.Seek(imageStreamOffset + Header.PointerToRawData, SeekOrigin.Begin);
                outputStream.Write(m_rawData, 0, Convert.ToInt32(Header.SizeOfRawData));
            }

            using (PENUTBinaryWriter writer = new PENUTBinaryWriter(outputStream, Encoding.ASCII, true))
            {
                // Write relocations (if any)
                if (Header.NumberOfRelocations != 0 && Header.PointerToRelocations != 0)
                {
                    EnsureStreamLength(outputStream, imageStreamOffset + Header.PointerToRelocations);  // Ensure target stream is big enough to seek to this location
                    outputStream.Seek(imageStreamOffset + Header.PointerToRelocations, SeekOrigin.Begin);
                    for (UInt16 i = 0; i < Header.NumberOfRelocations; i++)
                    {
                        writer.Write(m_relocations[i].VirtualAddress);
                        writer.Write(m_relocations[i].SymbolTableIndex);
                        writer.Write((UInt16)m_relocations[i].Type);
                    }
                }

                // Write linenumbers (if any)
                if (Header.NumberOfLineNumbers != 0 && Header.PointerToLineNumbers != 0)
                {
                    EnsureStreamLength(outputStream, imageStreamOffset + Header.PointerToLineNumbers);  // Ensure target stream is big enough to seek to this location
                    outputStream.Seek(imageStreamOffset + Header.PointerToLineNumbers, SeekOrigin.Begin);
                    for (UInt16 i = 0; i < Header.NumberOfLineNumbers; i++)
                    {
                        writer.Write(m_lineNumbers[i].Type);
                        writer.Write(m_lineNumbers[i].LineNumber);
                    }
                }
            }

            // Restore stream position to after header.
            outputStream.Seek(returnPosition, SeekOrigin.Begin);
        }

        /// <summary>
        /// Pads the stream to ensure that it's at least the specified length
        /// </summary>
        /// <param name="stream">The stream to pad</param>
        /// <param name="length">Size to pad to</param>
        private void EnsureStreamLength(Stream stream, long length)
        {
            if (stream.Length < length)
            {
                stream.SetLength(length);
            }
        }

        /// <summary>
        /// Throw an ArgumentOutOfRangeException if the specified RVA does not belong to this section or if the section does not contain initialized data for this address
        /// </summary>
        private void ThrowIfRvaOutOfRange(UInt32 rva)
        {
            if (!(rva >= Header.VirtualAddress && rva < Header.VirtualAddress + Header.VirtualSize))
            {
                throw new ArgumentOutOfRangeException("rva", string.Format("The virtual address 0x{0:X} does not belong to section '{1}' (0x{2:X})", rva, Name, Header.VirtualAddress));
            }

            if (rva - Header.VirtualAddress >= RawData.Length)
            {
                throw new ArgumentOutOfRangeException("rva", string.Format("The virtual address 0x{0:X} does not have any initialized data in section '{1}' (0x{2:X})", rva, Name, Header.VirtualAddress));
            }
        }

        /// <summary>
        /// Decodes the data at the specified relative virtual address in the section as a null terminated ASCII string
        /// If the RVA is not part of this section or the RVA doesn't point to valid string data an exception is thrown.
        /// </summary>
        public string GetStringFromRva(UInt32 rva)
        {
            return GetStringFromRva(rva, Encoding.ASCII);
        }

        /// <summary>
        /// Decodes the data at the specified relative virtual address in the section as a string in the specified encoding.
        /// ASCII strings are assumed to be null terminated and Unicode strings are assumed to be double null terminated.
        /// If the RVA is not part of this section or the RVA doesn't point to valid string data an exception is thrown.
        /// </summary>
        public string GetStringFromRva(UInt32 rva, Encoding encoding)
        {
            ThrowIfRvaOutOfRange(rva);

            uint startIndex = rva - Header.VirtualAddress;
            uint endIndex = startIndex;
            bool eos;

            // Look for terminating single or double null bytes depending on encoding
            do
            {
                eos = encoding.IsSingleByte ?
                (endIndex < RawData.Length - 1 && RawData[endIndex] == 0) :
                (endIndex + 1 < RawData.Length - 1 && RawData[endIndex] == 0 && RawData[endIndex + 1] == 0);
                if (!eos)
                    endIndex++;
            }
            while (!eos);

            if (endIndex > startIndex)
                return encoding.GetString(RawData, (int)startIndex, (int)(endIndex - startIndex));
            else
                return string.Empty;
        }

        /// <summary>
        /// Attempts to decode the data at the specified relative virtual address in the section as a null terminated ASCII string
        /// If the RVA is not part of this section or the RVA doesn't point to valid string data the method returns false and the output is set to an empty string
        /// </summary>
        public bool TryGetStringFromRva(UInt32 rva, out string str)
        {
            return TryGetStringFromRva(rva, Encoding.ASCII, out str);
        }

        /// <summary>
        /// Attempts to decode the data at the specified relative virtual address in the section as a string in the specified encoding.
        /// ASCII strings are assumed to be null terminated and Unicode strings are assumed to be double null terminated.
        /// If the RVA is not part of this section or the RVA doesn't point to valid string data the method returns false and the output is set to an empty string
        /// </summary>
        public bool TryGetStringFromRva(UInt32 rva, Encoding encoding, out string str)
        {
            try
            {
                str = GetStringFromRva(rva, encoding);
                return true;
            }
            catch (Exception)
            {
                str = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Attempts to decode the data at the specified relative virtual address in the section as an unsigned 16-bit number
        /// If the RVA is not part of this section or the section does not contain initialized data an exception is thrown
        /// </summary>
        public UInt16 GetUInt16FromRva(UInt32 rva)
        {
            ThrowIfRvaOutOfRange(rva + 1);  // Ensure we can decode 2 bytes from the specified RVA

            uint index = rva - Header.VirtualAddress;

            UInt16 result = (Image.Header.Characteristics & COFFImageCharacteristics.IMAGE_FILE_BYTES_REVERSED_HI) == 0 || (Image.Header.Characteristics & COFFImageCharacteristics.IMAGE_FILE_BYTES_REVERSED_LO) != 0 ?
                // Little Endian
                (UInt16)(
                ((UInt16)(RawData[index + 0])) |
                ((UInt16)(RawData[index + 1]) << 8) )
                : // Big Endian (I have yet to see an example of someone using COFF/PE with big endian data but it's in the specification so might as well support it)
                (UInt16)(
                ((UInt16)(RawData[index + 1])) |
                ((UInt16)(RawData[index + 0]) << 8) )
                ;

            return result;
        }

        /// <summary>
        /// Attempts to decode the data at the specified relative virtual address in the section as an unsigned 16-bit number
        /// If the RVA is not part of this section or the section does not contain initialized data the method returns false and the output is set to 0xFFFF
        /// </summary>
        public bool TryGetUInt16FromRva(UInt32 rva, out UInt16 result)
        {
            try
            {
                result = GetUInt16FromRva(rva);
                return true;
            }
            catch (Exception)
            {
                result = 0xFFFF;
                return false;
            }
        }

        /// <summary>
        /// Attempts to decode the data at the specified relative virtual address in the section as an unsigned 32-bit number
        /// If the RVA is not part of this section or the section does not contain initialized data an exception is thrown
        /// </summary>
        public UInt32 GetUInt32FromRva(UInt32 rva)
        {
            ThrowIfRvaOutOfRange(rva + 3);  // Ensure we can decode 4 bytes from the specified RVA

            uint index = rva - Header.VirtualAddress;

            // Note: the check for both IMAGE_FILE_BYTES_REVERSED_HI and IMAGE_FILE_BYTES_REVERSED_LO here is a guard against malformed files where some toolchain has done crazy things and set too many bits in the Characteristics field which includes both flags.
            // This ensures that we at least treat this as little endian in that corner case which is more than likely what was intented

            UInt32 result = (Image.Header.Characteristics & COFFImageCharacteristics.IMAGE_FILE_BYTES_REVERSED_HI) == 0 || (Image.Header.Characteristics & COFFImageCharacteristics.IMAGE_FILE_BYTES_REVERSED_LO) != 0 ?
                // Little Endian
                ((UInt32)(RawData[index + 0])) |
                ((UInt32)(RawData[index + 1]) << 8) |
                ((UInt32)(RawData[index + 2]) << 16) |
                ((UInt32)(RawData[index + 3]) << 24)
                : // Big Endian (I have yet to see an example of someone using COFF/PE with big endian data but it's in the specification so might as well support it)
                ((UInt32)(RawData[index + 3])) |
                ((UInt32)(RawData[index + 2]) << 8) |
                ((UInt32)(RawData[index + 1]) << 16) |
                ((UInt32)(RawData[index + 0]) << 24)
                ;

            return result;
        }

        /// <summary>
        /// Attempts to decode the data at the specified relative virtual address in the section as an unsigned 32-bit number
        /// If the RVA is not part of this section or the section does not contain initialized data the method returns false and the output is set to 0xFFFFFFFF
        /// </summary>
        public bool TryGetUInt32FromRva(UInt32 rva, out UInt32 result)
        {
            try
            {
                result = GetUInt32FromRva(rva);
                return true;
            }
            catch(Exception)
            {
                result = 0xFFFFFFFF;
                return false;
            }
        }

        /// <summary>
        /// Attempts to decode the data at the specified relative virtual address in the section as an unsigned 64-bit number
        /// If the RVA is not part of this section or the section does not contain initialized data an exception is thrown
        /// </summary>
        public UInt64 GetUInt64FromRva(UInt32 rva)
        {
            ThrowIfRvaOutOfRange(rva + 7);  // Ensure we can decode 8 bytes from the specified RVA

            uint index = rva - Header.VirtualAddress;

            UInt64 result = (Image.Header.Characteristics & COFFImageCharacteristics.IMAGE_FILE_BYTES_REVERSED_HI) == 0 || (Image.Header.Characteristics & COFFImageCharacteristics.IMAGE_FILE_BYTES_REVERSED_LO) != 0 ?
                // Little Endian
                ((UInt64)(RawData[index + 0])) |
                ((UInt64)(RawData[index + 1]) << 8)  |
                ((UInt64)(RawData[index + 2]) << 16) |
                ((UInt64)(RawData[index + 3]) << 24) |
                ((UInt64)(RawData[index + 4]) << 32) |
                ((UInt64)(RawData[index + 5]) << 40) |
                ((UInt64)(RawData[index + 6]) << 48) |
                ((UInt64)(RawData[index + 7]) << 56)
                : // Big Endian (I have yet to see an example of someone using COFF/PE with big endian data but it's in the specification so might as well support it)
                ((UInt64)(RawData[index + 7])) |
                ((UInt64)(RawData[index + 6]) << 8)  |
                ((UInt64)(RawData[index + 5]) << 16) |
                ((UInt64)(RawData[index + 4]) << 24) |
                ((UInt64)(RawData[index + 3]) << 32) |
                ((UInt64)(RawData[index + 2]) << 40) |
                ((UInt64)(RawData[index + 1]) << 48) |
                ((UInt64)(RawData[index + 0]) << 56)
                ;

            return result;
        }

        /// <summary>
        /// Attempts to decode the data at the specified relative virtual address in the section as an unsigned 64-bit number
        /// If the RVA is not part of this section or the section does not contain initialized data the method returns false and the output is set to 0xFFFFFFFFFFFFFFFF
        /// </summary>
        public bool TryGetUInt64FromRva(UInt32 rva, out UInt64 result)
        {
            try
            {
                result = GetUInt64FromRva(rva);
                return true;
            }
            catch (Exception)
            {
                result = 0xFFFFFFFFFFFFFFFF;
                return false;
            }
        }


        /// <summary>
        /// Adds a new relocation entry to the section
        /// </summary>
        /// <param name="relocation">The COFFRelocationEntry object to add</param>
        public void AddRelocation(COFFRelocation relocation)
        {
            lock (m_relocations)
            {
                m_relocations.Add(relocation);
                Header.NumberOfRelocations = Convert.ToUInt16(m_relocations.Count);
            }
        }

        /// <summary>
        /// Removes a relocation entry from the section
        /// </summary>
        /// <param name="relocation">The COFFRelocationEntry object to remove</param>
        public void RemoveRelocation(COFFRelocation relocation)
        {
            lock (m_relocations)
            {
                m_relocations.Remove(relocation);
                Header.NumberOfRelocations = Convert.ToUInt16(m_relocations.Count);
            }
        }

        /// <summary>
        /// Adds a new linenumber entry to the section
        /// </summary>
        /// <param name="lineNumber">The COFFLinenumberEntry object to add</param>
        public void AddLinenumber(COFFLineNumber lineNumber)
        {
            lock (m_lineNumbers)
            {
                m_lineNumbers.Add(lineNumber);
                Header.NumberOfLineNumbers = Convert.ToUInt16(m_lineNumbers.Count);
            }
        }

        /// <summary>
        /// Removes a linenumber entry from the section
        /// </summary>
        /// <param name="lineNumber">The COFFLinenumberEntry object to remove</param>
        public void RemoveLinenumber(COFFLineNumber lineNumber)
        {
            lock (m_lineNumbers)
            {
                m_lineNumbers.Remove(lineNumber);
                Header.NumberOfLineNumbers = Convert.ToUInt16(m_lineNumbers.Count);
            }
        }

    }

}
