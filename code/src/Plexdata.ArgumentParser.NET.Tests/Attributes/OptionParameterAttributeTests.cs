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

        [TestCase(0x0000)] // '\u0000'
        [TestCase(0x001F)] // '\u001F'
        [TestCase(0x007F)] // '\u007F'
        [TestCase(0x0080)] // '\u0080'
        [TestCase(0x009F)] // '\u009F'
        public void Separator_SetProperty_ThrowsException(Int16 actual)
        {
            // The test explorer may have an issue with unicode characters.
            OptionParameterAttribute attribute = new OptionParameterAttribute();
            Assert.That(() => { attribute.Separator = (Char)actual; }, Throws.InstanceOf<OptionAttributeException>());
        }

        [TestCase('#', '#')]
        [TestCase(' ', ' ')]
        [TestCase(':', ':')]
        [TestCase('=', '=')]
        public void Separator_SetProperty_ResultIsEqual(Char actual, Char expected)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { Separator = actual };
            Assert.That(attribute.Separator, Is.EqualTo(expected));
        }

        [TestCase(null, "")]
        [TestCase("#", "#")]
        [TestCase(",", ",")]
        [TestCase(":", ":")]
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

        [TestCase("other")]
        [TestCase("other:")]
        [TestCase("other:value")]
        public void IsSolidLabelAndStartsWith_SolidLabelIsValidAndSeparatorIsColonAndOtherIsDifferent_ResultIsFalse(String other)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { SolidLabel = "label", Separator = ':' };

            Assert.That(attribute.IsSolidLabelAndStartsWith(other), Is.False);
        }

        [TestCase("label")]
        [TestCase("label:")]
        [TestCase("label:value")]
        public void IsSolidLabelAndStartsWith_SolidLabelIsValidAndSeparatorIsColonAndOtherIsSame_ResultIsTrue(String other)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { SolidLabel = "label", Separator = ':' };

            Assert.That(attribute.IsSolidLabelAndStartsWith(other), Is.True);
        }

        [TestCase("other")]
        [TestCase("other=")]
        [TestCase("other=value")]
        public void IsSolidLabelAndStartsWith_SolidLabelIsValidAndSeparatorIsEqualAndOtherIsDifferent_ResultIsFalse(String other)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { SolidLabel = "label", Separator = '=' };

            Assert.That(attribute.IsSolidLabelAndStartsWith(other), Is.False);
        }

        [TestCase("label")]
        [TestCase("label=")]
        [TestCase("label=value")]
        public void IsSolidLabelAndStartsWith_SolidLabelIsValidAndSeparatorIsEqualAndOtherIsSame_ResultIsTrue(String other)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { SolidLabel = "label", Separator = '=' };

            Assert.That(attribute.IsSolidLabelAndStartsWith(other), Is.True);
        }

        [TestCase("other")]
        [TestCase("other:")]
        [TestCase("other:value")]
        public void IsBriefLabelAndStartsWith_BriefLabelIsValidAndSeparatorIsColonAndOtherIsDifferent_ResultIsFalse(String other)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { BriefLabel = "label", Separator = ':' };

            Assert.That(attribute.IsBriefLabelAndStartsWith(other), Is.False);
        }

        [TestCase("label")]
        [TestCase("label:")]
        [TestCase("label:value")]
        public void IsBriefLabelAndStartsWith_BriefLabelIsValidAndSeparatorIsColonAndOtherIsSame_ResultIsTrue(String other)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { BriefLabel = "label", Separator = ':' };

            Assert.That(attribute.IsBriefLabelAndStartsWith(other), Is.True);
        }

        [TestCase("other")]
        [TestCase("other=")]
        [TestCase("other=value")]
        public void IsBriefLabelAndStartsWith_BriefLabelIsValidAndSeparatorIsEqualAndOtherIsDifferent_ResultIsFalse(String other)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { BriefLabel = "label", Separator = '=' };

            Assert.That(attribute.IsBriefLabelAndStartsWith(other), Is.False);
        }

        [TestCase("label")]
        [TestCase("label=")]
        [TestCase("label=value")]
        public void IsBriefLabelAndStartsWith_BriefLabelIsValidAndSeparatorIsEqualAndOtherIsSame_ResultIsTrue(String other)
        {
            OptionParameterAttribute attribute = new OptionParameterAttribute() { BriefLabel = "label", Separator = '=' };

            Assert.That(attribute.IsBriefLabelAndStartsWith(other), Is.True);
        }
    }
}
