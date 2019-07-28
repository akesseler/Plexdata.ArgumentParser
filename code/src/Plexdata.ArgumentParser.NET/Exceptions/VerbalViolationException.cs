﻿/*
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

using System;

namespace Plexdata.ArgumentParser.Exceptions
{
    /// <summary>
    /// The exception is always thrown as soon as a violation of verbal rules take 
    /// place.
    /// </summary>
    /// <remarks>
    /// The exception is always thrown as soon as a violation of verbal rules take 
    /// place. Such a rule is for example that a verbal parameter is used more than 
    /// once.
    /// </remarks>
    public class VerbalViolationException : ArgumentParserException
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
        public VerbalViolationException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// The constructor with parameter and message arguments.
        /// </summary>
        /// <remarks>
        /// This constructor creates an instance of this class using a given message 
        /// as well as the name of the parameter that causes this exception.
        /// </remarks>
        /// <param name="parameter">
        /// The name of the parameter that has caused the exception.
        /// </param>
        /// <param name="message">
        /// The message to be assigned to an instance of this class.
        /// </param>
        public VerbalViolationException(String parameter, String message)
            : base(message, parameter)
        {
        }
    }
}
