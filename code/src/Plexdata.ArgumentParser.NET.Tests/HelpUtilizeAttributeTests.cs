/*
 * MIT License
 * 
 * Copyright (c) 2018 plexdata.de
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

namespace Plexdata.ArgumentParser.Tests
{
    [TestFixture]
    [TestOf(nameof(HelpUtilizeAttribute))]
    public class HelpUtilizeAttributeTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpUtilize_Construction_ResultIsEmptyContent(String actual)
        {
            HelpUtilizeAttribute attribute = new HelpUtilizeAttribute(actual);
            Assert.IsEmpty(attribute.Content);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpUtilize_SetProperty_ResultIsEmptyContent(String actual)
        {
            HelpUtilizeAttribute attribute = new HelpUtilizeAttribute();
            attribute.Content = actual;
            Assert.IsEmpty(attribute.Content);
        }

        [Test]
        [TestCase("Hello World")]
        [TestCase("  Hello World ")]
        [TestCase("  \t \v Hello World \n\r  ")]
        public void HelpUtilize_SetProperty_TrimmedContent(String actual)
        {
            HelpUtilizeAttribute attribute = new HelpUtilizeAttribute();
            attribute.Content = actual;
            Assert.AreEqual(attribute.Content, "Hello World");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpUtilize_SetProperty_ResultIsEmptyHeading(String actual)
        {
            HelpUtilizeAttribute attribute = new HelpUtilizeAttribute();
            attribute.Heading = actual;
            Assert.IsEmpty(attribute.Heading);
        }

        [Test]
        [TestCase("Hello World")]
        [TestCase("  Hello World ")]
        [TestCase("  \t \v Hello World \n\r  ")]
        public void HelpUtilize_SetProperty_TrimmedHeading(String actual)
        {
            HelpUtilizeAttribute attribute = new HelpUtilizeAttribute();
            attribute.Heading = actual;
            Assert.AreEqual(attribute.Heading, "Hello World");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpUtilize_SetProperty_ResultIsEmptySection(String actual)
        {
            HelpUtilizeAttribute attribute = new HelpUtilizeAttribute();
            attribute.Section = actual;
            Assert.IsEmpty(attribute.Section);
        }

        [Test]
        [TestCase("Hello World")]
        [TestCase("  Hello World ")]
        [TestCase("  \t \v Hello World \n\r  ")]
        public void HelpUtilize_SetProperty_TrimmedSection(String actual)
        {
            HelpUtilizeAttribute attribute = new HelpUtilizeAttribute();
            attribute.Section = actual;
            Assert.AreEqual(attribute.Section, "Hello World");
        }
    }
}
