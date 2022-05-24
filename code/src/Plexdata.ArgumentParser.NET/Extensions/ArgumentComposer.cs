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

using Plexdata.ArgumentParser.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plexdata.ArgumentParser.Extensions
{
    /// <summary>
    /// The argument composer extension.
    /// </summary>
    /// <remarks>
    /// Task of class argument composer is to provide some useful methods which might 
    /// be used by users.
    /// </remarks>
    public static class ArgumentComposer
    {
        #region Fields

        /// <summary>
        /// The double quotes field.
        /// </summary>
        /// <remarks>
        /// The field contains the double quotes value.
        /// </remarks>
        private const Char DoubleQuotes = '"';

        /// <summary>
        /// The zero termination field.
        /// </summary>
        /// <remarks>
        /// The field contains the zero termination value.
        /// </remarks>
        private const Char ZeroTermination = '\0';

        #endregion

        #region Publics

        /// <summary>
        /// This method tries to split a given string into its pieces.
        /// </summary>
        /// <remarks>
        /// If the given string contains a subsequence that is surrounded 
        /// by double quotes then this subsequence is kept together.
        /// </remarks>
        /// <param name="arguments">
        /// A typical command line argument string that should be split into pieces.
        /// </param>
        /// <returns>
        /// A string list representing all items of the processed argument string.
        /// </returns>
        public static String[] Extract(this String arguments)
        {
            return arguments.Extract(ParameterSeparators.DefaultSeparator, false);
        }

        /// <summary>
        /// This method tries to split a given string into its pieces and tries 
        /// to remove the path of the executable.
        /// </summary>
        /// <remarks>
        /// If the given string contains a subsequence that is surrounded 
        /// by double quotes then this subsequence is kept together.
        /// </remarks>
        /// <param name="arguments">
        /// A typical command line argument string that should be split into pieces.
        /// </param>
        /// <param name="cleaned">
        /// True if the path of the executable shall be removed and false if not.
        /// </param>
        /// <returns>
        /// A string list representing all items of the processed argument string.
        /// </returns>
        public static String[] Extract(this String arguments, Boolean cleaned)
        {
            return arguments.Extract(ParameterSeparators.DefaultSeparator, cleaned);
        }

        /// <summary>
        /// This method tries to split a given string into its pieces using given 
        /// separator.
        /// </summary>
        /// <remarks>
        /// If the given string contains a subsequence that is surrounded 
        /// by double quotes then this subsequence is kept together.
        /// </remarks>
        /// <param name="arguments">
        /// A typical command line argument string that should be split into pieces.
        /// </param>
        /// <param name="separator">
        /// The separator to be used to split the arguments.
        /// </param>
        /// <returns>
        /// A string list representing all items of the processed argument string.
        /// </returns>
        public static String[] Extract(this String arguments, Char separator)
        {
            return arguments.Extract(separator, false);
        }

        /// <summary>
        /// This method tries to split a given string into its pieces using given 
        /// separator and tries to remove the path of the executable.
        /// </summary>
        /// <remarks>
        /// If the given string contains a subsequence that is surrounded 
        /// by double quotes then this subsequence is kept together.
        /// </remarks>
        /// <param name="arguments">
        /// A typical command line argument string that should be split into pieces.
        /// </param>
        /// <param name="separator">
        /// The separator to be used to split the arguments.
        /// </param>
        /// <param name="cleaned">
        /// True if the path of the executable shall be removed and false if not.
        /// </param>
        /// <returns>
        /// A string list representing all items of the processed argument string.
        /// </returns>
        public static String[] Extract(this String arguments, Char separator, Boolean cleaned)
        {
            if (arguments is null)
            {
                return new String[0];
            }

            if (separator == ArgumentComposer.DoubleQuotes)
            {
                throw new NotSupportedException("Double quotes are not supported as separator.");
            }

            Boolean inside = false;

            List<String> results = new List<String>();

            StringBuilder builder = new StringBuilder(512);

            foreach (Char current in arguments)
            {
                if (!inside && current == ArgumentComposer.DoubleQuotes)
                {
                    inside = true;
                    continue;
                }

                if (inside)
                {
                    if (current != ArgumentComposer.DoubleQuotes)
                    {
                        builder.Append(current);
                    }
                    else
                    {
                        if (builder.Length > 0)
                        {
                            results.TryAddValue(builder.ToString(), cleaned);
                            builder.Length = 0;
                        }

                        inside = false;
                    }
                    continue;
                }

                if (current == separator)
                {
                    if (builder.Length > 0)
                    {
                        results.TryAddValue(builder.ToString(), cleaned);
                        builder.Length = 0;
                    }
                }
                else
                {
                    builder.Append(current);
                }
            }

            if (builder.Length > 0)
            {
                results.TryAddValue(builder.ToString(), cleaned);
                builder.Length = 0;
            }

            return results.ToArray();
        }

        /// <summary>
        /// This method converts the given list of strings into one single string using 
        /// the default separator as delimiter.
        /// </summary>
        /// <remarks>
        /// If one of string inside the list does contain whitespaces then this string 
        /// part is surrounded by double quotes.
        /// </remarks>
        /// <param name="arguments">
        /// The list of string to be combined.
        /// </param>
        /// <returns>
        /// A string representing all items of the processed argument list.
        /// </returns>
        public static String Combine(this String[] arguments)
        {
            return ArgumentComposer.Combine(arguments, ParameterSeparators.DefaultSeparator);
        }

        /// <summary>
        /// This method converts the given list of strings into one single string using 
        /// given separator as delimiter.
        /// </summary>
        /// <remarks>
        /// If one of string inside the list does contain whitespaces then this string 
        /// part is surrounded by double quotes.
        /// </remarks>
        /// <param name="arguments">
        /// The list of string to be combined.
        /// </param>
        /// <param name="separator">
        /// The separator to be used as delimiter.
        /// </param>
        /// <returns>
        /// A string representing all items of the processed argument list.
        /// </returns>
        public static String Combine(this String[] arguments, Char separator)
        {
            if (arguments != null)
            {
                return ArgumentComposer.Combine(arguments.ToList(), separator);
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// This method converts the given list of strings into one single string using 
        /// the default separator as delimiter.
        /// </summary>
        /// <remarks>
        /// If one of string inside the list does contain whitespaces then this string 
        /// part is surrounded by double quotes.
        /// </remarks>
        /// <param name="arguments">
        /// The list of string to be combined.
        /// </param>
        /// <returns>
        /// A string representing all items of the processed argument list.
        /// </returns>
        public static String Combine(this IEnumerable<String> arguments)
        {
            return ArgumentComposer.Combine(arguments, ParameterSeparators.DefaultSeparator);
        }

        /// <summary>
        /// This method converts the given list of strings into one single string using 
        /// given separator as delimiter.
        /// </summary>
        /// <remarks>
        /// If one of string inside the list does contain whitespaces then this string 
        /// part is surrounded by double quotes.
        /// </remarks>
        /// <param name="arguments">
        /// The list of string to be combined.
        /// </param>
        /// <param name="separator">
        /// The separator to be used as delimiter.
        /// </param>
        /// <returns>
        /// A string representing all items of the processed argument list.
        /// </returns>
        public static String Combine(this IEnumerable<String> arguments, Char separator)
        {
            if (separator == ArgumentComposer.ZeroTermination)
            {
                throw new ArgumentException("A zero termination is not allowed as separator character.", nameof(separator));
            }

            StringBuilder builder = new StringBuilder(512);

            if (arguments != null && arguments.Any())
            {
                foreach (String argument in arguments)
                {
                    if (!String.IsNullOrWhiteSpace(argument))
                    {
                        if (argument.HasWhiteSpaces())
                        {
                            builder.Append($"\"{argument}\"{separator}");
                        }
                        else
                        {
                            builder.Append($"{argument}{separator}");
                        }
                    }
                }
            }

            return builder.ToString(0, builder.Length - 1);
        }

        /// <summary>
        /// Checks if given string value contains any kind of whitespaces.
        /// </summary>
        /// <remarks>
        /// This methods checks if given string value contains any kind of whitespaces.
        /// </remarks>
        /// <param name="value">
        /// The string value to be analyzed.
        /// </param>
        /// <returns>
        /// True, if at least one of the whitespaces could be found.
        /// </returns>
        public static Boolean HasWhiteSpaces(this String value)
        {
            if (value != null)
            {
                foreach (Char current in value)
                {
                    if (Char.IsWhiteSpace(current))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// This method is actually a bonus functionality that might be used to convert 
        /// all public read/writable properties of an object into its string representation. 
        /// Such a string representation might perhaps be used for debug purposes.
        /// </summary>
        /// <remarks>
        /// Note that not all possible data types are supported. This applies especially 
        /// to all generic data types.
        /// </remarks>
        /// <param name="instance">
        /// The object to get a string representation for.
        /// </param>
        /// <returns>
        /// The string representation of given object.
        /// </returns>
        public static String Stringify(this Object instance)
        {
            String result = String.Empty;

            if (instance != null)
            {
                StringBuilder buffer = new StringBuilder(512);

                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;

                PropertyInfo[] properties = instance.GetType().GetProperties(flags);

                if (properties != null)
                {
                    for (Int32 index = 0; index < properties.Length; index++)
                    {
                        PropertyInfo property = properties[index];
                        Object value = property.GetValue(instance);
                        String output = String.Empty;

                        if (value != null)
                        {
                            if (value.GetType() == typeof(String[]))
                            {
                                output = $"[{ArgumentComposer.Stringify((String[])value)}]";
                            }
                            else if (value.GetType() == typeof(List<String>))
                            {
                                output = $"[{ArgumentComposer.Stringify((List<String>)value)}]";
                            }
                            else
                            {
                                if (value.GetType() == typeof(String))
                                {
                                    output = $"\"{value.ToString()}\"";
                                }
                                else
                                {
                                    output = $"{value.ToString()}";
                                }
                            }
                        }
                        else
                        {
                            output = "<null>";
                        }

                        if (index < properties.Length - 1)
                        {
                            buffer.Append($"{property.Name}: {output}, ");
                        }
                        else
                        {
                            buffer.Append($"{property.Name}: {output}");
                        }
                    }
                }

                result = buffer.ToString();
            }

            return result;
        }

        #endregion

        #region Privates

        /// <summary>
        /// Converts a string array into a string.
        /// </summary>
        /// <remarks>
        /// This method converts a string array into a string.
        /// </remarks>
        /// <param name="items">
        /// The list of strings to be stringified.
        /// </param>
        /// <returns>
        /// The resulting string.
        /// </returns>
        private static String Stringify(this String[] items)
        {
            if (items != null && items.Length > 0)
            {
                return ArgumentComposer.Stringify(new List<String>(items));
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Converts a string array into a string.
        /// </summary>
        /// <remarks>
        /// This method converts a string array into a string.
        /// </remarks>
        /// <param name="items">
        /// The list of strings to be stringified.
        /// </param>
        /// <returns>
        /// The resulting string.
        /// </returns>
        private static String Stringify(this List<String> items)
        {
            if (items != null && items.Count > 0)
            {
                StringBuilder buffer = new StringBuilder(512);

                for (Int32 index = 0; index < items.Count; index++)
                {
                    if (index < items.Count - 1)
                    {
                        buffer.Append($"\"{items[index]}\", ");
                    }
                    else
                    {
                        buffer.Append($"\"{items[index]}\"");
                    }
                }

                return buffer.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Tries to add provided <paramref name="value"/> to the list 
        /// of <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// This method tries to add provided <paramref name="value"/> 
        /// to the list of <paramref name="values"/>.
        /// </remarks>
        /// <param name="values">
        /// The list of values to add provided value to.
        /// </param>
        /// <param name="value">
        /// The value to be added to list of values.
        /// </param>
        /// <param name="cleaned">
        /// True, if a verification should be done at all and false to 
        /// skip verification.
        /// </param>
        /// <seealso cref="Extract(String, Char, Boolean)"/>
        /// <seealso cref="IsIgnorable(String, Boolean, Int32)"/>
        private static void TryAddValue(this List<String> values, String value, Boolean cleaned)
        {
            if (!value.IsIgnorable(cleaned, values.Count))
            {
                values.Add(value);
            }
        }

        /// <summary>
        /// Checks whether provided value can be ignored.
        /// </summary>
        /// <remarks>
        /// This method determines whether provided value can be ignored. For the moment, 
        /// this only applies to the very first item inside the resulting list. The very 
        /// first item is indicated by provided index.
        /// </remarks>
        /// <param name="value">
        /// The value to be verified.
        /// </param>
        /// <param name="cleaned">
        /// True, if a verification should be done at all and false to skip verification.
        /// </param>
        /// <param name="index">
        /// The currently affected index. The index must be zero to perform verification 
        /// at all. Otherwise the verification is skipped.
        /// </param>
        /// <returns>
        /// True if provided value can be ignored and false otherwise.
        /// </returns>
        /// <seealso cref="TryAddValue(List{String}, String, Boolean)"/>
        private static Boolean IsIgnorable(this String value, Boolean cleaned, Int32 index)
        {
            try
            {
                return cleaned && index == 0 && System.IO.File.Exists(value);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }

            return false;
        }

        #endregion
    }
}
