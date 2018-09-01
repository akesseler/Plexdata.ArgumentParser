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
using Plexdata.ArgumentParser.Extensions;
using Plexdata.ArgumentParser.Processors;
using System;

namespace Plexdata.ArgumentParser.Tests
{
    [TestFixture]
    [TestOf(nameof(ArgumentProcessorProcess))]
    public class ArgumentProcessorProcessTests
    {
        private class ArgumentProcessorProcess
        {
        }

        private class ArgumentProcessorProcessHelper
        {
            public String[] Arguments { get; set; }

            public dynamic Expected { get; set; }

            public override String ToString()
            {
                if (this.Arguments == null)
                {
                    return "<null>";
                }
                else if (this.Arguments.Length == 0)
                {
                    return "<empty>";
                }
                else
                {
                    return String.Join("; ", this.Arguments);
                }
            }
        }

        [ParametersGroup]
        private class TestClassSwitchProperties
        {
            [SwitchParameter(SolidLabel = "switch1", BriefLabel = "s1")]
            public Boolean Switch1 { get; set; }
            [SwitchParameter(SolidLabel = "switch2", BriefLabel = "s2")]
            public Boolean Switch2 { get; set; }
            [SwitchParameter(SolidLabel = "switch3", BriefLabel = "s3")]
            public Boolean Switch3 { get; set; }
            [SwitchParameter(SolidLabel = "switch4", BriefLabel = "s4")]
            public Boolean Switch4 { get; set; }
        }

        private static ArgumentProcessorProcessHelper[] OnlySwitchTestData = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch1" },
                Expected = new TestClassSwitchProperties { Switch1 = true } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch1", "--switch2" },
                Expected = new TestClassSwitchProperties { Switch1 = true, Switch2 = true } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch1", "--switch2", "--switch3" },
                Expected = new TestClassSwitchProperties { Switch1 = true, Switch2 = true, Switch3 = true } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch1", "--switch2", "--switch3", "--switch4" },
                Expected = new TestClassSwitchProperties { Switch1 = true, Switch2 = true, Switch3 = true, Switch4 = true } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch2", "--switch3", "--switch4" },
                Expected = new TestClassSwitchProperties { Switch2 = true, Switch3 = true, Switch4 = true } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch3", "--switch4" },
                Expected = new TestClassSwitchProperties { Switch3 = true, Switch4 = true } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch4" },
                Expected = new TestClassSwitchProperties { Switch4 = true } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch1", "--switch3" },
                Expected = new TestClassSwitchProperties { Switch1 = true, Switch3 = true } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch2", "--switch4" },
                Expected = new TestClassSwitchProperties { Switch2 = true, Switch4 = true } },
        };

        [ParametersGroup]
        private class TestClassOptionProperties
        {
            [OptionParameter(SolidLabel = "option1", BriefLabel = "o1")]
            public String Option1 { get; set; }
            [OptionParameter(SolidLabel = "option2", BriefLabel = "o2", Separator = ':')]
            public Int32? Option2 { get; set; }
            [OptionParameter(SolidLabel = "option3", BriefLabel = "o3", Separator = '=')]
            public String Option3 { get; set; }
            [OptionParameter(SolidLabel = "option4", BriefLabel = "o4", Separator = '#')]
            public Int32? Option4 { get; set; }
        }

        private static ArgumentProcessorProcessHelper[] OnlyOptionTestData = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--option1", "option-value-1" },
                Expected = new TestClassOptionProperties { Option1 = "option-value-1" } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--option1", "option-value-1", "--option2:42" },
                Expected = new TestClassOptionProperties { Option1 = "option-value-1", Option2 = 42 } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--option1", "option-value-1", "--option2:42", "--option3=\"option value 3\"" },
                Expected = new TestClassOptionProperties { Option1 = "option-value-1", Option2 = 42, Option3 = "option value 3" } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--option1", "option-value-1", "--option2:42", "--option3=\"option value 3\"", "--option4#23" },
                Expected = new TestClassOptionProperties { Option1 = "option-value-1", Option2 = 42, Option3 = "option value 3", Option4 = 23 } },
        };

        [ParametersGroup]
        private class TestClassVerbalProperties
        {
            [VerbalParameter]
            public String[] Verbal1 { get; set; }
        }

        private static ArgumentProcessorProcessHelper[] OnlyVerbalTestData = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "verbal1" },
                Expected = new TestClassVerbalProperties { Verbal1 = new String[] { "verbal1" } } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "verbal1", "verbal2" },
                Expected = new TestClassVerbalProperties { Verbal1 = new String[] { "verbal1", "verbal2" } } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "verbal1", "verbal2", "\"verbal 42\"" },
                Expected = new TestClassVerbalProperties { Verbal1 = new String[] { "verbal1", "verbal2", "verbal 42" } } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "verbal2", "\"verbal 42\"" },
                Expected = new TestClassVerbalProperties { Verbal1 = new String[] { "verbal2", "verbal 42" } } },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "\"verbal 42\"" },
                Expected = new TestClassVerbalProperties { Verbal1 = new String[] { "verbal 42" } } },
        };

        private static ArgumentProcessorProcessHelper[] InvalidArgumentsTestData = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper{
                Arguments = null,
                Expected = null, },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[0],
                Expected = null },
        };

        [ParametersGroup]
        private class TestClassExclusiveProperties
        {
            [SwitchParameter(SolidLabel = "switch1", BriefLabel = "s1", IsExclusive = true)]
            public Boolean Switch1 { get; set; }
            [OptionParameter(SolidLabel = "option1", BriefLabel = "o1", IsExclusive = true)]
            public String Option1 { get; set; }
            [VerbalParameter(IsExclusive = true)]
            public String[] Verbal1 { get; set; }
            [SwitchParameter(SolidLabel = "dummy1", BriefLabel = "d1")]
            public Boolean Dummy1 { get; set; }
        }

        private static ArgumentProcessorProcessHelper[] ExclusiveArgumentsTestData = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch1", "--dummy1" },
                Expected = null, },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--dummy1", "--switch1"  },
                Expected = null, },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--option1", "value", "--dummy1" },
                Expected = null, },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--dummy1", "--option1", "value" },
                Expected = null, },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "verbal1", "verbal2", "--dummy1" },
                Expected = null, },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--dummy1", "verbal1", "verbal2" },
                Expected = null, },
        };

        [ParametersGroup]
        private class TestClassSwitchRequiredProperties
        {
            [SwitchParameter(SolidLabel = "switch1", BriefLabel = "s1", IsRequired = true)]
            public Boolean Switch1 { get; set; }
            [SwitchParameter(SolidLabel = "switch-required")]
            public Boolean Dummy1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassOptionRequiredProperties
        {
            [OptionParameter(SolidLabel = "option1", BriefLabel = "o1", IsRequired = true)]
            public String Option1 { get; set; }
            [SwitchParameter(SolidLabel = "option-required")]
            public Boolean Dummy1 { get; set; }
        }

        [ParametersGroup]
        private class TestClassVerbalRequiredProperties
        {
            [VerbalParameter(IsRequired = true)]
            public String[] Verbal1 { get; set; }
            [SwitchParameter(SolidLabel = "verbal-required")]
            public Boolean Dummy1 { get; set; }
        }

        private static ArgumentProcessorProcessHelper[] RequiredArgumentsTestData = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--switch-required" },
                Expected = new TestClassSwitchRequiredProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--option-required"  },
                Expected = new TestClassOptionRequiredProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--verbal-required" },
                Expected = new TestClassVerbalRequiredProperties(), },
        };

        [ParametersGroup]
        private class TestClassSwitchDependencyProperties
        {
            [SwitchParameter(SolidLabel = "swt1", Dependencies = "Switch2,   Switch3")]
            public Boolean Switch1 { get; set; }
            [SwitchParameter(SolidLabel = "swt2", Dependencies = "Switch1:Switch3")]
            public Boolean Switch2 { get; set; }
            [SwitchParameter(SolidLabel = "swt3", Dependencies = "Switch2;   Switch1; ")]
            public Boolean Switch3 { get; set; }
        }

        [ParametersGroup]
        private class TestClassOptionDependencyProperties
        {
            [OptionParameter(SolidLabel = "pw", Dependencies = "Username")]
            public String Password { get; set; }
            [OptionParameter(SolidLabel = "un", Dependencies = "Password")]
            public String Username { get; set; }
        }

        [ParametersGroup]
        private class TestClassWrongDependencyProperties
        {
            [SwitchParameter(SolidLabel = "swtA", Dependencies = "swtB")]
            public Boolean Switch1 { get; set; }
            [SwitchParameter(SolidLabel = "swtB", Dependencies = "swtA")]
            public Boolean Switch2 { get; set; }
        }

        private static ArgumentProcessorProcessHelper[] DependencyArgumentsTestData = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swt1" },
                Expected = new TestClassSwitchDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swt2" },
                Expected = new TestClassSwitchDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swt3" },
                Expected = new TestClassSwitchDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swt1 --swt2" },
                Expected = new TestClassSwitchDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swt2 --swt1" },
                Expected = new TestClassSwitchDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swt3 --swt1" },
                Expected = new TestClassSwitchDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--pw aaa" },
                Expected = new TestClassOptionDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--un xxx" },
                Expected = new TestClassOptionDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swtA" },
                Expected = new TestClassWrongDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swtB" },
                Expected = new TestClassWrongDependencyProperties(), },
        };

        [ParametersGroup]
        private class TestClassMixedArgumentProperties
        {
            [SwitchParameter(SolidLabel = "verbose", BriefLabel = "v")]
            public Boolean Verbose { get; set; }

            [SwitchParameter(SolidLabel = "console", BriefLabel = "c")]
            public Boolean Console { get; set; }

            [OptionParameter(SolidLabel = "password", BriefLabel = "p", Dependencies = "Username")]
            public String Password { get; set; }

            [OptionParameter(SolidLabel = "username", BriefLabel = "u", Dependencies = "Password")]
            public String Username { get; set; }

            [SwitchParameter(SolidLabel = "fork", BriefLabel = "f", Dependencies = "LogFile")]
            public Boolean IsFork { get; set; }

            [OptionParameter(SolidLabel = "logfile", BriefLabel = "l", Dependencies = "IsFork")]
            public String LogFile { get; set; }

            [VerbalParameter]
            public String[] Files { get; set; }
        }

        private static ArgumentProcessorProcessHelper[] MixedArgumentsTestData = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--verbose" },
                Expected = new TestClassMixedArgumentProperties{ Verbose = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--console" },
                Expected = new TestClassMixedArgumentProperties{ Console = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--password", "ppp", "--username", "uuu" },
                Expected = new TestClassMixedArgumentProperties{ Password = "ppp", Username = "uuu" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--logfile", "log-file.log", "--fork" },
                Expected = new TestClassMixedArgumentProperties{ LogFile = "log-file.log", IsFork = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "file.txt" },
                Expected = new TestClassMixedArgumentProperties{ Files = new String[] { "file.txt" } },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "file1.txt", "file2.txt", "file3.txt" },
                Expected = new TestClassMixedArgumentProperties{ Files = new String[] { "file1.txt", "file2.txt", "file3.txt" } },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--verbose", "--console", "--password", "ppp", "--username", "uuu", "--fork", "--logfile", "log-file.log", "file1.txt", "file2.txt", "file3.txt" },
                Expected = new TestClassMixedArgumentProperties{ Verbose = true, Console = true, Password = "ppp", Username = "uuu", LogFile = "log-file.log", IsFork = true, Files = new String[] { "file1.txt", "file2.txt", "file3.txt" } },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "file1.txt", "file2.txt", "file3.txt", "--verbose", "--console", "--password", "ppp", "--username", "uuu", "--fork", "--logfile", "log-file.log" },
                Expected = new TestClassMixedArgumentProperties{ Verbose = true, Console = true, Password = "ppp", Username = "uuu", LogFile = "log-file.log", IsFork = true, Files = new String[] { "file1.txt", "file2.txt", "file3.txt" } },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--verbose", "--console", "--password", "ppp", "file1.txt", "file2.txt", "file3.txt", "--username", "uuu", "--fork", "--logfile", "log-file.log" },
                Expected = new TestClassMixedArgumentProperties{ Verbose = true, Console = true, Password = "ppp", Username = "uuu", LogFile = "log-file.log", IsFork = true, Files = new String[] { "file1.txt", "file2.txt", "file3.txt" } },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--verbose", "--console", "--password", "ppp", "--username", "uuu", "--logfile", "log-file.log", "--fork", "file1.txt", "file2.txt", "file3.txt" },
                Expected = new TestClassMixedArgumentProperties{ Verbose = true, Console = true, Password = "ppp", Username = "uuu", LogFile = "log-file.log", IsFork = true, Files = new String[] { "file1.txt", "file2.txt", "file3.txt" } },
            },
        };

        [Test]
        [TestCaseSource("OnlySwitchTestData")]
        public void Process_TestClassSwitchProperties_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassSwitchProperties expected = testHelper.Expected;
            TestClassSwitchProperties actual = new TestClassSwitchProperties();

            ArgumentProcessor<TestClassSwitchProperties> processor = new ArgumentProcessor<TestClassSwitchProperties>(actual, testHelper.Arguments);
            processor.Process();

            Assert.AreEqual(actual.Stringify(), expected.Stringify());
        }

        [Test]
        [TestCaseSource("OnlyOptionTestData")]
        public void Process_TestClassOptionProperties_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionProperties expected = testHelper.Expected;
            TestClassOptionProperties actual = new TestClassOptionProperties();

            ArgumentProcessor<TestClassOptionProperties> processor = new ArgumentProcessor<TestClassOptionProperties>(actual, testHelper.Arguments);
            processor.Process();

            Assert.AreEqual(actual.Stringify(), expected.Stringify());
        }

        [Test]
        [TestCaseSource("OnlyVerbalTestData")]
        public void Process_TestClassVerbalProperties_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassVerbalProperties expected = testHelper.Expected;
            TestClassVerbalProperties actual = new TestClassVerbalProperties();

            ArgumentProcessor<TestClassVerbalProperties> processor = new ArgumentProcessor<TestClassVerbalProperties>(actual, testHelper.Arguments);
            processor.Process();

            Assert.AreEqual(actual.Stringify(), expected.Stringify());
        }

        [Test]
        [TestCaseSource("InvalidArgumentsTestData")]
        public void Process_InvalidArguments_ThrowsException(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            Object instance = new Object();
            Assert.Throws<ArgumentNullException>(() => { ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(instance, testHelper.Arguments); });
        }

        [Test]
        public void Process_UnsupportedArguments_ThrowsException()
        {
            TestClassSwitchProperties instance = new TestClassSwitchProperties();
            String[] arguments = new String[] { "-s1", "-a1" };
            ArgumentProcessor<TestClassSwitchProperties> processor = new ArgumentProcessor<TestClassSwitchProperties>(instance, arguments);
            Assert.Throws<SupportViolationException>(() => { processor.Process(); });
        }

        [Test]
        public void Process_MissingOptionValue_ThrowsException()
        {
            TestClassOptionProperties instance = new TestClassOptionProperties();
            String[] arguments = new String[] { "-o1" };
            ArgumentProcessor<TestClassOptionProperties> processor = new ArgumentProcessor<TestClassOptionProperties>(instance, arguments);
            Assert.Throws<OptionViolationException>(() => { processor.Process(); });
        }

        [Test]
        public void Process_OptionFollowedByOption_ThrowsException()
        {
            TestClassOptionProperties instance = new TestClassOptionProperties();
            String[] arguments = new String[] { "-o1", "-o2" };
            ArgumentProcessor<TestClassOptionProperties> processor = new ArgumentProcessor<TestClassOptionProperties>(instance, arguments);
            Assert.Throws<OptionViolationException>(() => { processor.Process(); });
        }

        [Test]
        [TestCaseSource("ExclusiveArgumentsTestData")]
        public void Process_ExclusiveArguments_ThrowsException(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassExclusiveProperties instance = new TestClassExclusiveProperties();
            ArgumentProcessor<TestClassExclusiveProperties> processor = new ArgumentProcessor<TestClassExclusiveProperties>(instance, testHelper.Arguments);
            Assert.Throws<ExclusiveViolationException>(() => { processor.Process(); });
        }

        [Test]
        [TestCaseSource("RequiredArgumentsTestData")]
        public void Process_RequiredArguments_ThrowsException(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            dynamic instance = testHelper.Expected;
            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(instance, testHelper.Arguments);
            Assert.Throws<RequiredViolationException>(() => { processor.Process(); });
        }

        [Test]
        [TestCaseSource("DependencyArgumentsTestData")]
        public void Process_DependencyArguments_ThrowsException(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            dynamic instance = testHelper.Expected;
            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(instance, testHelper.Arguments);
            Assert.Throws<DependentViolationException>(() => { processor.Process(); });
        }

        [Test]
        [TestCaseSource("MixedArgumentsTestData")]
        public void Process_MixedArguments_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassMixedArgumentProperties expected = testHelper.Expected as TestClassMixedArgumentProperties;
            TestClassMixedArgumentProperties actual = new TestClassMixedArgumentProperties();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.AreEqual(expected.Stringify(), actual.Stringify());
        }
    }
}
