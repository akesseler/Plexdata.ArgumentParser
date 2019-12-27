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
using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Processors;
using System;

namespace Plexdata.ArgumentParser.Tests.Processors
{
    [TestFixture]
    [TestOf(nameof(ArgumentProcessorInitialize))]
    public class ArgumentProcessorInitializeTests
    {
        private class ArgumentProcessorInitialize
        {
        }

        [ParametersGroup]
        private class TestClassZeroProperties
        {
        }

        [ParametersGroup]
        private class TestClassUntaggedProperties
        {
            public Boolean Property1 { get; set; }
            public String Property2 { get; set; }
        }

        [ParametersGroup]
        private class TestClassSwitchProperties
        {
            [SwitchParameter(SolidLabel = "switch1")]
            public Boolean Switch1 { get; set; }
            [SwitchParameter(SolidLabel = "switch2")]
            public Boolean? Switch2 { get; set; }
        }

        [ParametersGroup]
        private class TestClassSwitchPropertiesWrongType
        {
            [SwitchParameter(SolidLabel = "switch1")]
            public Int32 Switch1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassSwitchPropertiesMissingLabels
        {
            [SwitchParameter]
            public Boolean Switch1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassSwitchPropertiesEmptyLabel
        {
            [SwitchParameter(SolidLabel = "")]
            public Boolean Switch1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassSwitchPropertiesSameSolidLabels
        {
            [SwitchParameter(SolidLabel = "same-label")]
            public Boolean Switch1 { get; set; }

            [SwitchParameter(SolidLabel = "same-label")]
            public Boolean Switch2 { get; set; }
        }

        [ParametersGroup]
        private class TestClassSwitchPropertiesSameBriefLabels
        {
            [SwitchParameter(BriefLabel = "sl")]
            public Boolean Switch1 { get; set; }

            [SwitchParameter(BriefLabel = "sl")]
            public Boolean Switch2 { get; set; }
        }

        [ParametersGroup]
        private class TestClassOptionProperties
        {
            [OptionParameter(SolidLabel = "option1")]
            public Int32 Option1 { get; set; }
            [OptionParameter(SolidLabel = "option2")]
            public Int32? Option2 { get; set; }
        }

        [ParametersGroup]
        private class TestClassOptionPropertiesWrongType
        {
            [OptionParameter(SolidLabel = "option1")]
            public Boolean Option1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassOptionPropertiesMissingLabels
        {
            [OptionParameter]
            public Int32 Option1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassOptionPropertiesEmptyLabel
        {
            [OptionParameter(SolidLabel = "")]
            public Int32 Option1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassOptionPropertiesSameSolidLabels
        {
            [OptionParameter(SolidLabel = "same-label")]
            public Int32 Option1 { get; set; }

            [OptionParameter(SolidLabel = "same-label")]
            public Int32 Option2 { get; set; }
        }

        [ParametersGroup]
        private class TestClassOptionPropertiesSameBriefLabels
        {
            [OptionParameter(BriefLabel = "sl")]
            public Int32 Option1 { get; set; }

            [OptionParameter(BriefLabel = "sl")]
            public Int32 Option2 { get; set; }
        }

        [ParametersGroup]
        private class TestClassVerbalProperties
        {
            [VerbalParameter]
            public String[] Verbal1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassVerbalPropertiesWrongType
        {
            [VerbalParameter]
            public Boolean[] Verbal1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassVerbalPropertiesManyTypes
        {
            [VerbalParameter]
            public String[] Verbal1 { get; set; }
            [VerbalParameter]
            public String[] Verbal2 { get; set; }
        }

        [ParametersGroup]
        private class TestClassVerbalPropertiesUsedLabel
        {
            [VerbalParameter(SolidLabel = "verbal1")]
            public Boolean[] Verbal1 { get; set; }
        }

        private class UnsupportedPropertyAttribute : ParameterObjectAttribute
        {
            public UnsupportedPropertyAttribute() : base() { }
        }

        [ParametersGroup]
        private class TestClassOptionPropertiesUnsupportedPropertyAttribute
        {
            [UnsupportedProperty]
            public Int32 Option1 { get; set; }
        }

        [Test]
        public void Initialize_TestClassIsNull_ThrowsException()
        {
            Object instance = null;
            Assert.Throws<ArgumentNullException>(() => { ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(instance); });
        }

        [Test]
        public void Initialize_TestClassZeroProperties_ResultIsZeroSettingsCount()
        {
            TestClassZeroProperties instance = new TestClassZeroProperties();
            ArgumentProcessor<TestClassZeroProperties> processor = new ArgumentProcessor<TestClassZeroProperties>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 0);
        }

        [Test]
        public void Initialize_TestClassUntaggedProperties_ResultIsZeroSettingsCount()
        {
            TestClassUntaggedProperties instance = new TestClassUntaggedProperties();
            ArgumentProcessor<TestClassUntaggedProperties> processor = new ArgumentProcessor<TestClassUntaggedProperties>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 0);
        }

        [Test]
        public void Initialize_TestClassSwitchProperties_ResultIsZeroSettingsCount()
        {
            TestClassSwitchProperties instance = new TestClassSwitchProperties();
            ArgumentProcessor<TestClassSwitchProperties> processor = new ArgumentProcessor<TestClassSwitchProperties>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 2);
        }

        [Test]
        public void Initialize_TestClassSwitchPropertiesWrongType_ThrowsException()
        {
            TestClassSwitchPropertiesWrongType instance = new TestClassSwitchPropertiesWrongType();
            ArgumentProcessor<TestClassSwitchPropertiesWrongType> processor = new ArgumentProcessor<TestClassSwitchPropertiesWrongType>(instance);
            Assert.Throws<SupportViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassSwitchPropertiesMissingLabels_ThrowsException()
        {
            TestClassSwitchPropertiesMissingLabels instance = new TestClassSwitchPropertiesMissingLabels();
            ArgumentProcessor<TestClassSwitchPropertiesMissingLabels> processor = new ArgumentProcessor<TestClassSwitchPropertiesMissingLabels>(instance);
            Assert.Throws<UtilizeViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassSwitchPropertiesEmptyLabel_ThrowsException()
        {
            TestClassSwitchPropertiesEmptyLabel instance = new TestClassSwitchPropertiesEmptyLabel();
            ArgumentProcessor<TestClassSwitchPropertiesEmptyLabel> processor = new ArgumentProcessor<TestClassSwitchPropertiesEmptyLabel>(instance);
            Assert.Throws<SwitchAttributeException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassSwitchPropertiesSameSolidLabels_ThrowsException()
        {
            TestClassSwitchPropertiesSameSolidLabels instance = new TestClassSwitchPropertiesSameSolidLabels();
            ArgumentProcessor<TestClassSwitchPropertiesSameSolidLabels> processor = new ArgumentProcessor<TestClassSwitchPropertiesSameSolidLabels>(instance);
            Assert.Throws<UtilizeViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassSwitchPropertiesSameBriefLabels_ThrowsException()
        {
            TestClassSwitchPropertiesSameBriefLabels instance = new TestClassSwitchPropertiesSameBriefLabels();
            ArgumentProcessor<TestClassSwitchPropertiesSameBriefLabels> processor = new ArgumentProcessor<TestClassSwitchPropertiesSameBriefLabels>(instance);
            Assert.Throws<UtilizeViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassOptionProperties_ResultIsZeroSettingsCount()
        {
            TestClassOptionProperties instance = new TestClassOptionProperties();
            ArgumentProcessor<TestClassOptionProperties> processor = new ArgumentProcessor<TestClassOptionProperties>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 2);
        }

        [Test]
        public void Initialize_TestClassOptionPropertiesWrongType_ThrowsException()
        {
            TestClassOptionPropertiesWrongType instance = new TestClassOptionPropertiesWrongType();
            ArgumentProcessor<TestClassOptionPropertiesWrongType> processor = new ArgumentProcessor<TestClassOptionPropertiesWrongType>(instance);
            Assert.Throws<SupportViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassOptionPropertiesMissingLabels_ThrowsException()
        {
            TestClassOptionPropertiesMissingLabels instance = new TestClassOptionPropertiesMissingLabels();
            ArgumentProcessor<TestClassOptionPropertiesMissingLabels> processor = new ArgumentProcessor<TestClassOptionPropertiesMissingLabels>(instance);
            Assert.Throws<UtilizeViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassOptionPropertiesEmptyLabel_ThrowsException()
        {
            TestClassOptionPropertiesEmptyLabel instance = new TestClassOptionPropertiesEmptyLabel();
            ArgumentProcessor<TestClassOptionPropertiesEmptyLabel> processor = new ArgumentProcessor<TestClassOptionPropertiesEmptyLabel>(instance);
            Assert.Throws<OptionAttributeException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassOptionPropertiesSameSolidLabels_ThrowsException()
        {
            TestClassOptionPropertiesSameSolidLabels instance = new TestClassOptionPropertiesSameSolidLabels();
            ArgumentProcessor<TestClassOptionPropertiesSameSolidLabels> processor = new ArgumentProcessor<TestClassOptionPropertiesSameSolidLabels>(instance);
            Assert.Throws<UtilizeViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassOptionPropertiesSameBriefLabels_ThrowsException()
        {
            TestClassOptionPropertiesSameBriefLabels instance = new TestClassOptionPropertiesSameBriefLabels();
            ArgumentProcessor<TestClassOptionPropertiesSameBriefLabels> processor = new ArgumentProcessor<TestClassOptionPropertiesSameBriefLabels>(instance);
            Assert.Throws<UtilizeViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassVerbalProperties_ResultIsZeroSettingsCount()
        {
            TestClassVerbalProperties instance = new TestClassVerbalProperties();
            ArgumentProcessor<TestClassVerbalProperties> processor = new ArgumentProcessor<TestClassVerbalProperties>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 1);
        }

        [Test]
        public void Initialize_TestClassVerbalPropertiesWrongType_ThrowsException()
        {
            TestClassVerbalPropertiesWrongType instance = new TestClassVerbalPropertiesWrongType();
            ArgumentProcessor<TestClassVerbalPropertiesWrongType> processor = new ArgumentProcessor<TestClassVerbalPropertiesWrongType>(instance);
            Assert.Throws<SupportViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassVerbalPropertiesManyTypes_ThrowsException()
        {
            TestClassVerbalPropertiesManyTypes instance = new TestClassVerbalPropertiesManyTypes();
            ArgumentProcessor<TestClassVerbalPropertiesManyTypes> processor = new ArgumentProcessor<TestClassVerbalPropertiesManyTypes>(instance);
            Assert.Throws<VerbalViolationException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassVerbalPropertiesUsedLabel_ThrowsException()
        {
            TestClassVerbalPropertiesUsedLabel instance = new TestClassVerbalPropertiesUsedLabel();
            ArgumentProcessor<TestClassVerbalPropertiesUsedLabel> processor = new ArgumentProcessor<TestClassVerbalPropertiesUsedLabel>(instance);
            Assert.Throws<VerbalAttributeException>(() => { processor.Initialize(); });
        }

        [Test]
        public void Initialize_TestClassOptionPropertiesUnsupportedPropertyAttribute_ThrowsException()
        {
            TestClassOptionPropertiesUnsupportedPropertyAttribute instance = new TestClassOptionPropertiesUnsupportedPropertyAttribute();
            ArgumentProcessor<TestClassOptionPropertiesUnsupportedPropertyAttribute> processor = new ArgumentProcessor<TestClassOptionPropertiesUnsupportedPropertyAttribute>(instance);
            Assert.Throws<SupportViolationException>(() => { processor.Initialize(); });
        }
    }
}
