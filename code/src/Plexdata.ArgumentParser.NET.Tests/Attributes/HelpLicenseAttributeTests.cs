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
using System;

namespace Plexdata.ArgumentParser.Tests.Attributes
{
    [TestFixture]
    [TestOf(nameof(HelpLicenseAttribute))]
    public class HelpLicenseAttributeTests
    {
        [Test]
        public void HelpLicense_DefaultConstruction_ResultIsDefaultContent()
        {
            HelpLicenseAttribute attribute = new HelpLicenseAttribute();
            Assert.AreEqual(attribute.Content, "Copyright © <company>");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpLicense_Construction_ResultIsEmptyContent(String actual)
        {
            HelpLicenseAttribute attribute = new HelpLicenseAttribute(actual);
            Assert.IsEmpty(attribute.Content);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpLicense_SetProperty_ResultIsEmptyContent(String actual)
        {
            HelpLicenseAttribute attribute = new HelpLicenseAttribute();
            attribute.Content = actual;
            Assert.IsEmpty(attribute.Content);
        }

        [Test]
        [TestCase("Hello World")]
        [TestCase("  Hello World ")]
        [TestCase("  \t \v Hello World \n\r  ")]
        public void HelpLicense_SetProperty_TrimmedContent(String actual)
        {
            HelpLicenseAttribute attribute = new HelpLicenseAttribute();
            attribute.Content = actual;
            Assert.AreEqual(attribute.Content, "Hello World");
        }
    }
}
