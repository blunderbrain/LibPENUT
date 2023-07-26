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
using System.Collections.Generic;
using System.IO;

namespace LibPENUT
{
    /// <summary>
    /// Represents a PE optional header
    /// </summary>
    public class PEOptionalHeader : COFFBaseOptionalHeader
    {
        // Size in bytes of the base header without data directories
        private const int PEOptionalHeaderBaseSize = 96;
        
        #region Private members
        private UInt32 m_baseOfData;
        private UInt64 m_imageBase;
        private UInt32 m_sectionAlignment;
        private UInt32 m_fileAlignment;
        private UInt16 m_majorOperatingSystemVersion;
        private UInt16 m_minorOperatingSystemVersion;
        private UInt16 m_majorImageVersion;
        private UInt16 m_minorImageVersion;
        private UInt16 m_majorSubsystemVersion;
        private UInt16 m_minorSubsystemVersion;
        private UInt32 m_reserved;
        private UInt32 m_sizeOfImage;
        private UInt32 m_sizeOfHeaders;
        private UInt32 m_checkSum;
        private PESubSystem m_subSystem;
        private PEDLLCharacteristics m_dllCharacteristic;
        private UInt64 m_sizeOfStackReserve;
        private UInt64 m_sizeOfStackCommit;
        private UInt64 m_sizeOfHeapReserve;
        private UInt64 m_sizeOfHeapCommit;
        private UInt32 m_loaderFlags;
        private UInt32 m_numberOfRvaAndSizes;
        private List<PEDataDirectory> m_dataDirectories;
        #endregion

        /// <summary>
        /// Creates a new PEOptionalHeader object
        /// </summary>
        public PEOptionalHeader() : base()
        {
            m_dataDirectories = new List<PEDataDirectory>();

            // Setting RawData will initialize all other properties to zero
            RawData = new byte[PEOptionalHeaderBaseSize];
        }

        #region Public Properties

        /// <summary>
        /// Address, relative to image base, of beginning of data section, when loaded into memory.
        /// </summary>
        public UInt32 BaseOfData
        {
            get { return m_baseOfData; }
            set { m_baseOfData = value; UpdateRawData(); }
        }

        /// <summary>
        /// Preferred address of first byte of image when loaded into memory; must be a multiple of 64K. The default for DLLs is 0x10000000. The default for Windows CE EXEs is 0x00010000. The default for Windows NT, Windows 95, and Windows 98 is 0x00400000.
        /// For PE32 files maximum value is 32 bit, for PE32+ 64 bits
        /// </summary>
        public UInt64 ImageBase
        {
            get { return m_imageBase; }
            set { m_imageBase = value; UpdateRawData(); }
        }

        /// <summary>
        /// Alignment (in bytes) of sections when loaded into memory. Must greater or equal to File Alignment. Default is the page size for the architecture.
        /// </summary>
        public UInt32 SectionAlignment
        {
            get { return m_sectionAlignment; }
            set { m_sectionAlignment = value; UpdateRawData(); }
        }

        /// <summary>
        /// Alignment factor (in bytes) used to align the raw data of sections in the image file. The value should be a power of 2 between 512 and 64K inclusive. The default is 512. If the SectionAlignment is less than the architecture’s page size than this must match the SectionAlignment.
        /// </summary>
        public UInt32 FileAlignment
        {
            get { return m_fileAlignment; }
            set { m_fileAlignment = value; UpdateRawData(); }
        }

        /// <summary>
        /// Major version number of required OS.
        /// </summary>
        public UInt16 MajorOperatingSystemVersion
        {
            get { return m_majorOperatingSystemVersion; }
            set { m_majorOperatingSystemVersion = value; UpdateRawData(); }
        }

        /// <summary>
        /// Minor version number of required OS.
        /// </summary>
        public UInt16 MinorOperatingSystemVersion
        {
            get { return m_minorOperatingSystemVersion; }
            set { m_minorOperatingSystemVersion = value; UpdateRawData(); }
        }

        /// <summary>
        /// Major version number of image.
        /// </summary>
        public UInt16 MajorImageVersion
        {
            get { return m_majorImageVersion; }
            set { m_majorImageVersion = value; UpdateRawData(); }
        }

        /// <summary>
        /// Minor version number of image.
        /// </summary>
        public UInt16 MinorImageVersion
        {
            get { return m_minorImageVersion; }
            set { m_minorImageVersion = value; UpdateRawData(); }
        }

        /// <summary>
        /// Major version number of subsystem.
        /// </summary>
        public UInt16 MajorSubsystemVersion
        {
            get { return m_majorSubsystemVersion; }
            set { m_majorSubsystemVersion = value; UpdateRawData(); }
        }

        /// <summary>
        /// Minor version number of subsystem.
        /// </summary>
        public UInt16 MinorSubsystemVersion
        {
            get { return m_minorSubsystemVersion; }
            set { m_minorSubsystemVersion = value; UpdateRawData(); }
        }

        /// <summary>
        /// Reserved
        /// </summary>
        public UInt32 Reserved
        {
            get { return m_reserved; }
            set { m_reserved = value; UpdateRawData(); }
        }

        /// <summary>
        /// Size, in bytes, of image, including all headers; must be a multiple of Section Alignment.
        /// </summary>
        public UInt32 SizeOfImage
        {
            get { return m_sizeOfImage; }
            set { m_sizeOfImage = value; UpdateRawData(); }
        }

        /// <summary>
        /// Combined size of MS-DOS stub, PE Header, and section headers rounded up to a multiple of FileAlignment.
        /// </summary>
        public UInt32 SizeOfHeaders
        {
            get { return m_sizeOfHeaders; }
            set { m_sizeOfHeaders = value; UpdateRawData(); }
        }

        /// <summary>
        /// Image file checksum. The algorithm for computing is incorporated into IMAGHELP.DLL. The following are checked for validation at load time: all drivers, any DLL loaded at boot time, and any DLL that ends up in the server.
        /// </summary>
        public UInt32 CheckSum
        {
            get { return m_checkSum; }
            set { m_checkSum = value; UpdateRawData(); }
        }

        /// <summary>
        /// Subsystem required to run this image.
        /// </summary>
        public PESubSystem SubSystem
        {
            get { return m_subSystem; }
            set { m_subSystem = value; UpdateRawData(); }
        }

        /// <summary>
        /// Flags describing characteristic for DLL images
        /// </summary>
        public PEDLLCharacteristics DLLCharacteristic
        {
            get { return m_dllCharacteristic; }
            set { m_dllCharacteristic = value; UpdateRawData(); }
        }

        /// <summary>
        /// Size of stack to reserve. Only the Stack Commit Size is committed; the rest is made available one page at a time, until reserve size is reached.
        /// For PE32 files maximum value is 32 bit, for PE32+ 64 bits
        /// </summary>
        public UInt64 SizeOfStackReserve
        {
            get { return m_sizeOfStackReserve; }
            set { m_sizeOfStackReserve = value; UpdateRawData(); }
        }

        /// <summary>
        /// Size of stack to commit.
        /// For PE32 files maximum value is 32 bit, for PE32+ 64 bits
        /// </summary>
        public UInt64 SizeOfStackCommit
        {
            get { return m_sizeOfStackCommit; }
            set { m_sizeOfStackCommit = value; UpdateRawData(); }
        }

        /// <summary>
        /// Size of local heap space to reserve. Only the Heap Commit Size is committed; the rest is made available one page at a time, until reserve size is reached.
        /// For PE32 files maximum value is 32 bit, for PE32+ 64 bits
        /// </summary>
        public UInt64 SizeOfHeapReserve
        {
            get { return m_sizeOfHeapReserve; }
            set { m_sizeOfHeapReserve = value; UpdateRawData(); }
        }

        /// <summary>
        /// Size of local heap space to commit.
        /// For PE32 files maximum value is 32 bit, for PE32+ 64 bits
        /// </summary>
        public UInt64 SizeOfHeapCommit
        {
            get { return m_sizeOfHeapCommit; }
            set { m_sizeOfHeapCommit = value; UpdateRawData(); }
        }

        /// <summary>
        /// Obsolete.
        /// </summary>
        public UInt32 LoaderFlags
        {
            get { return m_loaderFlags; }
            set { m_loaderFlags = value; UpdateRawData(); }
        }

        /// <summary>
        /// Number of data-dictionary entries in the remainder of the Optional Header. Each describes a location and size.
        /// </summary>
        public UInt32 NumberOfRvaAndSizes
        {
            get { return m_numberOfRvaAndSizes; }
            set { m_numberOfRvaAndSizes = value; UpdateRawData(); }
        }

        /// <summary>
        /// An array of PEDataDirectory objects containing information about tables loaded from the image into memory
        /// </summary>
        public IList<PEDataDirectory> DataDirectories
        {
            get { return m_dataDirectories.AsReadOnly(); }
        }

        #endregion

        /// <summary>
        /// Read header information from the specified Stream
        /// </summary>
        /// <param name="inputStream">The Stream object to read from</param>
        /// <param name="headerSize">Nr of bytes to read (the size of the header)</param>
        public override void Read(Stream inputStream, UInt16 headerSize)
        {
            base.Read(inputStream, headerSize);
            UpdateProperties();
        }

        /// <summary>
        /// Add a new entry to the Data Directory table
        /// </summary>
        /// <param name="directory">The PeDataDirectory object to add</param>
        public void AddDataDirectory(PEDataDirectory directory)
        {
            lock (m_dataDirectories)
            {
                m_dataDirectories.Add(directory);
                NumberOfRvaAndSizes = Convert.ToUInt16(m_dataDirectories.Count);
            }
        }

        /// <summary>
        /// Remove an entry from the Data Directory table
        /// </summary>
        /// <param name="directory">The PeDataDirectory object to remove</param>
        public void RemoveDataDirectory(PEDataDirectory directory)
        {
            lock (m_dataDirectories)
            {
                m_dataDirectories.Remove(directory);
                NumberOfRvaAndSizes = Convert.ToUInt16(m_dataDirectories.Count);
            }
        }

        /// <summary>
        /// Updates the RawData property with the contents of the properties
        /// </summary>
        protected override void UpdateRawData()
        {
            // Update base COFF fields
            base.UpdateRawData();

            if (RawData.Length < PEOptionalHeaderBaseSize)
                return;

            switch (MagicNumber)
            {
                case COFFMagicNumbers.PE32Plus:
                    RawDataWriter.Write(ImageBase);
                    break;
                default:
                    RawDataWriter.Write(BaseOfData);
                    RawDataWriter.Write(Convert.ToUInt32(ImageBase));
                    break;
            }

            RawDataWriter.Write(SectionAlignment);
            RawDataWriter.Write(FileAlignment);
            RawDataWriter.Write(MajorOperatingSystemVersion);
            RawDataWriter.Write(MinorOperatingSystemVersion);
            RawDataWriter.Write(MajorImageVersion);
            RawDataWriter.Write(MinorImageVersion);
            RawDataWriter.Write(MajorSubsystemVersion);
            RawDataWriter.Write(MinorSubsystemVersion);
            RawDataWriter.Write(Reserved);
            RawDataWriter.Write(SizeOfImage);
            RawDataWriter.Write(SizeOfHeaders);
            RawDataWriter.Write(CheckSum);
            RawDataWriter.Write((UInt16)SubSystem);
            RawDataWriter.Write((UInt16)DLLCharacteristic);

            switch (MagicNumber)
            {
                case COFFMagicNumbers.PE32Plus:
                    RawDataWriter.Write(SizeOfStackReserve);
                    RawDataWriter.Write(SizeOfStackCommit);
                    RawDataWriter.Write(SizeOfHeapReserve);
                    RawDataWriter.Write(SizeOfHeapCommit);
                    break;
                default:
                    RawDataWriter.Write(Convert.ToUInt32(SizeOfStackReserve));
                    RawDataWriter.Write(Convert.ToUInt32(SizeOfStackCommit));
                    RawDataWriter.Write(Convert.ToUInt32(SizeOfHeapReserve));
                    RawDataWriter.Write(Convert.ToUInt32(SizeOfHeapCommit));
                    break;
            }

            RawDataWriter.Write(LoaderFlags);
            RawDataWriter.Write(NumberOfRvaAndSizes);

            for (int i = 0; i < NumberOfRvaAndSizes; i++)
            {
                RawDataWriter.Write(m_dataDirectories[i].RVA);
                RawDataWriter.Write(m_dataDirectories[i].TableSize);
            }
        }

        /// <summary>
        /// Updates all the properties with the contents of RawData
        /// </summary>
        protected override void UpdateProperties()
        {
            // Update base COFF fields
            base.UpdateProperties();

            if (RawData.Length < PEOptionalHeaderBaseSize)
                return;

            switch (MagicNumber)
            {
                case COFFMagicNumbers.PE32Plus:
                    m_baseOfData = 0;
                    m_imageBase = RawDataReader.ReadUInt64();
                    break;
                default:
                    m_baseOfData = RawDataReader.ReadUInt32();
                    m_imageBase = RawDataReader.ReadUInt32();
                    break;
            }

            m_sectionAlignment = RawDataReader.ReadUInt32();
            m_fileAlignment = RawDataReader.ReadUInt32();
            m_majorOperatingSystemVersion = RawDataReader.ReadUInt16();
            m_minorOperatingSystemVersion = RawDataReader.ReadUInt16();
            m_majorImageVersion = RawDataReader.ReadUInt16();
            m_minorImageVersion = RawDataReader.ReadUInt16();
            m_majorSubsystemVersion = RawDataReader.ReadUInt16();
            m_minorSubsystemVersion = RawDataReader.ReadUInt16();
            m_reserved = RawDataReader.ReadUInt32();
            m_sizeOfImage = RawDataReader.ReadUInt32();
            m_sizeOfHeaders = RawDataReader.ReadUInt32();
            m_checkSum = RawDataReader.ReadUInt32();
            m_subSystem = (PESubSystem)RawDataReader.ReadUInt16();
            m_dllCharacteristic = (PEDLLCharacteristics)RawDataReader.ReadUInt16();

            switch (MagicNumber)
            {
                case COFFMagicNumbers.PE32Plus:
                    m_sizeOfStackReserve = RawDataReader.ReadUInt64();
                    m_sizeOfStackCommit = RawDataReader.ReadUInt64();
                    m_sizeOfHeapReserve = RawDataReader.ReadUInt64();
                    m_sizeOfHeapCommit = RawDataReader.ReadUInt64();
                    break;
                default:
                    m_sizeOfStackReserve = RawDataReader.ReadUInt32();
                    m_sizeOfStackCommit = RawDataReader.ReadUInt32();
                    m_sizeOfHeapReserve = RawDataReader.ReadUInt32();
                    m_sizeOfHeapCommit = RawDataReader.ReadUInt32();
                    break;
            }

            m_loaderFlags = RawDataReader.ReadUInt32();
            m_numberOfRvaAndSizes = RawDataReader.ReadUInt32();

            m_dataDirectories.Clear();
            for (UInt32 i = 0; i < m_numberOfRvaAndSizes; i++)
            {
                PEDataDirectory DataDirectory = new PEDataDirectory();
                DataDirectory.RVA = RawDataReader.ReadUInt32();
                DataDirectory.TableSize = RawDataReader.ReadUInt32();
                m_dataDirectories.Add(DataDirectory);
            }
        }
    }
}
