/*
 * MIT License
 * 
 * Copyright (c) 2020 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using NUnit.Framework;
using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Extensions;
using System;
using System.Collections.Generic;

namespace Plexdata.ArgumentParser.Tests.Extensions
{
    [TestFixture]
    [TestOf(nameof(ValidationExtension))]
    public class ValidationExtensionTests
    {
        private static readonly Object[] NullObjects = { (Object)null, (String)null, (Object[])null, (String[])null, (Int32[])null, };
        private static readonly IEnumerable<Object>[] EmptyObjects = { new Object[] { }, new String[] { }, };
        private static readonly String[] EmptyStrings = { (String)null, String.Empty, " ", "  \t \v \n\r  ", };
        private static readonly Exception[] ExceptionList = {
            new NotImplementedException("Outer Exception", new ArgumentException("Inner Exception", new ArgumentParserException("Expected Exception"))),
            new ArgumentException("Outer Exception", new ArgumentParserException("Expected Exception", new NotImplementedException("Inner Exception"))),
            new ArgumentParserException("Expected Exception", new NotImplementedException("Outer Exception", new ArgumentException("Inner Exception"))),
        };

        [Test]
        [TestCaseSource(nameof(NullObjects))]
        public void ThrowIfNull_MultipleTypes_ThrowsException(Object actual)
        {
            Assert.Throws<ArgumentNullException>(() => { actual.ThrowIfNull(); });
        }

        [Test]
        [TestCaseSource(nameof(EmptyObjects))]
        public void ThrowIfNullOrEmpty_MultipleTypes_ThrowsException(IEnumerable<Object> actual)
        {
            Assert.Throws<ArgumentNullException>(() => { actual.ThrowIfNullOrEmpty(); });
        }

        [Test]
        [TestCaseSource(nameof(EmptyStrings))]
        public void ThrowIfNullOrWhiteSpace_MultipleTypes_ThrowsException(String actual)
        {
            Assert.Throws<ArgumentNullException>(() => { actual.ThrowIfNullOrWhiteSpace(); });
        }

        [Test]
        [TestCaseSource(nameof(ExceptionList))]
        public void ThrowArgumentParserException_MultipleTypes_ThrowsException(Exception exception)
        {
            Assert.Throws<ArgumentParserException>(() => { exception.ThrowArgumentParserException(); });
        }
    }
}
