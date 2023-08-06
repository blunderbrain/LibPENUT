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
        /// The 32-bit VA of the target.
        /// </summary>
        IMAGE_REL_ARM64_ADDR32 = 0x0001,

        /// <summary>
        /// 32 bit address w/o image base (RVA: for Data/PData/XData)
        /// </summary>
        IMAGE_REL_ARM64_ADDR32NB = 0x0002,

        /// <summary>
        /// The 26-bit relative displacement to the target, for B and BL instructions.
        /// </summary>
        IMAGE_REL_ARM64_BRANCH26 = 0x0003,

        /// <summary>
        /// The page base of the target, for ADRP instruction.
        /// </summary>
        IMAGE_REL_ARM64_PAGEBASE_REL21 = 0x0004,

        /// <summary>
        /// The 12-bit relative displacement to the target, for instruction ADR
        /// </summary>
        IMAGE_REL_ARM64_REL21 = 0x0005,

        /// <summary>
        /// The 12-bit page offset of the target, for instructions ADD/ADDS (immediate) with zero shift.
        /// </summary>
        IMAGE_REL_ARM64_PAGEOFFSET_12A = 0x0006,

        /// <summary>
        /// The 12-bit page offset of the target, for instruction LDR (indexed, unsigned immediate).
        /// </summary>
        IMAGE_REL_ARM64_PAGEOFFSET_12L = 0x0007,

        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section. This is used to support debugging information and static thread local storage.
        /// </summary>
        IMAGE_REL_ARM64_SECREL = 0x0008,

        /// <summary>
        /// Bit 0:11 of section offset of the target, for instructions ADD/ADDS (immediate) with zero shift.
        /// </summary>
        IMAGE_REL_ARM64_SECREL_LOW12A = 0x0009,

        /// <summary>
        /// ABit 12:23 of section offset of the target, for instructions ADD/ADDS (immediate) with zero shift.
        /// </summary>
        IMAGE_REL_ARM64_SECREL_HIGH12A = 0x000A,

        /// <summary>
        /// Bit 0:11 of section offset of the target, for instruction LDR (indexed, unsigned immediate).
        /// </summary>
        IMAGE_REL_ARM64_SECREL_LOW12L = 0x000B,

        /// <summary>
        /// CLR Token
        /// </summary>
        IMAGE_REL_ARM64_TOKEN = 0x000C,

        /// <summary>
        /// The 16-bit section index of the section that contains the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_ARM64_SECTION = 0x000D,

        /// <summary>
        /// The 64-bit VA of the relocation target.
        /// </summary>
        IMAGE_REL_ARM64_ADDR64 = 0x000E,

        /// <summary>
        /// The 19-bit offset to the relocation target, for conditional B instruction.
        /// </summary>
        IMAGE_REL_ARM64_BRANCH19 = 0x000F,

        /// <summary>
        /// The 14-bit offset to the relocation target, for instructions TBZ and TBNZ.
        /// </summary>
        IMAGE_REL_ARM64_BRANCH14 = 0x0010,

        /// <summary>
        /// The 32-bit relative address from the byte following the relocation.
        /// </summary>
        IMAGE_REL_ARM64_REL32 = 0x0011
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
        /// The 64-bit VA of the relocation target.
        /// </summary>
        IMAGE_REL_AMD64_ADDR64 = 0x0001,

        /// <summary>
        /// The 32-bit VA of the relocation target.
        /// </summary>
        IMAGE_REL_AMD64_ADDR32 = 0x0002,

        /// <summary>
        /// The 32-bit address without an image base (RVA).
        /// </summary>
        IMAGE_REL_AMD64_ADDR32NB = 0x0003,

        /// <summary>
        /// The 32-bit relative address from the byte following the relocation.
        /// </summary>
        IMAGE_REL_AMD64_REL32 = 0x0004,

        /// <summary>
        /// The 32-bit address relative to byte distance 1 from the relocation.
        /// </summary>
        IMAGE_REL_AMD64_REL32_1 = 0x0005,

        /// <summary>
        /// The 32-bit address relative to byte distance 2 from the relocation.
        /// </summary>
        IMAGE_REL_AMD64_REL32_2 = 0x0006,

        /// <summary>
        /// The 32-bit address relative to byte distance 3 from the relocation.
        /// </summary>
        IMAGE_REL_AMD64_REL32_3 = 0x0007,

        /// <summary>
        /// The 32-bit address relative to byte distance 4 from the relocation.
        /// </summary>
        IMAGE_REL_AMD64_REL32_4 = 0x0008,

        /// <summary>
        /// The 32-bit address relative to byte distance 5 from the relocation.
        /// </summary>
        IMAGE_REL_AMD64_REL32_5 = 0x0009,

        /// <summary>
        /// The 16-bit section index of the section that contains the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_AMD64_SECTION = 0x000A,

        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section. This is used to support debugging information and static thread local storage.
        /// </summary>
        IMAGE_REL_AMD64_SECREL = 0x000B,

        /// <summary>
        /// A 7-bit unsigned offset from the base of the section that contains the target.
        /// </summary>
        IMAGE_REL_AMD64_SECREL7 = 0x000C,

        /// <summary>
        /// CLR token
        /// </summary>
        IMAGE_REL_AMD64_TOKEN = 0x000D,

        /// <summary>
        /// A 32-bit signed span-dependent value emitted into the object.
        /// </summary>
        IMAGE_REL_AMD64_SREL32 = 0x000E,

        /// <summary>
        /// A pair that must immediately follow every span-dependent value.
        /// </summary>
        IMAGE_REL_AMD64_PAIR = 0x000F,

        /// <summary>
        /// A 32-bit signed span-dependent value that is applied at link time.
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

    #region Intel IA64 relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the Intel IA64 architecture
    /// </summary>
    public enum COFFRelocationType_IA64 : ushort
    {
        /// <summary>
        /// Reference is absolute, no relocation is necessary
        /// </summary>
        IMAGE_REL_IA64_ABSOLUTE = 0x0000,

        /// <summary>
        /// The instruction relocation can be followed by an ADDEND relocation whose value is added to the target address before it is inserted into the specified slot in the IMM14 bundle. The relocation target must be absolute or the image must be fixed.
        /// </summary>
        IMAGE_REL_IA64_IMM14 = 0x0001,

        /// <summary>
        /// The instruction relocation can be followed by an ADDEND relocation whose value is added to the target address before it is inserted into the specified slot in the IMM22 bundle. The relocation target must be absolute or the image must be fixed.
        /// </summary>
        IMAGE_REL_IA64_IMM22 = 0x0002,

        /// <summary>
        /// The slot number of this relocation must be one (1). The relocation can be followed by an ADDEND relocation whose value is added to the target address before it is stored in all three slots of the IMM64 bundle.
        /// </summary>
        IMAGE_REL_IA64_IMM64 = 0x0003,

        /// <summary>
        /// The target's 32-bit VA. This is supported only for /LARGEADDRESSAWARE:NO images.
        /// </summary>
        IMAGE_REL_IA64_DIR32 = 0x0004,

        /// <summary>
        /// The target's 64-bit VA.
        /// </summary>
        IMAGE_REL_IA64_DIR64 = 0x0005,

        /// <summary>
        /// The instruction is fixed up with the 25-bit relative displacement to the 16-bit aligned target. The low 4 bits of the displacement are zero and are not stored.
        /// </summary>
        IMAGE_REL_IA64_PCREL21B = 0x0006,

        /// <summary>
        /// The instruction is fixed up with the 25-bit relative displacement to the 16-bit aligned target. The low 4 bits of the displacement, which are zero, are not stored.
        /// </summary>
        IMAGE_REL_IA64_PCREL21M = 0x0007,

        /// <summary>
        /// The LSBs of this relocation's offset must contain the slot number whereas the rest is the bundle address. The bundle is fixed up with the 25-bit relative displacement to the 16-bit aligned target. The low 4 bits of the displacement are zero and are not stored.
        /// </summary>
        IMAGE_REL_IA64_PCREL21F = 0x0008,

        /// <summary>
        /// The instruction relocation can be followed by an ADDEND relocation whose value is added to the target address and then a 22-bit GP-relative offset that is calculated and applied to the GPREL22 bundle.
        /// </summary>
        IMAGE_REL_IA64_GPREL22 = 0x0009,

        /// <summary>
        /// The instruction is fixed up with the 22-bit GP-relative offset to the target symbol's literal table entry. The linker creates this literal table entry based on this relocation and the ADDEND relocation that might follow.
        /// </summary>
        IMAGE_REL_IA64_LTOFF22 = 0x000A,

        /// <summary>
        /// The 16-bit section index of the section contains the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_IA64_SECTION = 0x000B,

        /// <summary>
        /// The instruction is fixed up with the 22-bit offset of the target from the beginning of its section. This relocation can be followed immediately by an ADDEND relocation, whose Value field contains the 32-bit unsigned offset of the target from the beginning of the section.
        /// </summary>
        IMAGE_REL_IA64_SECREL22 = 0x000C,

        /// <summary>
        /// The slot number for this relocation must be one (1). The instruction is fixed up with the 64-bit offset of the target from the beginning of its section. This relocation can be followed immediately by an ADDEND relocation whose Value field contains the 32-bit unsigned offset of the target from the beginning of the section.
        /// </summary>
        IMAGE_REL_IA64_SECREL64I = 0x000D,

        /// <summary>
        /// The address of data to be fixed up with the 32-bit offset of the target from the beginning of its section.
        /// </summary>
        IMAGE_REL_IA64_SECREL32 = 0x000E,

        /// <summary>
        /// The target's 32-bit RVA.
        /// </summary>
        IMAGE_REL_IA64_DIR32NB = 0x0010,

        /// <summary>
        /// This is applied to a signed 14-bit immediate that contains the difference between two relocatable targets. This is a declarative field for the linker that indicates that the compiler has already emitted this value.
        /// </summary>
        IMAGE_REL_IA64_SREL14 = 0x0011,

        /// <summary>
        /// This is applied to a signed 22-bit immediate that contains the difference between two relocatable targets. This is a declarative field for the linker that indicates that the compiler has already emitted this value.
        /// </summary>
        IMAGE_REL_IA64_SREL22 = 0x0012,

        /// <summary>
        /// This is applied to a signed 32-bit immediate that contains the difference between two relocatable values. This is a declarative field for the linker that indicates that the compiler has already emitted this value.
        /// </summary>
        IMAGE_REL_IA64_SREL32 = 0x0013,

        /// <summary>
        /// This is applied to an unsigned 32-bit immediate that contains the difference between two relocatable values. This is a declarative field for the linker that indicates that the compiler has already emitted this value.
        /// </summary>
        IMAGE_REL_IA64_UREL32 = 0x0014,

        /// <summary>
        /// A 60-bit PC-relative fixup that always stays as a BRL instruction of an MLX bundle.
        /// </summary>
        IMAGE_REL_IA64_PCREL60X = 0x0015,

        /// <summary>
        /// A 60-bit PC-relative fixup. If the target displacement fits in a signed 25-bit field, convert the entire bundle to an MBB bundle with NOP.B in slot 1 and a 25-bit BR instruction (with the 4 lowest bits all zero and dropped) in slot 2.
        /// </summary>
        IMAGE_REL_IA64_PCREL60B = 0x0016,

        /// <summary>
        /// A 60-bit PC-relative fixup. If the target displacement fits in a signed 25-bit field, convert the entire bundle to an MFB bundle with NOP.F in slot 1 and a 25-bit (4 lowest bits all zero and dropped) BR instruction in slot 2.
        /// </summary>
        IMAGE_REL_IA64_PCREL60F = 0x0017,

        /// <summary>
        /// A 60-bit PC-relative fixup. If the target displacement fits in a signed 25-bit field, convert the entire bundle to an MIB bundle with NOP.I in slot 1 and a 25-bit (4 lowest bits all zero and dropped) BR instruction in slot 2.
        /// </summary>
        IMAGE_REL_IA64_PCREL60I = 0x0018,

        /// <summary>
        /// A 60-bit PC-relative fixup. If the target displacement fits in a signed 25-bit field, convert the entire bundle to an MMB bundle with NOP.M in slot 1 and a 25-bit (4 lowest bits all zero and dropped) BR instruction in slot 2.
        /// </summary>
        IMAGE_REL_IA64_PCREL60M = 0x0019,

        /// <summary>
        /// A 64-bit GP-relative fixup.
        /// </summary>
        IMAGE_REL_IA64_IMMGPREL64 = 0x001A,

        /// <summary>
        /// CLR token
        /// </summary>
        IMAGE_REL_IA64_TOKEN = 0x001B,

        /// <summary>
        /// A 32-bit GP-relative fixup.
        /// </summary>
        IMAGE_REL_IA64_GPREL32 = 0x001C,

        /// <summary>
        /// The relocation is valid only when it immediately follows one of the following relocations: IMM14, IMM22, IMM64, GPREL22, LTOFF22, LTOFF64, SECREL22, SECREL64I, or SECREL32. Its value contains the addend to apply to instructions within a bundle, not for data.
        /// </summary>
        IMAGE_REL_IA64_ADDEND = 0x001F
    }

    #endregion

    #region Mitsubishi M32R relocation types
    /// <summary>
    /// Constants describing the supported relocation types for the Mitsubishi M32R architecture
    /// </summary>
    public enum COFFRelocationType_M32R : ushort
    {
        /// <summary>
        /// The relocation is ignored.
        /// </summary>
        IMAGE_REL_M32R_ABSOLUTE = 0x0000,

        /// <summary>
        /// The target's 32-bit VA.
        /// </summary>
        IMAGE_REL_M32R_ADDR32 = 0x0001,

        /// <summary>
        /// The target's 32-bit RVA without image base
        /// </summary>
        IMAGE_REL_M32R_ADDR32NB = 0x0002,

        /// <summary>
        /// The target's 24-bit VA.
        /// </summary>
        IMAGE_REL_M32R_ADDR24 = 0x0003,

        /// <summary>
        /// The target's 16-bit offset from the GP register.
        /// </summary>
        IMAGE_REL_M32R_GPREL16 = 0x0004,

        /// <summary>
        /// The target's 24-bit offset from the program counter (PC), shifted left by 2 bits and sign-extended
        /// </summary>
        IMAGE_REL_M32R_PCREL24 = 0x0005,

        /// <summary>
        /// The target's 16-bit offset from the PC, shifted left by 2 bits and sign-extended
        /// </summary>
        IMAGE_REL_M32R_PCREL16 = 0x0006,

        /// <summary>
        /// The target's 8-bit offset from the PC, shifted left by 2 bits and sign-extended
        /// </summary>
        IMAGE_REL_M32R_PCREL8 = 0x0007,

        /// <summary>
        /// The 16 MSBs of the target VA.
        /// </summary>
        IMAGE_REL_M32R_REFHALF = 0x0008,

        /// <summary>
        /// The 16 MSBs of the target VA, adjusted for LSB sign extension. This is used for the first instruction in a two-instruction sequence that loads a full 32-bit address. This relocation must be immediately followed by a PAIR relocation whose SymbolTableIndex contains a signed 16-bit displacement that is added to the upper 16 bits that are taken from the location that is being relocated.
        /// </summary>
        IMAGE_REL_M32R_REFHI = 0x0009,

        /// <summary>
        /// The 16 LSBs of the target VA.
        /// </summary>
        IMAGE_REL_M32R_REFLO = 0x000A,

        /// <summary>
        /// The relocation must follow the REFHI relocation. Its SymbolTableIndex contains a displacement and not an index into the symbol table.
        /// </summary>
        IMAGE_REL_M32R_PAIR = 0x000B,

        /// <summary>
        /// The 16-bit section index of the section that contains the target. This is used to support debugging information.
        /// </summary>
        IMAGE_REL_M32R_SECTION = 0x000C,

        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section. This is used to support debugging information and static thread local storage.
        /// </summary>
        IMAGE_REL_M32R_SECREL32 = 0x000D,

        /// <summary>
        /// CLR token
        /// </summary>
        IMAGE_REL_M32R_TOKEN = 0x000E
    }

    #endregion
}