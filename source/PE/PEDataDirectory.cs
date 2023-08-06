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

namespace LibPENUT
{
    /// <summary>
    /// Constants describing the index of each of the well known Data Directory tables in the PE optional header
    /// </summary>
    public struct PEDataDirectories
    {
        public const int Exports = 0;
        public const int Imports = 1;
        public const int Resources = 2;
        public const int Exceptions = 3;
        public const int Certificates = 4;
        public const int BaseRelocations = 5;
        public const int Debug = 6;
        public const int Architecture = 7;
        public const int GlobalPointers = 8;
        public const int TLS = 9;
        public const int LoadConfig = 10;
        public const int BoundImports = 11;
        public const int ImportAddresses = 12;
        public const int DelayImports = 13;
        public const int ClrRuntimeHeader = 14;
    }

    /// <summary>
    /// Represents a data directory in a PE optional header
    /// A data directory contains information about tables loaded from the image into memory when the image is loaded
    /// </summary>
    public class PEDataDirectory
    {

        /// <summary>
        /// Creates a new PEDataDirectory object
        /// </summary>
        public PEDataDirectory()
        {
            RVA = 0;
            TableSize = 0;
        }

        /// <summary>
        /// Creates a new PEDataDirectory object
        /// </summary>
        public PEDataDirectory(UInt32 rva, UInt32 tableSize)
        {
            RVA = rva;
            TableSize = tableSize;
        }

        /// <summary>
        /// The relative virtual address of the table. The RVA is the address of the table, when loaded, relative to the base address of the image.
        /// </summary>
        public UInt32 RVA
        {
            get;set;
        }

        /// <summary>
        /// The size in bytes of the table pointed to by the RVA field
        /// </summary>
        public UInt32 TableSize
        {
            get;set;
        }

        /// <summary>
        /// The size in bytes of a PEDataDirectory entry
        /// </summary>
        public static UInt32 Size
        {
            get { return 8; }
        }
    }
}
