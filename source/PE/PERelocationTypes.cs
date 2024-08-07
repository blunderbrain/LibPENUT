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
    /// Constants describing the supported relocation types in a PE .reloc section
    /// </summary>
    public enum PERelocationType : ushort
    {
        // TODO:  Should we split this into machine specific enums like the COFF types?

        /// <summary>
        /// The base relocation is skipped. This type can be used to pad a block.
        /// </summary>
        IMAGE_REL_BASED_ABSOLUTE = 0,

        /// <summary>
        /// The base relocation adds the high 16 bits of the difference to the 16-bit field at offset. The 16-bit field represents the high value of a 32-bit word.
        /// </summary>
        IMAGE_REL_BASED_HIGH = 1,

        /// <summary>
        /// The base relocation adds the low 16 bits of the difference to the 16-bit field at offset. The 16-bit field represents the low half of a 32-bit word.
        /// </summary>
        IMAGE_REL_BASED_LOW = 2,

        /// <summary>
        /// The base relocation applies all 32 bits of the difference to the 32-bit field at offset.
        /// </summary>
        IMAGE_REL_BASED_HIGHLOW = 3,

        /// <summary>
        /// The base relocation adds the high 16 bits of the difference to the 16-bit field at offset. The 16-bit field represents the high value of a 32-bit word. The low 16 bits of the 32-bit value are stored in the 16-bit word that follows this base relocation. This means that this base relocation occupies two slots.
        /// </summary>
        IMAGE_REL_BASED_HIGHADJ = 4,

        /// <summary>
        /// The relocation interpretation is dependent on the machine type. When the machine type is MIPS, the base relocation applies to a MIPS jump instruction.
        /// </summary>
        IMAGE_REL_BASED_MIPS_JMPADDR = 5,

        /// <summary>
        /// This relocation is meaningful only when the machine type is ARM or Thumb. The base relocation applies the 32-bit address of a symbol across a consecutive MOVW/MOVT instruction pair.
        /// </summary>
        IMAGE_REL_BASED_ARM_MOV32 = 5,

        /// <summary>
        /// Reserved, must be zero.
        /// </summary>
        IMAGE_REL_BASED_RESERVED = 6,

        /// <summary>
        /// This relocation is only meaningful when the machine type is RISC-V. The base relocation applies to the high 20 bits of a 32-bit absolute address.
        /// </summary>
        IMAGE_REL_BASED_RISCV_HIGH20 = 5,

        /// <summary>
        /// This relocation is meaningful only when the machine type is Thumb. The base relocation applies the 32-bit address of a symbol to a consecutive MOVW/MOVT instruction pair.
        /// </summary>
        IMAGE_REL_BASED_THUMB_MOV32 = 7,

        /// <summary>
        /// This relocation is only meaningful when the machine type is RISC-V. The base relocation applies to the low 12 bits of a 32-bit absolute address formed in RISC-V I-type instruction format.
        /// </summary>
        IMAGE_REL_BASED_RISCV_LOW12I = 7,

        /// <summary>
        /// This relocation is only meaningful when the machine type is RISC-V. The base relocation applies to the low 12 bits of a 32-bit absolute address formed in RISC-V S-type instruction format.
        /// </summary>
        IMAGE_REL_BASED_RISCV_LOW12S = 8,

        /// <summary>
        /// This relocation is only meaningful when the machine type is LoongArch 32-bit. The base relocation applies to a 32-bit absolute address formed in two consecutive instructions.
        /// </summary>
        IMAGE_REL_BASED_LOONGARCH32_MARK_LA = 8,

        /// <summary>
        /// This relocation is only meaningful when the machine type is LoongArch 64-bit. The base relocation applies to a 64-bit absolute address formed in four consecutive instructions.
        /// </summary>
        IMAGE_REL_BASED_LOONGARCH64_MARK_LA = 8,

        /// <summary>
        /// The relocation is only meaningful when the machine type is MIPS. The base relocation applies to a MIPS16 jump instruction.
        /// </summary>
        IMAGE_REL_BASED_MIPS_JMPADDR16 = 9,

        /// <summary>
        /// The base relocation applies the difference to the 64-bit field at offset.
        /// </summary>
        IMAGE_REL_BASED_DIR64 = 10
    }

}