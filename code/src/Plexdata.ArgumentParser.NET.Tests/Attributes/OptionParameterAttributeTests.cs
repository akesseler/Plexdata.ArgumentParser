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
using Plexdata.ArgumentParser.Attributes;
using Plexdata.ArgumentParser.Constants;
using Plexdata.ArgumentParser.Exceptions;
using System;

namespace Plexdata.ArgumentParser.Tests.Attributes
{
    [TestFixture]
    [TestOf(nameof(OptionParameterAttribute))]
    public class OptionParameterAttributeTests
    {
        [Test]
        public void OptionParameter_Construction_ResultAsExpected()
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute();
            Assert.That(attribute.Separator, Is.EqualTo(ParameterSeparators.DefaultSeparator));
            Assert.That(attribute.Delimiter, Is.EqualTo(ArgumentDelimiters.DefaultDelimiter));
            Assert.That(attribute.DefaultValue, Is.Null);
            Assert.That(attribute.HasDefaultValue, Is.False);
        }

        [Test]
        [TestCase('\u0000')]
        [TestCase('\u001F')]
        [TestCase('\u007F')]
        [TestCase('\u0080')]
        [TestCase('\u009F')]
        public void Separator_SetProperty_ThrowsException(Char actual)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute();
            Assert.That(() => { attribute.Separator = actual; }, Throws.InstanceOf<OptionAttributeException>());
        }

        [Test]
        [TestCase('#', '#')]
        [TestCase(ParameterSeparators.SpaceSeparator, ParameterSeparators.SpaceSeparator)]
        [TestCase(ParameterSeparators.ColonSeparator, ParameterSeparators.ColonSeparator)]
        [TestCase(ParameterSeparators.EqualSeparator, ParameterSeparators.EqualSeparator)]
        public void Separator_SetProperty_ResultIsEqual(Char actual, Char expected)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { Separator = actual };
            Assert.That(attribute.Separator, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("#", "#")]
        [TestCase(ArgumentDelimiters.CommaDelimiter, ArgumentDelimiters.CommaDelimiter)]
        [TestCase(ArgumentDelimiters.ColonDelimiter, ArgumentDelimiters.ColonDelimiter)]
        public void Delimiter_SetProperty_ResultIsEqual(String actual, String expected)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { Delimiter = actual };
            Assert.That(attribute.Delimiter, Is.EqualTo(expected));
        }

        [Test]
        public void DefaultValue_NotChanged_HasDefaultValueIsFalse()
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute();
            Assert.That(attribute.HasDefaultValue, Is.False);
        }

        [Test]
        public void DefaultValue_HasChanged_HasDefaultValueIsTrue()
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { DefaultValue = null };
            Assert.That(attribute.HasDefaultValue, Is.True);
        }
    }
}
