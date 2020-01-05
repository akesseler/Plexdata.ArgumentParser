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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plexdata.ArgumentParser.Converters
{
    /// <summary>
    /// The internally used option type converter.
    /// </summary>
    /// <remarks>
    /// This class is used to conververt option strings into their 
    /// expected value type.
    /// </remarks>
    internal class OptionTypeConverter
    {
        #region Fields

        /// <summary>
        /// The types field.
        /// </summary>
        /// <remarks>
        /// The field contains the type dependent list of conversion method references.
        /// </remarks>
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

        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the static fields of this class.
        /// </remarks>
        static OptionTypeConverter() { }

        /// <summary>
        /// The default class constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the fields and properties of this class.
        /// </remarks>
        public OptionTypeConverter() { }

        #endregion

        #region Properties

        /// <summary>
        /// The getter of supported types.
        /// </summary>
        /// <remarks>
        /// The property getter to retrieve the list of supported conversion types. 
        /// </remarks>
        /// <value>
        /// The list of supported conversion types. 
        /// </value>
        public static IEnumerable<Type> SupportedTypes
        {
            get
            {
                return OptionTypeConverter.types.Keys;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Convenient method to test if a particular type is supported.
        /// </summary>
        /// <remarks>
        /// This convenient method allows to test if a particular type is supported or not.
        /// </remarks>
        /// <param name="type">
        /// The type to be checked.
        /// </param>
        /// <returns>
        /// True if provided type is supported, otherwise false.
        /// </returns>
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

        /// <summary>
        /// Tries to convert provided value into its expected type.
        /// </summary>
        /// <remarks>
        /// This method tries to convert provided <paramref name="value"/> 
        /// into its expected <paramref name="type"/>.
        /// </remarks>
        /// <param name="value">
        /// The option string to be converted.
        /// </param>
        /// <param name="type">
        /// The expected option type.
        /// </param>
        /// <param name="result">
        /// The output parameter to retrieve the conversion result.
        /// </param>
        /// <returns>
        /// True if type conversion was successful and false otherwise.
        /// </returns>
        public static Boolean TryConvert(String value, Type type, out Object result)
        {
            return OptionTypeConverter.TryConvert(value, type, out result, out Exception error);
        }

        /// <summary>
        /// Tries to convert provided value into its expected type.
        /// </summary>
        /// <remarks>
        /// This method tries to convert provided <paramref name="value"/> 
        /// into its expected <paramref name="type"/>.
        /// </remarks>
        /// <param name="value">
        /// The option string to be converted.
        /// </param>
        /// <param name="type">
        /// The expected option type.
        /// </param>
        /// <param name="result">
        /// The output parameter to retrieve the conversion result.
        /// </param>
        /// <param name="error">
        /// An instance of type <see cref="Exception"/> to retrieve errors.
        /// </param>
        /// <returns>
        /// True if type conversion was successful and false otherwise.
        /// </returns>
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

        /// <summary>
        /// Converts the value into a string.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a string. 
        /// All escaped double-quotes are removed if the value contains them.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
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

        /// <summary>
        /// Converts the value into a signed byte.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a signed byte. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToSByteStandard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToSByte(value);
        }

        /// <summary>
        /// Converts the value into a signed byte.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable signed byte. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value or <c>null</c>.
        /// </returns>
        private static Object ToSByteNullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToSByte(value);
            }
        }

        /// <summary>
        /// Converts the value into an unsigned byte.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into an unsigned byte.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToByteStandard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToByte(value);
        }

        /// <summary>
        /// Converts the value into an unsigned byte.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable unsigned byte. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value or <c>null</c>.
        /// </returns>
        private static Object ToByteNullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToByte(value);
            }
        }

        /// <summary>
        /// Converts the value into a character.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a character.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToCharStandard(String value)
        {
            return System.Convert.ToChar(value.First());
        }

        /// <summary>
        /// Converts the value into a character.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable character. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value or <c>null</c>.
        /// </returns>
        private static Object ToCharNullable(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToChar(value.First());
            }
        }

        /// <summary>
        /// Converts the value into a 16-bit signed integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a 16-bit signed integer.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToInt16Standard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToInt16(value);
        }

        /// <summary>
        /// Converts the value into a 16-bit signed integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable 16-bit signed integer. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value or <c>null</c>.
        /// </returns>
        private static Object ToInt16Nullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToInt16(value);
            }
        }

        /// <summary>
        /// Converts the value into a 16-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a 16-bit unsigned integer.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToUInt16Standard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToUInt16(value);
        }

        /// <summary>
        /// Converts the value into a 16-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable 16-bit unsigned integer. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value or <c>null</c>.
        /// </returns>
        private static Object ToUInt16Nullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToUInt16(value);
            }
        }

        /// <summary>
        /// Converts the value into a 32-bit signed integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a 32-bit signed integer.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToInt32Standard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToInt32(value);
        }

        /// <summary>
        /// Converts the value into a 32-bit signed integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable 32-bit signed integer. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value or <c>null</c>.
        /// </returns>
        private static Object ToInt32Nullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Converts the value into a 32-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a 32-bit unsigned integer.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToUInt32Standard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToUInt32(value);
        }

        /// <summary>
        /// Converts the value into a 32-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable 32-bit unsigned integer. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value or <c>null</c>.
        /// </returns>
        private static Object ToUInt32Nullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToUInt32(value);
            }
        }

        /// <summary>
        /// Converts the value into a 64-bit signed integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a 64-bit signed integer.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToInt64Standard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToInt64(value);
        }

        /// <summary>
        /// Converts the value into a 64-bit signed integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable 64-bit signed integer. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value or <c>null</c>.
        /// </returns>
        private static Object ToInt64Nullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToInt64(value);
            }
        }

        /// <summary>
        /// Converts the value into a 64-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a 64-bit unsigned integer.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToUInt64Standard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToUInt64(value);
        }

        /// <summary>
        /// Converts the value into a 64-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable 64-bit unsigned integer. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value or <c>null</c>.
        /// </returns>
        private static Object ToUInt64Nullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToUInt64(value);
            }
        }

        /// <summary>
        /// Converts the value into a date time value.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a date time value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToDateTimeStandard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToDateTime(value);
        }

        /// <summary>
        /// Converts the value into a date time value.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable date time value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToDateTimeNullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToDateTime(value);
            }
        }

        /// <summary>
        /// Converts the value into a decimal value.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a decimal value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToDecimalStandard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToDecimal(value);
        }

        /// <summary>
        /// Converts the value into a decimal value.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable decimal value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToDecimalNullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToDecimal(value);
            }
        }

        /// <summary>
        /// Converts the value into a double value.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a double value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToDoubleStandard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToDouble(value);
        }

        /// <summary>
        /// Converts the value into a double value.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable double value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToDoubleNullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return System.Convert.ToDouble(value);
            }
        }

        /// <summary>
        /// Converts the value into a single value.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a single value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToSingleStandard(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return System.Convert.ToSingle(value);
        }

        /// <summary>
        /// Converts the value into a single value.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into a nullable single value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided value is <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </exception>
        private static Object ToSingleNullable(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
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
