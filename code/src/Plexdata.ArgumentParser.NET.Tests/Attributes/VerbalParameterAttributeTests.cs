﻿/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
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
using Plexdata.ArgumentParser.Attributes;
using Plexdata.ArgumentParser.Constants;
using Plexdata.ArgumentParser.Exceptions;
using System;

namespace Plexdata.ArgumentParser.Tests.Attributes
{
    [TestFixture]
    [TestOf(nameof(VerbalParameterAttribute))]
    public class VerbalParameterAttributeTests
    {
        [Test]
        [TestCase("value")]
        [TestCase("   value   ")]
        [TestCase(ParameterPrefixes.SolidPrefix + "value")]
        [TestCase(ParameterPrefixes.BriefPrefix + "value")]
        [TestCase(ParameterPrefixes.OtherPrefix + "value")]
        public void SolidLabel_SetProperty_ThrowsException(String actual)
        {
            VerbalParameterAttribute attribute = new VerbalParameterAttribute();
            Assert.Throws<VerbalAttributeException>(() => { attribute.SolidLabel = actual; });
        }

        [Test]
        [TestCase("value")]
        [TestCase("   value   ")]
        [TestCase(ParameterPrefixes.SolidPrefix + "value")]
        [TestCase(ParameterPrefixes.BriefPrefix + "value")]
        [TestCase(ParameterPrefixes.OtherPrefix + "value")]
        public void BriefLabel_SetProperty_ThrowsException(String actual)
        {
            VerbalParameterAttribute attribute = new VerbalParameterAttribute();
            Assert.Throws<VerbalAttributeException>(() => { attribute.BriefLabel = actual; });
        }
    }
}
