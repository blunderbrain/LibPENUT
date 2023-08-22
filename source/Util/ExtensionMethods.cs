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
    /// Various extension methods for convenience
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Pad the value to the nearest multiple of alignment
        /// </summary>
        public static UInt32 AlignTo(this UInt32 value,  UInt32 alignment)
        {
            if (alignment == 0 || value % alignment == 0)
                return value;
            else
                return value - (value % alignment) + alignment;
        }

        /// <summary>
        /// Returns the number of seconds from to the Unix epoch 1/1 1970 to the supplied DateTime
        /// </summary>
        public static long ToUnixTime(this DateTime value)
        {
            // The constant 621355968000000000 is the number of Ticks from DateTime.MinValue to 1/1 1970
            return (value.Ticks - 621355968000000000) / 10000000;
        }
        
        /// <summary>
        /// Returns a new DateTime representing the Unix time expressed as seconds from 1/1 1970
        /// </summary>
        public static DateTime FromUnixTime(this DateTime value, long unixTime)
        {
            return new DateTime(unixTime * 10000000 + 621355968000000000);
        }
    }
}
