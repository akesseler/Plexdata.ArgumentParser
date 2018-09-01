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

namespace Plexdata.ArgumentParser.Constants
{
    /// <summary>
    /// This class provides access to all supported parameter separators.
    /// </summary>
    public static class ParameterSeparators
    {
        /// <summary>
        /// A parameter's option is separated by space.
        /// </summary>
        public const Char SpaceSeparator = ' ';

        /// <summary>
        /// A parameter's option is separated by colon.
        /// </summary>
        public const Char ColonSeparator = ':';

        /// <summary>
        /// A parameter's option is separated by equal character.
        /// </summary>
        public const Char EqualSeparator = '=';

        /// <summary>
        /// The default separator for parameter processing.
        /// </summary>
        public const Char DefaultSeparator = ParameterSeparators.SpaceSeparator;

        /// <summary>
        /// Represents a list of allowed delimiters that separate an argument from its value.
        /// </summary>
        public static readonly Char[] AllowedSeparators = new Char[] {
            ParameterSeparators.SpaceSeparator,
            ParameterSeparators.ColonSeparator,
            ParameterSeparators.EqualSeparator,
        };
    }
}
