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

namespace LibPENUT
{
    /// <summary>
    /// Represents an single entry in a PE base relocation block
    /// </summary>
    public class PERelocation
    {

        /// <summary>
        /// Creates a new PERelocation object
        /// </summary>
        public PERelocation()
        {
            Type = PERelocationType.IMAGE_REL_BASED_ABSOLUTE;
            Offset = 0;
        }

        public PERelocation(UInt16 blockEntry)
        {
            Offset = (UInt16)(blockEntry & 0x0FFF);
            Type = (PERelocationType)((blockEntry & 0xF000) >> 12);
        }
        

        /// <summary>
        /// An offset from the starting address that was specified in the Page RVA field for the block. This offset specifies where the base relocation is to be applied.
        /// </summary>
        public UInt16 Offset
        {
            get; set;
        }

        /// <summary>
        ///	A value indicating what kind of relocation should be applied.
        /// </summary>
        public PERelocationType Type
        {
            get; set;
        }

        /// <summary>
        /// The combined values of Type and Offest ORed together in the representation used in the .reloc section
        /// </summary>
        public UInt16 EntryValue
        {
            get { return (UInt16)(((UInt16)Type << 12) & Offset); }
        }

        /// <summary>
        /// The size in bytes of a PERelocation entry
        /// </summary>
        public static UInt32 Size
        {
            get { return 2; }
        }

    }
}
