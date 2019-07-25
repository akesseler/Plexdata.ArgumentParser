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
using Plexdata.ArgumentParser.Converters;
using System;

namespace Plexdata.ArgumentParser.Tests
{
    [TestFixture]
    [TestOf(nameof(OptionTypeConverter))]
    public class OptionTypeConverterTests
    {
        #region Types

        [Test]
        [TestCase(typeof(String))]
        [TestCase(typeof(SByte))]
        [TestCase(typeof(SByte?))]
        [TestCase(typeof(Byte))]
        [TestCase(typeof(Byte?))]
        [TestCase(typeof(Char))]
        [TestCase(typeof(Char?))]
        [TestCase(typeof(Int16))]
        [TestCase(typeof(Int16?))]
        [TestCase(typeof(UInt16))]
        [TestCase(typeof(UInt16?))]
        [TestCase(typeof(Int32))]
        [TestCase(typeof(Int32?))]
        [TestCase(typeof(UInt32))]
        [TestCase(typeof(UInt32?))]
        [TestCase(typeof(Int64))]
        [TestCase(typeof(Int64?))]
        [TestCase(typeof(UInt64))]
        [TestCase(typeof(UInt64?))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTime?))]
        [TestCase(typeof(Decimal))]
        [TestCase(typeof(Decimal?))]
        [TestCase(typeof(Double))]
        [TestCase(typeof(Double?))]
        [TestCase(typeof(Single))]
        [TestCase(typeof(Single?))]
        public void TryConvert_CheckSupportedType_ResultIsEqual(Type type)
        {
            Assert.IsTrue(OptionTypeConverter.IsSupportedType(type));
        }

        [Test]
        [TestCase(typeof(Boolean))]
        [TestCase(typeof(Boolean?))]
        public void TryConvert_CheckUnsupportedType_ResultIsEqual(Type type)
        {
            Assert.IsFalse(OptionTypeConverter.IsSupportedType(type));
        }

        [Test]
        [TestCase("42", typeof(Int16), 42)]
        [TestCase("42", typeof(Int16?), 42)]
        public void Convert_SupportedType_ResultIsEqual(String actual, Type type, Object expected)
        {
            Assert.AreEqual(OptionTypeConverter.Convert(actual, type), expected);
        }

        [Test]
        [TestCase("True", typeof(Boolean))]
        [TestCase("True", typeof(Boolean?))]
        public void Convert_UnsupportedType_ThrowsException(String actual, Type type)
        {
            Assert.Throws<NotSupportedException>(() => { OptionTypeConverter.Convert(actual, type); });
        }

        #endregion

        #region Byte / SByte / Char / String

        [Test]
        [TestCase(null, typeof(Byte), false, null)]
        [TestCase(null, typeof(Byte?), true, null)]
        [TestCase("", typeof(Byte), false, null)]
        [TestCase("", typeof(Byte?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(Byte), false, null)]
        [TestCase("  \t \v \n \r ", typeof(Byte?), true, null)]
        [TestCase("42abc", typeof(Byte), false, null)]
        [TestCase("abc42", typeof(Byte?), false, null)]
        [TestCase("42", typeof(Byte), true, 42)]
        [TestCase(" 42 ", typeof(Byte), true, 42)]
        [TestCase("42", typeof(Byte?), true, 42)]
        [TestCase("\t42 ", typeof(Byte?), true, 42)]
        [TestCase("-42", typeof(Byte), false, null)]
        [TestCase(" -42\t", typeof(Byte), false, null)]
        [TestCase("0", typeof(Byte), true, Byte.MinValue)]
        [TestCase("0", typeof(Byte?), true, Byte.MinValue)]
        [TestCase("255", typeof(Byte), true, Byte.MaxValue)]
        [TestCase("255", typeof(Byte?), true, Byte.MaxValue)]
        public void TryConvert_ByteConversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        [Test]
        [TestCase(null, typeof(SByte), false, null)]
        [TestCase(null, typeof(SByte?), true, null)]
        [TestCase("", typeof(SByte), false, null)]
        [TestCase("", typeof(SByte?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(SByte), false, null)]
        [TestCase("  \t \v \n \r ", typeof(SByte?), true, null)]
        [TestCase("42abc", typeof(SByte), false, null)]
        [TestCase("abc42", typeof(SByte?), false, null)]
        [TestCase("42", typeof(SByte), true, 42)]
        [TestCase(" 42 ", typeof(SByte), true, 42)]
        [TestCase("42", typeof(SByte?), true, 42)]
        [TestCase("\t42 ", typeof(SByte?), true, 42)]
        [TestCase("-42", typeof(SByte), true, -42)]
        [TestCase(" -42\t", typeof(SByte), true, -42)]
        [TestCase("-42", typeof(SByte?), true, -42)]
        [TestCase("  -42  ", typeof(SByte?), true, -42)]
        [TestCase("300", typeof(SByte), false, null)]
        [TestCase("300", typeof(SByte?), false, null)]
        [TestCase("-300", typeof(SByte), false, null)]
        [TestCase("-300", typeof(SByte?), false, null)]
        [TestCase("-128", typeof(SByte), true, SByte.MinValue)]
        [TestCase("-128", typeof(SByte?), true, SByte.MinValue)]
        [TestCase("127", typeof(SByte), true, SByte.MaxValue)]
        [TestCase("127", typeof(SByte?), true, SByte.MaxValue)]
        public void TryConvert_SByteConversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        [Test]
        [TestCase(null, typeof(Char), false, null)]
        [TestCase(null, typeof(Char?), true, null)]
        [TestCase("", typeof(Char), false, null)]
        [TestCase("", typeof(Char?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(Char), true, ' ')]
        [TestCase("  \t \v \n \r ", typeof(Char?), true, ' ')]
        [TestCase("A string value", typeof(Char), true, 'A')]
        [TestCase("A string value", typeof(Char?), true, 'A')]
        [TestCase("   A string value", typeof(Char?), true, ' ')]
        [TestCase("\tA string value", typeof(Char?), true, '\t')]
        [TestCase("\0", typeof(Char), true, Char.MinValue)]
        [TestCase("\uffff", typeof(Char), true, Char.MaxValue)]
        [TestCase("\0", typeof(Char?), true, Char.MinValue)]
        [TestCase("\uffff", typeof(Char?), true, Char.MaxValue)]
        public void TryConvert_CharConversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        [Test]
        [TestCase(null, typeof(String), true, null)]
        [TestCase("", typeof(String), true, null)]
        [TestCase("  \t \v \n \r ", typeof(String), true, "  \t \v \n \r ")]
        [TestCase("A string value", typeof(String), true, "A string value")]
        [TestCase("   A string \tvalue  ", typeof(String), true, "   A string \tvalue  ")]
        public void TryConvert_StringConversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        #endregion

        #region Int16 / UInt16 / Int32 / UInt32 / Int64 / UInt64

        [Test]
        [TestCase(null, typeof(Int16), false, null)]
        [TestCase(null, typeof(Int16?), true, null)]
        [TestCase("", typeof(Int16), false, null)]
        [TestCase("", typeof(Int16?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(Int16), false, null)]
        [TestCase("  \t \v \n \r ", typeof(Int16?), true, null)]
        [TestCase("42abc", typeof(Int16), false, null)]
        [TestCase("abc42", typeof(Int16?), false, null)]
        [TestCase("42", typeof(Int16), true, 42)]
        [TestCase(" 42 ", typeof(Int16), true, 42)]
        [TestCase("42", typeof(Int16?), true, 42)]
        [TestCase(" 42 ", typeof(Int16?), true, 42)]
        [TestCase("-42", typeof(Int16), true, -42)]
        [TestCase(" -42 ", typeof(Int16), true, -42)]
        [TestCase("-42", typeof(Int16?), true, -42)]
        [TestCase("\t-42 ", typeof(Int16?), true, -42)]
        [TestCase("32768", typeof(Int16), false, null)]
        [TestCase("32768", typeof(Int16?), false, null)]
        [TestCase("-32769", typeof(Int16), false, null)]
        [TestCase("-32769", typeof(Int16?), false, null)]
        [TestCase("-32768", typeof(Int16), true, Int16.MinValue)]
        [TestCase("-32768", typeof(Int16?), true, Int16.MinValue)]
        [TestCase("32767", typeof(Int16), true, Int16.MaxValue)]
        [TestCase("32767", typeof(Int16?), true, Int16.MaxValue)]
        public void TryConvert_Int16Conversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        [Test]
        [TestCase(null, typeof(UInt16), false, null)]
        [TestCase(null, typeof(UInt16?), true, null)]
        [TestCase("", typeof(UInt16), false, null)]
        [TestCase("", typeof(UInt16?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(UInt16), false, null)]
        [TestCase("  \t \v \n \r ", typeof(UInt16?), true, null)]
        [TestCase("42abc", typeof(UInt16), false, null)]
        [TestCase("abc42", typeof(UInt16?), false, null)]
        [TestCase("42", typeof(UInt16), true, 42)]
        [TestCase(" 42 ", typeof(UInt16), true, 42)]
        [TestCase("42", typeof(UInt16?), true, 42)]
        [TestCase(" 42 ", typeof(UInt16?), true, 42)]
        [TestCase("-42", typeof(UInt16), false, null)]
        [TestCase("\t-42 ", typeof(UInt16), false, null)]
        [TestCase("-42", typeof(UInt16?), false, null)]
        [TestCase(" -42\t", typeof(UInt16?), false, null)]
        [TestCase("65536", typeof(UInt16), false, null)]
        [TestCase("65536", typeof(UInt16?), false, null)]
        [TestCase("-1", typeof(UInt16), false, null)]
        [TestCase("-1", typeof(UInt16?), false, null)]
        [TestCase("0", typeof(UInt16), true, UInt16.MinValue)]
        [TestCase("0", typeof(UInt16?), true, UInt16.MinValue)]
        [TestCase("65535", typeof(UInt16), true, UInt16.MaxValue)]
        [TestCase("65535", typeof(UInt16?), true, UInt16.MaxValue)]
        public void TryConvert_UInt16Conversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        [Test]
        [TestCase(null, typeof(Int32), false, null)]
        [TestCase(null, typeof(Int32?), true, null)]
        [TestCase("", typeof(Int32), false, null)]
        [TestCase("", typeof(Int32?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(Int32), false, null)]
        [TestCase("  \t \v \n \r ", typeof(Int32?), true, null)]
        [TestCase("42abc", typeof(Int32), false, null)]
        [TestCase("abc42", typeof(Int32?), false, null)]
        [TestCase("42", typeof(Int32), true, 42)]
        [TestCase(" 42 ", typeof(Int32), true, 42)]
        [TestCase("42", typeof(Int32?), true, 42)]
        [TestCase(" 42 ", typeof(Int32?), true, 42)]
        [TestCase("-42", typeof(Int32), true, -42)]
        [TestCase("\t-42 ", typeof(Int32), true, -42)]
        [TestCase("-42", typeof(Int32?), true, -42)]
        [TestCase(" -42\t", typeof(Int32?), true, -42)]
        [TestCase("2147483648", typeof(Int32), false, null)]
        [TestCase("2147483648", typeof(Int32?), false, null)]
        [TestCase("-2147483649", typeof(Int32), false, null)]
        [TestCase("-2147483649", typeof(Int32?), false, null)]
        [TestCase("-2147483648", typeof(Int32), true, Int32.MinValue)]
        [TestCase("-2147483648", typeof(Int32?), true, Int32.MinValue)]
        [TestCase("2147483647", typeof(Int32), true, Int32.MaxValue)]
        [TestCase("2147483647", typeof(Int32?), true, Int32.MaxValue)]
        public void TryConvert_Int32Conversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        [Test]
        [TestCase(null, typeof(UInt32), false, null)]
        [TestCase(null, typeof(UInt32?), true, null)]
        [TestCase("", typeof(UInt32), false, null)]
        [TestCase("", typeof(UInt32?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(UInt32), false, null)]
        [TestCase("  \t \v \n \r ", typeof(UInt32?), true, null)]
        [TestCase("42abc", typeof(UInt32), false, null)]
        [TestCase("abc42", typeof(UInt32?), false, null)]
        [TestCase("42", typeof(UInt32), true, 42)]
        [TestCase(" 42 ", typeof(UInt32), true, 42)]
        [TestCase("42", typeof(UInt32?), true, 42)]
        [TestCase(" 42 ", typeof(UInt32?), true, 42)]
        [TestCase("-42", typeof(UInt32), false, null)]
        [TestCase("\t-42 ", typeof(UInt32), false, null)]
        [TestCase("-42", typeof(UInt32?), false, null)]
        [TestCase(" -42\t", typeof(UInt32?), false, null)]
        [TestCase("4294967296", typeof(UInt32), false, null)]
        [TestCase("4294967296", typeof(UInt32?), false, null)]
        [TestCase("-1", typeof(UInt32), false, null)]
        [TestCase("-1", typeof(UInt32?), false, null)]
        [TestCase("0", typeof(UInt32), true, UInt32.MinValue)]
        [TestCase("0", typeof(UInt32?), true, UInt32.MinValue)]
        [TestCase("4294967295", typeof(UInt32), true, UInt32.MaxValue)]
        [TestCase("4294967295", typeof(UInt32?), true, UInt32.MaxValue)]
        public void TryConvert_UInt32Conversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        [Test]
        [TestCase(null, typeof(Int64), false, null)]
        [TestCase(null, typeof(Int64?), true, null)]
        [TestCase("", typeof(Int64), false, null)]
        [TestCase("", typeof(Int64?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(Int64), false, null)]
        [TestCase("  \t \v \n \r ", typeof(Int64?), true, null)]
        [TestCase("42abc", typeof(Int64), false, null)]
        [TestCase("abc42", typeof(Int64?), false, null)]
        [TestCase("42", typeof(Int64), true, 42)]
        [TestCase(" 42 ", typeof(Int64), true, 42)]
        [TestCase("42", typeof(Int64?), true, 42)]
        [TestCase(" 42 ", typeof(Int64?), true, 42)]
        [TestCase("-42", typeof(Int64), true, -42)]
        [TestCase("\t-42 ", typeof(Int64), true, -42)]
        [TestCase("-42", typeof(Int64?), true, -42)]
        [TestCase(" -42\t", typeof(Int64?), true, -42)]
        [TestCase("9223372036854775808", typeof(Int64), false, null)]
        [TestCase("9223372036854775808", typeof(Int64?), false, null)]
        [TestCase("-9223372036854775809", typeof(Int64), false, null)]
        [TestCase("-9223372036854775809", typeof(Int64?), false, null)]
        [TestCase("-9223372036854775808", typeof(Int64), true, Int64.MinValue)]
        [TestCase("-9223372036854775808", typeof(Int64?), true, Int64.MinValue)]
        [TestCase("9223372036854775807", typeof(Int64), true, Int64.MaxValue)]
        [TestCase("9223372036854775807", typeof(Int64?), true, Int64.MaxValue)]
        public void TryConvert_Int64Conversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        [Test]
        [TestCase(null, typeof(UInt64), false, null)]
        [TestCase(null, typeof(UInt64?), true, null)]
        [TestCase("", typeof(UInt64), false, null)]
        [TestCase("", typeof(UInt64?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(UInt64), false, null)]
        [TestCase("  \t \v \n \r ", typeof(UInt64?), true, null)]
        [TestCase("42abc", typeof(UInt64), false, null)]
        [TestCase("abc42", typeof(UInt64?), false, null)]
        [TestCase("42", typeof(UInt64), true, 42)]
        [TestCase(" 42 ", typeof(UInt64), true, 42)]
        [TestCase("42", typeof(UInt64?), true, 42)]
        [TestCase(" 42 ", typeof(UInt64?), true, 42)]
        [TestCase("-42", typeof(UInt64), false, null)]
        [TestCase("\t-42 ", typeof(UInt64), false, null)]
        [TestCase("-42", typeof(UInt64?), false, null)]
        [TestCase(" -42\t", typeof(UInt64?), false, null)]
        [TestCase("18446744073709551616", typeof(UInt64), false, null)]
        [TestCase("18446744073709551616", typeof(UInt64?), false, null)]
        [TestCase("-1", typeof(UInt64), false, null)]
        [TestCase("-1", typeof(UInt64?), false, null)]
        [TestCase("0", typeof(UInt64), true, UInt64.MinValue)]
        [TestCase("0", typeof(UInt64?), true, UInt64.MinValue)]
        [TestCase("18446744073709551615", typeof(UInt64), true, UInt64.MaxValue)]
        [TestCase("18446744073709551615", typeof(UInt64?), true, UInt64.MaxValue)]
        public void TryConvert_UInt64Conversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        #endregion

        #region DateTime

        [Test]
        [TestCase(null, typeof(DateTime), false, null)]
        [TestCase(null, typeof(DateTime?), true, null)]
        [TestCase("", typeof(DateTime), false, null)]
        [TestCase("", typeof(DateTime?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(DateTime), false, null)]
        [TestCase("  \t \v \n \r ", typeof(DateTime?), true, null)]
        [TestCase("42abc", typeof(DateTime), false, null)]
        [TestCase("abc42", typeof(DateTime?), false, null)]
        [TestCase("29.10.1967 23:05:42", typeof(DateTime), true, "29.10.1967 23:05:42")]
        [TestCase("  29.10.1967 23:05:42  ", typeof(DateTime), true, "29.10.1967 23:05:42")]
        [TestCase("29.10.1967 23:05:42", typeof(DateTime?), true, "29.10.1967 23:05:42")]
        [TestCase("  29.10.1967 23:05:42  ", typeof(DateTime?), true, "29.10.1967 23:05:42")]
        [TestCase("29.10.1967     23:05:42", typeof(DateTime), true, "29.10.1967 23:05:42")]
        [TestCase("  29.10.1967     23:05:42  ", typeof(DateTime), true, "29.10.1967 23:05:42")]
        [TestCase("29.10.1967     23:05:42", typeof(DateTime?), true, "29.10.1967 23:05:42")]
        [TestCase("  29.10.1967     23:05:42  ", typeof(DateTime?), true, "29.10.1967 23:05:42")]
        [TestCase("29.10.1967", typeof(DateTime), true, "29.10.1967 00:00:00")]
        [TestCase("29.10.1967", typeof(DateTime?), true, "29.10.1967 00:00:00")]
        [TestCase("23:05:42", typeof(DateTime), true, "current-date 23:05:42")]
        [TestCase("23:05:42", typeof(DateTime?), true, "current-date 23:05:42")]
        [TestCase("0000-00-00T00:00:00", typeof(DateTime), false, null)]
        [TestCase("0000-00-00T00:00:00", typeof(DateTime?), false, null)]
        [TestCase("0001-01-01T00:00:00", typeof(DateTime), true, "01.01.0001 00:00:00")]
        [TestCase("0001-01-01T00:00:00", typeof(DateTime?), true, "01.01.0001 00:00:00")]
        [TestCase("9999-12-31T23:59:59", typeof(DateTime), true, "31.12.9999 23:59:59")]
        [TestCase("9999-12-31T23:59:59", typeof(DateTime?), true, "31.12.9999 23:59:59")]
        public void TryConvert_DateTimeConversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (expected != null && expected.ToString().StartsWith("current-date"))
            {
                expected = (Object)expected.ToString().Replace("current-date", DateTime.Today.ToString("dd.MM.yyyy"));
            }

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected == null ? expected : DateTime.Parse(expected.ToString()));
        }

        #endregion

        #region Decimal, Double, Single

        [Test]
        [TestCase(null, typeof(Decimal), false, null)]
        [TestCase(null, typeof(Decimal?), true, null)]
        [TestCase("", typeof(Decimal), false, null)]
        [TestCase("", typeof(Decimal?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(Decimal), false, null)]
        [TestCase("  \t \v \n \r ", typeof(Decimal?), true, null)]
        [TestCase("42abc", typeof(Decimal), false, null)]
        [TestCase("abc42", typeof(Decimal?), false, null)]
        [TestCase("42", typeof(Decimal), true, 42)]
        [TestCase(" 42 ", typeof(Decimal), true, 42)]
        [TestCase("42", typeof(Decimal?), true, 42)]
        [TestCase(" 42 ", typeof(Decimal?), true, 42)]
        [TestCase("-42", typeof(Decimal), true, -42)]
        [TestCase(" -42 ", typeof(Decimal), true, -42)]
        [TestCase("-42", typeof(Decimal?), true, -42)]
        [TestCase("\t-42 ", typeof(Decimal?), true, -42)]
        [TestCase("79228162514264337593543950336", typeof(Decimal), false, null)]
        [TestCase("79228162514264337593543950336", typeof(Decimal?), false, null)]
        [TestCase("-79228162514264337593543950336", typeof(Decimal), false, null)]
        [TestCase("-79228162514264337593543950336", typeof(Decimal?), false, null)]
        [TestCase("-79228162514264337593543950335", typeof(Decimal), true, "-79228162514264337593543950335")]
        [TestCase("-79228162514264337593543950335", typeof(Decimal?), true, "-79228162514264337593543950335")]
        [TestCase("79228162514264337593543950335", typeof(Decimal), true, "79228162514264337593543950335")]
        [TestCase("79228162514264337593543950335", typeof(Decimal?), true, "79228162514264337593543950335")]
        public void TryConvert_DecimalConversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected == null ? expected : Decimal.Parse(expected.ToString()));
        }

        [Test]
        [TestCase(null, typeof(Double), false, null)]
        [TestCase(null, typeof(Double?), true, null)]
        [TestCase("", typeof(Double), false, null)]
        [TestCase("", typeof(Double?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(Double), false, null)]
        [TestCase("  \t \v \n \r ", typeof(Double?), true, null)]
        [TestCase("42abc", typeof(Double), false, null)]
        [TestCase("abc42", typeof(Double?), false, null)]
        [TestCase("42", typeof(Double), true, 42)]
        [TestCase(" 42 ", typeof(Double), true, 42)]
        [TestCase("42", typeof(Double?), true, 42)]
        [TestCase(" 42 ", typeof(Double?), true, 42)]
        [TestCase("-42", typeof(Double), true, -42)]
        [TestCase(" -42 ", typeof(Double), true, -42)]
        [TestCase("-42", typeof(Double?), true, -42)]
        [TestCase("\t-42 ", typeof(Double?), true, -42)]
        [TestCase("1.7976931348623158E+308", typeof(Double), false, null)]
        [TestCase("1.7976931348623158E+308", typeof(Double?), false, null)]
        [TestCase("-1.7976931348623158E+308", typeof(Double), false, null)]
        [TestCase("-1.7976931348623158E+308", typeof(Double?), false, null)]
        // This doesn't actually work! [TestCase("-1.7976931348623157E+308", typeof(Double), true, Double.MinValue)]
        // This doesn't actually work! [TestCase("-1.7976931348623157E+308", typeof(Double?), true, Double.MinValue)]
        // This doesn't actually work! [TestCase("1.7976931348623157E+308", typeof(Double), true, Double.MaxValue)]
        // This doesn't actually work! [TestCase("1.7976931348623157E+308", typeof(Double?), true, Double.MaxValue)]
        public void TryConvert_DoubleConversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        [Test]
        [TestCase(null, typeof(Single), false, null)]
        [TestCase(null, typeof(Single?), true, null)]
        [TestCase("", typeof(Single), false, null)]
        [TestCase("", typeof(Single?), true, null)]
        [TestCase("  \t \v \n \r ", typeof(Single), false, null)]
        [TestCase("  \t \v \n \r ", typeof(Single?), true, null)]
        [TestCase("42abc", typeof(Single), false, null)]
        [TestCase("abc42", typeof(Single?), false, null)]
        [TestCase("42", typeof(Single), true, 42)]
        [TestCase(" 42 ", typeof(Single), true, 42)]
        [TestCase("42", typeof(Single?), true, 42)]
        [TestCase(" 42 ", typeof(Single?), true, 42)]
        [TestCase("-42", typeof(Single), true, -42)]
        [TestCase(" -42 ", typeof(Single), true, -42)]
        [TestCase("-42", typeof(Single?), true, -42)]
        [TestCase("\t-42 ", typeof(Single?), true, -42)]
        [TestCase("3.40282347E+38", typeof(Single), false, null)]
        [TestCase("3.40282347E+38", typeof(Single?), false, null)]
        [TestCase("-3.40282347E+38", typeof(Single), false, null)]
        [TestCase("-3.40282347E+38", typeof(Single?), false, null)]
        // This doesn't actually work! [TestCase("-3.40282347E+38", typeof(Single), true, Single.MinValue)]
        // This doesn't actually work! [TestCase("-3.40282347E+38", typeof(Single?), true, Single.MinValue)]
        // This doesn't actually work! [TestCase("3.40282347E+38", typeof(Single), true, Single.MaxValue)]
        // This doesn't actually work! [TestCase("3.40282347E+38", typeof(Single?), true, Single.MaxValue)]
        public void TryConvert_SingleConversion_ResultIsEqual(String actual, Type type, Boolean isTrue, Object expected)
        {
            Object result = null;

            if (isTrue)
            {
                Assert.IsTrue(OptionTypeConverter.TryConvert(actual, type, out result));
            }
            else
            {
                Assert.IsFalse(OptionTypeConverter.TryConvert(actual, type, out result));
            }

            Assert.AreEqual(result, expected);
        }

        #endregion
    }
}
