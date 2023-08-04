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
using System.IO;

namespace LibPENUT
{
    /// <summary>
    /// Represents an entry in teh Attribute Certificate table in a PE image
    /// </summary>
    public class PEAttributeCertificate
    {

        public PEAttributeCertificate()
        {
            CertificateData = new byte[0];
            Revision = 0x0200;
            CertificateType = PEAttributeCertificateType.WIN_CERT_TYPE_PKCS_SIGNED_DATA;
        }

        public void Read(Stream inputStream)
        {
            using(PENUTBinaryReader reader = new PENUTBinaryReader(inputStream, System.Text.Encoding.ASCII, true))
            {
                uint length = reader.ReadUInt32();
                CertificateData = new byte[length - 8];

                Revision = reader.ReadUInt16();
                CertificateType = (PEAttributeCertificateType)reader.ReadUInt16();

                reader.Read(CertificateData,0, CertificateData.Length);
            }

        }

        public void Write(Stream outputStream)
        {
            using (PENUTBinaryWriter writer = new PENUTBinaryWriter(outputStream, System.Text.Encoding.ASCII, true))
            {
                writer.Write(Length);
                writer.Write(Revision);
                writer.Write((UInt16)CertificateType);
                writer.Write(CertificateData);
            }

        }

        public UInt32 Length
        {
            get { return (UInt32)(8 + CertificateData.Length); }
        }

        public byte[] CertificateData
        {
            get;set;
        }

        /// <summary>
        /// Version number of the data structure contained in CertificateData.
        /// Typical values are WIN_CERT_REVISION_1_0 = 0x0100 for legacy Autenticode and WIN_CERT_REVISION_2_0 = 0x0200 for current Authenticode
        /// </summary>
        public UInt16 Revision
        {
            get;set;
        }

        public PEAttributeCertificateType CertificateType
        {
            get;set;
        }

    }
}
