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

using Plexdata.ArgumentParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.ArgumentParser.Extensions
{
    /// <summary>
    /// The validation extension.
    /// </summary>
    /// <remarks>
    /// This extension class provides various methods needed to 
    /// validate command line arguments.
    /// </remarks>
    internal static class ValidationExtension
    {
        /// <summary>
        /// Tests the parameter <paramref name="type"/> for <c>null</c>.
        /// </summary>
        /// <remarks>
        /// This method tests the parameter <paramref name="type"/> for 
        /// <c>null</c> and throws an exception in that case.
        /// </remarks>
        /// <typeparam name="TType">
        /// The generic type to be tested.
        /// </typeparam>
        /// <param name="type">
        /// The value to be tested.
        /// </param>
        /// <param name="name">
        /// The name of the tested value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided <paramref name="type"/> is <c>null</c>.
        /// </exception>
        public static void ThrowIfNull<TType>(this TType type, String name = "unknown") where TType : class
        {
            if (type is null)
            {
                throw new ArgumentNullException(name, $"The value of \"{name}\" must not be null.");
            }
        }

        /// <summary>
        /// Tests the parameter <paramref name="type"/> for <c>null</c>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method tests the parameter <paramref name="type"/>, that should be 
        /// an <see cref="IEnumerable{Object}"/>, for <c>null</c> or being empty and 
        /// throws an exception in that case.
        /// </para>
        /// <para>
        /// In case of <typeparamref name="TType"/> is not an <see cref="IEnumerable{Object}"/>, 
        /// then this method works exactly as method <see cref="ThrowIfNull{TType}(TType, String)"/>.
        /// </para>
        /// </remarks>
        /// <typeparam name="TType">
        /// The generic type to be tested.
        /// </typeparam>
        /// <param name="type">
        /// The value to be tested.
        /// </param>
        /// <param name="name">
        /// The name of the tested value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided <paramref name="type"/> is <c>null</c> or empty.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TType>(this TType type, String name = "unknown") where TType : class
        {
            type.ThrowIfNull(name);

            if (type is IEnumerable<Object> && !(type as IEnumerable<Object>).Any())
            {
                throw new ArgumentNullException(name, $"The value of \"{name}\" must not be empty.");
            }
        }

        /// <summary>
        /// Tests the parameter <paramref name="value"/> for <c>null</c>, empty or 
        /// consists only of white spaces.
        /// </summary>
        /// <remarks>
        /// This method tests the parameter <paramref name="value"/> for <c>null</c>, 
        /// empty or consists only of white spaces.
        /// </remarks>
        /// <param name="value">
        /// The value to be tested.
        /// </param>
        /// <param name="name">
        /// The name of the tested value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided <paramref name="value"/> is <c>null</c> or 
        /// empty or consists only of white spaces.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace(this String value, String name = "unknown")
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(name, $"The value of \"{name}\" must not be an empty string.");
            }
        }

        /// <summary>
        /// Throws an argument parser exception.
        /// </summary>
        /// <remarks>
        /// This method throws an argument parser exception.
        /// </remarks>
        /// <param name="exception">
        /// The exception to be thrown.
        /// </param>
        public static void ThrowArgumentParserException(this Exception exception)
        {
            if (exception != null)
            {
                if (exception is ArgumentParserException)
                {
                    throw exception;
                }
                else
                {
                    if (exception.InnerException != null)
                    {
                        exception.InnerException.ThrowArgumentParserException();
                    }
                    else
                    {
                        throw exception;
                    }
                }
            }
        }
    }
}
