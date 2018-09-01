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
using System.Text;

namespace Plexdata.ArgumentParser.Attributes
{
    /// <summary>
    /// The help summary attribute is intended to be used on properties to 
    /// provide a details description what the purpose of this particular 
    /// command line option is.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HelpSummaryAttribute : Attribute
    {
        #region Fields

        private String heading = "Options:";
        private String section = String.Empty;
        private String content = String.Empty;
        private String options = String.Empty;

        #endregion

        #region Construction

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        public HelpSummaryAttribute()
            : this(null)
        {
        }

        /// <summary>
        /// Attribute constructor that takes a content parameter.
        /// </summary>
        /// <param name="content">
        /// The content to be used as summary.
        /// </param>
        public HelpSummaryAttribute(String content)
            : base()
        {
            this.Content = content;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets and gets the value of the heading property. Default value is "Options:".
        /// </summary>
        /// <remarks>
        /// Keep in mind, each of the command line argument summaries is grouped by its 
        /// heading as first. Thereafter, each command line argument summary inside one 
        /// heading is grouped by its section.
        /// </remarks>
        public String Heading
        {
            get
            {
                return this.heading;
            }
            set
            {
                this.heading = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// Convenient getter to query if the heading is used or not.
        /// </summary>
        public Boolean IsHeading
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Heading);
            }
        }

        /// <summary>
        /// Sets and gets the value of the section property. Default value is "empty".
        /// </summary>
        /// <remarks>
        /// Keep in mind, each of the command line argument summaries is grouped by its 
        /// heading as first. Thereafter, each command line argument summary inside one 
        /// heading is grouped by its section.
        /// </remarks>
        public String Section
        {
            get
            {
                return this.section;
            }
            set
            {
                this.section = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// Convenient getter to query if the section is used or not.
        /// </summary>
        public Boolean IsSection
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Section);
            }
        }

        /// <summary>
        /// Gets or set the current help text content.
        /// </summary>
        public String Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// Convenient getter to query if the content is used or not.
        /// </summary>
        public Boolean IsContent
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Content);
            }
        }

        /// <summary>
        /// Gets and set additional options. Additional options will be 
        /// used for example as a placeholder for option parameters.
        /// </summary>
        public String Options
        {
            get
            {
                return this.options;
            }
            set
            {
                this.options = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// Convenient getter to query if the options is used or not.
        /// </summary>
        public Boolean IsOptions
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Options);
            }
        }

        #endregion

        #region Publics 

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override String ToString()
        {
            StringBuilder result = new StringBuilder(64);

            if (this.IsHeading)
            {
                result.Append($"Heading: {this.Heading}, ");
            }

            if (this.IsSection)
            {
                result.Append($"Section: {this.Section}, ");
            }

            if (this.IsOptions)
            {
                result.Append($"Options: {this.Options}, ");
            }

            if (this.IsContent)
            {
                result.Append($"Content: {this.Content}, ");
            }

            if (result.Length >= 2)
            {
                result.Remove(result.Length - 2, 2);
            }

            return result.Length > 0 ? result.ToString() : base.ToString();
        }

        #endregion
    }
}
