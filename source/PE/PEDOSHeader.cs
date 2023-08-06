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
using System.IO;

namespace LibPENUT
{
    /// <summary>
    /// Represents the DOS header in a PE image
    /// </summary>
    public class PEDOSHeader
    {
        #region Private Members
        private MagicNumbers m_e_magic;               // Magic number
        private UInt16 m_e_cblp;                      // Bytes on last page of file
        private UInt16 m_e_cp;                        // Pages in file
        private UInt16 m_e_crlc;                      // Relocations
        private UInt16 m_e_cparhdr;                   // Size of header in paragraphs
        private UInt16 m_e_minalloc;                  // Minimum extra paragraphs needed
        private UInt16 m_e_maxalloc;                  // Maximum extra paragraphs needed
        private UInt16 m_e_ss;                        // Initial (relative) SS value
        private UInt16 m_e_sp;                        // Initial SP value
        private UInt16 m_e_csum;                      // Checksum
        private UInt16 m_e_ip;                        // Initial IP value
        private UInt16 m_e_cs;                        // Initial (relative) CS value
        private UInt16 m_e_lfarlc;                    // File address of relocation table
        private UInt16 m_e_ovno;                      // Overlay number
        private UInt16[] m_e_res;                    // Reserved words (4)
        private UInt16 m_e_oemid;                     // OEM identifier (for e_oeminfo)
        private UInt16 m_e_oeminfo;                   // OEM information; e_oemid specific
        private UInt16[] m_e_res2;                  // Reserved words (10)
        private Int32 m_e_lfanew;                    // File address of new exe header
        #endregion

        public PEDOSHeader()
        {
            e_magic = MagicNumbers.IMAGE_DOS_SIGNATURE;
            e_cblp = 0;
            e_cp = 0;
            e_crlc = 0;
            e_cparhdr = 0;
            e_minalloc = 0;
            e_maxalloc = 0;
            e_ss = 0;
            e_sp = 0;
            e_csum = 0;
            e_ip = 0;
            e_cs = 0;
            e_lfarlc = 0;
            e_ovno = 0;
            e_res = new UInt16[4];
            e_oemid = 0;
            e_oeminfo = 0;
            e_res2 = new UInt16[10];
            e_lfanew = 0;
        }

        /// <summary>
        /// Magic number
        /// </summary>
        public MagicNumbers e_magic
        {
            get { return m_e_magic; }
            set { m_e_magic = value; }
        }
        
        /// <summary>
        /// Bytes on last page of file
        /// </summary>
        public UInt16 e_cblp
        {
            get { return m_e_cblp; }
            set { m_e_cblp = value; }
        }

        /// <summary>
        /// Pages in file
        /// </summary>
        public UInt16 e_cp
        {
            get { return m_e_cp; }
            set { m_e_cp = value; }
        }

        /// <summary>
        /// Relocations
        /// </summary>
        public UInt16 e_crlc
        {
            get { return m_e_crlc; }
            set { m_e_crlc = value; }
        }

        /// <summary>
        /// Size of header in paragraphs
        /// </summary>
        public UInt16 e_cparhdr
        {
            get { return m_e_cparhdr; }
            set { m_e_cparhdr = value; }
        }

        /// <summary>
        /// Minimum extra paragraphs needed
        /// </summary>
        public UInt16 e_minalloc
        {
            get { return m_e_minalloc; }
            set { m_e_minalloc = value; }
        }
    
        /// <summary>
        /// Maximum extra paragraphs needed
        /// </summary>
        public UInt16 e_maxalloc
        {
            get { return m_e_maxalloc; }
            set { m_e_maxalloc = value; }
        }

        /// <summary>
        /// Initial (relative) SS value
        /// </summary>
        public UInt16 e_ss
        {
            get { return m_e_ss; }
            set { m_e_ss = value; }
        }

        /// <summary>
        /// Initial SP value
        /// </summary>
        public UInt16 e_sp
        {
            get { return m_e_sp; }
            set { m_e_sp = value; }
        }

        /// <summary>
        /// Checksum
        /// </summary>
        public UInt16 e_csum
        {
            get { return m_e_csum; }
            set { m_e_csum = value; }
        }

        /// <summary>
        /// Initial IP value
        /// </summary>
        public UInt16 e_ip
        {
            get { return m_e_ip; }
            set { m_e_ip = value; }
        }

        /// <summary>
        /// Initial (relative) CS value
        /// </summary>
        public UInt16 e_cs
        {
            get { return m_e_cs; }
            set { m_e_cs = value; }
        }

        /// <summary>
        /// File address of relocation table
        /// </summary>
        public UInt16 e_lfarlc
        {
            get { return m_e_lfarlc; }
            set { m_e_lfarlc = value; }
        }

        /// <summary>
        /// Overlay number
        /// </summary>
        public UInt16 e_ovno
        {
            get { return m_e_ovno; }
            set { m_e_ovno = value; }
        }

        /// <summary>
        /// Reserved
        /// </summary>
        public UInt16[] e_res
        {
            get { return m_e_res; }
            set
            {
                if (value.Length != 4)
                    throw new ArgumentException("Length of array must be 4 words", "value");
                else
                    m_e_res = value;
            }
        }

        /// <summary>
        /// OEM identifier (for e_oeminfo)
        /// </summary>
        public UInt16 e_oemid
        {
            get { return m_e_oemid; }
            set { m_e_oemid = value; }
        }

        /// <summary>
        /// OEM information; e_oemid specific
        /// </summary>
        public UInt16 e_oeminfo
        {
            get { return m_e_oeminfo; }
            set { m_e_oeminfo = value; }
        }

        /// <summary>
        /// Reserved
        /// </summary>
        public UInt16[] e_res2
        {
            get { return m_e_res2; }
            set
            {
                if (value.Length != 10)
                    throw new ArgumentException("Length of array must be 10 words", "value");
                else
                    m_e_res2 = value;
            }
        }

        /// <summary>
        /// File address of new exe header
        /// </summary>
        public Int32 e_lfanew
        {
            get { return m_e_lfanew; }
            set { m_e_lfanew = value; }
        }

        /// <summary>
        /// The size in bytes of a PEDOSHeader
        /// </summary>
        public static UInt32 Size
        {
            get{ return 64; }
        }
        
        /// <summary>
        /// Read header information from the specified Stream. The stream should be positioned at the beginning of the header data before calling this method
        /// </summary>
        /// <param name="inputStream">The Stream object to read from</param>
        public void Read(Stream inputStream)
        {
            using (PENUTBinaryReader reader = new PENUTBinaryReader(inputStream, System.Text.Encoding.ASCII, true))
            {
                UInt16 Tmp = reader.ReadUInt16();
                e_magic = (MagicNumbers)((Tmp >> 8) | (Tmp << 8));

                // First sanity check that we are dealing with a PE file
                if (!(e_magic == MagicNumbers.IMAGE_DOS_SIGNATURE || e_magic == MagicNumbers.IMAGE_OS2_SIGNATURE || e_magic == MagicNumbers.IMAGE_OS2_SIGNATURE_LE) )
                {
                    throw new BadImageFormatException(string.Format("Error: Unknown image signature 0x{0:X} in DOS header. Expected 0x4D5A (MZ), 0x4E45 (NE) or 0x4C45 (LE)", e_magic));
                }

                e_cblp = reader.ReadUInt16();
                e_cp = reader.ReadUInt16();
                e_crlc = reader.ReadUInt16();
                e_cparhdr = reader.ReadUInt16();
                e_minalloc = reader.ReadUInt16();
                e_maxalloc = reader.ReadUInt16();
                e_ss = reader.ReadUInt16();
                e_sp = reader.ReadUInt16();
                e_csum = reader.ReadUInt16();
                e_ip = reader.ReadUInt16();
                e_cs = reader.ReadUInt16();
                e_lfarlc = reader.ReadUInt16();
                e_ovno = reader.ReadUInt16();
                for (int i = 0; i < 4; i++)
                    e_res[i] = reader.ReadUInt16();
                e_oemid = reader.ReadUInt16();
                e_oeminfo = reader.ReadUInt16();
                for (int i = 0; i < 10; i++)
                    e_res2[i] = reader.ReadUInt16();
                e_lfanew = reader.ReadInt32();
            }
        }

        /// <summary>
        /// Write header to the specified Stream
        /// </summary>
        /// <param name="outputStream">The Stream object to write to</param>
        public void Write(Stream outputStream)
        {
            using (PENUTBinaryWriter writer = new PENUTBinaryWriter(outputStream, System.Text.Encoding.ASCII, true))
            {
                writer.Write((byte)((UInt16)e_magic >> 8));
                writer.Write((byte)e_magic);
                writer.Write(e_cblp);
                writer.Write(e_cp);
                writer.Write(e_crlc);
                writer.Write(e_cparhdr);
                writer.Write(e_minalloc);
                writer.Write(e_maxalloc);
                writer.Write(e_ss);
                writer.Write(e_sp);
                writer.Write(e_csum);
                writer.Write(e_ip);
                writer.Write(e_cs);
                writer.Write(e_lfarlc);
                writer.Write(e_ovno);
                for (int i = 0; i < 4; i++)
                    writer.Write(e_res[i]);
                writer.Write(e_oemid);
                writer.Write(e_oeminfo);
                for (int i = 0; i < 10; i++)
                    writer.Write(e_res2[i]);
                writer.Write(e_lfanew);
            }
        }

        /// <summary>
        /// Constants for the e_magic field in the DOS header
        /// </summary>
        public enum MagicNumbers : ushort
        {
            IMAGE_DOS_SIGNATURE = 0x4D5A,      // MZ
            IMAGE_OS2_SIGNATURE = 0x4E45,      // NE
            IMAGE_OS2_SIGNATURE_LE = 0x4C45      // LE
        }
    }
}
