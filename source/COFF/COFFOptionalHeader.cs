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
    /// Abstract base class describing the minimum standard fields for a COFF optional header
    /// </summary>
    public abstract class COFFOptionalHeader
    {
        /// <summary>
        /// Unsigned integer identifying the state of the image file. The most common number is 0413 octal (0x10B), identifying it as a normal executable file. 0407 (0x107) identifies a ROM image.
        /// </summary>
        public abstract COFFMagicNumbers MagicNumber
        {
            get;set;
        }

        /// <summary>
        /// Linker major version number.
        /// </summary>
        public abstract Byte MajorLinkerVersion
        {
            get;set;
        }

        /// <summary>
        /// Linker minor version number.
        /// </summary>
        public abstract Byte MinorLinkerVersion
        {
            get;set;
        }

        /// <summary>
        /// Size of the code (text) section, or the sum of all code sections if there are multiple sections.
        /// </summary>
        public abstract UInt32 SizeOfCode
        {
            get;set;
        }

        /// <summary>
        /// Size of the initialized data section, or the sum of all such sections if there are multiple data sections.
        /// </summary>
        public abstract UInt32 SizeOfInitializedData
        {
            get;set;
        }

        /// <summary>
        /// Size of the uninitialized data section (BSS), or the sum of all such sections if there are multiple BSS sections.
        /// </summary>
        public abstract UInt32 SizeOfUninitializedData
        {
            get;set;
        }

        /// <summary>
        /// Address of entry point, relative to image base, when executable file is loaded into memory. For program images, this is the starting address. For device drivers, this is the address of the initialization function. An entry point is optional for DLLs. When none is present this field should be 0.
        /// </summary>
        public abstract UInt32 AddressOfEntryPoint
        {
            get;set;
        }

        /// <summary>
        /// Address, relative to image base, of beginning of code section, when loaded into memory.
        /// </summary>
        public abstract UInt32 BaseOfCode
        {
            get;set;
        }

        /// <summary>
        /// Raw byte representation of the data contained in the header
        /// </summary>
        public abstract Byte[] RawData
        {
            get;set;
        }

        /// <summary>
        /// A reference to the COFF image that this header belongs to
        /// </summary>
        public abstract COFFObject Image
        {
            get;set;
        }

        /// <summary>
        /// Read header data from the specified stream
        /// </summary>
        /// <param name="InputStream">The stream to read from</param>
        /// <param name="Size">The number of bytes to read. This information can be found in the COFF file header</param>
        public abstract void Read(Stream InputStream, UInt16 Size);

        /// <summary>
        /// Write header data to the specified stream
        /// </summary>
        /// <param name="OutputStream">The stream object to write to</param>
        public abstract void Write(Stream OutputStream);

        /// <summary>
        /// The size in bytes of the header data
        /// </summary>
        public abstract UInt16 Size
        {
            get;
        }
    }

}
