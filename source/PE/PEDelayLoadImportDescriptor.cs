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
using System.Collections.Generic;

namespace LibPENUT
{
    /// <summary>
    /// Representing an entry in the delay load directory table in the PE image
    /// </summary>
    public class PEDelayLoadImportDescriptor
    {
        /// <summary>
        /// Create an empty import descriptor
        /// </summary>
        public PEDelayLoadImportDescriptor(PEImage image)
        {
            Attributes = 0;
            NameRVA = 0;
            ModuleHandleRVA = 0;
            DelayImportAddressTable = 0;
            DelayImportNameTable = 0;
            BoundDelayedImportTable = 0;
            UnloadDelayedImportTable = 0;
            TimeDateStamp = 0;

            Name = string.Empty;
            m_imports = new List<PEImportedSymbol>();

            Image = image;
        }

        /// <summary>
        /// Create an import descriptor and initialize it from an entry in the import directory table pointed to by the specified relative virtual address
        /// </summary>
        public PEDelayLoadImportDescriptor(PEImage image, uint rva) : this(image)
        {
            COFFSection section = image.GetSectionFromRva(rva);

            if (section != null)
            {
                Attributes = section.GetUInt32FromRva(rva);
                NameRVA = section.GetUInt32FromRva(rva + 4);
                ModuleHandleRVA = section.GetUInt32FromRva(rva + 8);
                DelayImportAddressTable = section.GetUInt32FromRva(rva + 12);
                DelayImportNameTable = section.GetUInt32FromRva(rva + 16);
                BoundDelayedImportTable = section.GetUInt32FromRva(rva + 20);
                UnloadDelayedImportTable = section.GetUInt32FromRva(rva + 24);
                TimeDateStamp = section.GetUInt32FromRva(rva + 28);

                if(NameRVA != 0)
                {
                    // Note, the name and import entries are not guaranteed to be in the same sections as the import descriptor itself so a section lookup is necessary here
                    if (image.TryGetSectionFromRva(NameRVA, out COFFSection nameSection))
                    {
                        Name = nameSection.GetStringFromRva(NameRVA);
                    }
                    else
                    {
                        Name = string.Empty;
                    }
                }

                if (DelayImportNameTable != 0)
                {
                    // Start parsing entries from the import lookup table
                    COFFSection entriesSection = image.GetSectionFromRva(DelayImportNameTable);
                    UInt32 importEntryRva = DelayImportNameTable;
                    do
                    {
                        if (Image.OptionalHeader.MagicNumber == COFFMagicNumbers.PE32Plus)  // PE32+ image, entry is a 64-bit number
                        {
                            UInt64 importEntry = entriesSection.GetUInt64FromRva(importEntryRva);

                            if (importEntry == 0)   // Found end of import lookup table
                                break;

                            if ( (importEntry & 0x8000000000000000) != 0)
                            {
                                // MSB bit is set, import is by ordinal and remaining bits is the ordinal nr
                                m_imports.Add(new PEImportedSymbol(0, (importEntry & 0x7FFFFFFFFFFFFFFF).ToString(), (Int16)(importEntry & 0x7FFFFFFFFFFFFFFF)));
                            }
                            else
                            {
                                // MSB bit is clear, remaning bits is an RVA to a hint/name table entry
                                // Note that altough this is a 64-bit address here in practice only the first 32 bits should be usable in the import lookup table so we should be safe to downcast in TryGetStringFromRVA()
                                // since the section headers can't represent full 64-bit addresses. The mirrored import address table used in runtime is another matter but we are not concerned with that here
                                UInt16 hint = entriesSection.GetUInt16FromRva((UInt32)importEntry);
                                string importName = entriesSection.GetStringFromRva((UInt32)(importEntry + 2));
                                m_imports.Add(new PEImportedSymbol(hint, importName));
                            }

                            importEntryRva += 8;
                        }
                        else // PE32 image, entry is a 32-bit number
                        {
                            UInt32 importEntry = entriesSection.GetUInt32FromRva(importEntryRva);

                            if (importEntry == 0) // Found end of import lookup table
                                break;

                            if ((importEntry & 0x80000000) != 0)
                            {
                                // MSB bit is set, import is by ordinal and remaining bits is the ordinal nr
                                m_imports.Add(new PEImportedSymbol(0, (importEntry & 0x7FFFFFFF).ToString(), (Int16)(importEntry & 0x7FFFFFFF)));
                            }
                            else
                            {
                                // MSB bit is clear, remaning bits is an RVA to a hint/name table entry
                                UInt16 hint = entriesSection.GetUInt16FromRva(importEntry);
                                string importName = entriesSection.GetStringFromRva(importEntry + 2);
                                m_imports.Add(new PEImportedSymbol(hint, importName));
                            }

                            importEntryRva += 4;
                        }

                    } while (importEntryRva < entriesSection.Header.VirtualAddress + entriesSection.Header.VirtualSize);  // we actually rely on the break; statements above to break the loop and not this condition but it's there for safety

                }
            }
        }

        /// <summary>
        /// Unused
        /// </summary>
        public UInt32 Attributes
        {
            get;set;
        }

        /// <summary>
        /// Relative virtual address to a string containing the name of the imported DLL
        /// </summary>
        public UInt32 NameRVA
        {
            get; set;
        }

        /// <summary>
        /// Name of the dll holding the imports
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// Relative virtual address of the module handle(in the data section of the image) of the DLL to be delay-loaded.
        /// Used for storage by the routine supplied to manage delay-loading.
        /// </summary>
        public UInt32 ModuleHandleRVA
        {
            get;set;
        }

        /// <summary>
        /// Relative virtual address of the delay-load import address
        /// </summary>
        public UInt32 DelayImportAddressTable
        {
            get;set;
        }

        /// <summary>
        /// Relative virtual address of the delay-load name table, which contains the names of the imports that may need to be loaded.
        /// </summary>
        public UInt32 DelayImportNameTable
        {
            get; set;
        }

        /// <summary>
        /// Relative virtual address of the bound delayload address table, if it exists.
        /// </summary>
        public UInt32 BoundDelayedImportTable
        {
            get;set;
        }

        /// <summary>
        /// Relative virtual address of the unload delayload address table, if it exists.
        /// </summary>
        public UInt32 UnloadDelayedImportTable
        {
            get; set;
        }

        /// <summary>
        /// This field will always be set to zero in the image. It is populated by the loader only when the image is loaded into memory where it is set to the timestamp of the loaded DLL
        /// </summary>
        public UInt32 TimeDateStamp 
        {
            get;set;
        }


        private List<PEImportedSymbol> m_imports;

        /// <summary>
        /// A list of PEImportedSymbol objects describing each imported symbol from the DLL
        /// </summary>
        public IEnumerable<PEImportedSymbol> Imports
        {
            get { return m_imports; }
        }

        /// <summary>
        /// A reference to the PEImage owning this import descriptor
        /// </summary>
        public PEImage Image
        {
            get; set;
        }

        /// <summary>
        /// Gets the size in bytes of an import desriptor in the import directory table
        /// </summary>
        public static UInt32 Size
        {
            get { return 32; }
        }

    }
}
