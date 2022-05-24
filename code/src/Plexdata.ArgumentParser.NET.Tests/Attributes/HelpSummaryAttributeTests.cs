/*
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
using System;

namespace Plexdata.ArgumentParser.Tests.Attributes
{
    [TestFixture]
    [TestOf(nameof(HelpSummaryAttribute))]
    public class HelpSummaryAttributeTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpSummary_Construction_ResultIsEmptyContent(String actual)
        {
            HelpSummaryAttribute attribute = new HelpSummaryAttribute(actual);
            Assert.IsEmpty(attribute.Content);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpSummary_SetProperty_ResultIsEmptyContent(String actual)
        {
            HelpSummaryAttribute attribute = new HelpSummaryAttribute();
            attribute.Content = actual;
            Assert.IsEmpty(attribute.Content);
        }

        [Test]
        [TestCase("Hello World")]
        [TestCase("  Hello World ")]
        [TestCase("  \t \v Hello World \n\r  ")]
        public void HelpSummary_SetProperty_TrimmedContent(String actual)
        {
            HelpSummaryAttribute attribute = new HelpSummaryAttribute();
            attribute.Content = actual;
            Assert.AreEqual(attribute.Content, "Hello World");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpSummary_SetProperty_ResultIsEmptyHeading(String actual)
        {
            HelpSummaryAttribute attribute = new HelpSummaryAttribute();
            attribute.Heading = actual;
            Assert.IsEmpty(attribute.Heading);
        }

        [Test]
        [TestCase("Hello World")]
        [TestCase("  Hello World ")]
        [TestCase("  \t \v Hello World \n\r  ")]
        public void HelpSummary_SetProperty_TrimmedHeading(String actual)
        {
            HelpSummaryAttribute attribute = new HelpSummaryAttribute();
            attribute.Heading = actual;
            Assert.AreEqual(attribute.Heading, "Hello World");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpSummary_SetProperty_ResultIsEmptySection(String actual)
        {
            HelpSummaryAttribute attribute = new HelpSummaryAttribute();
            attribute.Section = actual;
            Assert.IsEmpty(attribute.Section);
        }

        [Test]
        [TestCase("Hello World")]
        [TestCase("  Hello World ")]
        [TestCase("  \t \v Hello World \n\r  ")]
        public void HelpSummary_SetProperty_TrimmedSection(String actual)
        {
            HelpSummaryAttribute attribute = new HelpSummaryAttribute();
            attribute.Section = actual;
            Assert.AreEqual(attribute.Section, "Hello World");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  \t \v \n\r  ")]
        public void HelpSummary_SetProperty_ResultIsEmptyOptions(String actual)
        {
            HelpSummaryAttribute attribute = new HelpSummaryAttribute();
            attribute.Options = actual;
            Assert.IsEmpty(attribute.Options);
        }

        [Test]
        [TestCase("Hello World")]
        [TestCase("  Hello World ")]
        [TestCase("  \t \v Hello World \n\r  ")]
        public void HelpSummary_SetProperty_TrimmedOptions(String actual)
        {
            HelpSummaryAttribute attribute = new HelpSummaryAttribute();
            attribute.Options = actual;
            Assert.AreEqual(attribute.Options, "Hello World");
        }
    }
}
