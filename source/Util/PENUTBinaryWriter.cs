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

using System.IO;

namespace LibPENUT
{
    /// <summary>
    /// Binary stream writer helper class to deal with the fact that .NET 3.5 lacks the functionality to keep the underlying stream open when disposing the writer
    /// </summary>
    public class PENUTBinaryWriter : BinaryWriter
    {
        protected PENUTBinaryWriter() : base(null) { }

        public PENUTBinaryWriter(Stream stream) : base(stream) { }
        public PENUTBinaryWriter(Stream stream, System.Text.Encoding encoding) : base(stream, encoding) { }
#if NET35
        // For .NET 3.5 we implement the leaveOpen parameter using the NonClosingStreamWrapper helper class to isolate the stream from the default behaviour of BinaryWriter
        public PENUTBinaryWriter(Stream stream, System.Text.Encoding encoding, bool leaveOpen) : base(leaveOpen ? new NonClosingStreamWrapper(stream) : stream, encoding) { }
#else
        public PENUTBinaryWriter(Stream stream, System.Text.Encoding encoding, bool leaveOpen) : base(stream, encoding, leaveOpen) { }
#endif
    }
}