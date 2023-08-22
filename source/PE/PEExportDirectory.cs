// Copyright (c) 2023, Johan Nyvaller
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1.Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibPENUT
{
    /// <summary>
    /// Represents the one and only entry in the Export Directory table of the PE optional header
    /// </summary>
    public class PEExportDirectory
    {

        /// <summary>
        /// Creates a new empty PEExportDirectory object
        /// </summary>
        public PEExportDirectory()
        {
            ExportFlags = 0;
            m_timeDateStamp = DateTime.Now;
            MajorVersion = 0;
            MinorVersion = 0;
            NameRVA = 0;
            Name = string.Empty;
            OrdinalBase = 0;
            NumberOfAddressTableEntries = 0;
            NumberOfNamePointers = 0;
            ExportAddressTableRVA = 0;
            NamePointerRVA = 0;
            OrdinalTableRVA = 0;
            Image = null;

            Exports = new PEExportedSymbol[0];
            ExportNamePointerTable = new UInt32[0];
            ExportOrdinalTable = new UInt16[0];
            ExportAddressTable = new UInt32[0];
        }

        /// <summary>
        /// Creates a new PEExportDirectory object and populates it from the .edata section pointed to by the rva parameter
        /// </summary>
        public PEExportDirectory(PEImage image, UInt32 rva) : this()
        {
            Image = image;

            COFFSection edata = image.GetSectionFromRva(rva);
            if (edata != null)
            {
                ExportFlags = edata.GetUInt32FromRva(rva);
                UnixTimeDateStamp = Convert.ToInt64(edata.GetUInt32FromRva(rva + 4));
                MajorVersion = edata.GetUInt16FromRva(rva + 8);
                MinorVersion = edata.GetUInt16FromRva(rva + 10);
                NameRVA = edata.GetUInt32FromRva(rva + 12);
                OrdinalBase = edata.GetUInt32FromRva(rva + 16);
                NumberOfAddressTableEntries = edata.GetUInt32FromRva(rva + 20);
                NumberOfNamePointers = edata.GetUInt32FromRva(rva + 24);
                ExportAddressTableRVA = edata.GetUInt32FromRva(rva + 28);
                NamePointerRVA = edata.GetUInt32FromRva(rva + 32);
                OrdinalTableRVA = edata.GetUInt32FromRva(rva + 36);

                if (NameRVA != 0 && image.TryGetSectionFromRva(NameRVA, out COFFSection nameSection))
                {
                    Name = nameSection.GetStringFromRva(NameRVA);
                }
                else
                {
                    Name = string.Empty;
                }

                // Populate Export Address table
                // In theory everything should be in the .edata section we looked up previously but people do crazy things so for safety we explicitly lookup the section here as well
                if (ExportAddressTableRVA != 0 && image.TryGetSectionFromRva(ExportAddressTableRVA, out COFFSection eatSection))
                {
                    ExportAddressTable = new UInt32[NumberOfAddressTableEntries];

                    for (uint i = 0; i < NumberOfAddressTableEntries; i++)
                    {
                        if (eatSection.TryGetUInt32FromRva(ExportAddressTableRVA + (i * 4), out UInt32 eatRVA))
                        {
                            ExportAddressTable[i] = eatRVA;
                        }
                    }
                }

                // Populate Export Name Pointer table
                if (NamePointerRVA != 0 && image.TryGetSectionFromRva(NamePointerRVA, out COFFSection enpSection))
                {
                    ExportNamePointerTable = new UInt32[NumberOfNamePointers];

                    for (uint i = 0; i < NumberOfNamePointers; i++)
                    {
                        if (enpSection.TryGetUInt32FromRva(NamePointerRVA + (i * 4), out UInt32 enpRVA))
                        {
                            ExportNamePointerTable[i] = enpRVA;
                        }
                    }
                }

                // Populate Export Ordinal table and the Exports table with aggregated symbol info
                if (OrdinalTableRVA != 0 && image.TryGetSectionFromRva(OrdinalTableRVA, out COFFSection eoSection))
                {
                    ExportOrdinalTable = new UInt16[NumberOfNamePointers];
                    Exports = new PEExportedSymbol[NumberOfNamePointers];

                    for (UInt32 i = 0; i < NumberOfNamePointers; i++)
                    {
                        if (eoSection.TryGetUInt16FromRva(OrdinalTableRVA + (i * 2), out UInt16 ordinal))
                        {
                            ExportOrdinalTable[i] = ordinal;

                            PEExportedSymbol export = new PEExportedSymbol();
                            export.Ordinal = (Int16)(ordinal + OrdinalBase);
                            export.RVA = ExportAddressTable[ordinal];
                            if (edata.TryGetStringFromRva(ExportNamePointerTable[i], out string exportName))
                            {
                                export.Name = exportName;
                            }

                            // Symbol RVA is within the .edata section, this is a forward reference and the RVA points to reference string
                            if (export.RVA >= edata.Header.VirtualAddress && export.RVA < edata.Header.VirtualAddress + edata.Header.VirtualSize)
                            {
                                export.IsForwardReference = true;
                                if (edata.TryGetStringFromRva(export.RVA, out string refName))
                                {
                                    export.ReferenceName = refName;
                                }
                            }

                            Exports[i] = export;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Creates a new export directory in the form of an .edata section with export information based on a list of PEExportedSymbol objects.
        /// This overload automatically calculates the next free virtual address for the section in the specified image.
        /// </summary>
        /// <param name="image">The PEImage that the section should be created for. Note that this method does not add the section to the image automatically</param>
        /// <param name="imageName">The name of the image to generate exports for, typically the file name of the image</param>
        /// <param name="symbols">A list of symbols to add to the export directory</param>
        /// <param name="ordinalBase">Base ordinal for the exports - typically 1</param>
        /// <returns>A new COFFSection object that can be added to the image</returns>
        public static COFFSection CreateSection(PEImage image, string imageName, IEnumerable<PEExportedSymbol> symbols, int ordinalBase = 1)
        {
            COFFSection lastSection = image.Sections.Last();
            UInt32 rva = lastSection.Header.VirtualAddress + lastSection.Header.VirtualSize;

            rva = rva.AlignTo(image.PEOptionalHeader.SectionAlignment);

            return CreateSection(image, imageName, rva, symbols, ordinalBase);
        }

        /// <summary>
        /// Creates a new export directory in the form of an .edata section with export information based on a list of PEExportedSymbol objects
        /// </summary>
        /// <param name="image">The PEImage that the section should be created for. Note that this method does not add the section to the image automatically</param>
        /// <param name="imageName">The name of the image to generate exports for, typically the file name of the image</param>
        /// <param name="sectionRVA">The virtuall address to use for this section</param>
        /// <param name="symbols">A list of symbols to add to the export directory</param>
        /// <param name="ordinalBase">Base ordinal for the exports - typically 1</param>
        /// <returns>A new COFFSection object that can be added to the image</returns>
        public static COFFSection CreateSection(PEImage image, string imageName, UInt32 sectionRVA, IEnumerable<PEExportedSymbol> symbols, int ordinalBase = 1)
        {
            UInt32 nrOfSymbols = (UInt32)symbols.Count();
            
            COFFSection edata = new COFFSection(".edata");
            edata.Header.Characteristics = COFFSectionCharacteristics.IMAGE_SCN_CNT_INITIALIZED_DATA | COFFSectionCharacteristics.IMAGE_SCN_MEM_READ;
            edata.Header.VirtualAddress = sectionRVA;

            // Calculate size of the section data
            edata.Header.VirtualSize = (UInt32)(
                PEExportDirectory.Size +      // Header
                (nrOfSymbols * 8) +       // Address table + Namepointer table
                (nrOfSymbols * 2 ) +          // Ordinal table
                (Encoding.ASCII.GetByteCount(imageName) + 1) +     // Null terminated image name string    
                symbols.Sum(s => Encoding.ASCII.GetByteCount(s.Name) + 1) +  // Null terminated symbol names
                symbols.Where(s => s.IsForwardReference).Sum(s => Encoding.ASCII.GetByteCount(s.ReferenceName) + 1)     // Null terminated forward reference strings
                );

            // Raw data size padded to file alignment
            edata.Header.SizeOfRawData = edata.Header.VirtualSize.AlignTo(image.PEOptionalHeader.FileAlignment);

            edata.RawData = new byte[edata.Header.SizeOfRawData];

            using (PENUTBinaryWriter writer = new PENUTBinaryWriter(new MemoryStream(edata.RawData), Encoding.ASCII))
            {
                // Write header data
                writer.Write((UInt32)0);    // Flags
                writer.Write((UInt32)DateTime.UtcNow.ToUnixTime()); //Timestamp
                writer.Write((UInt32)0);    // Major and Minor version
                writer.Write((UInt32)(edata.Header.VirtualAddress + PEExportDirectory.Size + (nrOfSymbols * 8) + (nrOfSymbols * 2)));    // NameRVA located after the 3 tables
                writer.Write((UInt32)ordinalBase);    // OrdinalBase
                writer.Write((UInt32)nrOfSymbols);    // NumberOfAddressTableEntries
                writer.Write((UInt32)nrOfSymbols);    // NumberOfNamePointers
                writer.Write((UInt32)edata.Header.VirtualAddress + PEExportDirectory.Size);    // ExportAddressTableRVA, immediately following the header
                writer.Write((UInt32)edata.Header.VirtualAddress + PEExportDirectory.Size + (nrOfSymbols * 4));    // NamePointerRVA, immediately following the address table
                writer.Write((UInt32)edata.Header.VirtualAddress + PEExportDirectory.Size + (nrOfSymbols * 8));    // OrdinalTableRVA, immediately following the namepointer table

                // Setup tables but don't write yet
                UInt32[] addressTable = symbols.Select(s => s.RVA).ToArray();
                UInt32[] namePointerTable = new UInt32[nrOfSymbols];
                Int16[] ordinalTable = symbols.Select(s => (Int16)(s.Ordinal - ordinalBase)).ToArray();   // Ordinals written to the image should be unbiased by OrdinalBase

                writer.Seek((int)((nrOfSymbols * 8) + (nrOfSymbols * 2)), SeekOrigin.Current);  // Move past the tables and start writing the string data

                writer.Write(Encoding.ASCII.GetBytes(imageName + "\0"));

                int i = 0;
                foreach (PEExportedSymbol sym in symbols)
                {
                    namePointerTable[i] = (UInt32)(edata.Header.VirtualAddress + writer.BaseStream.Position);   // Set RVA in name pointer table for this symbol
                    writer.Write(Encoding.ASCII.GetBytes(sym.Name + "\0"));
                    if (sym.IsForwardReference)
                    {
                        // Forward reference symbol - it's RVA should point to the reference string
                        addressTable[i] = (UInt32)(edata.Header.VirtualAddress + writer.BaseStream.Position);
                        writer.Write(Encoding.ASCII.GetBytes(sym.ReferenceName + "\0"));
                    }
                    i++;
                }

                // Move back to start of tables and write them out
                writer.Seek((int)(PEExportDirectory.Size), SeekOrigin.Begin);

                foreach (UInt32 rva in addressTable)
                    writer.Write(rva);
                
                foreach (UInt32 rva in namePointerTable)
                    writer.Write(rva);

                foreach (Int16 ordinal in ordinalTable)
                    writer.Write(ordinal);
            }

            return edata;

        }
               

        /// <summary>
        /// A list of PEExported symbols objects representing the exports from this image. This is a read only aggregation of the information in the various export tables
        /// </summary>
        public PEExportedSymbol[] Exports
        {
            get;private set;
        }

        /// <summary>
        /// The list of relative virtuell addresses containing the names of the exported symbols
        /// </summary>
        public UInt32[] ExportNamePointerTable
        {
            get;set;
        }

        /// <summary>
        /// The list of ordinals for the exported symbols
        /// </summary>
        public UInt16[] ExportOrdinalTable
        {
            get;set;
        }

        /// <summary>
        /// The list of relative virtuell addresses for the exported symbols
        /// </summary>
        public UInt32[] ExportAddressTable
        {
            get;set;
        }

        /// <summary>
        /// Export flags - Currently reserved for future use
        /// </summary>
        public UInt32 ExportFlags
        {
            get; set;
        }

        private DateTime m_timeDateStamp;
        /// <summary>
        /// Time and date the export data was created
        /// </summary>
        public DateTime TimeDateStamp
        {
            get { return m_timeDateStamp; }
            set
            {
                m_timeDateStamp = value;
                m_unixTimeDateStamp = m_timeDateStamp.ToUniversalTime().ToUnixTime();
            }
        }

        private Int64 m_unixTimeDateStamp;
        /// <summary>
        /// Time and date the export data was created expressed as seconds since the UNIX epoch 1/1 00:00 1970 
        /// </summary>
        public Int64 UnixTimeDateStamp
        {
            get { return m_unixTimeDateStamp; }
            set
            {
                m_unixTimeDateStamp = value;
                m_timeDateStamp = DateTime.Now.FromUnixTime(value).ToLocalTime();
            }
        }

        /// <summary>
        /// Major version number
        /// </summary>
        public UInt16 MajorVersion
        {
            get; set;
        }

        /// <summary>
        /// Minor version number
        /// </summary>
        public UInt16 MinorVersion
        {
            get; set;
        }

        /// <summary>
        /// Relative virtual address of an ASCII string containing the name of the exporting image
        /// </summary>
        public UInt32 NameRVA
        {
            get; set;
        }

        /// <summary>
        /// The name of the exporting image
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// The starting ordinal number for exports in this image. This field specifies the starting ordinal number for the export address table. It is usually set to 1.
        /// </summary>
        public UInt32 OrdinalBase
        {
            get; set;
        }

        /// <summary>
        /// The number of entries in the export address table
        /// </summary>
        public UInt32 NumberOfAddressTableEntries
        {
            get; set;
        }

        /// <summary>
        /// The number of entries in the name pointer table. This is also the number of entries in the ordinal table
        /// </summary>
        public UInt32 NumberOfNamePointers
        {
            get; set;
        }

        /// <summary>
        /// The relative virtual address of the export address table
        /// </summary>
        public UInt32 ExportAddressTableRVA
        {
            get; set;
        }

        /// <summary>
        /// The relative virtual address of the export name pointer table. The table size is given by the NumberOfNamePointers field
        /// </summary>
        public UInt32 NamePointerRVA
        {
            get; set;
        }

        /// <summary>
        /// The relative virtual address of the ordinal table.
        /// </summary>
        public UInt32 OrdinalTableRVA
        {
            get; set;
        }

        /// <summary>
        /// A reference to the PEImage containing the export directory
        /// </summary>
        public PEImage Image
        {
            get; set;
        }

        /// <summary>
        /// The size in bytes of a PEExportDirectory entry
        /// </summary>
        public static UInt32 Size
        {
            get { return 40; }
        }
    }
}
