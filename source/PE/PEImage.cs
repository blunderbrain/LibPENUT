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
    /// Represents a PE executable image
    /// </summary>
    public class PEImage : COFFObject
    {
        [Flags]
        public enum ReadOptions
        {
            /// <summary>
            /// No special options
            /// </summary>
            None = 0,

            /// <summary>
            /// Ignore any extra data at the end of the image that is not referenced by any header or other datastructure.
            /// When this flag is set the parser will stop reading after the last section (or certificate table if present) even if there are more data left in the stream
            /// </summary>
            StripOverlay = 0x2
        }
        
        public PEImage() : base()
        {
            DOSHeader = new PEDOSHeader();
            DOSStub = new byte[0];
            OptionalHeader = new PEOptionalHeader();
            Overlay = new byte[0];

            ExportsDirectory = new PEExportDirectory();

            m_certificates = new List<PEAttributeCertificate>();
            m_importDescriptors = new List<PEImportDescriptor>();
            m_delayLoadimportDescriptors = new List<PEDelayLoadImportDescriptor>();
        }

        public PEImage(string fileName, ReadOptions readOptions = ReadOptions.None) : this()
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Read(fs, 0, readOptions);
            }
        }

        /// <summary>
        /// DOS header
        /// </summary>
        public PEDOSHeader DOSHeader
        {
            get;set;
        }

        /// <summary>
        /// Raw byte representation of the images DOS stub code
        /// </summary>
        public byte[] DOSStub
        {
            get;set;
        }

        private UInt32 Signature
        {
            get; set;
        }

        /// <summary>
        /// Convenience property to get the full PE optional header without having to cast the OptionalHeader property
        /// </summary>
        public PEOptionalHeader PEOptionalHeader
        {
            get { return OptionalHeader as PEOptionalHeader; }
        }

        /// <summary>
        /// Any extra data present at the end of the image that is not part of any section or referenced by other PE data structures
        /// </summary>
        public byte[] Overlay
        {
            get;set;
        }

        /// <summary>
        /// A PEExportDirectory object that describes exported symbols from the image (if any)
        /// </summary>
        public PEExportDirectory ExportsDirectory
        {
            get; private set;
        }

        private List<PEAttributeCertificate> m_certificates;

        public IList<PEAttributeCertificate> Certificates
        {
            get { return m_certificates; }
        }

        private List<PEImportDescriptor> m_importDescriptors;

        public IEnumerable<PEImportDescriptor> ImportDescriptors
        {
            get { return m_importDescriptors; }
        }

        private List<PEDelayLoadImportDescriptor> m_delayLoadimportDescriptors;

        public IEnumerable<PEDelayLoadImportDescriptor> DelayLoadImportDescriptors
        {
            get { return m_delayLoadimportDescriptors; }
        }

        /// <summary>
        /// Update all offsets and sizes in the file header, optional header and section headers based on the current content of the image
        /// </summary>
        public override void UpdateLayout()
        {
            // Compute SizeOfHeaders field for the PE optional header which should essentially point to the start of the raw data for the first section
            uint sizeOfHeaders = Convert.ToUInt32(PEDOSHeader.Size + DOSStub.Length + COFFFileHeader.Size + OptionalHeader.Size + COFFSectionHeader.Size * Header.NrOfSections);

            // Pad for FileAlignment
            sizeOfHeaders = sizeOfHeaders.AlignTo(PEOptionalHeader.FileAlignment);

            // The nominal size here with a file alignment of 0x200 will be 0x400 but some tools set this to 0x600 for some reason and we don't want to cause unnecessary differenses with the orignal image
            if (sizeOfHeaders > PEOptionalHeader.SizeOfHeaders)
                PEOptionalHeader.SizeOfHeaders = sizeOfHeaders;

            // Compute size of the loaded image by taking the highest section RVA and pad for SectionAlignment
            UInt32 sizeofImage = Sections.Last().Header.VirtualAddress + Sections.Last().Header.VirtualSize;
            PEOptionalHeader.SizeOfImage = sizeofImage.AlignTo(PEOptionalHeader.SectionAlignment);

            // BaseOfData is probably more or less obsolete and is not present for 64-bit images.
            // Many files do not set this in the 32-bit case either so filling it in here is just going to create unnecessary differences between input and output files
            /*
            COFFSection firstDataSection = Sections.FirstOrDefault(s => (s.Header.Characteristics & COFFSectionCharacteristics.IMAGE_SCN_CNT_INITIALIZED_DATA) != 0 && (s.Header.Characteristics & COFFSectionCharacteristics.IMAGE_SCN_CNT_CODE) == 0);
            if (firstDataSection != null)
                PEOptionalHeader.BaseOfData = firstDataSection.Header.VirtualAddress;
            */

            // Note: Recalculating SizeOfCode like this may not be accurate since unfortunately there are PE files out there with data sections that set the IMAGE_SCN_CNT_CODE flag.
            // This makes it hard to reproduce the results from the original toolchain since there is no easy way to determine what the "true" code sections are after the fact.
            // Instead of doing alot of guess work we leave it to the user who whish to do things that affect this value to adjust it themselves instead if necessary.
            // It's also unclear if this field is actually used for anything critical by the loader so it may not even matter

            // PEOptionalHeader.SizeOfCode = (UInt32)Sections.Where(s => (s.Header.Characteristics & COFFSectionCharacteristics.IMAGE_SCN_CNT_CODE) != 0).Sum(s => s.Header.SizeOfRawData);

            // Similar to SizeOfCode there is no clear documentation on exactly how to calculate SizeOfInitializedData/SizeOfUnInitializedData so results vary between different tools.
            // The most common way seems to be to sum up the virtual sizes for the data sections, after adjusting them for FileAlignment but some tools seem to just sum up the RawData sizes instead which can yield slightly different results
            // Again we leave these for the end user to mess with instead of trying to guess what the original toolchain did

            // PEOptionalHeader.SizeOfInitializedData = (UInt32)Sections.Where(s => (s.Header.Characteristics & COFFSectionCharacteristics.IMAGE_SCN_CNT_INITIALIZED_DATA) != 0).Sum(s => s.Header.VirtualSize.AlignTo(PEOptionalHeader.FileAlignment));
            // PEOptionalHeader.SizeOfUninitializedData = (UInt32)Sections.Where(s => (s.Header.Characteristics & COFFSectionCharacteristics.IMAGE_SCN_CNT_UNINITIALIZED_DATA) != 0).Sum(s => s.Header.VirtualSize.AlignTo(PEOptionalHeader.FileAlignment));

            base.UpdateLayout(PEOptionalHeader.FileAlignment, PEOptionalHeader.SizeOfHeaders);
        }

        /// <summary>
        /// Reads a PE image from the specified file
        /// </summary>
        /// <param name="fileName">The name of the file to load the image from</param>
        public override void Read(string fileName)
        {
            Read(fileName, ReadOptions.None);
        }

        /// <summary>
        /// Reads a PE image from the specified file
        /// </summary>
        /// <param name="fileName">The name of the file to load the image from</param>
        /// <param name="readOptions">Flags from the PEImage.ReadOptions enum that can alter the behaviour of the parser</param>
        public void Read(string fileName, ReadOptions readOptions = ReadOptions.None)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Read(fs, 0, readOptions);
            }
        }

        /// <summary>
        /// Read a PE image from a stream
        /// </summary>
        /// <param name="inputStream">The stream to read data from. The specified stream must support random access, a.k.a seeking</param>
        /// <param name="imageStreamOffset">Offset to the position in the stream to be considered as the starting point of this image. This value is added to all data pointers in the image when seeking in the stream</param>
        public override void Read(Stream inputStream, long imageStreamOffset = 0)
        {
            Read(inputStream, imageStreamOffset, ReadOptions.None);
        }

        /// <summary>
        /// Read a PE image from a stream
        /// </summary>
        /// <param name="inputStream">The stream to read data from. The specified stream must support random access, a.k.a seeking</param>
        /// <param name="imageStreamOffset">Offset to the position in the stream to be considered as the starting point of this image. This value is added to all data pointers in the image when seeking in the stream</param>
        /// <param name="readOptions">Flags from the PEImage.ReadOptions enum that can alter the behaviour of the parser</param>
        public void Read(Stream inputStream, long imageStreamOffset = 0, ReadOptions readOptions = ReadOptions.None)
        {
            if (imageStreamOffset == 0)
                imageStreamOffset = inputStream.Position;

            DOSHeader.Read(inputStream);
            
            int stubSize = Convert.ToInt32(DOSHeader.e_lfanew - PEDOSHeader.Size);
            DOSStub = new byte[stubSize];
            inputStream.Read(DOSStub, 0, stubSize);
            
            Signature = ((UInt32)inputStream.ReadByte()) << 24;
            Signature |= ((UInt32)inputStream.ReadByte()) << 16;
            Signature |= ((UInt32)inputStream.ReadByte()) << 8;
            Signature |= (UInt32)inputStream.ReadByte();

            if (Signature != 0x50450000)  // "PE\0\0"
            {
                throw new BadImageFormatException("Invalid image signature 0x{0:X} - expected 'PE' (0x50450000)");
            }

            base.Read(inputStream, imageStreamOffset);

            // ********** Start processing data directories **********

            // TODO: support for remaining data directories

            // ********** Certificates Directory **********

            // Read and parse certificate directory i.e. pretty much always an Authenticode signature if the image is signed
            m_certificates.Clear();
            if (PEOptionalHeader.DataDirectories.Count > PEDataDirectories.Certificates &&
                PEOptionalHeader.DataDirectories[PEDataDirectories.Certificates].RVA != 0 &&
                PEOptionalHeader.DataDirectories[PEDataDirectories.Certificates].RVA + PEOptionalHeader.DataDirectories[PEDataDirectories.Certificates].TableSize + imageStreamOffset <= inputStream.Length)
            {
                PEDataDirectory certificateDirectory = PEOptionalHeader.DataDirectories[PEDataDirectories.Certificates];

                // Check for and handle extra data between last section and certificate table
                long overlaySize = certificateDirectory.RVA + imageStreamOffset - inputStream.Position;
                if (overlaySize > 0 && (readOptions & ReadOptions.StripOverlay) == 0)
                {
                    Overlay = new byte[overlaySize];
                    inputStream.Read(Overlay, 0, Overlay.Length);
                    if (overlaySize < 8 && Overlay.All(x => x == 0))  // Less than 8 bytes that are all zero - this is almost certainly just padding for the certificate table so lets ignore it
                        Overlay = new byte[0];
                }

                // Seek to first certificate entry
                inputStream.Seek(certificateDirectory.RVA + imageStreamOffset, SeekOrigin.Begin);
                do
                {
                    // Read past any alignment padding
                    while ((inputStream.Position % 8) != 0) { inputStream.ReadByte(); }

                    PEAttributeCertificate certificateEntry = new PEAttributeCertificate();
                    certificateEntry.Read(inputStream);
                    m_certificates.Add(certificateEntry);

                } while (inputStream.Position < imageStreamOffset + certificateDirectory.RVA + certificateDirectory.TableSize);
            }
            else if (inputStream.Position < inputStream.Length - 1 && (readOptions & ReadOptions.StripOverlay) == 0)
            {
                // No certificates table and there is still data in the stream, populate Overlay unless explicitly disabled
                long overlaySize = inputStream.Length - inputStream.Position;
                Overlay = new byte[overlaySize];
                inputStream.Read(Overlay, 0, Overlay.Length);
            }

            // ********** Exports Directory **********
            if (PEOptionalHeader.DataDirectories.Count > PEDataDirectories.Exports && PEOptionalHeader.DataDirectories[PEDataDirectories.Exports].RVA != 0)
            {
                ExportsDirectory = new PEExportDirectory(this, PEOptionalHeader.DataDirectories[PEDataDirectories.Exports].RVA);
            }

            // ********** Import Directory **********
            m_importDescriptors.Clear();
            if (PEOptionalHeader.DataDirectories.Count > PEDataDirectories.Imports && PEOptionalHeader.DataDirectories[PEDataDirectories.Imports].RVA != 0)
            {
                UInt32 importTableRVA = PEOptionalHeader.DataDirectories[PEDataDirectories.Imports].RVA;
                PEImportDescriptor importDescriptor;
                // Create and read new descriptors until we find the terminating entry with all members set to zero
                do
                {
                    importDescriptor = new PEImportDescriptor(this, importTableRVA);
                    if (importDescriptor.OriginalFirstThunk != 0 && importDescriptor.FirstThunk != 0)
                    {
                        m_importDescriptors.Add(importDescriptor);
                        importTableRVA += PEImportDescriptor.Size;
                    }
                } while (importDescriptor.OriginalFirstThunk != 0 && importDescriptor.FirstThunk != 0);
            }

            // ********** Delay Import Directory **********

            m_delayLoadimportDescriptors.Clear();
            if (PEOptionalHeader.DataDirectories.Count > PEDataDirectories.DelayImports && PEOptionalHeader.DataDirectories[PEDataDirectories.DelayImports].RVA != 0)
            {
                UInt32 dealyloadimportTableRVA = PEOptionalHeader.DataDirectories[PEDataDirectories.DelayImports].RVA;
                PEDelayLoadImportDescriptor importDescriptor;
                // Create and read new descriptors until we find the terminating entry with all members set to zero
                do
                {
                    importDescriptor = new PEDelayLoadImportDescriptor(this, dealyloadimportTableRVA);
                    if (importDescriptor.DelayImportAddressTable != 0 && importDescriptor.DelayImportNameTable != 0)
                    {
                        m_delayLoadimportDescriptors.Add(importDescriptor);
                        dealyloadimportTableRVA += PEDelayLoadImportDescriptor.Size;
                    }
                } while (importDescriptor.DelayImportAddressTable != 0 && importDescriptor.DelayImportNameTable != 0);
            }

        }

        /// <summary>
        /// Write this image to the specified Stream
        /// </summary>
        /// <param name="outputStream">The Stream to write to. The stream must support seeking</param>
        /// <param name="imageStreamOffset">Offset to the position in the stream to be considered as the starting point of this image. This value is added to all filepointers in the image when seeking in the stream</param>
        public override void Write(Stream outputStream, long imageStreamOffset = 0)
        {
            DOSHeader.Write(outputStream);
            outputStream.Write(DOSStub, 0, DOSStub.Length);
            outputStream.WriteByte((byte)(Signature >> 24));
            outputStream.WriteByte((byte)(Signature >> 16));
            outputStream.WriteByte((byte)(Signature >> 8));
            outputStream.WriteByte((byte)Signature);

            // Save position of optional header in case we need to modify it again later on
            long pointerToOptionalHeader = outputStream.Position + COFFFileHeader.Size;

            base.Write(outputStream, imageStreamOffset);

            // Write any extra overlay data
            if (Overlay.Length > 0)
            {
                outputStream.Write(Overlay,0, Overlay.Length);
            }

            // Write any certificate data (i.e Authenticode signature)
            if(m_certificates.Count > 0)
            {
                long tableStart = -1;
                foreach (PEAttributeCertificate cert in m_certificates)
                {
                    while ((outputStream.Position % 8) != 0)    // Pad to ensure we end up on an 8 byte boundry
                        outputStream.WriteByte(0);

                    if (tableStart == -1)
                        tableStart = outputStream.Position;

                    cert.Write(outputStream);
                }

                // Update file pointer and size in optional header
                PEOptionalHeader.DataDirectories[PEDataDirectories.Certificates].RVA = (UInt32)(tableStart - imageStreamOffset);
                PEOptionalHeader.DataDirectories[PEDataDirectories.Certificates].TableSize = (UInt32)(outputStream.Position - tableStart);

                // Go back and write updated optional header again
                long currentStreamPos = outputStream.Position;
                outputStream.Seek(pointerToOptionalHeader, SeekOrigin.Begin);
                PEOptionalHeader.Write(outputStream);
                outputStream.Seek(currentStreamPos, SeekOrigin.Begin);
            }
        }

    }
}
