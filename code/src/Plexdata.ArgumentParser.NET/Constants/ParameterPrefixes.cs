﻿/*
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
    /// The list of supported parameter prefixes.
    /// </summary>
    /// <remarks>
    /// This class provides access to all supported parameter prefixes.
    /// </remarks>
    public static class ParameterPrefixes
    {
        /// <summary>
        /// The prefix for solid label parameters.
        /// </summary>
        /// <remarks>
        /// This value represents the prefix to be used for solid label parameters.
        /// </remarks>
        public const String SolidPrefix = "--";

        /// <summary>
        /// The prefix for brief label parameters.
        /// </summary>
        /// <remarks>
        /// This value represents the prefix to be used for brief label parameters.
        /// </remarks>
        public const String BriefPrefix = "-";

        /// <summary>
        /// The alternative parameter prefix.
        /// </summary>
        /// <remarks>
        /// This value represents the prefix to be used as alternative parameter prefix.
        /// </remarks>
        public const String OtherPrefix = "/";
    }
}
