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

namespace Plexdata.ArgumentParser.Constants
{
    /// <summary>
    /// The list of all supported argument delimiters.
    /// </summary>
    /// <remarks>
    /// This class provides access to all supported argument delimiters.
    /// </remarks>
    public static class ArgumentDelimiters
    {
        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the static fields of this class.
        /// </remarks>
        static ArgumentDelimiters() { }

        #endregion

        /// <summary>
        /// A parameter's argument is separated by comma.
        /// </summary>
        /// <remarks>
        /// This delimiter in nothing else but a comma character.
        /// </remarks>
        public const String CommaDelimiter = ",";

        /// <summary>
        /// A parameter's argument is separated by colon.
        /// </summary>
        /// <remarks>
        /// This delimiter in nothing else but a colon character.
        /// </remarks>
        public const String ColonDelimiter = ":";

        /// <summary>
        /// The default delimiter for argument processing.
        /// </summary>
        /// <remarks>
        /// This delimiter in nothing else but the <see cref="ArgumentDelimiters.ColonDelimiter"/>.
        /// </remarks>
        public const String DefaultDelimiter = ArgumentDelimiters.ColonDelimiter;
    }
}
