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
    /// Represents a file header in a COFF image
    /// </summary>
    public class COFFFileHeader
    {
        private DateTime m_timeDateStamp;
        private Int64 m_unixTimeDateStamp;

        /// <summary>
        /// Constant identifying type of target machine/architecture
        /// </summary>
        public COFFMachineType MachineType
        {
            get; set;
        }

        /// <summary>
        /// Number of sections contained in the image
        /// </summary>
        public UInt16 NrOfSections
        {
            get;set;
        }

        /// <summary>
        /// Time and date the image was created
        /// </summary>
        public DateTime TimeDateStamp
        {
            get { return m_timeDateStamp; }
            set { m_timeDateStamp = value; m_unixTimeDateStamp = (m_timeDateStamp.ToUniversalTime().Ticks - 621355968000000000) / 10000000; }
        }
        /// <summary>
        /// Time and date the image was created expressed as seconds since the UNIX epoch 1/1 00:00 1970 
        /// </summary>
        public Int64 UnixTimeDateStamp
        {
            get { return m_unixTimeDateStamp; }
        }

        /// <summary>
        /// Offset of the symbol table within the image or 0 if no symbol table is available
        /// </summary>
        public UInt32 PointerToSymbolTable
        {
            get;set;
        }

        /// <summary>
        /// Number of entries in the symbol table
        /// </summary>
        public UInt32 NumberOfSymbols
        {
            get; set;
        }

        /// <summary>
        /// Size of the optional header, which is required for executable files but not for object files. An object file should have a value of 0 here
        /// </summary>
        public UInt16 SizeOfOptionalHeader
        {
            get; set;
        }

        /// <summary>
        /// Flags indicating characteristics of the file. 
        /// </summary>
        public COFFImageCharacteristics Characteristics
        {
            get; set;
        }


        /// <summary>
        /// Reference to the COFF image that this header belongs to
        /// </summary>
        public COFFObject Image
        {
            get;set;
        }

        /// <summary>
        /// Size in bytes of a COFF file header
        /// </summary>
        public static UInt32 Size
        {
            get { return 20; }
        }

 

        /// <summary>
        /// Creates a new COFFFileHeader object with all members initialized to zero
        /// </summary>
        public COFFFileHeader()
        {
            MachineType = COFFMachineType.IMAGE_FILE_MACHINE_UNKNOWN;
            NrOfSections = 0;
            TimeDateStamp = DateTime.Now;
            PointerToSymbolTable = 0;
            NumberOfSymbols = 0;
            SizeOfOptionalHeader = 0;
            Characteristics = COFFImageCharacteristics.IMAGE_FILE_NONE;
            Image = null;
        }

        /// <summary>
        /// Read header information from the specified stream.
        /// </summary>
        /// <param name="inputStream">The Stream object to read from</param>
        public void Read(Stream inputStream)
        {
            using (PENUTBinaryReader reader = new PENUTBinaryReader(inputStream, Encoding.ASCII, true))
            {
                MachineType = (COFFMachineType)reader.ReadUInt16();
                NrOfSections = reader.ReadUInt16();
                Int64 ticks = reader.ReadUInt32();
                PointerToSymbolTable = reader.ReadUInt32();
                NumberOfSymbols = reader.ReadUInt32();
                SizeOfOptionalHeader = reader.ReadUInt16();
                Characteristics = (COFFImageCharacteristics)reader.ReadUInt16();

                ticks *= 10000000;      // Convert from seconds to ticks
                ticks += 621355968000000000;    // Add nr of ticks from 1/1 00:00 0001 to 1/1 00:00 1970
                TimeDateStamp = new DateTime(ticks).ToLocalTime();
            }
        }

        /// <summary>
        /// Write header information to the specified stream
        /// </summary>
        /// <param name="outputStream">The Stream object to write to</param>
        public void Write(Stream outputStream)
        {
            using (PENUTBinaryWriter writer = new PENUTBinaryWriter(outputStream, Encoding.ASCII, true))
            {
                writer.Write((UInt16)MachineType);
                writer.Write(NrOfSections);
                writer.Write(Convert.ToUInt32(UnixTimeDateStamp));
                writer.Write(PointerToSymbolTable);
                writer.Write(NumberOfSymbols);
                writer.Write(SizeOfOptionalHeader);
                writer.Write((UInt16)Characteristics);
            }
        }
        
    }
}
