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

using System.Threading;

namespace LibPENUT
{
    #region I386 relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the I386 architecture
    /// </summary>
    public enum COFFRelocationType_I386 : ushort
    {
        /// <summary>
        /// This relocation is ignored
        /// </summary>
        IMAGE_REL_I386_ABSOLUTE = 0x0000,

        /// <summary>
        /// Not supported
        /// </summary>
        IMAGE_REL_I386_DIR16 = 0x0001,

        /// <summary>
        /// Not supported.
        /// </summary>
        IMAGE_REL_I386_REL16 = 0x0002,

        /// <summary>
        /// The target’s 32-bit virtual address
        /// </summary>
        IMAGE_REL_I386_DIR32 = 0x0006,

        /// <summary>
        /// The target’s 32-bit relative virtual address
        /// </summary>
        IMAGE_REL_I386_DIR32NB = 0x0007,

        /// <summary>
        /// Not supported.
        /// </summary>
        IMAGE_REL_I386_SEG12 = 0x0009,

        /// <summary>
        /// The 16-bit-section index of the section containing the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_I386_SECTION = 0x000A,

        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section. This is used to support debugging information as well as static thread local storage.
        /// </summary>
        IMAGE_REL_I386_SECREL = 0x000B,

        /// <summary>
        /// Clr token
        /// </summary>
        IMAGE_REL_I386_TOKEN = 0x000C,

        /// <summary>
        /// 7 bit offset from base of section containing target
        /// </summary>
        IMAGE_REL_I386_SECREL7 = 0x000D,

        /// <summary>
        /// PC-relative 32-bit reference to the symbols virtual address
        /// </summary>
        IMAGE_REL_I386_REL32 = 0x0014,
    }
    #endregion

    #region MIPS relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the MIPS architecture
    /// </summary>
    public enum COFFRelocationType_MIPS : ushort
    {
        /// <summary>
        /// Reference is absolute, no relocation is necessary
        /// </summary>
        IMAGE_REL_MIPS_ABSOLUTE = 0x0000,
        
        /// <summary>
        /// The high 16 bits of the target’s 32-bit virtual address.
        /// </summary>
        IMAGE_REL_MIPS_REFHALF = 0x0001,
        
        /// <summary>
        /// The target’s 32-bit virtual address.
        /// </summary>
        IMAGE_REL_MIPS_REFWORD = 0x0002,
        
        /// <summary>
        /// The low 26 bits of the target’s virtual address. This supports the MIPS J and JAL instructions.
        /// </summary>
        IMAGE_REL_MIPS_JMPADDR = 0x0003,
        
        /// <summary>
        /// The high 16 bits of the target’s 32-bit virtual address. Used for the first instruction in a twoinstruction sequence that loads a full address.
        /// This relocation must be immediately followed by a PAIR relocations whose SymbolTableIndex contains a signed 16-bit displacement which is
        /// added to the upper 16 bits taken from the location being relocated.
        /// </summary>
        IMAGE_REL_MIPS_REFHI = 0x0004,
        
        /// <summary>
        /// The low 16 bits of the target’s virtual address.
        /// </summary>
        IMAGE_REL_MIPS_REFLO = 0x0005,
        
        /// <summary>
        /// 16-bit signed displacement of the target relative to the Global Pointer (GP) register.
        /// </summary>
        IMAGE_REL_MIPS_GPREL = 0x0006,
        
        /// <summary>
        /// 16-bit signed displacement of the target relative to the Global Pointer (GP) register.
        /// </summary>
        IMAGE_REL_MIPS_LITERAL = 0x0007,
        
        /// <summary>
        /// The 16-bit section index of the section containing the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_MIPS_SECTION = 0x000A,
        
        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section. This is used to support debugging information as well as static thread local storage.
        /// </summary>
        IMAGE_REL_MIPS_SECREL = 0x000B,
        
        /// <summary>
        /// The low 16 bits of the 32-bit offset of the target from the beginning of its section.used for >32k TLS
        /// </summary>
        IMAGE_REL_MIPS_SECRELLO = 0x000C,
        
        /// <summary>
        /// The high 16 bits of the 32-bit offset of the target from the beginning of its section. A PAIR relocation must immediately follow this on.
        /// The SymbolTableIndex of the PAIR relocation contains a signed 16-bit displacement, which is added to the upper 16 bits taken from the         /// location being relocated.
        /// Used for >32k TLS
        /// </summary>
        IMAGE_REL_MIPS_SECRELHI = 0x000D,
        
        /// <summary>
        /// clr token
        /// </summary>
        IMAGE_REL_MIPS_TOKEN = 0x000E,
        
        /// <summary>
        /// The low 26 bits of the target’s virtual address. This supports the MIPS16 JAL instruction.
        /// </summary>
        IMAGE_REL_MIPS_JMPADDR16 = 0x0010,
        
        /// <summary>
        /// The target’s 32-bit relative virtual address.
        /// </summary>
        IMAGE_REL_MIPS_REFWORDNB = 0x0022,
        
        /// <summary>
        /// This relocation is only valid when it immediately follows a REFHI or SECRELHI relocation.
        /// It's SymbolTableIndex contains a displacement and not an index into the symbol table.
        /// </summary>
        IMAGE_REL_MIPS_PAIR = 0x0025
    }
    #endregion

    #region Alpha relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the Alpha architecture
    /// </summary>
    public enum COFFRelocationType_Alpha : ushort
    {
        /// <summary>
        /// This relocation is ignored.
        /// </summary>
        IMAGE_REL_ALPHA_ABSOLUTE = 0x0000,
        
        /// <summary>
        /// The target’s 32-bit virtual address.
        /// This fixup is illegal in a PE32+ image unless the image has been sandboxed by clearing the IMAGE_FILE_LARGE_ADDRESS_AWARE bit in the File Header.
        /// </summary>
        IMAGE_REL_ALPHA_REFLONG = 0x0001,
        
        /// <summary>
        /// The target’s 64-bit virtual address.
        /// </summary>
        IMAGE_REL_ALPHA_REFQUAD = 0x0002,
        
        /// <summary>
        /// 32-bit signed displacement of the target relative to the Global Pointer (GP) register.
        /// </summary>
        IMAGE_REL_ALPHA_GPREL32 = 0x0003,

        /// <summary>
        /// 16-bit signed displacement of the target relative to the Global Pointer (GP) register.
        /// </summary>
        IMAGE_REL_ALPHA_LITERAL = 0x0004,
        
        /// <summary>
        /// Unused
        /// </summary>
        IMAGE_REL_ALPHA_LITUSE = 0x0005,

        /// <summary>
        /// Unused
        /// </summary>
        IMAGE_REL_ALPHA_GPDISP = 0x0006,

        /// <summary>
        /// The 21-bit relative displacement to the target. This supports the Alpha relative branch instructions.
        /// </summary>
        IMAGE_REL_ALPHA_BRADDR = 0x0007,

        /// <summary>
        /// 14-bit hints to the processor for the target of an Alpha jump instruction.
        /// </summary>
        IMAGE_REL_ALPHA_HINT = 0x0008,

        /// <summary>
        /// The target’s 32-bit virtual address split into high and low 16-bit parts. Either an ABSOLUTE or MATCH relocation must immediately follow this relocation.
        /// The high 16 bits of the target address are stored in the location identified by the INLINE_REFLONG relocation.
        /// The low 16 bits are stored four bytes later if the following relocation is of type ABSOLUTE or at a signed displacement
        /// given in the SymbolTableIndex if the following relocation is of type MATCH.
        /// </summary>
        IMAGE_REL_ALPHA_INLINE_REFLONG = 0x0009,

        /// <summary>
        /// The high 16 bits of the target’s 32-bit virtual address. Used for the first instruction in a twoinstruction sequence that loads a full address.
        /// This relocation must be immediately followed by a PAIR relocations whose SymbolTableIndex contains a signed 16-bit displacement which is 
        /// added to the upper 16 bits taken from the location being relocated.
        /// </summary>
        IMAGE_REL_ALPHA_REFHI = 0x000A,

        /// <summary>
        /// The low 16 bits of the target’s virtual address.
        /// </summary>
        IMAGE_REL_ALPHA_REFLO = 0x000B,

        /// <summary>
        /// This relocation is only valid when it immediately follows a REFHI, REFQ3, REFQ2, or SECRELHI relocation.
        /// Its SymbolTableIndex contains a displacement and not an index into the symbol table.
        /// </summary>
        IMAGE_REL_ALPHA_PAIR = 0x000C,

        /// <summary>
        /// This relocation is only valid when it immediately follows INLINE_REFLONG relocation.
        /// Its SymbolTableIndex contains the displacement in bytes of the location for the matching low address and not an index into the symbol table.
        /// </summary>
        IMAGE_REL_ALPHA_MATCH = 0x000D,

        /// <summary>
        /// The 16-bit section index of the section containing the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_ALPHA_SECTION = 0x000E,

        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section. This is used to support debugging information as well as static thread local storage.
        /// </summary>
        IMAGE_REL_ALPHA_SECREL = 0x000F,

        /// <summary>
        /// The target’s 32-bit relative virtual address.
        /// </summary>
        IMAGE_REL_ALPHA_REFLONGNB = 0x0010,

        /// <summary>
        /// The low 16 bits of the 32-bit offset of the target from the beginning of its section.
        /// </summary>
        IMAGE_REL_ALPHA_SECRELLO = 0x0011,

        /// <summary>
        /// The high 16 bits of the 32-bit offset of the target from the beginning of its section. A PAIR relocation must immediately follow this on.
        /// The SymbolTableIndex of the PAIR relocation contains a signed 16-bit displacement which is added to the upper 16 bits taken from the location being relocated.
        /// </summary>
        IMAGE_REL_ALPHA_SECRELHI = 0x0012,
        
        /// <summary>
        /// The low 16 bits of the high 32 bits of the target’s 64-bit virtual address. This relocation must be immediately followed by a PAIR relocations whose
        /// SymbolTableIndex contains a signed 32-bit displacement which is added to the 16 bits taken from the location being relocated.
        /// The 16 bits in the relocated location are shifted left by 32 before this addition.
        /// </summary>
        IMAGE_REL_ALPHA_REFQ3 = 0x0013,
        
        /// <summary>
        /// The high 16 bits of the low 32 bits of the target’s 64-bit virtual address. This relocation must be immediately followed by a PAIR relocations whose
        /// SymbolTableIndex contains a signed 16-bit displacement which is added to the upper 16 bits taken from the location being relocated.
        /// </summary>
        IMAGE_REL_ALPHA_REFQ2 = 0x0014,

        /// <summary>
        /// The low 16 bits of the target’s 64-bit virtual address.
        /// </summary>
        IMAGE_REL_ALPHA_REFQ1 = 0x0015,

        /// <summary>
        /// The low 16 bits of the 32-bit signed displacement of the target relative to the Global Pointer (GP) register.
        /// </summary>
        IMAGE_REL_ALPHA_GPRELLO = 0x0016,

        /// <summary>
        /// The high 16 bits of the 32-bit signed displacement of the target relative to the Global Pointer (GP) register.
        /// </summary>
        IMAGE_REL_ALPHA_GPRELHI = 0x0017
    }
    #endregion

    #region PowerPC relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the IBM PowerPC architecture
    /// </summary>
    public enum COFFRelocationType_PPC : ushort
    {
        /// <summary>
        /// This relocation is ignored.
        /// </summary>
        IMAGE_REL_PPC_ABSOLUTE = 0x0000,

        /// <summary>
        /// The target’s 64-bit virtual address.
        /// </summary>
        IMAGE_REL_PPC_ADDR64 = 0x0001,

        /// <summary>
        /// The target’s 32-bit virtual address.
        /// </summary>
        IMAGE_REL_PPC_ADDR32 = 0x0002,

        /// <summary>
        /// The low 24 bits of the target’s virtual address. This is only valid when the target symbol is absolute and can be sign extended to its original value.
        /// </summary>
        IMAGE_REL_PPC_ADDR24 = 0x0003,

        /// <summary>
        /// The low 16 bits of the target’s virtual address.
        /// </summary>
        IMAGE_REL_PPC_ADDR16 = 0x0004,

        /// <summary>
        /// The low 14 bits of the target’s virtual address. This is only valid when the target symbol is absolute and can be sign extended to its original value.
        /// </summary>
        IMAGE_REL_PPC_ADDR14 = 0x0005,

        /// <summary>
        /// A 24-bit PC-relative offset to the symbol’s location.
        /// </summary>
        IMAGE_REL_PPC_REL24 = 0x0006,

        /// <summary>
        /// A 14-bit PC-relative offset to the symbol’s location.
        /// </summary>
        IMAGE_REL_PPC_REL14 = 0x0007,

        /// <summary>
        /// 16-bit offset from TOC base
        /// </summary>
        IMAGE_REL_PPC_TOCREL16 = 0x0008,

        /// <summary>
        /// 16-bit offset from TOC base, shifted left 2 (load doubleword)
        /// </summary>
        IMAGE_REL_PPC_TOCREL14 = 0x0009,

        /// <summary>
        /// The target’s 32-bit relative virtual address.
        /// </summary>
        IMAGE_REL_PPC_ADDR32NB = 0x000A,

        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section. This is used to support debugging information as well as static thread local storage.        /// </summary>
        IMAGE_REL_PPC_SECREL = 0x000B,  // va of containing section (as in an image sectionhdr)

        /// <summary>
        /// The 16-bit section index of the section containing the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_PPC_SECTION = 0x000C,  // sectionheader number

        /// <summary>
        /// substitute TOC restore instruction iff symbol is glue code
        /// </summary>
        IMAGE_REL_PPC_IFGLUE = 0x000D,

        /// <summary>
        /// Symbol is glue code; virtual address is TOC restore instruction
        /// </summary>
        IMAGE_REL_PPC_IMGLUE = 0x000E,

        /// <summary>
        /// The 16-bit offset of the target from the beginning of its section. This is used to support debugging information as well as static thread local storage.
        /// </summary>
        IMAGE_REL_PPC_SECREL16 = 0x000F,

        /// <summary>
        /// The high 16 bits of the target’s 32-bit virtual address. Used for the first instruction in a two-instruction sequence that loads a full address.
        /// This relocation must be immediately followed by a PAIR relocations whose SymbolTableIndex contains a signed 16-bit displacement which is added to
        /// the upper 16 bits taken from the location being relocated.
        /// </summary>
        IMAGE_REL_PPC_REFHI = 0x0010,

        /// <summary>
        /// The low 16 bits of the target’s virtual address.
        /// </summary>
        IMAGE_REL_PPC_REFLO = 0x0011,

        /// <summary>
        /// This relocation is only valid when it immediately follows a REFHI or SECRELHI relocation. Its SymbolTableIndex contains a displacement and not an index into the symbol table.
        /// </summary>
        IMAGE_REL_PPC_PAIR = 0x0012,

        /// <summary>
        /// The low 16 bits of the 32-bit offset of the target from the beginning of its section.
        /// </summary>
        IMAGE_REL_PPC_SECRELLO = 0x0013,  // Low 16-bit section relative reference (used for >32k TLS)

        /// <summary>
        /// The high 16 bits of the 32-bit offset of the target from the beginning of its section. A PAIR relocation must immediately follow this on.
        /// The SymbolTableIndex of the PAIR relocation contains a signed 16-bit displacement which is added to the upper 16 bits taken from the location being relocated.
        /// </summary>
        IMAGE_REL_PPC_SECRELHI = 0x0014,  // High 16-bit section relative reference (used for >32k TLS)

        /// <summary>
        /// 16-bit signed displacement of the target relative to the Global Pointer (GP) register.
        /// </summary>
        IMAGE_REL_PPC_GPREL = 0x0015,

        /// <summary>
        /// CLR token
        /// </summary>
        IMAGE_REL_PPC_TOKEN = 0x0016,
    }
    #endregion

    #region Hitachi SH3/4 relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the Hitachi SH3/4 architecture
    /// </summary>
    public enum COFFRelocationType_SHx : ushort
    {
        /// <summary>
        /// No relocation
        /// </summary>
        IMAGE_REL_SH3_ABSOLUTE = 0x0000,

        /// <summary>
        /// Reference to the 16-bit location that contains  the virtual address of the target symbol.
        /// </summary>
        IMAGE_REL_SH3_DIRECT16 = 0x0001,

        /// <summary>
        /// The target’s 32-bit virtual address.
        /// </summary>
        IMAGE_REL_SH3_DIRECT32 = 0x0002,

        /// <summary>
        /// Reference to the 8-bit location that contains the virtual address of the target symbol.
        /// </summary>
        IMAGE_REL_SH3_DIRECT8 = 0x0003,

        /// <summary>
        /// Reference to the 8-bit instruction that contains the effective 16-bit virtual address of the target symbol.
        /// </summary>
        IMAGE_REL_SH3_DIRECT8_WORD = 0x0004,

        /// <summary>
        /// Reference to the 8-bit instruction that contains the effective 32-bit virtual address of the target symbol.
        /// </summary>
        IMAGE_REL_SH3_DIRECT8_LONG = 0x0005,

        /// <summary>
        /// Reference to the 8-bit location whose low 4 bits contain the virtual address of the target symbol.
        /// </summary>
        IMAGE_REL_SH3_DIRECT4 = 0x0006,

        /// <summary>
        /// Reference to the 8-bit instruction whose low 4 bits contain the effective 16-bit virtual address of the target symbol.
        /// </summary>
        IMAGE_REL_SH3_DIRECT4_WORD = 0x0007,

        /// <summary>
        /// Reference to the 8-bit instruction whose low 4 bits contain the effective 32-bit virtual address of the target symbol.
        /// </summary>
        IMAGE_REL_SH3_DIRECT4_LONG = 0x0008,

        /// <summary>
        /// Reference to the 8-bit instruction which contains the effective 16-bit relative offset of the target symbol.
        /// </summary>
        IMAGE_REL_SH3_PCREL8_WORD = 0x0009,

        /// <summary>
        /// Reference to the 8-bit instruction which contains the effective 32-bit relative offset of the target symbol
        /// </summary>
        IMAGE_REL_SH3_PCREL8_LONG = 0x000A,

        /// <summary>
        /// Reference to the 16-bit instruction whose low 12 bits contain the effective 16-bit relative offset of the target symbol.
        /// </summary>
        IMAGE_REL_SH3_PCREL12_WORD = 0x000B,

        /// <summary>
        /// Reference to a 32-bit location that is the virtual address of the symbol's section.
        /// </summary>
        IMAGE_REL_SH3_STARTOF_SECTION = 0x000C,

        /// <summary>
        /// Reference to the 32-bit location that is the size of the symbol’s section.
        /// </summary>
        IMAGE_REL_SH3_SIZEOF_SECTION = 0x000D,

        /// <summary>
        /// The 16-bit section index of the section containing the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_SH3_SECTION = 0x000E,

        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section. This is used to support debugging information as well as static thread local storage.
        /// </summary>
        IMAGE_REL_SH3_SECREL = 0x000F,

        /// <summary>
        /// The target’s 32-bit relative virtual address.
        /// </summary>
        IMAGE_REL_SH3_DIRECT32_NB = 0x0010,

        /// <summary>
        /// GP-relative addressing
        /// </summary>
        IMAGE_REL_SH3_GPREL4_LONG = 0x0011,

        /// <summary>
        /// CLR Token
        /// </summary>
        IMAGE_REL_SH3_TOKEN = 0x0012,

        /// <summary>
        /// Offset from current instruction in longwords if not NOMODE, insert the inverse of the low bit at bit 32 to select PTA/PTB
        /// </summary>
        IMAGE_REL_SHM_PCRELPT = 0x0013,

        /// <summary>
        /// Low bits of 32-bit address
        /// </summary>
        IMAGE_REL_SHM_REFLO = 0x0014,

        /// <summary>
        /// High bits of 32-bit address
        /// </summary>
        IMAGE_REL_SHM_REFHALF = 0x0015,

        /// <summary>
        /// Low bits of relative reference
        /// </summary>
        IMAGE_REL_SHM_RELLO = 0x0016,

        /// <summary>
        /// High bits of relative reference
        /// </summary>
        IMAGE_REL_SHM_RELHALF = 0x0017,

        /// <summary>
        /// offset operand for relocation
        /// </summary>
        IMAGE_REL_SHM_PAIR = 0x0018,

        /// <summary>
        /// relocation ignores section mode
        /// </summary>
        IMAGE_REL_SH_NOMODE = 0x8000
    }
    #endregion

    #region ARM relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the ARM architecture
    /// </summary>
    public enum COFFRelocationType_ARM : ushort
    {

        /// <summary>
        /// This relocation is ignored.
        /// </summary>
        IMAGE_REL_ARM_ABSOLUTE = 0x0000,

        /// <summary>
        /// The target’s 32-bit virtual address.
        /// </summary>
        IMAGE_REL_ARM_ADDR32 = 0x0001,

        /// <summary>
        /// The target’s 32-bit relative virtual address.
        /// </summary>
        IMAGE_REL_ARM_ADDR32NB = 0x0002,

        /// <summary>
        /// The 24-bit relative displacement to the target.
        /// </summary>
        IMAGE_REL_ARM_BRANCH24 = 0x0003,

        /// <summary>
        /// Reference to a subroutine call, consisting of two 16-bit instructions with 11-bit offsets.
        /// </summary>
        IMAGE_REL_ARM_BRANCH11 = 0x0004,

        /// <summary>
        /// CLR token
        /// </summary>
        IMAGE_REL_ARM_TOKEN = 0x0005,

        /// <summary>
        /// GP-relative addressing (ARM)
        /// </summary>
        IMAGE_REL_ARM_GPREL12 = 0x0006,

        /// <summary>
        /// GP-relative addressing (Thumb)
        /// </summary>
        IMAGE_REL_ARM_GPREL7 = 0x0007,

        /// <summary>
        /// 
        /// </summary>
        IMAGE_REL_ARM_BLX24 = 0x0008,

        /// <summary>
        /// 
        /// </summary>
        IMAGE_REL_ARM_BLX11 = 0x0009,

        /// <summary>
        /// The 16-bit section index of the section containing the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_ARM_SECTION = 0x000E,

        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section. This is used to support debugging information as well as static threadlocal storage.
        /// </summary>
        IMAGE_REL_ARM_SECREL = 0x000F,  // Offset within section

        /// <summary>
        /// ARM: MOVW/MOVT
        /// </summary>
        IMAGE_REL_ARM_MOV32A = 0x0010,

        /// <summary>
        /// Thumb: MOVW/MOVT
        /// </summary>
        IMAGE_REL_ARM_MOV32T = 0x0011,

        /// <summary>
        /// Thumb: 32-bit conditional B
        /// </summary>
        IMAGE_REL_ARM_BRANCH20T = 0x0012,

        /// <summary>
        /// Thumb: 32-bit B or BL
        /// </summary>
        IMAGE_REL_ARM_BRANCH24T = 0x0014,

        /// <summary>
        /// Thumb: BLX immediate
        /// </summary>
        IMAGE_REL_ARM_BLX23T = 0x0015

    }
    #endregion

    #region ARM64 relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the ARM64 architecture
    /// </summary>
    public enum COFFRelocationType_ARM64 : ushort
    {

        /// <summary>
        /// This relocation is ignored.
        /// </summary>
        IMAGE_REL_ARM64_ABSOLUTE = 0x0000,

        /// <summary>
        ///  32 bit address.
        /// </summary>
        IMAGE_REL_ARM64_ADDR32 = 0x0001,

        /// <summary>
        /// 32 bit address w/o image base (RVA: for Data/PData/XData)
        /// </summary>
        IMAGE_REL_ARM64_ADDR32NB = 0x0002,

        /// <summary>
        /// 26 bit offset << 2 & sign ext. for B & BL
        /// </summary>
        IMAGE_REL_ARM64_BRANCH26 = 0x0003,

        /// <summary>
        /// ADRP
        /// </summary>
        IMAGE_REL_ARM64_PAGEBASE_REL21 = 0x0004,

        /// <summary>
        /// ADR
        /// </summary>
        IMAGE_REL_ARM64_REL21 = 0x0005,

        /// <summary>
        /// ADD/ADDS (immediate) with zero shift, for page offset
        /// </summary>
        IMAGE_REL_ARM64_PAGEOFFSET_12A = 0x0006,

        /// <summary>
        /// LDR (indexed, unsigned immediate), for page offset
        /// </summary>
        IMAGE_REL_ARM64_PAGEOFFSET_12L = 0x0007,

        /// <summary>
        /// Offset within section
        /// </summary>
        IMAGE_REL_ARM64_SECREL = 0x0008,

        /// <summary>
        /// ADD/ADDS (immediate) with zero shift, for bit 0:11 of section offset
        /// </summary>
        IMAGE_REL_ARM64_SECREL_LOW12A = 0x0009,

        /// <summary>
        /// ADD/ADDS (immediate) with zero shift, for bit 12:23 of section offset
        /// </summary>
        IMAGE_REL_ARM64_SECREL_HIGH12A = 0x000A,

        /// <summary>
        /// LDR (indexed, unsigned immediate), for bit 0:11 of section offset
        /// </summary>
        IMAGE_REL_ARM64_SECREL_LOW12L = 0x000B,

        /// <summary>
        /// CLR Token
        /// </summary>
        IMAGE_REL_ARM64_TOKEN = 0x000C,

        /// <summary>
        /// Section table index
        /// </summary>
        IMAGE_REL_ARM64_SECTION = 0x000D,

        /// <summary>
        /// 64 bit address
        /// </summary>
        IMAGE_REL_ARM64_ADDR64 = 0x000E,

        /// <summary>
        /// 19 bit offset << 2 & sign ext. for conditional B
        /// </summary>
        IMAGE_REL_ARM64_BRANCH19 = 0x000F
    }
    #endregion

    #region AMD64 (x64) relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the AMD64 (x64) architecture
    /// </summary>
    public enum COFFRelocationType_AMD64 : ushort
    {
        /// <summary>
        /// Reference is absolute, no relocation is necessary
        /// </summary>
        IMAGE_REL_AMD64_ABSOLUTE = 0x0000,

        /// <summary>
        /// 64-bit address (VA).
        /// </summary>
        IMAGE_REL_AMD64_ADDR64 = 0x0001,

        /// <summary>
        /// 32-bit address (VA).
        /// </summary>
        IMAGE_REL_AMD64_ADDR32 = 0x0002,

        /// <summary>
        /// 32-bit address w/o image base (RVA).
        /// </summary>
        IMAGE_REL_AMD64_ADDR32NB = 0x0003,

        /// <summary>
        /// 32-bit relative address from byte following reloc
        /// </summary>
        IMAGE_REL_AMD64_REL32 = 0x0004,

        /// <summary>
        /// 32-bit relative address from byte distance 1 from reloc
        /// </summary>
        IMAGE_REL_AMD64_REL32_1 = 0x0005,

        /// <summary>
        /// 32-bit relative address from byte distance 2 from reloc
        /// </summary>
        IMAGE_REL_AMD64_REL32_2 = 0x0006,

        /// <summary>
        /// 32-bit relative address from byte distance 3 from reloc
        /// </summary>
        IMAGE_REL_AMD64_REL32_3 = 0x0007,

        /// <summary>
        /// 32-bit relative address from byte distance 4 from reloc
        /// </summary>
        IMAGE_REL_AMD64_REL32_4 = 0x0008,

        /// <summary>
        /// 32-bit relative address from byte distance 5 from reloc
        /// </summary>
        IMAGE_REL_AMD64_REL32_5 = 0x0009,

        /// <summary>
        /// Section index
        /// </summary>
        IMAGE_REL_AMD64_SECTION = 0x000A,

        /// <summary>
        /// 32 bit offset from base of section containing target
        /// </summary>
        IMAGE_REL_AMD64_SECREL = 0x000B,

        /// <summary>
        /// 7 bit unsigned offset from base of section containing target
        /// </summary>
        IMAGE_REL_AMD64_SECREL7 = 0x000C,

        /// <summary>
        /// 32 bit metadata token
        /// </summary>
        IMAGE_REL_AMD64_TOKEN = 0x000D,

        /// <summary>
        /// 32 bit signed span-dependent value emitted into object
        /// </summary>
        IMAGE_REL_AMD64_SREL32 = 0x000E,

        /// <summary>
        /// 
        /// </summary>
        IMAGE_REL_AMD64_PAIR = 0x000F,

        /// <summary>
        /// 32 bit signed span-dependent value applied at link time
        /// </summary>
        IMAGE_REL_AMD64_SSPAN32 = 0x0010,

        /// <summary>
        /// 
        /// </summary>
        IMAGE_REL_AMD64_EHANDLER = 0x0011,

        /// <summary>
        /// Indirect branch to an import
        /// </summary>
        IMAGE_REL_AMD64_IMPORT_BR = 0x0012,

        /// <summary>
        /// Indirect call to an import
        /// </summary>
        IMAGE_REL_AMD64_IMPORT_CALL = 0x0013,

        /// <summary>
        /// Indirect branch to a CFG check
        /// </summary>
        IMAGE_REL_AMD64_CFG_BR = 0x0014,

        /// <summary>
        /// Indirect branch to a CFG check, with REX.W prefix
        /// </summary>
        IMAGE_REL_AMD64_CFG_BR_REX = 0x0015,

        /// <summary>
        /// Indirect call to a CFG check
        /// </summary>
        IMAGE_REL_AMD64_CFG_CALL = 0x0016,

        /// <summary>
        /// Indirect branch to a target in RAX (no CFG)
        /// </summary>
        IMAGE_REL_AMD64_INDIR_BR = 0x0017,

        /// <summary>
        /// Indirect branch to a target in RAX, with REX.W prefix (no CFG)
        /// </summary>
        IMAGE_REL_AMD64_INDIR_BR_REX = 0x0018,

        /// <summary>
        /// Indirect call to a target in RAX (no CFG)
        /// </summary>
        IMAGE_REL_AMD64_INDIR_CALL = 0x0019,

        /// <summary>
        /// Indirect branch for a switch table using Reg 0 (RAX)
        /// </summary>
        IMAGE_REL_AMD64_INDIR_BR_SWITCHTABLE_FIRST = 0x0020,

        /// <summary>
        /// Indirect branch for a switch table using Reg 15 (R15)
        /// </summary>
        IMAGE_REL_AMD64_INDIR_BR_SWITCHTABLE_LAST = 0x002F
    }

    #endregion

    // TODO:
    // public enum COFFRelocationType_IA64 : ushort
    // public enum COFFRelocationType_M32R : ushort
}