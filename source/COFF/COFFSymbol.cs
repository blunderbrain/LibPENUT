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
    /// Represents an entry in a COFF image symbol table
    /// </summary>
    public class COFFSymbol
    {
        private string m_name;

        /// <summary>
        /// Creates a new COFFSymbolTableEntry object
        /// </summary>
        public COFFSymbol()
        {
            Name = "UndefSym";
            NameOffset = 0;
            NameIsIndex = false;
            Image = null;
            Value = 0;
            SectionNumber = (Int16)COFFSymbolSectionConstants.IMAGE_SYM_UNDEFINED;
            SymbolType = COFFSymbolType.IMAGE_SYM_TYPE_NULL;
            StorageClass = COFFSymbolStorageClass.IMAGE_SYM_CLASS_NULL;
            NumberOfAuxSymbols = 0;
            AuxiliaryEntries = new COFFAuxiliarySymbol[0];
        }

        /// <summary>
        /// Name of the symbol.
        /// </summary>
        public string Name
        {
            get
            {
                if (NameIsIndex && Image != null)
                {
                    if (Image.StringTable != null && Image.StringTable.ContainsKey(NameOffset))
                        return Image.StringTable[NameOffset];
                    else
                        return m_name;
                }
                else
                    return m_name;
            }
            set { m_name = value; }
        }

        /// <summary>
        /// Stringtable offset of the symbols name
        /// If the name is longer than 8 bytes this is a byte offset to an entry in the string table, otherwise it's value is undefined.
        /// </summary>
        public UInt32 NameOffset
        {
            get; set;
        }

        /// <summary>
        /// True if the NameOffset property contains a valid index into the stringtable, false otherwise
        /// </summary>
        public bool NameIsIndex
        {
            get; private set;
        }

        /// <summary>
        /// Value associated with the symbol. The interpretation of this field depends on Section Number and Storage Class. A typical meaning is the relocatable address.
        /// </summary>
        public UInt32 Value
        {
            get; set;
        }

        /// <summary>
        /// Signed integer identifying the section, using a one-based index into the Section Table.
        /// </summary>
        public Int16 SectionNumber
        {
            get; set;
        }

        /// <summary>
        /// A number representing type. Microsoft tools set this field to 0x20 (function) or 0x0 (not a function).
        /// </summary>
        public COFFSymbolType SymbolType
        {
            get; set;
        }

        /// <summary>
        /// Enumerated value representing storage class.
        /// </summary>
        public COFFSymbolStorageClass StorageClass
        {
            get; set;
        }

        /// <summary>
        /// Number of auxiliary symbol table entries that follow this record.
        /// </summary>
        public byte NumberOfAuxSymbols
        {
            get; set;
        }

        /// <summary>
        /// Zero or more auxiliary symbol table entries that gives additional information about the symbol
        /// </summary>
        public COFFAuxiliarySymbol[] AuxiliaryEntries
        {
            get; set;
        }

        /// <summary>
        ///  A reference to the COFFObject that this symbol belongs to
        /// </summary>
        public COFFObject Image
        {
            get; set;
        }

        /// <summary>
        /// The size in bytes of a COFF symboltable entry
        /// </summary>
        public static UInt32 Size
        {
            get { return 18; }
        }

        /// <summary>
        /// Read data for this entry from a Stream
        /// </summary>
        /// <param name="inputStream">The Stream to read from</param>
        public void Read(Stream inputStream)
        {
            using (PENUTBinaryReader reader = new PENUTBinaryReader(inputStream, Encoding.ASCII, true))
            {
                UInt32 firstNamePart = reader.ReadUInt32();
                UInt32 secondNamePart = reader.ReadUInt32();

                if (firstNamePart == 0) // First DWORD of Name is zero, remaining 4 bytes is a byte offset into the string table
                {
                    Name = secondNamePart.ToString();
                    NameOffset = secondNamePart;
                    NameIsIndex = true;
                }
                else    // First DWORD is non-zero, Name is an ASCII string
                {
                    // Convert the two read DWORDs from little endian format to a byte array and then to a string.
                    byte[] Buf = new byte[8];
                    Buf[0] = Convert.ToByte((firstNamePart & 0x000000FF));
                    Buf[1] = Convert.ToByte((firstNamePart & 0x0000FF00) >> 8);
                    Buf[2] = Convert.ToByte((firstNamePart & 0x00FF0000) >> 16);
                    Buf[3] = Convert.ToByte((firstNamePart & 0xFF000000) >> 24);
                    Buf[4] = Convert.ToByte((secondNamePart & 0x000000FF));
                    Buf[5] = Convert.ToByte((secondNamePart & 0x0000FF00) >> 8);
                    Buf[6] = Convert.ToByte((secondNamePart & 0x00FF0000) >> 16);
                    Buf[7] = Convert.ToByte((secondNamePart & 0xFF000000) >> 24);
                    Name = Encoding.ASCII.GetString(Buf, 0, 8).Trim(new char[] { '\0' });
                    NameOffset = 0;
                    NameIsIndex = false;
                }

                Value = reader.ReadUInt32();
                SectionNumber = reader.ReadInt16();
                SymbolType = (COFFSymbolType)reader.ReadUInt16();
                StorageClass = (COFFSymbolStorageClass)reader.ReadByte();
                NumberOfAuxSymbols = reader.ReadByte();

                AuxiliaryEntries = new COFFAuxiliarySymbol[NumberOfAuxSymbols];
                for (byte i = 0; i < NumberOfAuxSymbols; i++)
                {
                    AuxiliaryEntries[i] = new COFFAuxiliarySymbol();
                    AuxiliaryEntries[i].Data = reader.ReadBytes((int)COFFSymbol.Size);
                }
            }
        }

        /// <summary>
        /// Write this symboltable entry to a Stream
        /// </summary>
        /// <param name="outputStream">The Stream to write to</param>
        public void Write(Stream outputStream)
        {
            using (PENUTBinaryWriter writer = new PENUTBinaryWriter(outputStream, Encoding.ASCII, true))
            {
                if (!NameIsIndex)
                {
                    writer.Write(Encoding.ASCII.GetBytes((Name as string).PadRight(8, '\0')));
                }
                else
                {
                    writer.Write((UInt32)0);
                    writer.Write(NameOffset);
                }

                writer.Write(Value);
                writer.Write(SectionNumber);
                writer.Write((UInt16)SymbolType);
                writer.Write((Byte)StorageClass);
                writer.Write(NumberOfAuxSymbols);

                for (byte i = 0; i < NumberOfAuxSymbols; i++)
                {
                    writer.Write(AuxiliaryEntries[i].Data);
                }
            }
        }
    }
}
