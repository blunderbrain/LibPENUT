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
    /// Helper class that prevents the wrapped stream object from being closed when the wrapper is closed/disposed
    /// </summary>
    internal class NonClosingStreamWrapper : Stream
    {

        public NonClosingStreamWrapper(Stream wrappedStream)
        {
            WrappedStream = wrappedStream;
        }
        public Stream WrappedStream
        {
            get; private set;
        }

        public override bool CanRead
        {
            get { return WrappedStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return WrappedStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return WrappedStream.CanWrite; }
        }

        public override long Length
        {
            get { return WrappedStream.Length; }
        }

        public override long Position
        {
            get { return WrappedStream.Position; }
            set { WrappedStream.Position = value; }
        }

        public override void Flush()
        {
            WrappedStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return WrappedStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return WrappedStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            WrappedStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            WrappedStream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Removes the reference to the wrapped stream but does not close it
        /// </summary>
        public override void Close()
        {
            WrappedStream = null;
        }

    }
}
