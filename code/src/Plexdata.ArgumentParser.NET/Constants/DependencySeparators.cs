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

namespace Plexdata.ArgumentParser.Constants
{
    /// <summary>
    /// The list of supported dependency separators.
    /// </summary>
    /// <remarks>
    /// This class provides access to all supported dependency separators.
    /// </remarks>
    public static class DependencySeparators
    {
        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the static fields of this class.
        /// </remarks>
        static DependencySeparators() { }

        #endregion

        /// <summary>
        /// A dependency list entry is separated by a space.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but a space character.
        /// </remarks>
        public const Char SpaceSeparator = ' ';

        /// <summary>
        /// A dependency list entry is separated by a colon.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but a colon character.
        /// </remarks>
        public const Char ColonSeparator = ':';

        /// <summary>
        /// A dependency list entry is separated by a comma.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but a comma character.
        /// </remarks>
        public const Char CommaSeparator = ',';

        /// <summary>
        /// A dependency list entry is separated by a semicolon.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but a semicolon character.
        /// </remarks>
        public const Char SemicolonSeparator = ';';

        /// <summary>
        /// The default separator for dependency processing.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but the <see cref="DependencySeparators.CommaSeparator"/>.
        /// </remarks>
        public const Char DefaultSeparator = DependencySeparators.CommaSeparator;

        /// <summary>
        /// The list of allowed delimiters that separate each item in a dependency list.
        /// </summary>
        /// <remarks>
        /// This array represents a list of allowed delimiters that separate each item in 
        /// a dependency list.
        /// </remarks>
        public static readonly Char[] AllowedSeparators = new Char[] {
            DependencySeparators.CommaSeparator,
            DependencySeparators.ColonSeparator,
            DependencySeparators.SemicolonSeparator,
            DependencySeparators.SpaceSeparator,
        };
    }
}
