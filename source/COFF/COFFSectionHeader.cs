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

namespace LibPENUT
{
    /// <summary>
    /// Represents a section header in a COFF image
    /// </summary>
    public class COFFSectionHeader
    {
        private string m_name;

        /// <summary>
        /// Name of the section. A section name must only contain 7-bit ASCII characters and can not exceed 8 characters in length
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set
            {
                if (Encoding.ASCII.GetByteCount(value) > 8)
                    throw new ArgumentOutOfRangeException("Name", "Section name can not be longer than 8 characters");
                else
                    m_name = value;
            }
        }

        /// <summary>
        /// Total size of the section when loaded into memory.
        /// If this value is greater than SizeOfRawData, the section is zero-padded.
        /// This field is valid only for executable images and should be set to 0 for object files.
        /// </summary>
        public UInt32 VirtualSize
        {
            get;set;
        }

        /// <summary>
        /// For executable images this is the address of the first byte of the section when loaded into memory, relative to the image base.
        /// For object files, this field is the address of the first byte before relocation is applied; for simplicity, compilers should set this to zero.
        /// Otherwise, it is an arbitrary value that is subtracted from offsets during relocation.
        /// </summary>
        public UInt32 VirtualAddress
        {
            get;set;
        }

        /// <summary>
        /// Size of the section (object file) or size of the initialized data on disk (image files).
        /// For executable image, this must be a multiple of FileAlignment from the optional header.
        /// If this is less than VirtualSize the remainder of the section is zero filled.
        /// Because this field is rounded while the VirtualSize field is not it is possible for this to be greater than VirtualSize as well.
        /// When a section contains only uninitialized data, this field should be 0.
        /// </summary>
        public UInt32 SizeOfRawData
        {
            get;set;
        }

        /// <summary>
        /// Offset to section’s first page within the COFF image.
        /// For executable images, this must be a multiple of FileAlignment from the optional header.
        /// For object files, the value should be aligned on a four-byte boundary for best performance.
        /// When a section contains only uninitialized data, this field should be 0.
        /// </summary>
        public UInt32 PointerToRawData
        {
            get;set;
        }

        /// <summary>
        /// Offset to beginning of relocation entries for the section within the image
        /// Set to 0 for executable images or if there are no relocations.
        /// </summary>
        public UInt32 PointerToRelocations
        {
            get;set;
        }

        /// <summary>
        /// Offset to beginning of line-number entries for the section within the image
        /// Set to 0 if there are no COFF line numbers.
        /// </summary>
        public UInt32 PointerToLineNumbers
        {
            get;set;
        }

        /// <summary>
        /// Number of relocation entries for the section. Set to 0 for executable images.
        /// </summary>
        public UInt16 NumberOfRelocations
        {
            get;set;
        }

        /// <summary>
        /// Number of line-number entries for the section.
        /// </summary>
        public UInt16 NumberOfLineNumbers
        {
            get;set;
        }

        /// <summary>
        /// A combination of values from the COFFSectionCharacteristics enumeration describing section’s characteristics
        /// </summary>
        public COFFSectionCharacteristics Characteristics
        {
            get;set;
        }

        /// <summary>
        /// The size in bytes of a COFF section header
        /// </summary>
        public static UInt32 Size
        {
            get { return 40; }
        }

        /// <summary>
        /// Creates a new COFFSectionHeader with the default name ".section"
        /// </summary>
        public COFFSectionHeader() : this(".section") { }

        /// <summary>
        /// Creates a new COFFSectionHeader with the specified name
        /// </summary>
        public COFFSectionHeader(string name)
        {
            this.Name = name;
            VirtualSize = 0;
            VirtualAddress = 0;
            SizeOfRawData = 0;
            PointerToRawData = 0;
            PointerToRelocations = 0;
            PointerToLineNumbers = 0;
            NumberOfRelocations = 0;
            NumberOfLineNumbers = 0;
            Characteristics = COFFSectionCharacteristics.IMAGE_SCN_CNT_UNINITIALIZED_DATA;
        }

        /// <summary>
        /// Read header data from the specified Stream
        /// </summary>
        /// <param name="inputStream">The Stream object to read from</param>
        public void Read(Stream inputStream)
        {
            using (PENUTBinaryReader reader = new PENUTBinaryReader(inputStream, Encoding.ASCII, true))
            {
                byte[] name = reader.ReadBytes(8);
                Name = Encoding.ASCII.GetString(name, 0, 8).Trim(new char[] { '\0' });
                VirtualSize = reader.ReadUInt32();
                VirtualAddress = reader.ReadUInt32();
                SizeOfRawData = reader.ReadUInt32();
                PointerToRawData = reader.ReadUInt32();
                PointerToRelocations = reader.ReadUInt32();
                PointerToLineNumbers = reader.ReadUInt32();
                NumberOfRelocations = reader.ReadUInt16();
                NumberOfLineNumbers = reader.ReadUInt16();
                Characteristics = (COFFSectionCharacteristics)reader.ReadUInt32();
            }
        }

        /// <summary>
        /// Write header data to the specified Stream
        /// </summary>
        /// <param name="outputStream">The Stream object to write to</param>
        public void Write(Stream outputStream)
        {
            using (PENUTBinaryWriter writer = new PENUTBinaryWriter(outputStream, Encoding.ASCII, true))
            {
                byte[] name = new byte[8];
                Encoding.ASCII.GetBytes(Name, 0, Name.Length, name, 0);

                writer.Write(name);
                writer.Write(VirtualSize);
                writer.Write(VirtualAddress);
                writer.Write(SizeOfRawData);
                writer.Write(PointerToRawData);
                writer.Write(PointerToRelocations);
                writer.Write(PointerToLineNumbers);
                writer.Write(NumberOfRelocations);
                writer.Write(NumberOfLineNumbers);
                writer.Write((UInt32)Characteristics);
            }
        }

    }
}
