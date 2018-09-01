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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plexdata.ArgumentParser.Converters
{
    internal class OptionTypeConverter
    {
        #region Fields

        private static readonly Dictionary<Type, Delegate> types = new Dictionary<Type, Delegate>
        {
            [typeof(String)] = new Func<String, Object>(ToStringSimple),
            [typeof(SByte)] = new Func<String, Object>(ToSByteStandard),
            [typeof(SByte?)] = new Func<String, Object>(ToSByteNullable),
            [typeof(Byte)] = new Func<String, Object>(ToByteStandard),
            [typeof(Byte?)] = new Func<String, Object>(ToByteNullable),
            [typeof(Char)] = new Func<String, Object>(ToCharStandard),
            [typeof(Char?)] = new Func<String, Object>(ToCharNullable),
            [typeof(Int16)] = new Func<String, Object>(ToInt16Standard),
            [typeof(Int16?)] = new Func<String, Object>(ToInt16Nullable),
            [typeof(UInt16)] = new Func<String, Object>(ToUInt16Standard),
            [typeof(UInt16?)] = new Func<String, Object>(ToUInt16Nullable),
            [typeof(Int32)] = new Func<String, Object>(ToInt32Standard),
            [typeof(Int32?)] = new Func<String, Object>(ToInt32Nullable),
            [typeof(UInt32)] = new Func<String, Object>(ToUInt32Standard),
            [typeof(UInt32?)] = new Func<String, Object>(ToUInt32Nullable),
            [typeof(Int64)] = new Func<String, Object>(ToInt64Standard),
            [typeof(Int64?)] = new Func<String, Object>(ToInt64Nullable),
            [typeof(UInt64)] = new Func<String, Object>(ToUInt64Standard),
            [typeof(UInt64?)] = new Func<String, Object>(ToUInt64Nullable),
            [typeof(DateTime)] = new Func<String, Object>(ToDateTimeStandard),
            [typeof(DateTime?)] = new Func<String, Object>(ToDateTimeNullable),
            [typeof(Decimal)] = new Func<String, Object>(ToDecimalStandard),
            [typeof(Decimal?)] = new Func<String, Object>(ToDecimalNullable),
            [typeof(Double)] = new Func<String, Object>(ToDoubleStandard),
            [typeof(Double?)] = new Func<String, Object>(ToDoubleNullable),
            [typeof(Single)] = new Func<String, Object>(ToSingleStandard),
            [typeof(Single?)] = new Func<String, Object>(ToSingleNullable)
        };

        #endregion

        #region Properties

        public static IEnumerable<Type> SupportedTypes
        {
            get
            {
                return OptionTypeConverter.types.Keys;
            }
        }

        #endregion

        #region Public Methods

        public static Boolean IsSupportedType(Type type)
        {
            return OptionTypeConverter.types.ContainsKey(type);
        }

        public static Object Convert(String value, Type type)
        {
            if (OptionTypeConverter.IsSupportedType(type))
            {
                return OptionTypeConverter.types[type].DynamicInvoke(value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static Boolean TryConvert(String value, Type type, out Object result)
        {
            return OptionTypeConverter.TryConvert(value, type, out result, out Exception error);
        }

        public static Boolean TryConvert(String value, Type type, out Object result, out Exception error)
        {
            error = null;
            result = null;
            try
            {
                result = OptionTypeConverter.Convert(value, type);
                return true;
            }
            catch (Exception exception)
            {
                error = exception;

                if (exception is TargetInvocationException)
                {
                    if (exception.InnerException != null)
                    {
                        error = exception.InnerException;
                    }
                }

                return false;
            }
        }

        #endregion

        #region Private Methods

        private static Object ToStringSimple(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToString(value).Replace("\"", String.Empty);
            }
        }

        private static Object ToSByteStandard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToSByte(value);
        }

        private static Object ToSByteNullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToSByte(value);
            }
        }

        private static Object ToByteStandard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToByte(value);
        }

        private static Object ToByteNullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToByte(value);
            }
        }

        private static Object ToCharStandard(String value)
        {
            return System.Convert.ToChar(value.First());
        }

        private static Object ToCharNullable(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToChar(value.First());
            }
        }

        private static Object ToInt16Standard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToInt16(value);
        }

        private static Object ToInt16Nullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToInt16(value);
            }
        }

        private static Object ToUInt16Standard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToUInt16(value);
        }

        private static Object ToUInt16Nullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToUInt16(value);
            }
        }

        private static Object ToInt32Standard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToInt32(value);
        }

        private static Object ToInt32Nullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToInt32(value);
            }
        }

        private static Object ToUInt32Standard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToUInt32(value);
        }

        private static Object ToUInt32Nullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToUInt32(value);
            }
        }

        private static Object ToInt64Standard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToInt64(value);
        }

        private static Object ToInt64Nullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToInt64(value);
            }
        }

        private static Object ToUInt64Standard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToUInt64(value);
        }

        private static Object ToUInt64Nullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToUInt64(value);
            }
        }

        private static Object ToDateTimeStandard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToDateTime(value);
        }

        private static Object ToDateTimeNullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToDateTime(value);
            }
        }

        private static Object ToDecimalStandard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToDecimal(value);
        }

        private static Object ToDecimalNullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToDecimal(value);
            }
        }

        private static Object ToDoubleStandard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToDouble(value);
        }

        private static Object ToDoubleNullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToDouble(value);
            }
        }

        private static Object ToSingleStandard(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToSingle(value);
        }

        private static Object ToSingleNullable(String value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToSingle(value);
            }
        }

        #endregion
    }
}
