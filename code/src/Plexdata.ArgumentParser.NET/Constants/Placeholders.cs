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
    /// This class provides access to all supported placeholders.
    /// </summary>
    public static class Placeholders
    {
        /// <summary>
        /// The internal placeholder for the company name.
        /// </summary>
        /// <remarks>
        /// Users may combine the Company, the Version and the Copyright placeholders. 
        /// In such a case a combination of the company name, the version number as well 
        /// as the copyright statement is generated.
        /// </remarks>
        public const String Company = "<company>";

        /// <summary>
        /// The internal placeholder to be used to force a replacement 
        /// of license statement by the copyright of executing assembly.
        /// </summary>
        /// <remarks>
        /// Users may combine the Company, the Version and the Copyright placeholders. 
        /// In such a case a combination of the company name, the version number as well 
        /// as the copyright statement is generated.
        /// </remarks>
        public const String Copyright = "<copyright>";

        /// <summary>
        /// The internal placeholder for the program version.
        /// </summary>
        /// <remarks>
        /// Users may combine the Company, the Version and the Copyright placeholders. 
        /// In such a case a combination of the company name, the version number as well 
        /// as the copyright statement is generated. But keep in mind, a program's version 
        /// attribute is used and not its file version attribute!
        /// </remarks>
        public const String Version = "<version>";

        /// <summary>
        /// The internal placeholder for a program's name.
        /// </summary>
        /// <remarks>
        /// Keep in mind, the program placeholder is replaced in 
        /// each utilize attribute that includes this label.
        /// </remarks>
        public const String Program = "<program>";

        /// <summary>
        /// The internal placeholder for a program's description.
        /// </summary>
        /// <remarks>
        /// Keep in mind, the description placeholder is only available 
        /// for the preface attribute. This placeholder will replaced 
        /// by a program's real description.
        /// </remarks>
        public const String Description = "<description>";
    }
}
