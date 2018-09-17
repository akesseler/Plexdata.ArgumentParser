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
using Plexdata.ArgumentParser.Constants;
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
            [SwitchParameter(SolidLabel = "swt1", DependencyList = "Switch2,   Switch3", DependencyType = DependencyType.Required)]
            public Boolean Switch1 { get; set; }
            [SwitchParameter(SolidLabel = "swt2", DependencyList = "Switch1:Switch3", DependencyType = DependencyType.Required)]
            public Boolean Switch2 { get; set; }
            [SwitchParameter(SolidLabel = "swt3", DependencyList = "Switch2;   Switch1; ", DependencyType = DependencyType.Required)]
            public Boolean Switch3 { get; set; }
        }

        [ParametersGroup]
        private class TestClassOptionDependencyProperties
        {
            [OptionParameter(SolidLabel = "pw", DependencyList = "Username")]
            public String Password { get; set; }
            [OptionParameter(SolidLabel = "un", DependencyList = "Password")]
            public String Username { get; set; }
        }

        [ParametersGroup]
        private class TestClassWrongDependencyProperties
        {
            [SwitchParameter(SolidLabel = "swtA", DependencyList = "swtB, swtC, Switch4")]
            public Boolean Switch1 { get; set; }
            [SwitchParameter(SolidLabel = "swtB", DependencyList = "swtA")]
            public Boolean Switch2 { get; set; }
            [SwitchParameter(SolidLabel = "swtC", DependencyList = "swtA, Switch4")]
            public Boolean Switch3 { get; set; }
            [SwitchParameter(SolidLabel = "swtD", DependencyList = "swtA")]
            public Boolean Switch4 { get; set; }
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
                Arguments = new String[] { "--swt1", "--swt2" },
                Expected = new TestClassSwitchDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swt2", "--swt1" },
                Expected = new TestClassSwitchDependencyProperties(), },
            new ArgumentProcessorProcessHelper{
                Arguments = new String[] { "--swt3", "--swt1" },
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

            [OptionParameter(SolidLabel = "password", BriefLabel = "p", DependencyList = "Username")]
            public String Password { get; set; }

            [OptionParameter(SolidLabel = "username", BriefLabel = "u", DependencyList = "Password")]
            public String Username { get; set; }

            [SwitchParameter(SolidLabel = "fork", BriefLabel = "f", DependencyList = "LogFile")]
            public Boolean IsFork { get; set; }

            [OptionParameter(SolidLabel = "logfile", BriefLabel = "l", DependencyList = "IsFork")]
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

        [ParametersGroup]
        private class BugfixTestClassSwitchDebugDefaultA
        {
            [SwitchParameter(SolidLabel = "debug")]
            public Boolean Debug { get; set; }

            [SwitchParameter(SolidLabel = "default", BriefLabel = "d")]
            public Boolean Default { get; set; }

            public Boolean IsEqual(BugfixTestClassSwitchDebugDefaultA other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataSwitchDebugDefaultA = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug" },
                Expected = new BugfixTestClassSwitchDebugDefaultA { Debug = true, Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default" },
                Expected = new BugfixTestClassSwitchDebugDefaultA { Debug = false, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d" },
                Expected = new BugfixTestClassSwitchDebugDefaultA { Debug = false, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "--default" },
                Expected = new BugfixTestClassSwitchDebugDefaultA { Debug = true, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "--debug" },
                Expected = new BugfixTestClassSwitchDebugDefaultA { Debug = true, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "-d" },
                Expected = new BugfixTestClassSwitchDebugDefaultA { Debug = true, Default = true },
            },
         };

        [Test]
        [TestCaseSource("BugfixTestDataSwitchDebugDefaultA")]
        public void Process_BugfixTestDataSwitchDebugDefaultA_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassSwitchDebugDefaultA expected = testHelper.Expected as BugfixTestClassSwitchDebugDefaultA;
            BugfixTestClassSwitchDebugDefaultA actual = new BugfixTestClassSwitchDebugDefaultA();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassSwitchDebugDefaultB
        {
            [SwitchParameter(SolidLabel = "debug", BriefLabel = "d")]
            public Boolean Debug { get; set; }

            [SwitchParameter(SolidLabel = "default")]
            public Boolean Default { get; set; }

            public Boolean IsEqual(BugfixTestClassSwitchDebugDefaultB other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataSwitchDebugDefaultB = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug" },
                Expected = new BugfixTestClassSwitchDebugDefaultB { Debug = true, Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default" },
                Expected = new BugfixTestClassSwitchDebugDefaultB { Debug = false, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d" },
                Expected = new BugfixTestClassSwitchDebugDefaultB { Debug = true, Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "--default" },
                Expected = new BugfixTestClassSwitchDebugDefaultB { Debug = true, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "--debug" },
                Expected = new BugfixTestClassSwitchDebugDefaultB { Debug = true, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "-d" },
                Expected = new BugfixTestClassSwitchDebugDefaultB { Debug = true, Default = true },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataSwitchDebugDefaultB")]
        public void Process_BugfixTestDataSwitchDebugDefaultB_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassSwitchDebugDefaultB expected = testHelper.Expected as BugfixTestClassSwitchDebugDefaultB;
            BugfixTestClassSwitchDebugDefaultB actual = new BugfixTestClassSwitchDebugDefaultB();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassSwitchDefaultDebugA
        {
            [SwitchParameter(SolidLabel = "default")]
            public Boolean Default { get; set; }

            [SwitchParameter(SolidLabel = "debug", BriefLabel = "d")]
            public Boolean Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassSwitchDefaultDebugA other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataSwitchDefaultDebugA = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug" },
                Expected = new BugfixTestClassSwitchDefaultDebugA { Debug = true, Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default" },
                Expected = new BugfixTestClassSwitchDefaultDebugA { Debug = false, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d" },
                Expected = new BugfixTestClassSwitchDefaultDebugA { Debug = true, Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "--default" },
                Expected = new BugfixTestClassSwitchDefaultDebugA { Debug = true, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "--debug" },
                Expected = new BugfixTestClassSwitchDefaultDebugA { Debug = true, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "-d" },
                Expected = new BugfixTestClassSwitchDefaultDebugA { Debug = true, Default = true },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataSwitchDefaultDebugA")]
        public void Process_BugfixTestDataSwitchDefaultDebugA_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassSwitchDefaultDebugA expected = testHelper.Expected as BugfixTestClassSwitchDefaultDebugA;
            BugfixTestClassSwitchDefaultDebugA actual = new BugfixTestClassSwitchDefaultDebugA();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassSwitchDefaultDebugB
        {
            [SwitchParameter(SolidLabel = "default", BriefLabel = "d")]
            public Boolean Default { get; set; }

            [SwitchParameter(SolidLabel = "debug")]
            public Boolean Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassSwitchDefaultDebugB other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataSwitchDefaultDebugB = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug" },
                Expected = new BugfixTestClassSwitchDefaultDebugB { Debug = true, Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default" },
                Expected = new BugfixTestClassSwitchDefaultDebugB { Debug = false, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d" },
                Expected = new BugfixTestClassSwitchDefaultDebugB { Debug = false, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "--default" },
                Expected = new BugfixTestClassSwitchDefaultDebugB { Debug = true, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "--debug" },
                Expected = new BugfixTestClassSwitchDefaultDebugB { Debug = true, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "-d" },
                Expected = new BugfixTestClassSwitchDefaultDebugB { Debug = true, Default = true },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataSwitchDefaultDebugB")]
        public void Process_BugfixTestDataSwitchDefaultDebugB_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassSwitchDefaultDebugB expected = testHelper.Expected as BugfixTestClassSwitchDefaultDebugB;
            BugfixTestClassSwitchDefaultDebugB actual = new BugfixTestClassSwitchDefaultDebugB();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassOptionDebugDefaultA
        {
            [OptionParameter(SolidLabel = "debug")]
            public String Debug { get; set; }

            [OptionParameter(SolidLabel = "default", BriefLabel = "d")]
            public String Default { get; set; }

            public Boolean IsEqual(BugfixTestClassOptionDebugDefaultA other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataOptionDebugDefaultA = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultA { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultA { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultA { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value", "--default", "default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultA { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value", "--debug", "debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultA { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value", "-d", "default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultA { Debug = "debug-value", Default = "default-value" },
            },
   };

        [Test]
        [TestCaseSource("BugfixTestDataOptionDebugDefaultA")]
        public void Process_BugfixTestDataOptionDebugDefaultA_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassOptionDebugDefaultA expected = testHelper.Expected as BugfixTestClassOptionDebugDefaultA;
            BugfixTestClassOptionDebugDefaultA actual = new BugfixTestClassOptionDebugDefaultA();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassOptionDebugDefaultASeparator
        {
            [OptionParameter(SolidLabel = "debug", Separator = '=')]
            public String Debug { get; set; }

            [OptionParameter(SolidLabel = "default", BriefLabel = "d", Separator = '=')]
            public String Default { get; set; }

            public Boolean IsEqual(BugfixTestClassOptionDebugDefaultASeparator other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataOptionDebugDefaultASeparator = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug=debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultASeparator { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default=default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultASeparator { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d=default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultASeparator { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug=debug-value", "--default=default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultASeparator { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default=default-value", "--debug=debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultASeparator { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug=debug-value", "-d=default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultASeparator { Debug = "debug-value", Default = "default-value" },
            },
   };

        [Test]
        [TestCaseSource("BugfixTestDataOptionDebugDefaultASeparator")]
        public void Process_BugfixTestDataOptionDebugDefaultASeparator_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassOptionDebugDefaultASeparator expected = testHelper.Expected as BugfixTestClassOptionDebugDefaultASeparator;
            BugfixTestClassOptionDebugDefaultASeparator actual = new BugfixTestClassOptionDebugDefaultASeparator();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassOptionDebugDefaultB
        {
            [OptionParameter(SolidLabel = "debug", BriefLabel = "d")]
            public String Debug { get; set; }

            [OptionParameter(SolidLabel = "default")]
            public String Default { get; set; }

            public Boolean IsEqual(BugfixTestClassOptionDebugDefaultB other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataOptionDebugDefaultB = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultB { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultB { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultB { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value", "--default", "default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultB { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value", "--debug", "debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultB { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "debug-value", "-default", "default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultB { Debug = "debug-value", Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataOptionDebugDefaultB")]
        public void Process_BugfixTestDataOptionDebugDefaultB_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassOptionDebugDefaultB expected = testHelper.Expected as BugfixTestClassOptionDebugDefaultB;
            BugfixTestClassOptionDebugDefaultB actual = new BugfixTestClassOptionDebugDefaultB();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassOptionDebugDefaultBSeparator
        {
            [OptionParameter(SolidLabel = "debug", BriefLabel = "d", Separator = '=')]
            public String Debug { get; set; }

            [OptionParameter(SolidLabel = "default", Separator = '=')]
            public String Default { get; set; }

            public Boolean IsEqual(BugfixTestClassOptionDebugDefaultBSeparator other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataOptionDebugDefaultBSeparator = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug=debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultBSeparator { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default=default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultBSeparator { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d=debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultBSeparator { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug=debug-value", "--default=default-value" },
                Expected = new BugfixTestClassOptionDebugDefaultBSeparator { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default=default-value", "--debug=debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultBSeparator { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default=default-value", "-d=debug-value" },
                Expected = new BugfixTestClassOptionDebugDefaultBSeparator { Debug = "debug-value", Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataOptionDebugDefaultBSeparator")]
        public void Process_BugfixTestDataOptionDebugDefaultBSeparator_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassOptionDebugDefaultBSeparator expected = testHelper.Expected as BugfixTestClassOptionDebugDefaultBSeparator;
            BugfixTestClassOptionDebugDefaultBSeparator actual = new BugfixTestClassOptionDebugDefaultBSeparator();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassOptionDefaultDebugA
        {
            [OptionParameter(SolidLabel = "default")]
            public String Default { get; set; }

            [OptionParameter(SolidLabel = "debug", BriefLabel = "d")]
            public String Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassOptionDefaultDebugA other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataOptionDefaultDebugA = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugA { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugA { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugA { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value", "--default", "default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugA { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value", "--debug", "debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugA { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "debug-value", "-default", "default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugA { Debug = "debug-value", Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataOptionDefaultDebugA")]
        public void Process_BugfixTestDataOptionDefaultDebugA_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassOptionDefaultDebugA expected = testHelper.Expected as BugfixTestClassOptionDefaultDebugA;
            BugfixTestClassOptionDefaultDebugA actual = new BugfixTestClassOptionDefaultDebugA();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassOptionDefaultDebugASeparator
        {
            [OptionParameter(SolidLabel = "default", Separator = '=')]
            public String Default { get; set; }

            [OptionParameter(SolidLabel = "debug", BriefLabel = "d", Separator = '=')]
            public String Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassOptionDefaultDebugASeparator other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataOptionDefaultDebugASeparator = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug=debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugASeparator { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default=default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugASeparator { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d=debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugASeparator { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug=debug-value", "--default=default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugASeparator { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default=default-value", "--debug=debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugASeparator { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--d=debug-value", "-default=default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugASeparator { Debug = "debug-value", Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataOptionDefaultDebugASeparator")]
        public void Process_BugfixTestDataOptionDefaultDebugASeparator_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassOptionDefaultDebugASeparator expected = testHelper.Expected as BugfixTestClassOptionDefaultDebugASeparator;
            BugfixTestClassOptionDefaultDebugASeparator actual = new BugfixTestClassOptionDefaultDebugASeparator();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassOptionDefaultDebugB
        {
            [OptionParameter(SolidLabel = "default", BriefLabel = "d")]
            public String Default { get; set; }

            [OptionParameter(SolidLabel = "debug")]
            public String Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassOptionDefaultDebugB other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataOptionDefaultDebugB = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugB { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugB { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugB { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value", "--default", "default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugB { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value", "--debug", "debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugB { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "default-value", "-debug", "debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugB { Debug = "debug-value", Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataOptionDefaultDebugB")]
        public void Process_BugfixTestDataOptionDefaultDebugB_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassOptionDefaultDebugB expected = testHelper.Expected as BugfixTestClassOptionDefaultDebugB;
            BugfixTestClassOptionDefaultDebugB actual = new BugfixTestClassOptionDefaultDebugB();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassOptionDefaultDebugBSeparator
        {
            [OptionParameter(SolidLabel = "default", BriefLabel = "d", Separator = '=')]
            public String Default { get; set; }

            [OptionParameter(SolidLabel = "debug", Separator = '=')]
            public String Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassOptionDefaultDebugBSeparator other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataOptionDefaultDebugBSeparator = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug=debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugBSeparator { Debug = "debug-value", Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default=default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugBSeparator { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d=default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugBSeparator { Debug = null, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug=debug-value", "--default=default-value" },
                Expected = new BugfixTestClassOptionDefaultDebugBSeparator { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default=default-value", "--debug=debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugBSeparator { Debug = "debug-value", Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d=default-value", "-debug=debug-value" },
                Expected = new BugfixTestClassOptionDefaultDebugBSeparator { Debug = "debug-value", Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataOptionDefaultDebugBSeparator")]
        public void Process_BugfixTestDataOptionDefaultDebugBSeparator_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassOptionDefaultDebugBSeparator expected = testHelper.Expected as BugfixTestClassOptionDefaultDebugBSeparator;
            BugfixTestClassOptionDefaultDebugBSeparator actual = new BugfixTestClassOptionDefaultDebugBSeparator();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassMixedDefaultDebugA
        {
            [SwitchParameter(SolidLabel = "default")]
            public Boolean Default { get; set; }

            [OptionParameter(SolidLabel = "debug", BriefLabel = "d")]
            public String Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassMixedDefaultDebugA other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataMixedDefaultDebugA = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDefaultDebugA { Debug = "debug-value", Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default" },
                Expected = new BugfixTestClassMixedDefaultDebugA { Debug = null, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "debug-value" },
                Expected = new BugfixTestClassMixedDefaultDebugA { Debug = "debug-value", Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value", "--default" },
                Expected = new BugfixTestClassMixedDefaultDebugA { Debug = "debug-value", Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDefaultDebugA { Debug = "debug-value", Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "debug-value", "--default" },
                Expected = new BugfixTestClassMixedDefaultDebugA { Debug = "debug-value", Default = true},
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataMixedDefaultDebugA")]
        public void Process_BugfixTestDataMixedDefaultDebugA_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassMixedDefaultDebugA expected = testHelper.Expected as BugfixTestClassMixedDefaultDebugA;
            BugfixTestClassMixedDefaultDebugA actual = new BugfixTestClassMixedDefaultDebugA();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassMixedDefaultDebugB
        {
            [SwitchParameter(SolidLabel = "default", BriefLabel = "d")]
            public Boolean Default { get; set; }

            [OptionParameter(SolidLabel = "debug")]
            public String Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassMixedDefaultDebugB other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataMixedDefaultDebugB = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDefaultDebugB { Debug = "debug-value", Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default" },
                Expected = new BugfixTestClassMixedDefaultDebugB { Debug = null, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d" },
                Expected = new BugfixTestClassMixedDefaultDebugB { Debug = null, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value", "--default" },
                Expected = new BugfixTestClassMixedDefaultDebugB { Debug = "debug-value", Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDefaultDebugB { Debug = "debug-value", Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDefaultDebugB { Debug = "debug-value", Default = true },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataMixedDefaultDebugB")]
        public void Process_BugfixTestDataMixedDefaultDebugB_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassMixedDefaultDebugB expected = testHelper.Expected as BugfixTestClassMixedDefaultDebugB;
            BugfixTestClassMixedDefaultDebugB actual = new BugfixTestClassMixedDefaultDebugB();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassMixedDefaultDebugC
        {
            [OptionParameter(SolidLabel = "default")]
            public String Default { get; set; }

            [SwitchParameter(SolidLabel = "debug", BriefLabel = "d")]
            public Boolean Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassMixedDefaultDebugC other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataMixedDefaultDebugC = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug" },
                Expected = new BugfixTestClassMixedDefaultDebugC { Debug = true, Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value" },
                Expected = new BugfixTestClassMixedDefaultDebugC { Debug = false, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d" },
                Expected = new BugfixTestClassMixedDefaultDebugC { Debug = true, Default = null},
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "--default", "default-value" },
                Expected = new BugfixTestClassMixedDefaultDebugC { Debug = true, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value", "--debug" },
                Expected = new BugfixTestClassMixedDefaultDebugC { Debug = true, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "--default", "default-value" },
                Expected = new BugfixTestClassMixedDefaultDebugC { Debug = true, Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataMixedDefaultDebugC")]
        public void Process_BugfixTestDataMixedDefaultDebugC_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassMixedDefaultDebugC expected = testHelper.Expected as BugfixTestClassMixedDefaultDebugC;
            BugfixTestClassMixedDefaultDebugC actual = new BugfixTestClassMixedDefaultDebugC();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassMixedDefaultDebugD
        {
            [OptionParameter(SolidLabel = "default", BriefLabel = "d")]
            public String Default { get; set; }

            [SwitchParameter(SolidLabel = "debug")]
            public Boolean Debug { get; set; }

            public Boolean IsEqual(BugfixTestClassMixedDefaultDebugD other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataMixedDefaultDebugD = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug" },
                Expected = new BugfixTestClassMixedDefaultDebugD { Debug = true, Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value" },
                Expected = new BugfixTestClassMixedDefaultDebugD { Debug = false, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "default-value" },
                Expected = new BugfixTestClassMixedDefaultDebugD { Debug = false, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "--default", "default-value" },
                Expected = new BugfixTestClassMixedDefaultDebugD { Debug = true, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value", "--debug" },
                Expected = new BugfixTestClassMixedDefaultDebugD { Debug = true, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "default-value", "--debug" },
                Expected = new BugfixTestClassMixedDefaultDebugD { Debug = true, Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataMixedDefaultDebugD")]
        public void Process_BugfixTestDataMixedDefaultDebugD_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassMixedDefaultDebugD expected = testHelper.Expected as BugfixTestClassMixedDefaultDebugD;
            BugfixTestClassMixedDefaultDebugD actual = new BugfixTestClassMixedDefaultDebugD();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassMixedDebugDefaultA
        {
            [OptionParameter(SolidLabel = "debug", BriefLabel = "d")]
            public String Debug { get; set; }

            [SwitchParameter(SolidLabel = "default")]
            public Boolean Default { get; set; }

            public Boolean IsEqual(BugfixTestClassMixedDebugDefaultA other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataMixedDebugDefaultA = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDebugDefaultA { Debug = "debug-value", Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default" },
                Expected = new BugfixTestClassMixedDebugDefaultA { Debug = null, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "debug-value" },
                Expected = new BugfixTestClassMixedDebugDefaultA { Debug = "debug-value", Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value", "--default" },
                Expected = new BugfixTestClassMixedDebugDefaultA { Debug = "debug-value", Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDebugDefaultA { Debug = "debug-value", Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "debug-value", "--default" },
                Expected = new BugfixTestClassMixedDebugDefaultA { Debug = "debug-value", Default = true},
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataMixedDebugDefaultA")]
        public void Process_BugfixTestDataMixedDebugDefaultA_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassMixedDebugDefaultA expected = testHelper.Expected as BugfixTestClassMixedDebugDefaultA;
            BugfixTestClassMixedDebugDefaultA actual = new BugfixTestClassMixedDebugDefaultA();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassMixedDebugDefaultB
        {
            [OptionParameter(SolidLabel = "debug")]
            public String Debug { get; set; }

            [SwitchParameter(SolidLabel = "default", BriefLabel = "d")]
            public Boolean Default { get; set; }

            public Boolean IsEqual(BugfixTestClassMixedDebugDefaultB other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataMixedDebugDefaultB = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDebugDefaultB { Debug = "debug-value", Default = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default" },
                Expected = new BugfixTestClassMixedDebugDefaultB { Debug = null, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d" },
                Expected = new BugfixTestClassMixedDebugDefaultB { Debug = null, Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "debug-value", "--default" },
                Expected = new BugfixTestClassMixedDebugDefaultB { Debug = "debug-value", Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDebugDefaultB { Debug = "debug-value", Default = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "--debug", "debug-value" },
                Expected = new BugfixTestClassMixedDebugDefaultB { Debug = "debug-value", Default = true },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataMixedDebugDefaultB")]
        public void Process_BugfixTestDataMixedDebugDefaultB_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassMixedDebugDefaultB expected = testHelper.Expected as BugfixTestClassMixedDebugDefaultB;
            BugfixTestClassMixedDebugDefaultB actual = new BugfixTestClassMixedDebugDefaultB();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassMixedDebugDefaultC
        {
            [SwitchParameter(SolidLabel = "debug", BriefLabel = "d")]
            public Boolean Debug { get; set; }

            [OptionParameter(SolidLabel = "default")]
            public String Default { get; set; }

            public Boolean IsEqual(BugfixTestClassMixedDebugDefaultC other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataMixedDebugDefaultC = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug" },
                Expected = new BugfixTestClassMixedDebugDefaultC { Debug = true, Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value" },
                Expected = new BugfixTestClassMixedDebugDefaultC { Debug = false, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d" },
                Expected = new BugfixTestClassMixedDebugDefaultC { Debug = true, Default = null},
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "--default", "default-value" },
                Expected = new BugfixTestClassMixedDebugDefaultC { Debug = true, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value", "--debug" },
                Expected = new BugfixTestClassMixedDebugDefaultC { Debug = true, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "--default", "default-value" },
                Expected = new BugfixTestClassMixedDebugDefaultC { Debug = true, Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataMixedDebugDefaultC")]
        public void Process_BugfixTestDataMixedDebugDefaultC_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassMixedDebugDefaultC expected = testHelper.Expected as BugfixTestClassMixedDebugDefaultC;
            BugfixTestClassMixedDebugDefaultC actual = new BugfixTestClassMixedDebugDefaultC();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class BugfixTestClassMixedDebugDefaultD
        {
            [SwitchParameter(SolidLabel = "debug")]
            public Boolean Debug { get; set; }

            [OptionParameter(SolidLabel = "default", BriefLabel = "d")]
            public String Default { get; set; }

            public Boolean IsEqual(BugfixTestClassMixedDebugDefaultD other)
            {
                return this.Debug == other.Debug && this.Default == other.Default;
            }
        }

        private static ArgumentProcessorProcessHelper[] BugfixTestDataMixedDebugDefaultD = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug" },
                Expected = new BugfixTestClassMixedDebugDefaultD { Debug = true, Default = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value" },
                Expected = new BugfixTestClassMixedDebugDefaultD { Debug = false, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "default-value" },
                Expected = new BugfixTestClassMixedDebugDefaultD { Debug = false, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--debug", "--default", "default-value" },
                Expected = new BugfixTestClassMixedDebugDefaultD { Debug = true, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--default", "default-value", "--debug" },
                Expected = new BugfixTestClassMixedDebugDefaultD { Debug = true, Default = "default-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "-d", "default-value", "--debug" },
                Expected = new BugfixTestClassMixedDebugDefaultD { Debug = true, Default = "default-value" },
            },
        };

        [Test]
        [TestCaseSource("BugfixTestDataMixedDebugDefaultD")]
        public void Process_BugfixTestDataMixedDebugDefaultD_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            BugfixTestClassMixedDebugDefaultD expected = testHelper.Expected as BugfixTestClassMixedDebugDefaultD;
            BugfixTestClassMixedDebugDefaultD actual = new BugfixTestClassMixedDebugDefaultD();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassMixedSeparators
        {
            [OptionParameter(SolidLabel = "option1", BriefLabel = "op1", Separator = '=')]
            public String Option1 { get; set; }

            [OptionParameter(SolidLabel = "option2", BriefLabel = "op2", Separator = ':')]
            public String Option2 { get; set; }

            public Boolean IsEqual(TestClassMixedSeparators other)
            {
                return this.Option1 == other.Option1 && this.Option2 == other.Option2;
            }
        }

        private static ArgumentProcessorProcessHelper[] TestDataMixedSeparators = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1=option1-value", "--option2:option2-value" },
                Expected = new TestClassMixedSeparators { Option1 = "option1-value", Option2 = "option2-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--op1=option1-value", "--op2:option2-value" },
                Expected = new TestClassMixedSeparators { Option1 = "option1-value", Option2 = "option2-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--op1=option1-value", "--option2:option2-value" },
                Expected = new TestClassMixedSeparators { Option1 = "option1-value", Option2 = "option2-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1=option1-value", "--op2:option2-value" },
                Expected = new TestClassMixedSeparators { Option1 = "option1-value", Option2 = "option2-value" },
            },
        };

        [Test]
        [TestCaseSource("TestDataMixedSeparators")]
        public void Process_TestDataMixedSeparators_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassMixedSeparators expected = testHelper.Expected as TestClassMixedSeparators;
            TestClassMixedSeparators actual = new TestClassMixedSeparators();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassImplicitDependencyAssignmentSingleSingle
        {
            [OptionParameter(SolidLabel = "option1", DependencyList = "Switch1", DependencyType = DependencyType.Required)]
            public String Option1 { get; set; }

            [SwitchParameter(SolidLabel = "switch1")]
            public Boolean Switch1 { get; set; }

            public Boolean IsEqual(TestClassImplicitDependencyAssignmentSingleSingle other)
            {
                return this.Option1 == other.Option1 && this.Switch1 == other.Switch1;
            }
        }

        [Test]
        public void Process_UnsatisfiedArgumentList_ThrowsDependentViolationException()
        {
            String[] arguments = new String[] { "--option1", "option1-value" };

            TestClassImplicitDependencyAssignmentSingleSingle actual = new TestClassImplicitDependencyAssignmentSingleSingle();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, arguments);

            Assert.Throws<DependentViolationException>(() => { processor.Process(); });
        }

        private static ArgumentProcessorProcessHelper[] TestDataDependencySingleSingle = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--switch1" },
                Expected = new TestClassImplicitDependencyAssignmentSingleSingle { Option1 = "option1-value", Switch1 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch1", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentSingleSingle { Option1 = "option1-value", Switch1 = true },
            },
        };

        [Test]
        [TestCaseSource("TestDataDependencySingleSingle")]
        public void Process_TestDataDependencySingleSingle_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassImplicitDependencyAssignmentSingleSingle expected = testHelper.Expected as TestClassImplicitDependencyAssignmentSingleSingle;
            TestClassImplicitDependencyAssignmentSingleSingle actual = new TestClassImplicitDependencyAssignmentSingleSingle();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassImplicitDependencyAssignmentSingleDouble
        {
            [OptionParameter(SolidLabel = "option1")]
            public String Option1 { get; set; }

            [SwitchParameter(SolidLabel = "switch1", DependencyList = "Option1")]
            public Boolean Switch1 { get; set; }

            [SwitchParameter(SolidLabel = "switch2", DependencyList = "Option1")]
            public Boolean Switch2 { get; set; }

            public Boolean IsEqual(TestClassImplicitDependencyAssignmentSingleDouble other)
            {
                return this.Option1 == other.Option1 && this.Switch1 == other.Switch1 && this.Switch2 == other.Switch2;
            }
        }

        private static ArgumentProcessorProcessHelper[] TestDataDependencySingleDouble = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--switch1" },
                Expected = new TestClassImplicitDependencyAssignmentSingleDouble { Option1 = "option1-value", Switch1 = true, Switch2 = false },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch1", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentSingleDouble { Option1 = "option1-value", Switch1 = true, Switch2 = false  },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--switch2" },
                Expected = new TestClassImplicitDependencyAssignmentSingleDouble { Option1 = "option1-value", Switch1 = false, Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch2", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentSingleDouble { Option1 = "option1-value", Switch1 = false, Switch2 = true  },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--switch1", "--switch2" },
                Expected = new TestClassImplicitDependencyAssignmentSingleDouble { Option1 = "option1-value", Switch1 = true, Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch1", "--switch2", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentSingleDouble { Option1 = "option1-value", Switch1 = true, Switch2 = true  },
            },
        };

        [Test]
        [TestCaseSource("TestDataDependencySingleDouble")]
        public void Process_TestDataDependencySingleDouble_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassImplicitDependencyAssignmentSingleDouble expected = testHelper.Expected as TestClassImplicitDependencyAssignmentSingleDouble;
            TestClassImplicitDependencyAssignmentSingleDouble actual = new TestClassImplicitDependencyAssignmentSingleDouble();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassImplicitDependencyAssignmentDoubleSingle
        {
            [OptionParameter(SolidLabel = "option1")]
            public String Option1 { get; set; }

            [SwitchParameter(SolidLabel = "switch1", DependencyList = "Option1")]
            public Boolean Switch1 { get; set; }

            [OptionParameter(SolidLabel = "option2")]
            public String Option2 { get; set; }

            [SwitchParameter(SolidLabel = "switch2", DependencyList = "Option2")]
            public Boolean Switch2 { get; set; }

            public Boolean IsEqual(TestClassImplicitDependencyAssignmentDoubleSingle other)
            {
                return this.Option1 == other.Option1 && this.Switch1 == other.Switch1 &&
                       this.Option2 == other.Option2 && this.Switch2 == other.Switch2;
            }
        }

        private static ArgumentProcessorProcessHelper[] TestDataDependencyDoubleSingle = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--switch1", "--option2", "option2-value", "--switch2" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleSingle { Option1 = "option1-value", Switch1 = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch1", "--option1", "option1-value", "--switch2", "--option2", "option2-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleSingle { Option1 = "option1-value", Switch1 = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option2", "option2-value", "--switch2", "--option1", "option1-value", "--switch1" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleSingle { Option1 = "option1-value", Switch1 = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch2", "--option2", "option2-value", "--switch1", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleSingle { Option1 = "option1-value", Switch1 = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch1", "--switch2", "--option1", "option1-value", "--option2", "option2-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleSingle { Option1 = "option1-value", Switch1 = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--option2", "option2-value", "--switch1", "--switch2" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleSingle { Option1 = "option1-value", Switch1 = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch2", "--switch1", "--option2", "option2-value", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleSingle { Option1 = "option1-value", Switch1 = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option2", "option2-value", "--option1", "option1-value", "--switch2", "--switch1" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleSingle { Option1 = "option1-value", Switch1 = true, Option2 = "option2-value", Switch2 = true },
            },
        };

        [Test]
        [TestCaseSource("TestDataDependencyDoubleSingle")]
        public void Process_TestDataDependencyDoubleSingle_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassImplicitDependencyAssignmentDoubleSingle expected = testHelper.Expected as TestClassImplicitDependencyAssignmentDoubleSingle;
            TestClassImplicitDependencyAssignmentDoubleSingle actual = new TestClassImplicitDependencyAssignmentDoubleSingle();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassImplicitDependencyAssignmentDoubleDouble
        {
            [OptionParameter(SolidLabel = "option1")]
            public String Option1 { get; set; }

            [SwitchParameter(SolidLabel = "switch1a", DependencyList = "Option1")]
            public Boolean Switch1A { get; set; }

            [SwitchParameter(SolidLabel = "switch1b", DependencyList = "Option1")]
            public Boolean Switch1B { get; set; }

            [OptionParameter(SolidLabel = "option2")]
            public String Option2 { get; set; }

            [SwitchParameter(SolidLabel = "switch2", DependencyList = "Option2")]
            public Boolean Switch2 { get; set; }

            public Boolean IsEqual(TestClassImplicitDependencyAssignmentDoubleDouble other)
            {
                return this.Option1 == other.Option1 && this.Switch1A == other.Switch1A && this.Switch1B == other.Switch1B &&
                       this.Option2 == other.Option2 && this.Switch2 == other.Switch2;
            }
        }

        private static ArgumentProcessorProcessHelper[] TestDataDependencyDoubleDouble = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--switch1a", "--option2", "option2-value", "--switch2" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1A = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch1a", "--option1", "option1-value", "--switch2", "--option2", "option2-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1A = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option2", "option2-value", "--switch2", "--option1", "option1-value", "--switch1a" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1A = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch2", "--option2", "option2-value", "--switch1a", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1A = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch1a", "--switch2", "--option1", "option1-value", "--option2", "option2-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1A = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--option2", "option2-value", "--switch1a", "--switch2" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1A = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch2", "--switch1a", "--option2", "option2-value", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1A = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option2", "option2-value", "--option1", "option1-value", "--switch2", "--switch1a" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1A = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--switch1b", "--option2", "option2-value", "--switch2" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1B = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch1b", "--option1", "option1-value", "--switch2", "--option2", "option2-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1B = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option2", "option2-value", "--switch2", "--option1", "option1-value", "--switch1b" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1B = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch2", "--option2", "option2-value", "--switch1b", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1B = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch1b", "--switch2", "--option1", "option1-value", "--option2", "option2-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1B = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option1", "option1-value", "--option2", "option2-value", "--switch1b", "--switch2" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1B = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--switch2", "--switch1b", "--option2", "option2-value", "--option1", "option1-value" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1B = true, Option2 = "option2-value", Switch2 = true },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--option2", "option2-value", "--option1", "option1-value", "--switch2", "--switch1b" },
                Expected = new TestClassImplicitDependencyAssignmentDoubleDouble { Option1 = "option1-value", Switch1B = true, Option2 = "option2-value", Switch2 = true },
            },
        };

        [Test]
        [TestCaseSource("TestDataDependencyDoubleDouble")]
        public void Process_TestDataDependencyDoubleDouble_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassImplicitDependencyAssignmentDoubleDouble expected = testHelper.Expected as TestClassImplicitDependencyAssignmentDoubleDouble;
            TestClassImplicitDependencyAssignmentDoubleDouble actual = new TestClassImplicitDependencyAssignmentDoubleDouble();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassOptionalRequiredImplicitStrong
        {
            [OptionParameter(SolidLabel = "file-name", DependencyList = "Operation, Execution", DependencyType = DependencyType.Optional)]
            public String Filename { get; set; } // 'Filename' depends on 'Operation' or 'Execution'.

            [OptionParameter(SolidLabel = "operation", DependencyList = "Filename", DependencyType = DependencyType.Required)]
            public String Operation { get; set; } // 'Operation' cannot be used without 'Filename'.

            [OptionParameter(SolidLabel = "execution", DependencyList = "Filename", DependencyType = DependencyType.Required)]
            public String Execution { get; set; } // 'Execution' cannot be used without 'Filename'.

            public Boolean IsEqual(Object other)
            {
                if (other == null)
                {
                    throw new ArgumentNullException(nameof(other));
                }

                if (other.GetType() != this.GetType())
                {
                    throw new NotSupportedException($"Type of {nameof(other)} is different from {this.GetType()}");
                }

                dynamic result = Convert.ChangeType(other, this.GetType());

                return
                    this.Filename == result.Filename &&
                    this.Operation == result.Operation &&
                    this.Execution == result.Execution;
            }
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredImplicitStrongInvalid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--operation", "operation-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--execution", "execution-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--operation", "operation-value", "--execution", "execution-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredImplicitStrongInvalid))]
        public void Process_TestDataOptionalRequiredImplicitStrongInvalid_ThrowsDependentViolationException(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredImplicitStrong actual = new TestClassOptionalRequiredImplicitStrong();
            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            Assert.Throws<DependentViolationException>(() => { processor.Process(); });
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredImplicitStrongValid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--operation", "operation-value" },
                Expected = new TestClassOptionalRequiredImplicitStrong { Filename = "file-name.ext", Operation = "operation-value", Execution = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--execution", "execution-value" },
                Expected = new TestClassOptionalRequiredImplicitStrong { Filename = "file-name.ext", Operation = null, Execution = "execution-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--execution", "execution-value", "--operation", "operation-value" },
                Expected = new TestClassOptionalRequiredImplicitStrong { Filename = "file-name.ext", Operation = "operation-value", Execution = "execution-value" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredImplicitStrongValid))]
        public void Process_TestDataOptionalRequiredImplicitStrongValid_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredImplicitStrong expected = testHelper.Expected as TestClassOptionalRequiredImplicitStrong;
            TestClassOptionalRequiredImplicitStrong actual = new TestClassOptionalRequiredImplicitStrong();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassOptionalRequiredImplicitWeak
        {
            [OptionParameter(SolidLabel = "file-name")]
            public String Filename { get; set; } // 'Filename' depends on 'Operation' or 'Execution'.

            [OptionParameter(SolidLabel = "operation", DependencyList = "Filename", DependencyType = DependencyType.Required)]
            public String Operation { get; set; } // 'Operation' cannot be used without 'Filename'.

            [OptionParameter(SolidLabel = "execution", DependencyList = "Filename", DependencyType = DependencyType.Required)]
            public String Execution { get; set; } // 'Execution' cannot be used without 'Filename'.

            public Boolean IsEqual(Object other)
            {
                if (other == null)
                {
                    throw new ArgumentNullException(nameof(other));
                }

                if (other.GetType() != this.GetType())
                {
                    throw new NotSupportedException($"Type of {nameof(other)} is different from {this.GetType()}");
                }

                dynamic result = Convert.ChangeType(other, this.GetType());

                return
                    this.Filename == result.Filename &&
                    this.Operation == result.Operation &&
                    this.Execution == result.Execution;
            }
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredImplicitWeakInvalid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--operation", "operation-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--execution", "execution-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--operation", "operation-value", "--execution", "execution-value" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredImplicitWeakInvalid))]
        public void Process_TestDataOptionalRequiredImplicitWeakInvalid_ThrowsDependentViolationException(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredImplicitWeak actual = new TestClassOptionalRequiredImplicitWeak();
            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            Assert.Throws<DependentViolationException>(() => { processor.Process(); });
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredImplicitWeakValid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext" },
                Expected = new TestClassOptionalRequiredImplicitWeak { Filename = "file-name.ext", Operation = null, Execution = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--operation", "operation-value" },
                Expected = new TestClassOptionalRequiredImplicitWeak { Filename = "file-name.ext", Operation = "operation-value", Execution = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--execution", "execution-value" },
                Expected = new TestClassOptionalRequiredImplicitWeak { Filename = "file-name.ext", Operation = null, Execution = "execution-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--execution", "execution-value", "--operation", "operation-value" },
                Expected = new TestClassOptionalRequiredImplicitWeak { Filename = "file-name.ext", Operation = "operation-value", Execution = "execution-value" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredImplicitWeakValid))]
        public void Process_TestDataOptionalRequiredImplicitWeakValid_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredImplicitWeak expected = testHelper.Expected as TestClassOptionalRequiredImplicitWeak;
            TestClassOptionalRequiredImplicitWeak actual = new TestClassOptionalRequiredImplicitWeak();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassOptionalRequiredExplicitStrong
        {
            [OptionParameter(SolidLabel = "file-name", DependencyList = "Operation, Execution", DependencyType = DependencyType.Required)]
            public String Filename { get; set; } // 'Filename' depends on 'Operation' and 'Execution'.

            [OptionParameter(SolidLabel = "operation", DependencyList = "Filename", DependencyType = DependencyType.Optional)]
            public String Operation { get; set; } // 'Operation' cannot be used without 'Filename'.

            [OptionParameter(SolidLabel = "execution", DependencyList = "Filename", DependencyType = DependencyType.Optional)]
            public String Execution { get; set; } // 'Execution' cannot be used without 'Filename'.

            public Boolean IsEqual(Object other)
            {
                if (other == null)
                {
                    throw new ArgumentNullException(nameof(other));
                }

                if (other.GetType() != this.GetType())
                {
                    throw new NotSupportedException($"Type of {nameof(other)} is different from {this.GetType()}");
                }

                dynamic result = Convert.ChangeType(other, this.GetType());

                return
                    this.Filename == result.Filename &&
                    this.Operation == result.Operation &&
                    this.Execution == result.Execution;
            }
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredExplicitStrongInvalid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--operation", "operation-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--execution", "execution-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--operation", "operation-value", "--execution", "execution-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--operation", "operation-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--execution", "execution-value" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredExplicitStrongInvalid))]
        public void Process_TestDataOptionalRequiredExplicitStrongInvalid_ThrowsDependentViolationException(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredExplicitStrong actual = new TestClassOptionalRequiredExplicitStrong();
            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            Assert.Throws<DependentViolationException>(() => { processor.Process(); });
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredExplicitStrongValid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--execution", "execution-value", "--operation", "operation-value" },
                Expected = new TestClassOptionalRequiredExplicitStrong { Filename = "file-name.ext", Operation = "operation-value", Execution = "execution-value" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredExplicitStrongValid))]
        public void Process_TestDataOptionalRequiredExplicitStrongValid_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredExplicitStrong expected = testHelper.Expected as TestClassOptionalRequiredExplicitStrong;
            TestClassOptionalRequiredExplicitStrong actual = new TestClassOptionalRequiredExplicitStrong();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassOptionalRequiredExplicitWeak
        {
            [OptionParameter(SolidLabel = "file-name", DependencyList = "Operation, Execution", DependencyType = DependencyType.Required)]
            public String Filename { get; set; } // 'Filename' depends on 'Operation' and 'Execution'.

            [OptionParameter(SolidLabel = "operation")]
            public String Operation { get; set; } // 'Operation' can be used without 'Filename'.

            [OptionParameter(SolidLabel = "execution")]
            public String Execution { get; set; } // 'Execution' can be used without 'Filename'.

            public Boolean IsEqual(Object other)
            {
                if (other == null)
                {
                    throw new ArgumentNullException(nameof(other));
                }

                if (other.GetType() != this.GetType())
                {
                    throw new NotSupportedException($"Type of {nameof(other)} is different from {this.GetType()}");
                }

                dynamic result = Convert.ChangeType(other, this.GetType());

                return
                    this.Filename == result.Filename &&
                    this.Operation == result.Operation &&
                    this.Execution == result.Execution;
            }
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredExplicitWeakInvalid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--operation", "operation-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--execution", "execution-value" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredExplicitWeakInvalid))]
        public void Process_TestDataOptionalRequiredExplicitWeakInvalid_ThrowsDependentViolationException(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredExplicitWeak actual = new TestClassOptionalRequiredExplicitWeak();
            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            Assert.Throws<DependentViolationException>(() => { processor.Process(); });
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredExplicitWeakValid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--operation", "operation-value" },
                Expected = new TestClassOptionalRequiredExplicitWeak { Filename = null, Operation = "operation-value", Execution = null },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--execution", "execution-value" },
                Expected = new TestClassOptionalRequiredExplicitWeak { Filename = null, Operation = null, Execution = "execution-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--operation", "operation-value", "--execution", "execution-value" },
                Expected = new TestClassOptionalRequiredExplicitWeak { Filename = null, Operation = "operation-value", Execution = "execution-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--file-name", "file-name.ext", "--execution", "execution-value", "--operation", "operation-value" },
                Expected = new TestClassOptionalRequiredExplicitWeak { Filename = "file-name.ext", Operation = "operation-value", Execution = "execution-value" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredExplicitWeakValid))]
        public void Process_TestDataOptionalRequiredExplicitWeakValid_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredExplicitWeak expected = testHelper.Expected as TestClassOptionalRequiredExplicitWeak;
            TestClassOptionalRequiredExplicitWeak actual = new TestClassOptionalRequiredExplicitWeak();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [ParametersGroup]
        private class TestClassOptionalRequiredCrossing
        {
            [OptionParameter(SolidLabel = "password", DependencyList = "Username", DependencyType = DependencyType.Required)]
            public String Password { get; set; } // 'Password' depends on 'Username'.

            [OptionParameter(SolidLabel = "username", DependencyList = "Password", DependencyType = DependencyType.Required)]
            public String Username { get; set; } // 'Username' depends on 'Password'.

            public Boolean IsEqual(Object other)
            {
                if (other == null)
                {
                    throw new ArgumentNullException(nameof(other));
                }

                if (other.GetType() != this.GetType())
                {
                    throw new NotSupportedException($"Type of {nameof(other)} is different from {this.GetType()}");
                }

                dynamic result = Convert.ChangeType(other, this.GetType());

                return
                    this.Password == result.Password &&
                    this.Username == result.Username;
            }
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredCrossingInvalid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--password", "password-value" },
            },
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--username", "username-value" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredCrossingInvalid))]
        public void Process_TestDataOptionalRequiredCrossingInvalid_ThrowsDependentViolationException(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredCrossing actual = new TestClassOptionalRequiredCrossing();
            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            Assert.Throws<DependentViolationException>(() => { processor.Process(); });
        }

        private static ArgumentProcessorProcessHelper[] TestDataOptionalRequiredCrossingValid = new ArgumentProcessorProcessHelper[]
        {
            new ArgumentProcessorProcessHelper
            {
                Arguments = new String[] { "--password", "password-value", "--username", "username-value" },
                Expected = new TestClassOptionalRequiredCrossing { Password = "password-value", Username = "username-value" },
            },
        };

        [Test]
        [TestCaseSource(nameof(TestDataOptionalRequiredCrossingValid))]
        public void Process_TestDataOptionalRequiredCrossingValid_ResultIsEqual(Object testObject)
        {
            ArgumentProcessorProcessHelper testHelper = testObject as ArgumentProcessorProcessHelper;
            TestClassOptionalRequiredCrossing expected = testHelper.Expected as TestClassOptionalRequiredCrossing;
            TestClassOptionalRequiredCrossing actual = new TestClassOptionalRequiredCrossing();

            ArgumentProcessor<Object> processor = new ArgumentProcessor<Object>(actual, testHelper.Arguments);
            processor.Process();

            Assert.IsTrue(expected.IsEqual(actual));
        }
    }
}
