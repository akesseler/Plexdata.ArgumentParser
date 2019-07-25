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

using Plexdata.ArgumentParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.ArgumentParser.Extensions
{
    internal static class ValidationExtension
    {
        public static void ThrowIfNull<TType>(this TType type, String name = "unknown") where TType : class
        {
            if (type == null)
            {
                throw new ArgumentNullException(name, $"The value of \"{name}\" must not be null.");
            }
        }

        public static void ThrowIfNullOrEmpty<TType>(this TType type, String name = "unknown") where TType : class
        {
            type.ThrowIfNull(name);

            if (type is IEnumerable<Object>)
            {
                if (!(type as IEnumerable<Object>).Any())
                {
                    throw new ArgumentNullException(name, $"The value of \"{name}\" must not be empty.");
                }
            }
        }

        public static void ThrowIfNullOrWhiteSpace(this String value, String name = "unknown")
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(name, $"The value of \"{name}\" must not be an empty string.");
            }
        }

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
