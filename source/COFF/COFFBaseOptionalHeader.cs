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

namespace LibPENUT
{
    /// <summary>
    /// Base implementation for the minimum required fields of the COFF optional header
    /// </summary>
    public class COFFBaseOptionalHeader : COFFOptionalHeader
    {
        // Size in bytes of the fields in a base COFF optional header
        private const int COFFBaseOptionalHeaderSize = 24;

        private byte[] m_rawData;
        private COFFMagicNumbers m_magicNumber;
        private Byte m_majorLinkerVersion;
        private Byte m_minorLinkerVersion;
        private UInt32 m_sizeOfCode;
        private UInt32 m_sizeOfInitializedData;
        private UInt32 m_sizeOfUninitializedData;
        private UInt32 m_addressOfEntryPoint;
        private UInt32 m_baseOfCode;

        // Writer/Reader objects used to keep the RawData member in sync with the object properties
        protected PENUTBinaryWriter RawDataWriter;
        protected PENUTBinaryReader RawDataReader;

        /// <summary>
        /// Creates a new COFFStandardOptionalHeader with all fields initialized to zero
        /// </summary>
        public COFFBaseOptionalHeader()
        {
            // Setting RawData property will initialize all other properties to zero
            RawData = new byte[COFFBaseOptionalHeaderSize];
            MagicNumber = COFFMagicNumbers.PE32;
        }


        #region Public Properties
        /// <summary>
        /// A value from the COFFMagicNumbers enumeration identifying the type of the image
        /// </summary>
        public override COFFMagicNumbers MagicNumber
        {
            get { return m_magicNumber; }
            set { m_magicNumber = value; UpdateRawData(); }
        }

        /// <summary>
        /// Linker major version number.
        /// </summary>
        public override Byte MajorLinkerVersion
        {
            get { return m_majorLinkerVersion; }
            set { m_majorLinkerVersion = value; UpdateRawData(); }
        }

        /// <summary>
        /// Linker minor version number.
        /// </summary>
        public override Byte MinorLinkerVersion
        {
            get { return m_minorLinkerVersion; }
            set { m_minorLinkerVersion = value; UpdateRawData(); }
        }

        /// <summary>
        /// Size of the code (text) section, or the sum of all code sections if there are multiple sections.
        /// </summary>
        public override UInt32 SizeOfCode
        {
            get { return m_sizeOfCode; }
            set { m_sizeOfCode = value; UpdateRawData(); }
        }

        /// <summary>
        /// Size of the initialized data section, or the sum of all such sections if there are multiple data sections.
        /// </summary>
        public override UInt32 SizeOfInitializedData
        {
            get { return m_sizeOfInitializedData; }
            set { m_sizeOfInitializedData = value; UpdateRawData(); }
        }

        /// <summary>
        /// Size of the uninitialized data section (BSS), or the sum of all such sections if there are multiple BSS sections.
        /// </summary>
        public override UInt32 SizeOfUninitializedData
        {
            get { return m_sizeOfUninitializedData; }
            set { m_sizeOfUninitializedData = value; UpdateRawData(); }
        }

        /// <summary>
        /// Address of entry point, relative to image base, when executable file is loaded into memory. For program images, this is the starting address. For device drivers, this is the address of the initialization function. An entry point is optional for DLLs. When none is present this field should be 0.
        /// </summary>
        public override UInt32 AddressOfEntryPoint
        {
            get { return m_addressOfEntryPoint; }
            set { m_addressOfEntryPoint = value; UpdateRawData(); }
        }

        /// <summary>
        /// Address, relative to image base, of beginning of code section, when loaded into memory.
        /// </summary>
        public override UInt32 BaseOfCode
        {
            get { return m_baseOfCode; }
            set { m_baseOfCode = value; UpdateRawData(); }
        }

        /// <summary>
        /// Raw byte representation of the data contained in the optional header
        /// </summary>
        public override Byte[] RawData
        {
            get { return m_rawData; }
            set
            {
                m_rawData = value;
                RawDataWriter = new PENUTBinaryWriter(new MemoryStream(m_rawData, true));
                RawDataReader = new PENUTBinaryReader(new MemoryStream(m_rawData));
                UpdateProperties();
            }
        }

        /// <summary>
        /// Reference to the image object that this header belongs to
        /// </summary>
        public override COFFObject Image
        {
            get;set;

        }

        /// <summary>
        /// The size in bytes of this header object
        /// </summary>
        public override UInt16 Size
        {
            get { return Convert.ToUInt16(RawData.Length); }
        }

        #endregion

        /// <summary>
        /// Read header information from the specified Stream
        /// </summary>
        /// <param name="inputStream">The Stream object to read from</param>
        /// <param name="size">The number of bytes to read. This value can be found in the COFF fileheader</param>
        public override void Read(Stream inputStream, UInt16 size)
        {
            RawData = new byte[size];
            inputStream.Read(RawData, 0, size);
            UpdateProperties();
        }
        
        /// <summary>
        /// Write header data to the specified stream
        /// </summary>
        /// <param name="outputStream">The Stream object to write to</param>
        public override void Write(Stream outputStream)
        {
            UpdateRawData();
            outputStream.Write(RawData, 0, RawData.Length);
        }

        /// <summary>
        /// Updates the RawData member with the contents of the properties
        /// </summary>
        protected virtual void UpdateRawData()
        {
            RawDataWriter.BaseStream.Position = 0;

            if (RawData.Length < COFFBaseOptionalHeaderSize)    // Guard against RawData being asigned an undersized value
                return;

            RawDataWriter.Write((UInt16)MagicNumber);
            RawDataWriter.Write(MajorLinkerVersion);
            RawDataWriter.Write(MinorLinkerVersion);
            RawDataWriter.Write(SizeOfCode);
            RawDataWriter.Write(SizeOfInitializedData);
            RawDataWriter.Write(SizeOfUninitializedData);
            RawDataWriter.Write(AddressOfEntryPoint);
            RawDataWriter.Write(BaseOfCode);
        }

        /// <summary>
        /// Updates the properties with the contents of RawData
        /// </summary>
        protected virtual void UpdateProperties()
        {
            RawDataReader.BaseStream.Position = 0;

            if (RawData.Length < COFFBaseOptionalHeaderSize)    // Guard against RawData being asigned an undersized value
                return;

            m_magicNumber = (COFFMagicNumbers)RawDataReader.ReadUInt16();
            m_majorLinkerVersion = RawDataReader.ReadByte();
            m_minorLinkerVersion = RawDataReader.ReadByte();
            m_sizeOfCode = RawDataReader.ReadUInt32();
            m_sizeOfInitializedData = RawDataReader.ReadUInt32();
            m_sizeOfUninitializedData = RawDataReader.ReadUInt32();
            m_addressOfEntryPoint = RawDataReader.ReadUInt32();
            m_baseOfCode = RawDataReader.ReadUInt32();
        }

    }
}
