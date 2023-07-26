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
    /// Constants for the SubSystem field in the PE optional header
    /// </summary>
    public enum PESubSystem : ushort
    {
        /// <summary>
        /// Unknown subsystem.
        /// </summary>
        IMAGE_SUBSYSTEM_UNKNOWN = 0,

        /// <summary>
        /// Used for device drivers and native Windows NT processes.
        /// </summary>
        IMAGE_SUBSYSTEM_NATIVE = 1,

        /// <summary>
        /// Image runs in the Windows™ graphical user interface (GUI) subsystem.
        /// </summary>
        IMAGE_SUBSYSTEM_WINDOWS_GUI = 2,

        /// <summary>
        /// Image runs in the Windows character subsystem.
        /// </summary>
        IMAGE_SUBSYSTEM_WINDOWS_CUI = 3,

        /// <summary>
        /// image runs in the OS/2 character subsystem.
        /// </summary>
        IMAGE_SUBSYSTEM_OS2_CUI = 5,

        /// <summary>
        /// Image runs in the Posix character subsystem.
        /// </summary>
        IMAGE_SUBSYSTEM_POSIX_CUI = 7,

        /// <summary>
        /// image is a native Win9x driver.
        /// </summary>
        IMAGE_SUBSYSTEM_NATIVE_WINDOWS = 8,
        
        /// <summary>
        /// Image runs in on Windows CE.
        /// </summary>
        IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9,

        /// <summary>
        /// Image is an EFI application.
        /// </summary>
        IMAGE_SUBSYSTEM_EFI_APPLICATION = 10,

        /// <summary>
        /// Image is an EFI driver that provides boot services.
        /// </summary>
        IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11,

        /// <summary>
        /// Image is an EFI driver that provides runtime services.
        /// </summary>
        IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12,

        /// <summary>
        /// EFI ROM
        /// </summary>
        IMAGE_SUBSYSTEM_EFI_ROM = 13,
        
        /// <summary>
        /// XBOX
        /// </summary>
        IMAGE_SUBSYSTEM_XBOX = 14,
        
        /// <summary>
        /// Windows Boot application
        /// </summary>
        IMAGE_SUBSYSTEM_WINDOWS_BOOT_APPLICATION = 16,
        
        /// <summary>
        /// XBOX code catalog
        /// </summary>
        IMAGE_SUBSYSTEM_XBOX_CODE_CATALOG = 17
    }
}
