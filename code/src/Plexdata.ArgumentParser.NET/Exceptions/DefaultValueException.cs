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

using System;

namespace Plexdata.ArgumentParser.Exceptions
{
    /// <summary>
    /// This exception is always thrown as soon as applying a default value fails.
    /// </summary>
    /// <remarks>
    /// This exception is always thrown as soon as applying a default value fails.
    /// Such a case could be when a default value of null is applied to a non-nullable 
    /// value or a default value could not be converted into for example an integer 
    /// type.
    /// </remarks>
    public class DefaultValueException : ArgumentParserException
    {
        /// <summary>
        /// The constructor with message argument.
        /// </summary>
        /// <remarks>
        /// This constructor creates an instance of this class using a given message.
        /// </remarks>
        /// <param name="message">
        /// The message to be assigned to an instance of this class.
        /// </param>
        public DefaultValueException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// The constructor with message and exception arguments.
        /// </summary>
        /// <remarks>
        /// This constructor creates an instance of this class using a given message 
        /// as well as an inner exception.
        /// </remarks>
        /// <param name="message">
        /// The message to be assigned to an instance of this class.
        /// </param>
        /// <param name="exception">
        /// An inner exception that describes the original exception source.
        /// </param>
        public DefaultValueException(String message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
