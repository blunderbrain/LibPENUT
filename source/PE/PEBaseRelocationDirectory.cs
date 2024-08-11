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
using System.IO;
using System.Linq;

namespace LibPENUT
{
    /// <summary>
    /// Represents the collection of PEBaseRelocationBlocks contained in the .reloc section of a relocatable image
    /// </summary>
    public class PEBaseRelocationDirectory
    {

        /// <summary>
        /// Creates a new PEBaseRelocationDirectory object
        /// </summary>
        public PEBaseRelocationDirectory()
        {
            RelocationBlocks = new List<PEBaseRelocationBlock>();
        }

        /// <summary>
        /// Creates a new PEBaseRelocationDirectory object and populates it from the .reloc section pointed to by the sectionRva parameter
        /// </summary>
        public PEBaseRelocationDirectory(PEImage image, UInt32 sectionRva, UInt32 sizeOfRelocationTable) : this()
        {
            Image = image;

            COFFSection relocSection = image.GetSectionFromRva(sectionRva); // Find the .reloc section
            if (relocSection != null)
            {
                UInt32 blockRva = sectionRva;   // Initialize pointer to the current block to parse
                while (blockRva < sectionRva + sizeOfRelocationTable)
                {
                    PEBaseRelocationBlock block = new PEBaseRelocationBlock();
                    block.PageRVA = relocSection.GetUInt32FromRva(blockRva);    // First part of block header - the target page RVA for the relocations
                    UInt32 blockSize = relocSection.GetUInt32FromRva(blockRva + 4); // Second part of block header - size of the block, including the header

                    // Start reading entries from the block - each entry is a 16 bit number that encodes offset and type
                    for (UInt32 relocationRva = blockRva + 8; relocationRva < blockRva + blockSize ; relocationRva += PEBaseRelocation.Size)
                    {
                        block.Entries.Add(new PEBaseRelocation(relocSection.GetUInt16FromRva(relocationRva)));
                    }
                    RelocationBlocks.Add(block);    // Add block to our list
                    // Advance block pointer to next block. The AlignTo() call is probably unecessary since the blocks are typically padded to an even 32-bit with a zero value entry anyway but it doesn't hurt and serves as a guard for tools that may be less strict about the spec.
                    blockRva = (blockRva + blockSize).AlignTo(4);
                }
            }
        }

        /// <summary>
        /// Creates a new .reloc section from a list of one or more PEBaseRelocationBlock objects 
        /// This overload automatically calculates the next free virtual address for the section in the specified image.
        /// </summary>
        /// <param name="image">The PEImage that the section should be created for. Note that this method does not add the section to the image automatically</param>
        /// <param name="relocationBlocks">The list of relocation blocks to add to the section</param>
        public static COFFSection CreateSection(PEImage image, IEnumerable<PEBaseRelocationBlock> relocationBlocks)
        {
            COFFSection lastSection = image.Sections.Last();
            UInt32 rva = lastSection.Header.VirtualAddress + lastSection.Header.VirtualSize;

            rva = rva.AlignTo(image.PEOptionalHeader.SectionAlignment);

            return CreateSection(image, rva, relocationBlocks);
        }

        /// <summary>
        /// Creates a new .reloc section from a list of one or more PEBaseRelocationBlock objects 
        /// </summary>
        /// <param name="image">The PEImage that the section should be created for. Note that this method does not add the section to the image automatically</param>
        /// <param name="sectionRVA">The virtual address to assign to the new section</param>
        /// <param name="relocationBlocks">The list of relocation blocks to add to the section</param>
        public static COFFSection CreateSection(PEImage image, UInt32 sectionRVA, IEnumerable<PEBaseRelocationBlock> relocationBlocks)
        {
            COFFSection relocSection = new COFFSection();
            relocSection.Header.Name = ".reloc";
            relocSection.Header.Characteristics =
                COFFSectionCharacteristics.IMAGE_SCN_CNT_INITIALIZED_DATA |
                COFFSectionCharacteristics.IMAGE_SCN_MEM_READ |
                COFFSectionCharacteristics.IMAGE_SCN_MEM_DISCARDABLE;

            relocSection.Header.VirtualAddress = sectionRVA;
            relocSection.Header.VirtualSize = Convert.ToUInt32(relocationBlocks.Sum(b => b.BlockSize));

            // Note: for new sections we would normally pad the data size to SectionAlignment but Microsoft tools seem to use FileAlignment instead for the .reloc section. Perhaps because it's only data for the loader that is marked as discardable so section alignment doesn't actually matter?
            relocSection.Header.SizeOfRawData = relocSection.Header.VirtualSize.AlignTo(image.PEOptionalHeader.FileAlignment);
            relocSection.RawData = new byte[relocSection.Header.SizeOfRawData];
            
            using (PENUTBinaryWriter writer = new PENUTBinaryWriter(new MemoryStream(relocSection.RawData)))
            {
                foreach(PEBaseRelocationBlock block in relocationBlocks)
                {
                    writer.Write(block.PageRVA);
                    writer.Write(block.BlockSize);
                    foreach (PEBaseRelocation entry in block.Entries)
                    {
                        writer.Write(entry.EntryValue);
                    }
                }
            }
            
            return relocSection;
        }

        /// <summary>
        /// The list of relocation blocks contained in the images .reloc section
        /// </summary>
        public IList<PEBaseRelocationBlock> RelocationBlocks
        {  get; set; }

        /// <summary>
        /// The PEImage that this relocation directory belongs to
        /// </summary>
        public PEImage Image
        { get; private set; }

    }
}
