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
    /// Represents an entry in a COFF section line number table
    /// </summary>
    public class COFFLineNumber

    {
        /// <summary>
        /// Creates a new COFFLinenumberEntry object
        /// </summary>
        public COFFLineNumber()
        {
            Type = 0;
            LineNumber = 0;
        }

        /// <summary>
        /// When Linenumber is 0: index to symbol table entry for a function. This format is used to indicate the function that a group of line-number records refer to.
        /// When Linenumber is non-zero: relative virtual address of the executable code that corresponds to the source line indicated. In an object file, this contains the virtual address within the section.
        /// </summary>
        public UInt32 Type
        {
            get;set;
        }

        /// <summary>
        /// When nonzero, this field specifies a one-based line number. When zero, the Type field is interpreted as a Symbol Table Index for a function
        /// </summary>
        public UInt16 LineNumber
        {
            get;set;
        }

        /// <summary>
        /// The size in bytes of a COFFLinenumberEntry
        /// </summary>
        public static UInt32 Size
        {
            get { return 6; }
        }

    }
}
