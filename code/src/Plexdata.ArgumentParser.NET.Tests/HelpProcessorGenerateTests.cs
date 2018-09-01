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
using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Processors;
using System;

namespace Plexdata.ArgumentParser.Tests
{
    [TestFixture]
    [TestOf(nameof(HelpProcessorGenerate))]
    public class HelpProcessorGenerateTests
    {
        private class HelpProcessorGenerate
        {
        }

        private class TestClassWithEmptyHelpAttributes
        {
            [HelpSummary]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        private class TestClassWithMinimumHelpAttributes
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        private class TestClassWithShortHelpSummary
        {
            [HelpSummary("Short help on switch 1.")]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary("Short help on option 2.")]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        private class TestClassWithLongHelpSummary
        {
            [HelpSummary("Long help on switch 1. Long help on switch 1. Long help on switch 1. Long help on switch 1. Long help on switch 1. Long help on switch 1.")]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary("Long help on option 2. Long help on option 2. Long help on option 2. Long help on option 2.")]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        private class TestClassWithLongHelpSummaryDifferentParameterLength
        {
            [HelpSummary("Long help on switch 1. Long help on switch 1. Long help on switch 1. Long help on switch 1. Long help on switch 1. Long help on switch 1.")]
            [SwitchParameter(SolidLabel = "less")]
            public Boolean Property1 { get; set; }
            [HelpSummary("Long help on option 2. Long help on option 2. Long help on option 2. Long help on option 2.")]
            [OptionParameter(SolidLabel = "a-bit-longer")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        private class TestClassHelpSummaryAndOptionArguments
        {
            [HelpSummary("String option with additional arguments in help output.", Options = "str-val")]
            [OptionParameter(SolidLabel = "string-option", BriefLabel = "so")]
            public String StringOption { get; set; }
            [HelpSummary("Integer option with additional arguments in help output.", Options = "num-val")]
            [OptionParameter(SolidLabel = "int-option", BriefLabel = "io")]
            public Int32 IntOption { get; set; }
        }

        [HelpUtilize]
        private class TestClassHelpSectionAndSummary
        {
            [HelpSummary("Switch 1 in section A.", Section = "Section A")]
            [SwitchParameter(SolidLabel = "switcha1")]
            public Boolean SwitchA1 { get; set; }
            [HelpSummary("Switch 2 in section A.", Section = "Section A")]
            [SwitchParameter(SolidLabel = "switcha2")]
            public Boolean SwitchA2 { get; set; }
            [HelpSummary("Switch 3 in section A.", Section = "Section A")]
            [SwitchParameter(SolidLabel = "switcha3")]
            public Boolean SwitchA3 { get; set; }
            [HelpSummary("Switch 1 in section B.", Section = "Section B")]
            [SwitchParameter(SolidLabel = "switchb1")]
            public Boolean SwitchB1 { get; set; }
            [HelpSummary("Switch 2 in section B.", Section = "Section B")]
            [SwitchParameter(SolidLabel = "switchb2")]
            public Boolean SwitchB2 { get; set; }
        }

        [HelpUtilize]
        [HelpLicense("Copyright (c) company")]
        private class TestClassWithShortCopyright
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        [HelpPreface("This is a pretty short prologue.")]
        private class TestClassWithShortPrologue
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        [HelpClosure("This is a pretty short epilogue.")]
        private class TestClassWithShortEpilogue
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        [HelpLicense("Copyright (c) company")]
        [HelpPreface("This is a pretty short prologue.")]
        private class TestClassWithShortCopyrightPrologue
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        [HelpLicense("Copyright (c) company")]
        [HelpClosure("This is a pretty short epilogue.")]
        private class TestClassWithShortCopyrightEpilogue
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        [HelpPreface("This is a pretty short prologue.")]
        [HelpClosure("This is a pretty short epilogue.")]
        private class TestClassWithShortPrologueEpilogue
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        [HelpLicense("Copyright (c) company")]
        [HelpPreface("This is a pretty short prologue.")]
        [HelpClosure("This is a pretty short epilogue.")]
        private class TestClassWithShortCopyrightPrologueEpilogue
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        [HelpLicense("Copyright (c) company. Test data set 1 to fill the line up to " +
                     "its end. Test data set 2 to fill the line up to its end.")]
        [HelpPreface("This is a pretty short prologue. Test data set 1 to fill the line " +
                     "up to its end. Test data set 2 to fill the line up to its end.")]
        [HelpClosure("This is a pretty short epilogue. Test data set 1 to fill the line " +
                     "up to its end. Test data set 2 to fill the line up to its end.")]
        private class TestClassWithLongCopyrightPrologueEpilogue
        {
            [HelpSummary]
            [SwitchParameter(SolidLabel = "property1")]
            public Boolean Property1 { get; set; }
            [HelpSummary]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        private class TestClassWithMinimumHelpAttributesButVeryLongParameterName
        {
            [HelpSummary("The summary of the very long parameter name.")]
            [SwitchParameter(SolidLabel = "this-is-a-very-long-parameter-name-longer-than-line-with")]
            public Boolean Property1 { get; set; }
            [HelpSummary("The summary of the other parameter name.")]
            [OptionParameter(SolidLabel = "property2")]
            public String Property2 { get; set; }
        }

        [HelpUtilize]
        [HelpUtilize]
        private class TestClassTwoEmptyUtilize
        {
        }

        [HelpUtilize("Content AAA")]
        [HelpUtilize("Content BBB")]
        private class TestClassTwoContentUtilize
        {
        }

        [HelpUtilize("Content AAA", Heading = "Other:")]
        [HelpUtilize("Content BBB", Heading = "Other:")]
        private class TestClassTwoContentUtilizeHeadingOther
        {
        }

        [HelpUtilize("Content AAA", Section = "Hello:")]
        [HelpUtilize("Content BBB", Section = "Hello:")]
        private class TestClassTwoContentUtilizeWithSection
        {
        }

        [HelpUtilize("D-A")]
        [HelpUtilize("D-B")]
        [HelpUtilize("U-1.0.1", Heading = "H-1")]
        [HelpUtilize("U-1.0.2", Heading = "H-1")]
        [HelpUtilize("U-1.1.1", Heading = "H-1", Section = "C-1.1")]
        [HelpUtilize("U-1.1.2", Heading = "H-1", Section = "C-1.1")]
        [HelpUtilize("U-1.1.3", Heading = "H-1", Section = "C-1.1")]
        [HelpUtilize("U-1.2.1", Heading = "H-1", Section = "C-1.2")]
        [HelpUtilize("U-1.2.2", Heading = "H-1", Section = "C-1.2")]
        [HelpUtilize("U-1.2.3", Heading = "H-1", Section = "C-1.2")]
        [HelpUtilize("D-C")]
        [HelpUtilize("D-D")]
        [HelpUtilize("U-2.1.1", Heading = "H-2", Section = "C-1.1")]
        [HelpUtilize("U-2.1.2", Heading = "H-2", Section = "C-1.1")]
        [HelpUtilize("U-2.1.3", Heading = "H-2", Section = "C-1.1")]
        [HelpUtilize("U-2.2.1", Heading = "H-2", Section = "C-1.2")]
        [HelpUtilize("U-2.2.2", Heading = "H-2", Section = "C-1.2")]
        [HelpUtilize("U-2.2.3", Heading = "H-2", Section = "C-1.2")]
        [HelpUtilize("U-2.0.1", Heading = "H-2")]
        [HelpUtilize("U-2.0.2", Heading = "H-2")]
        private class TestClassUtilizeFullFeatured
        {
        }

        private class TestClassSummaryTwoHeadings
        {
            [HelpSummary("Property 1 in heading 1", Heading = "Heading-1:")]
            [SwitchParameter(SolidLabel = "prop-1-head-1", BriefLabel = "p1h1")]
            public Boolean P1H1 { get; set; }
            [HelpSummary("Property 2 in heading 1", Heading = "Heading-1:")]
            [SwitchParameter(SolidLabel = "prop-2-head-1", BriefLabel = "p2h1")]
            public Boolean P2H1 { get; set; }
            [HelpSummary("Property 1 in heading 2", Heading = "Heading-2:")]
            [SwitchParameter(SolidLabel = "prop-1-head-2", BriefLabel = "p1h2")]
            public Boolean P1H2 { get; set; }
            [HelpSummary("Property 2 in heading 2", Heading = "Heading-2:")]
            [SwitchParameter(SolidLabel = "prop-2-head-2", BriefLabel = "p2h2")]
            public Boolean P2H2 { get; set; }
            [HelpSummary("Property 3 in heading 2", Heading = "Heading-2:")]
            [SwitchParameter(SolidLabel = "prop-3-head-2", BriefLabel = "p3h2")]
            public Boolean P3H2 { get; set; }
        }

        private class TestClassSummaryTwoHeadingsTwoSections
        {
            [HelpSummary("Property 1 in heading 1 and section 1", Heading = "Heading-1:", Section = "Section-1")]
            [SwitchParameter(SolidLabel = "prop-1-head-1-sect-1", BriefLabel = "p1h1s1")]
            public Boolean P1H1S1 { get; set; }
            [HelpSummary("Property 2 in heading 1 and section 1", Heading = "Heading-1:", Section = "Section-1")]
            [SwitchParameter(SolidLabel = "prop-2-head-1-sect-1", BriefLabel = "p2h1s1")]
            public Boolean P2H1S1 { get; set; }
            [HelpSummary("Property 1 in heading 1 and section 2", Heading = "Heading-1:", Section = "Section-2")]
            [SwitchParameter(SolidLabel = "prop-1-head-1-sect-2", BriefLabel = "p1h1s2")]
            public Boolean P1H1S2 { get; set; }
            [HelpSummary("Property 2 in heading 1 and section 2", Heading = "Heading-1:", Section = "Section-2")]
            [SwitchParameter(SolidLabel = "prop-2-head-1-sect-2", BriefLabel = "p2h1s2")]
            public Boolean P2H1S2 { get; set; }
            [HelpSummary("Property 1 in heading 2 and section 1", Heading = "Heading-2:", Section = "Section-1")]
            [SwitchParameter(SolidLabel = "prop-1-head-2-sect-1", BriefLabel = "p1h2s1")]
            public Boolean P1H2S1 { get; set; }
            [HelpSummary("Property 2 in heading 2 and section 1", Heading = "Heading-2:", Section = "Section-1")]
            [SwitchParameter(SolidLabel = "prop-2-head-2-sect-1", BriefLabel = "p2h2s1")]
            public Boolean P2H2S1 { get; set; }
            [HelpSummary("Property 1 in heading 2 and section 2", Heading = "Heading-2:", Section = "Section-2")]
            [SwitchParameter(SolidLabel = "prop-1-head-2-sect-2", BriefLabel = "p1h2s2")]
            public Boolean P1H2S2 { get; set; }
            [HelpSummary("Property 2 in heading 2 and section 2", Heading = "Heading-2:", Section = "Section-2")]
            [SwitchParameter(SolidLabel = "prop-2-head-2-sect-2", BriefLabel = "p2h2s2")]
            public Boolean P2H2S2 { get; set; }
        }

        [Test]
        public void Generate_ZeroBounds_ThrowsException()
        {
            TestClassWithEmptyHelpAttributes instance = new TestClassWithEmptyHelpAttributes();
            HelpProcessor<TestClassWithEmptyHelpAttributes> processor = new HelpProcessor<TestClassWithEmptyHelpAttributes>(instance);
            Assert.Throws<HelpGeneratorException>(() => { processor.Generate(0); });
        }

        [Test]
        public void Generate_TestClassWithEmptyHelpAttributes_ResultIsEmpty()
        {
            TestClassWithEmptyHelpAttributes instance = new TestClassWithEmptyHelpAttributes();
            HelpProcessor<TestClassWithEmptyHelpAttributes> processor = new HelpProcessor<TestClassWithEmptyHelpAttributes>(instance);
            processor.Generate();
            Assert.AreEqual(processor.Results, String.Empty);
        }

        [Test]
        public void Generate_TestClassWithMinimumHelpAttributes_ResultIsMinimumHelp()
        {
            String expected = "\r\nUsage:\r\n\r\n" +
                "  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  \r\n\r\n" +
                "  --property2  \r\n\r\n";

            TestClassWithMinimumHelpAttributes instance = new TestClassWithMinimumHelpAttributes();
            HelpProcessor<TestClassWithMinimumHelpAttributes> processor = new HelpProcessor<TestClassWithMinimumHelpAttributes>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithShortHelpSummary_ResultIsShortHelp()
        {
            String expected = "\r\nUsage:\r\n\r\n" +
                "  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  Short help on switch 1.\r\n\r\n" +
                "  --property2  Short help on option 2.\r\n\r\n";

            TestClassWithShortHelpSummary instance = new TestClassWithShortHelpSummary();
            HelpProcessor<TestClassWithShortHelpSummary> processor = new HelpProcessor<TestClassWithShortHelpSummary>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithLongHelpSummary_ResultIsLongtHelp()
        {
            String expected = "\r\nUsage:\r\n\r\n" +
                "  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  Long help on switch 1. Long help on switch 1. Long help on\r\n" +
                "               switch 1. Long help on switch 1. Long help on switch 1. Long\r\n" +
                "               help on switch 1.\r\n\r\n" +
                "  --property2  Long help on option 2. Long help on option 2. Long help on\r\n" +
                "               option 2. Long help on option 2.\r\n\r\n";

            TestClassWithLongHelpSummary instance = new TestClassWithLongHelpSummary();
            HelpProcessor<TestClassWithLongHelpSummary> processor = new HelpProcessor<TestClassWithLongHelpSummary>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithLongHelpSummaryDifferentParameterLength_ResultIsLongtHelp()
        {
            String expected = "\r\nUsage:\r\n\r\n" +
                "  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --less          Long help on switch 1. Long help on switch 1. Long help on\r\n" +
                "                  switch 1. Long help on switch 1. Long help on switch 1.\r\n" +
                "                  Long help on switch 1.\r\n\r\n" +
                "  --a-bit-longer  Long help on option 2. Long help on option 2. Long help on\r\n" +
                "                  option 2. Long help on option 2.\r\n\r\n";

            TestClassWithLongHelpSummaryDifferentParameterLength instance = new TestClassWithLongHelpSummaryDifferentParameterLength();
            HelpProcessor<TestClassWithLongHelpSummaryDifferentParameterLength> processor = new HelpProcessor<TestClassWithLongHelpSummaryDifferentParameterLength>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassHelpSummaryAndOptionArguments_ResultIsLongtHelp()
        {
            String expected = "\r\nUsage:\r\n\r\n" +
                "  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --string-option [-so] str-val  String option with additional arguments in\r\n" +
                "                                 help output.\r\n\r\n" +
                "  --int-option [-io] num-val     Integer option with additional arguments in\r\n" +
                "                                 help output.\r\n\r\n";

            TestClassHelpSummaryAndOptionArguments instance = new TestClassHelpSummaryAndOptionArguments();
            HelpProcessor<TestClassHelpSummaryAndOptionArguments> processor = new HelpProcessor<TestClassHelpSummaryAndOptionArguments>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassHelpSectionAndSummary_ResultIsLongtHelp()
        {
            String expected = "\r\nUsage:\r\n\r\n" +
                "  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  Section A\r\n\r\n" +
                "  --switcha1  Switch 1 in section A.\r\n\r\n" +
                "  --switcha2  Switch 2 in section A.\r\n\r\n" +
                "  --switcha3  Switch 3 in section A.\r\n\r\n" +
                "  Section B\r\n\r\n" +
                "  --switchb1  Switch 1 in section B.\r\n\r\n" +
                "  --switchb2  Switch 2 in section B.\r\n\r\n";

            TestClassHelpSectionAndSummary instance = new TestClassHelpSectionAndSummary();
            HelpProcessor<TestClassHelpSectionAndSummary> processor = new HelpProcessor<TestClassHelpSectionAndSummary>(instance);
            processor.Generate();

            System.Diagnostics.Debug.WriteLine("------------------------");
            System.Diagnostics.Debug.WriteLine(processor.Results);
            System.Diagnostics.Debug.WriteLine("------------------------");
            System.Diagnostics.Debug.WriteLine(expected);
            System.Diagnostics.Debug.WriteLine("------------------------");

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithShortCopyright_ResultIsShortHelp()
        {
            String expected = "\r\nCopyright (c) company\r\n\r\n" +
                "Usage:\r\n\r\n  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  \r\n\r\n  --property2  \r\n\r\n";

            TestClassWithShortCopyright instance = new TestClassWithShortCopyright();
            HelpProcessor<TestClassWithShortCopyright> processor = new HelpProcessor<TestClassWithShortCopyright>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithShortPrologue_ResultIsShortHelp()
        {
            String expected = "\r\nThis is a pretty short prologue.\r\n\r\n" +
                "Usage:\r\n\r\n  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  \r\n\r\n  --property2  \r\n\r\n";

            TestClassWithShortPrologue instance = new TestClassWithShortPrologue();
            HelpProcessor<TestClassWithShortPrologue> processor = new HelpProcessor<TestClassWithShortPrologue>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithShortEpilogue_ResultIsShortHelp()
        {
            String expected = "\r\nUsage:\r\n\r\n  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  \r\n\r\n  --property2  \r\n\r\n" +
                "This is a pretty short epilogue.\r\n";

            TestClassWithShortEpilogue instance = new TestClassWithShortEpilogue();
            HelpProcessor<TestClassWithShortEpilogue> processor = new HelpProcessor<TestClassWithShortEpilogue>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithShortCopyrightPrologue_ResultIsShortHelp()
        {
            String expected = "\r\nCopyright (c) company\r\n\r\n" +
                "This is a pretty short prologue.\r\n\r\n" +
                "Usage:\r\n\r\n  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  \r\n\r\n  --property2  \r\n\r\n";

            TestClassWithShortCopyrightPrologue instance = new TestClassWithShortCopyrightPrologue();
            HelpProcessor<TestClassWithShortCopyrightPrologue> processor = new HelpProcessor<TestClassWithShortCopyrightPrologue>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithShortCopyrightEpilogue_ResultIsShortHelp()
        {
            String expected = "\r\nCopyright (c) company\r\n\r\n" +
                "Usage:\r\n\r\n  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  \r\n\r\n  --property2  \r\n\r\n" +
                "This is a pretty short epilogue.\r\n";

            TestClassWithShortCopyrightEpilogue instance = new TestClassWithShortCopyrightEpilogue();
            HelpProcessor<TestClassWithShortCopyrightEpilogue> processor = new HelpProcessor<TestClassWithShortCopyrightEpilogue>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithShortPrologueEpilogue_ResultIsShortHelp()
        {
            String expected = "\r\nThis is a pretty short prologue.\r\n\r\n" +
                "Usage:\r\n\r\n  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  \r\n\r\n  --property2  \r\n\r\n" +
                "This is a pretty short epilogue.\r\n";

            TestClassWithShortPrologueEpilogue instance = new TestClassWithShortPrologueEpilogue();
            HelpProcessor<TestClassWithShortPrologueEpilogue> processor = new HelpProcessor<TestClassWithShortPrologueEpilogue>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithShortCopyrightPrologueEpilogue_ResultIsShortHelp()
        {
            String expected = "\r\nCopyright (c) company\r\n\r\n" +
                "This is a pretty short prologue.\r\n\r\n" +
                "Usage:\r\n\r\n  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --property1  \r\n\r\n  --property2  \r\n\r\n" +
                "This is a pretty short epilogue.\r\n";

            TestClassWithShortCopyrightPrologueEpilogue instance = new TestClassWithShortCopyrightPrologueEpilogue();
            HelpProcessor<TestClassWithShortCopyrightPrologueEpilogue> processor = new HelpProcessor<TestClassWithShortCopyrightPrologueEpilogue>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithLongCopyrightPrologueEpilogue_ResultIsLongHelp()
        {
            String expected = "\r\nCopyright (c) company. Test data set 1 to fill the line up to its end. Test\r\n" +
                "data set 2 to fill the line up to its end.\r\n\r\n" +
                "This is a pretty short prologue. Test data set 1 to fill the line up to its\r\n" +
                "end. Test data set 2 to fill the line up to its end.\r\n\r\n" +
                "Usage:\r\n\r\n  <program> [options]\r\n\r\n" +
                "Options:\r\n\r\n  --property1  \r\n\r\n  --property2  \r\n\r\n" +
                "This is a pretty short epilogue. Test data set 1 to fill the line up to its\r\n" +
                "end. Test data set 2 to fill the line up to its end.\r\n";

            TestClassWithLongCopyrightPrologueEpilogue instance = new TestClassWithLongCopyrightPrologueEpilogue();
            HelpProcessor<TestClassWithLongCopyrightPrologueEpilogue> processor = new HelpProcessor<TestClassWithLongCopyrightPrologueEpilogue>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassWithMinimumHelpAttributesButVeryLongParameterName_ResultIsMinimumHelpButLineBreak()
        {
            String expected = "\r\nUsage:\r\n\r\n  <program> [options]\r\n\r\nOptions:\r\n\r\n" +
                "  --this-is-a-very-long-parameter-name-longer-than-line-with  \r\n" +
                "               The summary of the very long parameter name.\r\n\r\n" +
                "  --property2                                                 \r\n" +
                "               The summary of the other parameter name.\r\n\r\n";

            TestClassWithMinimumHelpAttributesButVeryLongParameterName instance = new TestClassWithMinimumHelpAttributesButVeryLongParameterName();
            HelpProcessor<TestClassWithMinimumHelpAttributesButVeryLongParameterName> processor = new HelpProcessor<TestClassWithMinimumHelpAttributesButVeryLongParameterName>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassTwoEmptyUtilize_ResultIsTwoWithDefaultUsages()
        {
            String expected = "\r\nUsage:\r\n\r\n  <program> [options]\r\n  <program> [options]\r\n\r\n";

            TestClassTwoEmptyUtilize instance = new TestClassTwoEmptyUtilize();
            HelpProcessor<TestClassTwoEmptyUtilize> processor = new HelpProcessor<TestClassTwoEmptyUtilize>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassTwoContentUtilize_ResultIsTwoWithOtherUsages()
        {
            String expected = "\r\nUsage:\r\n\r\n  Content AAA\r\n  Content BBB\r\n\r\n";

            TestClassTwoContentUtilize instance = new TestClassTwoContentUtilize();
            HelpProcessor<TestClassTwoContentUtilize> processor = new HelpProcessor<TestClassTwoContentUtilize>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassTwoContentUtilizeHeadingOther_ResultIsTwoInDefaultUsageInSameSection()
        {
            String expected = "\r\nOther:\r\n\r\n  Content AAA\r\n  Content BBB\r\n\r\n";

            TestClassTwoContentUtilizeHeadingOther instance = new TestClassTwoContentUtilizeHeadingOther();
            HelpProcessor<TestClassTwoContentUtilizeHeadingOther> processor = new HelpProcessor<TestClassTwoContentUtilizeHeadingOther>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassTwoContentUtilizeWithSection_ResultIsTwoDifferentUsagesInSameHeader()
        {
            String expected = "\r\nUsage:\r\n\r\n  Hello:\r\n    Content AAA\r\n    Content BBB\r\n\r\n";

            TestClassTwoContentUtilizeWithSection instance = new TestClassTwoContentUtilizeWithSection();
            HelpProcessor<TestClassTwoContentUtilizeWithSection> processor = new HelpProcessor<TestClassTwoContentUtilizeWithSection>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassUtilizeFullFeatured_ResultIsAsExpected()
        {
            String expected = "\r\nUsage:\r\n\r\n  D-A\r\n  D-B\r\n  D-C\r\n  D-D\r\n\r\nH-1\r\n\r\n  U-1.0.1\r\n  U-1.0.2\r\n  " +
                "C-1.1\r\n    U-1.1.1\r\n    U-1.1.2\r\n    U-1.1.3\r\n  C-1.2\r\n    U-1.2.1\r\n    U-1.2.2\r\n    U-1.2.3\r\n\r\nH-2\r\n\r\n  " +
                "C-1.1\r\n    U-2.1.1\r\n    U-2.1.2\r\n    U-2.1.3\r\n  C-1.2\r\n    U-2.2.1\r\n    U-2.2.2\r\n    U-2.2.3\r\n  U-2.0.1\r\n  U-2.0.2\r\n\r\n";

            TestClassUtilizeFullFeatured instance = new TestClassUtilizeFullFeatured();
            HelpProcessor<TestClassUtilizeFullFeatured> processor = new HelpProcessor<TestClassUtilizeFullFeatured>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassSummaryTwoHeadings_ResultIsAsExpected()
        {
            String expected = "\r\nHeading-1:\r\n\r\n" +
                "  --prop-1-head-1 [-p1h1]  Property 1 in heading 1\r\n\r\n" +
                "  --prop-2-head-1 [-p2h1]  Property 2 in heading 1\r\n\r\n" +
                "Heading-2:\r\n\r\n" +
                "  --prop-1-head-2 [-p1h2]  Property 1 in heading 2\r\n\r\n" +
                "  --prop-2-head-2 [-p2h2]  Property 2 in heading 2\r\n\r\n" +
                "  --prop-3-head-2 [-p3h2]  Property 3 in heading 2\r\n\r\n";

            TestClassSummaryTwoHeadings instance = new TestClassSummaryTwoHeadings();
            HelpProcessor<TestClassSummaryTwoHeadings> processor = new HelpProcessor<TestClassSummaryTwoHeadings>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }

        [Test]
        public void Generate_TestClassSummaryTwoHeadingsTwoSections_ResultIsAsExpected()
        {
            String expected = "\r\nHeading-1:\r\n\r\n" +
                "  Section-1\r\n\r\n" +
                "  --prop-1-head-1-sect-1 [-p1h1s1]  Property 1 in heading 1 and section 1\r\n\r\n" +
                "  --prop-2-head-1-sect-1 [-p2h1s1]  Property 2 in heading 1 and section 1\r\n\r\n" +
                "  Section-2\r\n\r\n" +
                "  --prop-1-head-1-sect-2 [-p1h1s2]  Property 1 in heading 1 and section 2\r\n\r\n" +
                "  --prop-2-head-1-sect-2 [-p2h1s2]  Property 2 in heading 1 and section 2\r\n\r\n" +
                "Heading-2:\r\n\r\n" +
                "  Section-1\r\n\r\n" +
                "  --prop-1-head-2-sect-1 [-p1h2s1]  Property 1 in heading 2 and section 1\r\n\r\n" +
                "  --prop-2-head-2-sect-1 [-p2h2s1]  Property 2 in heading 2 and section 1\r\n\r\n" +
                "  Section-2\r\n\r\n" +
                "  --prop-1-head-2-sect-2 [-p1h2s2]  Property 1 in heading 2 and section 2\r\n\r\n" +
                "  --prop-2-head-2-sect-2 [-p2h2s2]  Property 2 in heading 2 and section 2\r\n\r\n";

            TestClassSummaryTwoHeadingsTwoSections instance = new TestClassSummaryTwoHeadingsTwoSections();
            HelpProcessor<TestClassSummaryTwoHeadingsTwoSections> processor = new HelpProcessor<TestClassSummaryTwoHeadingsTwoSections>(instance);
            processor.Generate();

            Assert.AreEqual(processor.Results, expected);
        }
    }
}
