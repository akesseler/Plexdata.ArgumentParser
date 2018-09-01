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

namespace Plexdata.ArgumentParser.Exceptions
{
    /// <summary>
    /// The exception serves as base class for all exceptions exposed by this library.
    /// </summary>
    public class ArgumentParserException : SystemException
    {
        /// <summary>
        /// This constructor creates an instance of this class using a given message.
        /// </summary>
        /// <param name="message">
        /// The message to be assigned to an instance of this class.
        /// </param>
        public ArgumentParserException(String message)
            : this(null, message, null)
        {
        }

        /// <summary>
        /// This constructor creates an instance of this class using a given message 
        /// as well as the name of the parameter that causes this exception.
        /// </summary>
        /// <param name="parameter">
        /// The name of the parameter that has caused the exception.
        /// </param>
        /// <param name="message">
        /// The message to be assigned to an instance of this class.
        /// </param>
        public ArgumentParserException(String parameter, String message)
            : this(parameter, message, null)
        {
        }

        /// <summary>
        /// This constructor creates an instance of this class using a given message 
        /// as well as an inner exception.
        /// </summary>
        /// <param name="message">
        /// The message to be assigned to an instance of this class.
        /// </param>
        /// <param name="exception">
        /// An inner exception that describes the original exception source.
        /// </param>
        public ArgumentParserException(String message, Exception exception)
            : this(null, message, exception)
        {
        }

        /// <summary>
        /// This constructor creates an instance of this class using a given message 
        /// as well as the name of the parameter that causes this exception. Additionally, 
        /// this constructor takes an inner exception as parameter.
        /// </summary>
        /// <param name="parameter">
        /// The name of the parameter that has caused the exception.
        /// </param>
        /// <param name="message">
        /// The message to be assigned to an instance of this class.
        /// </param>
        /// <param name="exception">
        /// An inner exception that describes the original exception source.
        /// </param>
        public ArgumentParserException(String parameter, String message, Exception exception)
            : base(message, exception)
        {
            this.Parameter = parameter ?? String.Empty;
        }

        /// <summary>
        /// A getter that returns the name of parameter, or an empty string if unused.
        /// </summary>
        public String Parameter { get; private set; }
    }
}
