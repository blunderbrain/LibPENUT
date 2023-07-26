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
    /// Enum describing target machine/architecture
    /// </summary>
    public enum COFFMachineType : ushort
    {
        /// <summary>
        /// Contents assumed to be applicable to any CPU
        /// </summary>
        IMAGE_FILE_MACHINE_UNKNOWN = 0x0,

        /// <summary>
        /// Useful for indicating we want to interact with the host and not a WoW guest.
        /// </summary>
        IMAGE_FILE_MACHINE_TARGET_HOST = 0x1,

        /// <summary>
        /// Intel 386 or later, and compatible processors.
        /// </summary>
        IMAGE_FILE_MACHINE_I386 = 0x14c,

        /// <summary>
        /// MIPS big-endian
        /// </summary>
        IMAGE_FILE_MACHINE_R3000BE = 0x160,

        /// <summary>
        /// MIPS little-endian
        /// </summary>
        IMAGE_FILE_MACHINE_R3000 = 0x162,

        /// <summary>
        /// MIPS little endian.
        /// </summary>
        IMAGE_FILE_MACHINE_R4000 = 0x166,

        /// <summary>
        /// MIPS little endian.
        /// </summary>
        IMAGE_FILE_MACHINE_R10000 = 0x168,

        /// <summary>
        /// MIPS little-endian WCE v2
        /// </summary>
        IMAGE_FILE_MACHINE_WCEMIPSV2 =  0x0169,

        /// <summary>
        /// Alpha AXP
        /// </summary>
        IMAGE_FILE_MACHINE_ALPHA = 0x184,

        /// <summary>
        /// Hitachi SH3
        /// </summary>
        IMAGE_FILE_MACHINE_SH3 = 0x1a2,

        /// <summary>
        /// Hitachi SH3 DSP
        /// </summary>
        IMAGE_FILE_MACHINE_SH3DSP = 0x01a3,

        /// <summary>
        /// Hitachi SH3E
        /// </summary>
        IMAGE_FILE_MACHINE_SH3E = 0x01a4,

        /// <summary>
        /// Hitachi SH4
        /// </summary>
        IMAGE_FILE_MACHINE_SH4 = 0x1a6,

        /// <summary>
        /// Hitachi SH5
        /// </summary>
        IMAGE_FILE_MACHINE_SH5 = 0x01a8,

        /// <summary>
        /// ARM CPU
        /// </summary>
        IMAGE_FILE_MACHINE_ARM = 0x1c0,

        /// <summary>
        /// ARM Thumb
        /// </summary>
        IMAGE_FILE_MACHINE_THUMB = 0x1c2,

        /// <summary>
        /// ARM Thumb-2 Little-Endian
        /// </summary>
        IMAGE_FILE_MACHINE_ARMNT = 0x01c4,

        /// <summary>
        /// AM33
        /// </summary>
        IMAGE_FILE_MACHINE_AM33 = 0x01d3,

        /// <summary>
        /// Power PC, little endian.
        /// </summary>
        IMAGE_FILE_MACHINE_POWERPC = 0x1f0,

        /// <summary>
        /// Power PC
        /// </summary>
        IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,

        /// <summary>
        /// Intel IA64
        /// </summary>
        IMAGE_FILE_MACHINE_IA64 = 0x200,

        /// <summary>
        /// MIPS16 CPU
        /// </summary>
        IMAGE_FILE_MACHINE_MIPS16 = 0x266,

        /// <summary>
        /// Motorola 68000 series.
        /// </summary>
        IMAGE_FILE_MACHINE_M68K = 0x268,

        /// <summary>
        /// Alpha AXP 64-bit.
        /// </summary>
        IMAGE_FILE_MACHINE_ALPHA64 = 0x284,

        /// <summary>
        /// MIPS with FPU
        /// </summary>
        IMAGE_FILE_MACHINE_MIPSFPU = 0x366,

        /// <summary>
        /// MIPS16 with FPU
        /// </summary>
        IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,

        /// <summary>
        /// Infineon
        /// </summary>
        IMAGE_FILE_MACHINE_TRICORE = 0x0520,

        /// <summary>
        /// CEF
        /// </summary>
        IMAGE_FILE_MACHINE_CEF = 0x0CEF,

        /// <summary>
        /// EFI Byte Code
        /// </summary>
        IMAGE_FILE_MACHINE_EBC = 0x0EBC,

        /// <summary>
        /// AMD64 (x64)
        /// </summary>
        IMAGE_FILE_MACHINE_AMD64 = 0x8664,

        /// <summary>
        /// M32R
        /// </summary>
        IMAGE_FILE_MACHINE_M32R = 0x9041,

        /// <summary>
        /// ARM64 Little-Endian
        /// </summary>
        IMAGE_FILE_MACHINE_ARM64 = 0xAA64,
        
        /// <summary>
        /// CEE
        /// </summary>
        IMAGE_FILE_MACHINE_CEE = 0xC0EE
    }
}
