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
using Plexdata.ArgumentParser.Extensions;
using System;

namespace Plexdata.ArgumentParser.Tests
{
    [TestFixture]
    [TestOf(nameof(ArgumentComposer))]
    public class ArgumentComposerTests
    {
        private class ArgumentComposerHelper
        {
            public String Actual { get; set; }

            public String[] Expected { get; set; }

            public Char Separator { get; set; }

            public override String ToString()
            {
                if (this.Actual == null)
                {
                    return "<null>";
                }
                else if (String.IsNullOrEmpty(this.Actual))
                {
                    return "<empty>";
                }
                else if (String.IsNullOrWhiteSpace(this.Actual))
                {
                    return "<whitespace>";
                }
                else
                {
                    return this.Actual;
                }
            }
        }

        private static ArgumentComposerHelper[] ExtractTestObjects = {
            new ArgumentComposerHelper {
                Actual    = null,
                Expected  = new String[] { },
                Separator =' ' },
            new ArgumentComposerHelper {
                Actual    = String.Empty,
                Expected  = new String[] { },
                Separator =' ' },
            new ArgumentComposerHelper {
                Actual    = "     ",
                Expected  = new String[] { },
                Separator =' ' },
            new ArgumentComposerHelper {
                Actual    = "--arg1 --arg2 --opt1 string",
                Expected  = new String[] { "--arg1", "--arg2","--opt1", "string" },
                Separator =' ' },
            new ArgumentComposerHelper {
                Actual    = "--arg1 --arg2 --opt1 \"string with spaces\"",
                Expected  = new String[] { "--arg1", "--arg2","--opt1", "string with spaces" },
                Separator =' ' },
            new ArgumentComposerHelper {
                Actual    = "--arg1,--arg2,--opt1,\"string_without_spaces\"",
                Expected  = new String[] { "--arg1", "--arg2","--opt1", "string_without_spaces" },
                Separator =',' },
            new ArgumentComposerHelper {
                Actual    = "   --arg1      --arg2     --opt1 \"   string\twith\tspaces   \"",
                Expected  = new String[] { "--arg1", "--arg2","--opt1", "   string\twith\tspaces   " },
                Separator =' ' },
        };

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void HasWhiteSpaces_GetProperty_ResultIsFalse(String value)
        {
            Assert.IsFalse(value.HasWhiteSpaces());
        }

        [Test]
        [TestCase("  \t \v \n\r  ")]
        [TestCase("\r\n")]
        [TestCase("\n")]
        [TestCase("Test string with \tinner white spaces\v \n\r.")]
        [TestCase("\tTest string with \tinner white spaces\v \n\r.")]
        public void HasWhiteSpaces_InvokeMethod_ResultIsTrue(String actual)
        {
            Assert.IsTrue(actual.HasWhiteSpaces());
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("str1,str2,str3", "str1 str2 str3")]
        public void Combine_InvokeMethod_ResultIsEqual(String actual, String expected)
        {
            String[] value = actual?.Split(',');
            Assert.AreEqual(value.Combine(), expected);
        }

        [Test]
        [TestCase(null, "", ' ')]
        [TestCase(null, "", ':')]
        [TestCase(null, "", '=')]
        [TestCase(null, "", '#')]
        [TestCase("str1,str2,str3", "str1 str2 str3", ' ')]
        [TestCase("str1,str2,str3", "str1:str2:str3", ':')]
        [TestCase("str1,str2,str3", "str1=str2=str3", '=')]
        [TestCase("str1,str2,str3", "str1#str2#str3", '#')]
        public void Combine_InvokeMethod_ResultIsEqual(String actual, String expected, Char separator)
        {
            String[] value = actual?.Split(',');
            Assert.AreEqual(value.Combine(separator), expected);
        }

        [Test]
        [TestCase("str1,str2,str3")]
        public void Combine_InvokeMethod_ThrowsException(String actual)
        {
            String[] value = actual?.Split(',');
            Assert.Throws<ArgumentException>(() => { String result = value.Combine('\0'); });
        }

        [Test]
        [TestCaseSource("ExtractTestObjects")]
        public void Extract_InvokeMethod_ResultIsEqual(Object testObject)
        {
            ArgumentComposerHelper testHelper = testObject as ArgumentComposerHelper;
            Assert.AreEqual(testHelper.Actual.Extract(testHelper.Separator), testHelper.Expected);
        }
    }
}
