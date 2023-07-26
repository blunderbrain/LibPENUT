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

namespace LibPENUT
{
    /// <summary>
    /// Constants for the StorageClass field in a symboltable entry
    /// </summary>
    public enum COFFSymbolStorageClass : byte
    {
        /// <summary>
        /// Special symbol representing end of function, for debugging purposes.
        /// </summary>
        IMAGE_SYM_CLASS_END_OF_FUNCTION = 0xFF,

        /// <summary>
        /// No storage class assigned.
        /// </summary>
        IMAGE_SYM_CLASS_NULL = 0x00,

        /// <summary>
        /// Automatic (stack) variable. The Value field specifies stack frame offset.
        /// </summary>
        IMAGE_SYM_CLASS_AUTOMATIC = 0x01,

        /// <summary>
        /// Used by Microsoft tools for external symbols. The Value field indicates the size if the section number is IMAGE_SYM_UNDEFINED (0). If the section number is not 0, then the Value field specifies the offset within the section.
        /// </summary>
        IMAGE_SYM_CLASS_EXTERNAL = 0x02,

        /// <summary>
        /// The Value field specifies the offset of the symbol within the section. If the Value is 0, then the symbol represents a section name.
        /// </summary>
        IMAGE_SYM_CLASS_STATIC = 0x03,

        /// <summary>
        /// Register variable. The Value field specifies register number.
        /// </summary>
        IMAGE_SYM_CLASS_REGISTER = 0x04,

        /// <summary>
        /// Symbol is defined externally.
        /// </summary>
        IMAGE_SYM_CLASS_EXTERNAL_DEF = 0x05,

        /// <summary>
        /// Code label defined within the module. The Value field specifies the offset of the symbol within the section.
        /// </summary>
        IMAGE_SYM_CLASS_LABEL = 0x06,

        /// <summary>
        /// Reference to a code label not defined.
        /// </summary>
        IMAGE_SYM_CLASS_UNDEFINED_LABEL = 0x07,

        /// <summary>
        /// Structure member. The Value field specifies nth member.
        /// </summary>
        IMAGE_SYM_CLASS_MEMBER_OF_STRUCT = 0x08,

        /// <summary>
        /// Formal argument (parameter)of a function. The Value field specifies nth argument.
        /// </summary>
        IMAGE_SYM_CLASS_ARGUMENT = 0x09,

        /// <summary>
        /// Structure tag-name entry.
        /// </summary>
        IMAGE_SYM_CLASS_STRUCT_TAG = 0x0A,

        /// <summary>
        /// Union member. The Value field specifies nth member.
        /// </summary>
        IMAGE_SYM_CLASS_MEMBER_OF_UNION = 0x0B,

        /// <summary>
        /// Union tag-name entry.
        /// </summary>
        IMAGE_SYM_CLASS_UNION_TAG = 0x0C,

        /// <summary>
        /// Typedef entry.
        /// </summary>
        IMAGE_SYM_CLASS_TYPE_DEFINITION = 0x0D,

        /// <summary>
        /// Static data declaration.
        /// </summary>
        IMAGE_SYM_CLASS_UNDEFINED_STATIC = 0x0E,

        /// <summary>
        /// Enumerated type tagname entry.
        /// </summary>
        IMAGE_SYM_CLASS_ENUM_TAG = 0x0F,

        /// <summary>
        /// Member of enumeration. Value specifies nth member.
        /// </summary>
        IMAGE_SYM_CLASS_MEMBER_OF_ENUM = 0x10,

        /// <summary>
        /// Register parameter.
        /// </summary>
        IMAGE_SYM_CLASS_REGISTER_PARAM = 0x11,

        /// <summary>
        /// Bit-field reference. Value specifies nth bit in the bit field.
        /// </summary>
        IMAGE_SYM_CLASS_BIT_FIELD = 0x12,

        IMAGE_SYM_CLASS_FAR_EXTERNAL = 0x44,

        /// <summary>
        /// A .bb (beginning of block) or .eb (end of block) record. Value is the relocatable address of the code location.
        /// </summary>
        IMAGE_SYM_CLASS_BLOCK = 0x64,

        /// <summary>
        /// Used by Microsoft tools for symbol records that define the extent of a function: begin function (named .bf), end function (.ef), and lines in function (.lf). For .lf records, Value gives the number of source lines in the function. For .ef records, Value gives the size of function code.
        /// </summary>
        IMAGE_SYM_CLASS_FUNCTION = 0x65,

        /// <summary>
        /// End of structure entry.
        /// </summary>
        IMAGE_SYM_CLASS_END_OF_STRUCT = 0x66,

        /// <summary>
        /// Used by Microsoft tools, as well as traditional COFF format, for the source-file symbol record. The symbol is followed by auxiliary records that name the file.
        /// </summary>
        IMAGE_SYM_CLASS_FILE = 0x67,

        /// <summary>
        /// Definition of a section (Microsoft tools use STATIC storage class instead).
        /// </summary>
        IMAGE_SYM_CLASS_SECTION = 0x68,

        /// <summary>
        /// Weak external.
        /// </summary>
        IMAGE_SYM_CLASS_WEAK_EXTERNAL = 0x69,

        /// <summary>
        /// CLR Token
        /// </summary>
        IMAGE_SYM_CLASS_CLR_TOKEN = 0x006B
    }
}
