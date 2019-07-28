/*
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

using Plexdata.ArgumentParser.Attributes;
using System;
using System.Reflection;

namespace Plexdata.ArgumentParser.Processors
{
    /// <summary>
    /// The help processor setting implementation.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of an help processor setting.
    /// </remarks>
    internal class HelpProcessorSetting
    {
        #region Construction

        /// <summary>
        /// Standard construction.
        /// </summary>
        /// <remarks>
        /// The only one and default constructor of this class.
        /// </remarks>
        /// <param name="property">
        /// An instance of property information to be applied.
        /// </param>
        /// <param name="setting">
        /// An instance of a parameter object attribute to be applied.
        /// </param>
        /// <param name="summary">
        /// An instance of a help summary attribute to be applied.
        /// </param>
        public HelpProcessorSetting(PropertyInfo property, ParameterObjectAttribute setting, HelpSummaryAttribute summary)
            : base()
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.Setting = setting ?? throw new ArgumentNullException(nameof(setting));
            this.Summary = summary ?? throw new ArgumentNullException(nameof(summary));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the assigned property information.
        /// </summary>
        /// <remarks>
        /// This property gets the assigned instance of additional property information.
        /// </remarks>
        /// <value>
        /// The assigned property information.
        /// </value>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Gets the assigned setting information.
        /// </summary>
        /// <remarks>
        /// This property gets the assigned instance of setting information.
        /// </remarks>
        /// <value>
        /// The assigned setting information.
        /// </value>
        public ParameterObjectAttribute Setting { get; private set; }

        /// <summary>
        /// Gets the assigned summary information.
        /// </summary>
        /// <remarks>
        /// This property gets the assigned instance of summary information.
        /// </remarks>
        /// <value>
        /// The assigned summary information.
        /// </value>
        public HelpSummaryAttribute Summary { get; private set; }

        /// <summary>
        /// Gets the assigned heading string.
        /// </summary>
        /// <remarks>
        /// This property gets the assigned heading string.
        /// </remarks>
        /// <value>
        /// The assigned heading string.
        /// </value>
        public String Heading
        {
            get
            {
                if (this.IsHeading)
                {
                    return this.Summary.Heading;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// Convenient property to check if heading is applied.
        /// </summary>
        /// <remarks>
        /// This convenient property checks if heading is applied.
        /// </remarks>
        /// <value>
        /// True if heading is applied and false otherwise.
        /// </value>
        public Boolean IsHeading
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Summary.Heading);
            }
        }

        /// <summary>
        /// Gets the assigned section string.
        /// </summary>
        /// <remarks>
        /// This property gets the assigned section string.
        /// </remarks>
        /// <value>
        /// The assigned section string.
        /// </value>
        public String Section
        {
            get
            {
                if (this.IsSection)
                {
                    return this.Summary.Section;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// Convenient property to check if section is applied.
        /// </summary>
        /// <remarks>
        /// This convenient property checks if section is applied.
        /// </remarks>
        /// <value>
        /// True if section is applied and false otherwise.
        /// </value>
        public Boolean IsSection
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Summary.Section);
            }
        }

        /// <summary>
        /// Sets and Gets the assigned caption string.
        /// </summary>
        /// <remarks>
        /// This property sets and gets the assigned caption string.
        /// </remarks>
        /// <value>
        /// The assigned caption string.
        /// </value>
        public String Caption { get; set; }

        /// <summary>
        /// Convenient property to check if caption is applied.
        /// </summary>
        /// <remarks>
        /// This convenient property checks if caption is applied.
        /// </remarks>
        /// <value>
        /// True if caption is applied and false otherwise.
        /// </value>
        public Boolean IsCaption
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Caption);
            }
        }

        /// <summary>
        /// Sets and Gets the assigned content string.
        /// </summary>
        /// <remarks>
        /// This property sets and gets the assigned content string.
        /// </remarks>
        /// <value>
        /// The assigned content string.
        /// </value>
        public String Content { get; set; }

        /// <summary>
        /// Convenient property to check if content is applied.
        /// </summary>
        /// <remarks>
        /// This convenient property checks if content is applied.
        /// </remarks>
        /// <value>
        /// True if content is applied and false otherwise.
        /// </value>
        public Boolean IsContent
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Content);
            }
        }

        #endregion
    }
}
