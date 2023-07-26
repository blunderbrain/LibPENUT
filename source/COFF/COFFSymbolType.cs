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
    /// Constants for the SymbolType field in a symbol table entry
    /// </summary>
    [Flags]
    public enum COFFSymbolType : ushort
    {
        // *********** Base type flags, stored in LSB of the symbol type field ***********

        /// <summary>
        /// No type information or unknown base type.
        /// </summary>
        IMAGE_SYM_TYPE_NULL = 0,

        /// <summary>
        /// No valid type, used with void pointers and functions.
        /// </summary>
        IMAGE_SYM_TYPE_VOID = 1,

        /// <summary>
        /// Character (signed byte).
        /// </summary>
        IMAGE_SYM_TYPE_CHAR = 2,

        /// <summary>
        /// Two-byte signed integer.
        /// </summary>
        IMAGE_SYM_TYPE_SHORT = 3,

        /// <summary>
        /// Natural integer type
        /// </summary>
        IMAGE_SYM_TYPE_INT = 4,

        /// <summary>
        /// Four-byte signed integer.
        /// </summary>
        IMAGE_SYM_TYPE_LONG = 5,

        /// <summary>
        /// Four-byte floating-point number.
        /// </summary>
        IMAGE_SYM_TYPE_FLOAT = 6,

        /// <summary>
        /// Eight-byte floating-point number.
        /// </summary>
        IMAGE_SYM_TYPE_DOUBLE = 7,

        /// <summary>
        /// Structure.
        /// </summary>
        IMAGE_SYM_TYPE_STRUCT = 8,

        /// <summary>
        /// Union.
        /// </summary>
        IMAGE_SYM_TYPE_UNION = 9,

        /// <summary>
        /// Enumerated type.
        /// </summary>
        IMAGE_SYM_TYPE_ENUM = 10,

        /// <summary>
        /// Member of enumeration (a specific value).
        /// </summary>
        IMAGE_SYM_TYPE_MOE = 11,

        /// <summary>
        /// Byte, unsigned one-byte integer.
        /// </summary>
        IMAGE_SYM_TYPE_BYTE = 12,

        /// <summary>
        /// Word, unsigned two-byte integer.
        /// </summary>
        IMAGE_SYM_TYPE_WORD = 13,

        /// <summary>
        /// Unsigned integer of natural size (normally, four bytes).
        /// </summary>
        IMAGE_SYM_TYPE_UINT = 14,

        /// <summary>
        /// Unsigned four-byte integer.
        /// </summary>
        IMAGE_SYM_TYPE_DWORD = 15,

        IMAGE_SYM_TYPE_PCODE = 0x8000,

        // *********** Derived type flags, stored in MSB of the symbol type field ***********

        /// <summary>
        /// No derived type, the symbol is a simple scalar variable. 
        /// </summary>
        IMAGE_SYM_DTYPE_NULL = 0x0000,

        /// <summary>
        /// Pointer to base type.
        /// </summary>
        IMAGE_SYM_DTYPE_POINTER = 0x1000,

        /// <summary>
        /// Function returning base type.
        /// </summary>
        IMAGE_SYM_DTYPE_FUNCTION = 0x2000,

        /// <summary>
        /// Array of base type.
        /// </summary>
        IMAGE_SYM_DTYPE_ARRAY = 0x3000
    }
}
