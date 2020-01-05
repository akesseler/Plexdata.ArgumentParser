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
using System;

namespace Plexdata.ArgumentParser.Tests.Attributes
{
    [TestFixture]
    [TestOf(nameof(ParameterObjectAttribute))]
    public class ParameterObjectAttributeTests
    {
        [Test]
        public void ParameterObjectAttribute_VerifyDefaultSettings_DefaultSettingsAreApplied()
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(actual.SolidLabel, Is.Empty);
            Assert.That(actual.IsSolidLabel, Is.False);
            Assert.That(actual.BriefLabel, Is.Empty);
            Assert.That(actual.IsBriefLabel, Is.False);
            Assert.That(actual.IsRequired, Is.False);
            Assert.That(actual.IsExclusive, Is.False);
            Assert.That(actual.DependencyList, Is.Empty);
            Assert.That(actual.DependencyType, Is.EqualTo(DependencyType.Optional));
            Assert.That(actual.IsDependencies, Is.False);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SolidLabel_ValueInvalid_ThrowsException(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(() => actual.SolidLabel = value, Throws.InstanceOf<NotImplementedException>());
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void BriefLabel_ValueInvalid_ThrowsException(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(() => actual.BriefLabel = value, Throws.InstanceOf<NotImplementedException>());
        }

        [Test]
        [TestCase(ParameterPrefixes.SolidPrefix)]
        [TestCase(ParameterPrefixes.BriefPrefix)]
        [TestCase(ParameterPrefixes.OtherPrefix)]
        public void SolidLabel_LengthInvalid_ThrowsException(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(() => actual.SolidLabel = value, Throws.InstanceOf<NotImplementedException>());
        }

        [Test]
        [TestCase(ParameterPrefixes.SolidPrefix)]
        [TestCase(ParameterPrefixes.BriefPrefix)]
        [TestCase(ParameterPrefixes.OtherPrefix)]
        public void BriefLabel_LengthInvalid_ThrowsException(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(() => actual.BriefLabel = value, Throws.InstanceOf<NotImplementedException>());
        }

        [Test]
        [TestCase(ParameterPrefixes.SolidPrefix + "value")]
        [TestCase(ParameterPrefixes.BriefPrefix + "value")]
        [TestCase(ParameterPrefixes.OtherPrefix + "value")]
        public void IsSolidLabel_ValueValid_ResultIsTrue(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { SolidLabel = value };

            Assert.That(actual.IsSolidLabel, Is.True);
        }

        [Test]
        [TestCase("value", "value")]
        [TestCase("   value   ", "value")]
        [TestCase(ParameterPrefixes.SolidPrefix + "value", "value")]
        [TestCase(ParameterPrefixes.BriefPrefix + "value", "value")]
        [TestCase(ParameterPrefixes.OtherPrefix + "value", "value")]
        public void SolidLabel_ValueValid_ResultAsExpected(String value, String expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { SolidLabel = value };

            Assert.That(actual.SolidLabel, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(ParameterPrefixes.SolidPrefix + "value")]
        [TestCase(ParameterPrefixes.BriefPrefix + "value")]
        [TestCase(ParameterPrefixes.OtherPrefix + "value")]
        public void IsBriefLabel_ValueValid_ResultIsTrue(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { BriefLabel = value };

            Assert.That(actual.IsBriefLabel, Is.True);
        }

        [Test]
        [TestCase("value", "value")]
        [TestCase("   value   ", "value")]
        [TestCase(ParameterPrefixes.SolidPrefix + "value", "value")]
        [TestCase(ParameterPrefixes.BriefPrefix + "value", "value")]
        [TestCase(ParameterPrefixes.OtherPrefix + "value", "value")]
        public void BriefLabel_ValueValid_ResultAsExpected(String value, String expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { BriefLabel = value };

            Assert.That(actual.BriefLabel, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void IsRequired_VariousValues_ResultAsExpected(Boolean value, Boolean expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { IsRequired = value };

            Assert.That(actual.IsRequired, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void IsExclusive_VariousValues_ResultAsExpected(Boolean value, Boolean expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { IsExclusive = value };

            Assert.That(actual.IsExclusive, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("  ", "")]
        [TestCase("dependency-list", "dependency-list")]
        [TestCase("  dependency-list  ", "dependency-list")]
        public void DependencyList_VariousValues_ResultAsExpected(String value, String expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { DependencyList = value };

            Assert.That(actual.DependencyList, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("  ", false)]
        [TestCase("dependency-list", true)]
        [TestCase("  dependency-list  ", true)]
        public void IsDependencies_VariousValues_ResultAsExpected(String value, Boolean expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { DependencyList = value };

            Assert.That(actual.IsDependencies, Is.EqualTo(expected));
        }

        private class ParameterObjectAttributeTestClass : ParameterObjectAttribute
        {
            public ParameterObjectAttributeTestClass() : base() { }
        }
    }
}
