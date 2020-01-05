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
    /// The list of all supported parameter separators.
    /// </summary>
    /// <remarks>
    /// This class provides access to all supported parameter separators.
    /// </remarks>
    public static class ParameterSeparators
    {
        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the static fields of this class.
        /// </remarks>
        static ParameterSeparators() { }

        #endregion

        /// <summary>
        /// A parameter's option is separated by space.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but a space character.
        /// </remarks>
        public const Char SpaceSeparator = ' ';

        /// <summary>
        /// A parameter's option is separated by colon.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but a colon character.
        /// </remarks>
        public const Char ColonSeparator = ':';

        /// <summary>
        /// A parameter's option is separated by equal character.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but a equal character.
        /// </remarks>
        public const Char EqualSeparator = '=';

        /// <summary>
        /// The default separator for parameter processing.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but the <see cref="ParameterSeparators.SpaceSeparator"/>.
        /// </remarks>
        public const Char DefaultSeparator = ParameterSeparators.SpaceSeparator;

        /// <summary>
        /// The list of allowed delimiters that separate an argument from its value.
        /// </summary>
        /// <remarks>
        /// This array represents a list of allowed delimiters that separate an 
        /// argument from its value.
        /// </remarks>
        public static readonly Char[] AllowedSeparators = new Char[] {
            ParameterSeparators.SpaceSeparator,
            ParameterSeparators.ColonSeparator,
            ParameterSeparators.EqualSeparator,
        };
    }
}
