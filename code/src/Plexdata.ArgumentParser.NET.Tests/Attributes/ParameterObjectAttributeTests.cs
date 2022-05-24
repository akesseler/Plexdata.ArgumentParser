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
using System;
using System.Linq;

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

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SolidLabel_ValueInvalid_ThrowsException(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(() => actual.SolidLabel = value, Throws.InstanceOf<NotImplementedException>());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void BriefLabel_ValueInvalid_ThrowsException(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(() => actual.BriefLabel = value, Throws.InstanceOf<NotImplementedException>());
        }

        [TestCase("--")]
        [TestCase("-")]
        [TestCase("/")]
        public void SolidLabel_LengthInvalid_ThrowsException(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(() => actual.SolidLabel = value, Throws.InstanceOf<NotImplementedException>());
        }

        [TestCase("--")]
        [TestCase("-")]
        [TestCase("/")]
        [TestCase("--,-,/")]
        [TestCase("-,--,/")]
        [TestCase("/,--,-")]
        public void BriefLabel_LengthInvalid_ThrowsException(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(() => actual.BriefLabel = value, Throws.InstanceOf<NotImplementedException>());
            Assert.That(actual.BriefLabels.Count(), Is.Zero);
        }

        [TestCase("--value")]
        [TestCase("-value")]
        [TestCase("/value")]
        [TestCase("  --value  ")]
        [TestCase("  -value   ")]
        [TestCase("  /value   ")]
        public void IsSolidLabel_ValueValid_ResultIsTrue(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { SolidLabel = value };

            Assert.That(actual.IsSolidLabel, Is.True);
        }

        [TestCase("value", "value")]
        [TestCase("   value   ", "value")]
        [TestCase("--value", "value")]
        [TestCase("-value", "value")]
        [TestCase("/value", "value")]
        [TestCase("  --value  ", "value")]
        [TestCase("  -value  ", "value")]
        [TestCase("  /value  ", "value")]
        public void SolidLabel_ValueValid_ResultAsExpected(String value, String expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { SolidLabel = value };

            Assert.That(actual.SolidLabel, Is.EqualTo(expected));
        }

        [TestCase("--value")]
        [TestCase("-value")]
        [TestCase("/value")]
        [TestCase("  --value  ")]
        [TestCase("  -value  ")]
        [TestCase("  /value  ")]
        public void IsBriefLabel_ValueValid_ResultIsTrue(String value)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { BriefLabel = value };

            Assert.That(actual.IsBriefLabel, Is.True);
        }

        [TestCase("value", "value")]
        [TestCase("   value   ", "value")]
        [TestCase("--value", "value")]
        [TestCase("-value", "value")]
        [TestCase("/value", "value")]
        [TestCase("  --value  ", "value")]
        [TestCase("  -value  ", "value")]
        [TestCase("  /value  ", "value")]
        public void BriefLabel_ValueValid_ResultAsExpected(String value, String expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { BriefLabel = value };

            Assert.That(actual.BriefLabel, Is.EqualTo(expected));
        }

        [TestCase("a,b,c", "a,b,c")]
        [TestCase("-a,-b,-c", "a,b,c")]
        [TestCase("   , a, b,,c", "a,b,c")]
        [TestCase("   , /a, -b,,--c", "a,b,c")]
        [TestCase(" -  , /a, -b,--,  ,--c, /,  ", "a,b,c")]
        public void BriefLabels_ValueWithCommaSeparatedList_ResultAsExpected(String value, String expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { BriefLabel = value };

            Assert.That(actual.BriefLabel, Is.EqualTo(expected));
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public void IsRequired_VariousValues_ResultAsExpected(Boolean value, Boolean expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { IsRequired = value };

            Assert.That(actual.IsRequired, Is.EqualTo(expected));
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public void IsExclusive_VariousValues_ResultAsExpected(Boolean value, Boolean expected)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { IsExclusive = value };

            Assert.That(actual.IsExclusive, Is.EqualTo(expected));
        }

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

        [Test]
        public void IsSolidLabelAndStartsWith_SolidLabelIsFalseAndOtherIsValid_ResultIsFalse()
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(actual.IsSolidLabelAndStartsWith("other"), Is.False);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void IsSolidLabelAndStartsWith_SolidLabelIsTrueButOtherIsInvalid_ResultIsFalse(String other)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { SolidLabel = "value" };

            Assert.That(actual.IsSolidLabelAndStartsWith(other), Is.False);
        }

        [Test]
        public void IsSolidLabelAndStartsWith_SolidLabelIsTrueAndOtherIsDifferent_ResultIsFalse()
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { SolidLabel = "value" };

            Assert.That(actual.IsSolidLabelAndStartsWith("other"), Is.False);
        }

        [Test]
        public void IsSolidLabelAndStartsWith_SolidLabelIsTrueAndOtherIsSame_ResultIsTrue()
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { SolidLabel = "label" };

            Assert.That(actual.IsSolidLabelAndStartsWith("label"), Is.True);
        }

        [Test]
        public void IsSolidLabelAndStartsWith_SolidLabelIsTrueAndOtherIsSameButUpperCase_ResultIsFalse()
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { SolidLabel = "label" };

            Assert.That(actual.IsSolidLabelAndStartsWith("LABEL"), Is.False);
        }

        [Test]
        public void IsBriefLabelAndStartsWith_BriefLabelIsFalseAndOtherIsValid_ResultIsFalse()
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass();

            Assert.That(actual.IsBriefLabelAndStartsWith("other"), Is.False);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void IsBriefLabelAndStartsWith_BriefLabelIsTrueButOtherIsInvalid_ResultIsFalse(String other)
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { BriefLabel = "value" };

            Assert.That(actual.IsBriefLabelAndStartsWith(other), Is.False);
        }

        [Test]
        public void IsBriefLabelAndStartsWith_BriefLabelIsTrueAndOtherIsDifferent_ResultIsFalse()
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { BriefLabel = "value" };

            Assert.That(actual.IsBriefLabelAndStartsWith("other"), Is.False);
        }

        [Test]
        public void IsBriefLabelAndStartsWith_BriefLabelIsTrueAndOtherIsSame_ResultIsTrue()
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { BriefLabel = "label" };

            Assert.That(actual.IsBriefLabelAndStartsWith("label"), Is.True);
        }

        [Test]
        public void IsBriefLabelAndStartsWith_BriefLabelIsTrueAndOtherIsSameButUpperCase_ResultIsFalse()
        {
            ParameterObjectAttribute actual = new ParameterObjectAttributeTestClass() { BriefLabel = "label" };

            Assert.That(actual.IsBriefLabelAndStartsWith("LABEL"), Is.False);
        }

        private class ParameterObjectAttributeTestClass : ParameterObjectAttribute
        {
            public ParameterObjectAttributeTestClass() : base() { }
        }
    }
}
