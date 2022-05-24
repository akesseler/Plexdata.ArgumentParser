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
    /// The list of all supported attribute separators.
    /// </summary>
    /// <remarks>
    /// This class provides access to all supported attribute separators.
    /// </remarks>
    internal static class AttributeSeparators
    {
        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the static fields of this class.
        /// </remarks>
        static AttributeSeparators() { }

        #endregion

        /// <summary>
        /// An attribute's argument is separated by comma.
        /// </summary>
        /// <remarks>
        /// This separator in nothing else but a comma character.
        /// </remarks>
        public const String CommaSeparator = ",";

        /// <summary>
        /// Gets the list of supported attribute separators.
        /// </summary>
        /// <remarks>
        /// This method returns the list of supported attribute separators.
        /// </remarks>
        /// <returns>
        /// The list of supported attribute separators.
        /// </returns>
        internal static Char[] GetSeparators()
        {
            return AttributeSeparators.CommaSeparator.ToCharArray();
        }
    }
}
