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
using Plexdata.ArgumentParser.Processors;
using System;

namespace Plexdata.ArgumentParser.Tests
{
    [TestFixture]
    [TestOf(nameof(HelpProcessorInitialize))]
    public class HelpProcessorInitializeTests
    {
        private class HelpProcessorInitialize
        {
        }

        private class TestClassZeroProperties
        {
        }

        private class TestClassUntaggedProperties
        {
            public Boolean Property1 { get; set; }
            public String Property2 { get; set; }
        }

        private class TestClassHelpSummaryNoParameters
        {
            [HelpSummary]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            public String Property2 { get; set; }
        }

        private class TestClassHelpTwoSummaryAndParameters
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        private class TestClassThreeHelpSummaryAndParameters
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property3")]
            public String Property3 { get; set; }
        }

        [HelpLicense]
        private class TestClassOnlyHelpLicense
        {
        }
        [HelpPreface]
        private class TestClassOnlyHelpPreface
        {
        }

        [HelpClosure]
        private class TestClassOnlyHelpClosure
        {
        }

        [HelpUtilize]
        private class TestClassOnlyOneHelpUtilize
        {
        }

        [HelpUtilize]
        [HelpUtilize]
        private class TestClassOnlyTwoHelpUtilize
        {
        }

        [Test]
        public void Initialize_TestClassIsNull_ThrowsException()
        {
            Object instance = null;
            Assert.Throws<ArgumentNullException>(() => { HelpProcessor<Object> processor = new HelpProcessor<Object>(instance); });
        }

        [Test]
        public void Initialize_TestClassZeroProperties_ResultIsZeroSettingsCount()
        {
            TestClassZeroProperties instance = new TestClassZeroProperties();
            HelpProcessor<TestClassZeroProperties> processor = new HelpProcessor<TestClassZeroProperties>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 0);
        }

        [Test]
        public void Initialize_TestClassUntaggedProperties_ResultIsZeroSettingsCount()
        {
            TestClassUntaggedProperties instance = new TestClassUntaggedProperties();
            HelpProcessor<TestClassUntaggedProperties> processor = new HelpProcessor<TestClassUntaggedProperties>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 0);
        }

        [Test]
        public void Initialize_TestClassHelpSummaryNoParameters_ResultIsOneSettingsCount()
        {
            TestClassHelpSummaryNoParameters instance = new TestClassHelpSummaryNoParameters();
            HelpProcessor<TestClassHelpSummaryNoParameters> processor = new HelpProcessor<TestClassHelpSummaryNoParameters>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 0);
        }

        [Test]
        public void Initialize_TestClassHelpTwoSummaryAndParameters_ResultIsTwoSettingsCount()
        {
            TestClassHelpTwoSummaryAndParameters instance = new TestClassHelpTwoSummaryAndParameters();
            HelpProcessor<TestClassHelpTwoSummaryAndParameters> processor = new HelpProcessor<TestClassHelpTwoSummaryAndParameters>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 2);
        }

        [Test]
        public void Initialize_TestClassThreeHelpSummaryAndParameters_ResultIsThreeSettingsCount()
        {
            TestClassThreeHelpSummaryAndParameters instance = new TestClassThreeHelpSummaryAndParameters();
            HelpProcessor<TestClassThreeHelpSummaryAndParameters> processor = new HelpProcessor<TestClassThreeHelpSummaryAndParameters>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Settings.Count, 3);
        }

        [Test]
        public void Initialize_TestClassOnlyHelpLicense_ResultIsLicenseNotNull()
        {
            TestClassOnlyHelpLicense instance = new TestClassOnlyHelpLicense();
            HelpProcessor<TestClassOnlyHelpLicense> processor = new HelpProcessor<TestClassOnlyHelpLicense>(instance);
            processor.Initialize();
            Assert.AreNotEqual(processor.License, null);
        }

        [Test]
        public void Initialize_TestClassOnlyHelpPreface_ResultIsPrefaceNotNull()
        {
            TestClassOnlyHelpPreface instance = new TestClassOnlyHelpPreface();
            HelpProcessor<TestClassOnlyHelpPreface> processor = new HelpProcessor<TestClassOnlyHelpPreface>(instance);
            processor.Initialize();
            Assert.AreNotEqual(processor.Preface, null);
        }

        [Test]
        public void Initialize_TestClassOnlyHelpClosure_ResultIsClosureNotNull()
        {
            TestClassOnlyHelpClosure instance = new TestClassOnlyHelpClosure();
            HelpProcessor<TestClassOnlyHelpClosure> processor = new HelpProcessor<TestClassOnlyHelpClosure>(instance);
            processor.Initialize();
            Assert.AreNotEqual(processor.Closure, null);
        }

        [Test]
        public void Initialize_TestClassOnlyOneHelpUtilize_ResultIsOneUtilizeCount()
        {
            TestClassOnlyOneHelpUtilize instance = new TestClassOnlyOneHelpUtilize();
            HelpProcessor<TestClassOnlyOneHelpUtilize> processor = new HelpProcessor<TestClassOnlyOneHelpUtilize>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Utilizes.Count, 1);
        }

        [Test]
        public void Initialize_TestClassOnlyTwoHelpUtilize_ResultIsTwoUtilizeCount()
        {
            TestClassOnlyTwoHelpUtilize instance = new TestClassOnlyTwoHelpUtilize();
            HelpProcessor<TestClassOnlyTwoHelpUtilize> processor = new HelpProcessor<TestClassOnlyTwoHelpUtilize>(instance);
            processor.Initialize();
            Assert.AreEqual(processor.Utilizes.Count, 2);
        }
    }
}
