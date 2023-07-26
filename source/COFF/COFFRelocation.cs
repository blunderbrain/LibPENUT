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
    /// Represents an entry in a COFF section relocation table
    /// </summary>
    public class COFFRelocation
    {

        /// <summary>
        /// Creates a new COFFRelocationEntry object
        /// </summary>
        public COFFRelocation()
        {
            VirtualAddress = 0;
            SymbolTableIndex = 0;
            Type = 0;
        }


        /// <summary>
        /// Address of the item to which relocation is applied: this is the offset from the beginning of the section, plus the value of the section’s RVA/Offset field.
        /// For example, if the first byte of the section has an address of 0x10, the third byte has an address of 0x12.
        /// </summary>
        public UInt32 VirtualAddress
        {
            get; set;
        }

        /// <summary>
        /// A zero-based index into the symbol table. This symbol gives the address to be used for the relocation.
        /// If the specified symbol has section storage class, then the symbol’s address is the address with the first section of the same name.
        /// </summary>
        public UInt32 SymbolTableIndex
        {
            get; set;
        }

        /// <summary>
        ///	A value indicating what kind of relocation should be performed.
        ///	The interpretation of this value depends on the target architecture and is described by the corresponding COFFRelocationType_[architecture] enum.
        /// </summary>
        public UInt16 Type
        {
            get; set;
        }

        /// <summary>
        /// The size in bytes of a COFFRelocation
        /// </summary>
        public static UInt32 Size
        {
            get { return 10; }
        }

    }
}
