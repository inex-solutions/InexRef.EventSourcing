﻿#region Copyright & Licence
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Threading;
using InexRef.EventSourcing.Common;

namespace InexRef.EventSourcing.Tests.Common
{
    public class DeterministicallyIncreasingDateTimeProvider : IDateTimeProvider
    {
        private readonly Lazy<DateTime> _lazyDateTime = new Lazy<DateTime>(() => DateTime.UtcNow);
        private int _numCalls;

        public DateTime GetUtcNow()
        {
            var dt = _lazyDateTime.Value;
            var numCalls = Interlocked.Increment(ref _numCalls);
            return dt.AddSeconds(numCalls);
        }
    }
}