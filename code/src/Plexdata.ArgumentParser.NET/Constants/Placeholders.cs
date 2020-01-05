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
    /// The list of all supported placeholders.
    /// </summary>
    /// <remarks>
    /// This class provides access to all supported placeholders.
    /// </remarks>
    public static class Placeholders
    {
        /// <summary>
        /// The internal placeholder for the company name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Users may combine the <see cref="Placeholders.Company"/>, the <see cref="Placeholders.Version"/> 
        /// and the <see cref="Placeholders.Copyright"/> placeholders. In such a case a combination of the 
        /// company name, the version number as well as the copyright statement is generated.
        /// </para>
        /// <para>
        /// This placeholder is replaced in each license attribute that includes this label.
        /// </para>
        /// </remarks>
        /// <seealso cref="Attributes.HelpLicenseAttribute"/>
        public const String Company = "<company>";

        /// <summary>
        /// The internal placeholder to be used to force a replacement 
        /// of license statement by the copyright of executing assembly.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Users may combine the <see cref="Placeholders.Company"/>, the <see cref="Placeholders.Version"/> 
        /// and the <see cref="Placeholders.Copyright"/> placeholders. In such a case a combination of the 
        /// company name, the version number as well as the copyright statement is generated.
        /// </para>
        /// <para>
        /// This placeholder is replaced in each license attribute that includes this label.
        /// </para>
        /// </remarks>
        /// <seealso cref="Attributes.HelpLicenseAttribute"/>
        public const String Copyright = "<copyright>";

        /// <summary>
        /// The internal placeholder for the program version.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Users may combine the <see cref="Placeholders.Company"/>, the <see cref="Placeholders.Version"/> 
        /// and the <see cref="Placeholders.Copyright"/> placeholders. In such a case a combination of the 
        /// company name, the version number as well as the copyright statement is generated.
        /// </para>
        /// <para>
        /// Please be aware, a program's version attribute is used and not its file version attribute.
        /// </para>
        /// <para>
        /// This placeholder is replaced in each license attribute that includes this label.
        /// </para>
        /// </remarks>
        /// <seealso cref="Attributes.HelpLicenseAttribute"/>
        public const String Version = "<version>";

        /// <summary>
        /// The internal placeholder for a program's name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This placeholder causes to take the name of the executing program.
        /// </para>
        /// <para>
        /// This placeholder is replaced in each utilize attribute that includes this label.
        /// </para>
        /// </remarks>
        /// <seealso cref="Placeholders.Product"/>
        /// <seealso cref="Attributes.HelpUtilizeAttribute"/>
        public const String Program = "<program>";

        /// <summary>
        /// The internal placeholder for the product name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In contrast to placeholder <see cref="Placeholders.Program"/>, this placeholder causes to take 
        /// the value of <see cref="System.Reflection.AssemblyProductAttribute"/>. And only if it fails, 
        /// this placeholder is replaced by the name of the executing program.
        /// </para>
        /// <para>
        /// This placeholder might be used as an alternative to placeholder <see cref="Placeholders.Program"/>.
        /// </para>
        /// <para>
        /// This placeholder is replaced in each utilize attribute that includes this label.
        /// </para>
        /// </remarks>
        /// <seealso cref="Placeholders.Program"/>
        /// <seealso cref="Attributes.HelpUtilizeAttribute"/>
        public const String Product = "<product>";

        /// <summary>
        /// The internal placeholder for a program's description.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This placeholder will be replaced by a program's real description.
        /// </para>
        /// <para>
        /// The description placeholder is only available for the preface attribute. 
        /// </para>
        /// </remarks>
        /// <seealso cref="Attributes.HelpPrefaceAttribute"/>
        public const String Description = "<description>";
    }
}
