/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
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

namespace Plexdata.ArgumentParser.Tests
{
    [TestFixture]
    [TestOf(nameof(SwitchParameterAttribute))]
    public class SwitchParameterAttributeTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        [TestCase(ParameterPrefixes.SolidPrefix)]
        [TestCase(ParameterPrefixes.BriefPrefix)]
        [TestCase(ParameterPrefixes.OtherPrefix)]
        public void SolidLabel_SetProperty_ThrowsException(String actual)
        {
            SwitchParameterAttribute attribute = new SwitchParameterAttribute();
            Assert.Throws<SwitchAttributeException>(() => { attribute.SolidLabel = actual; });
        }

        [Test]
        [TestCase("value", "value")]
        [TestCase("   value   ", "value")]
        [TestCase(ParameterPrefixes.SolidPrefix + "value", "value")]
        [TestCase(ParameterPrefixes.BriefPrefix + "value", "value")]
        [TestCase(ParameterPrefixes.OtherPrefix + "value", "value")]
        public void SolidLabel_SetProperty_ResultIsEqual(String actual, String expected)
        {
            SwitchParameterAttribute attribute = new SwitchParameterAttribute();
            attribute.SolidLabel = actual;
            Assert.AreEqual(attribute.SolidLabel, expected);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        [TestCase(ParameterPrefixes.SolidPrefix)]
        [TestCase(ParameterPrefixes.BriefPrefix)]
        [TestCase(ParameterPrefixes.OtherPrefix)]
        public void BriefLabel_SetProperty_ThrowsException(String actual)
        {
            SwitchParameterAttribute attribute = new SwitchParameterAttribute();
            Assert.Throws<SwitchAttributeException>(() => { attribute.BriefLabel = actual; });
        }

        [Test]
        [TestCase("value", "value")]
        [TestCase("   value   ", "value")]
        [TestCase(ParameterPrefixes.SolidPrefix + "value", "value")]
        [TestCase(ParameterPrefixes.BriefPrefix + "value", "value")]
        [TestCase(ParameterPrefixes.OtherPrefix + "value", "value")]
        public void BriefLabel_SetProperty_ResultIsEqual(String actual, String expected)
        {
            SwitchParameterAttribute attribute = new SwitchParameterAttribute();
            attribute.BriefLabel = actual;
            Assert.AreEqual(attribute.BriefLabel, expected);
        }
    }
}
