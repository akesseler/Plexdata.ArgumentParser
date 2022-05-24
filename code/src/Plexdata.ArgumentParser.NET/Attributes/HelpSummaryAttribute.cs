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
using System.Text;

namespace Plexdata.ArgumentParser.Attributes
{
    /// <summary>
    /// The help summary attribute.
    /// </summary>
    /// <remarks>
    /// The help summary attribute is intended to be used on properties to 
    /// provide a details description what the purpose of this particular 
    /// command line option is.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class HelpSummaryAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// The heading field.
        /// </summary>
        /// <remarks>
        /// The field contains the help heading value.
        /// </remarks>
        private String heading = "Options:";

        /// <summary>
        /// The section field.
        /// </summary>
        /// <remarks>
        /// The field contains the help section value.
        /// </remarks>
        private String section = String.Empty;

        /// <summary>
        /// The content field.
        /// </summary>
        /// <remarks>
        /// The field contains the help summary value.
        /// </remarks>
        private String content = String.Empty;

        /// <summary>
        /// The options field.
        /// </summary>
        /// <remarks>
        /// The field contains the help options value.
        /// </remarks>
        private String options = String.Empty;

        #endregion

        #region Construction

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls its parameterized class constructor.
        /// </remarks>
        public HelpSummaryAttribute()
            : this(null)
        {
        }

        /// <summary>
        /// Attribute constructor that takes a content parameter.
        /// </summary>
        /// <remarks>
        /// This constructor calls its base class constructor and than it 
        /// initializes its <see cref="Content"/> property.
        /// </remarks>
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
        /// <value>
        /// The heading assigned to an instance of this attribute.
        /// </value>
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
        /// <remarks>
        /// This property just represents a convenient getter to be able 
        /// to query if the heading is used or not.
        /// </remarks>
        /// <value>
        /// True or false depending on current heading availability.
        /// </value>
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
        /// <value>
        /// The section assigned to an instance of this attribute.
        /// </value>
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
        /// <remarks>
        /// This property just represents a convenient getter to be able 
        /// to query if the section is used or not.
        /// </remarks>
        /// <value>
        /// True or false depending on current section availability.
        /// </value>
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
        /// <remarks>
        /// This property gets or set the current help text content.
        /// </remarks>
        /// <value>
        /// The content assigned to an instance of this attribute.
        /// </value>
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
        /// <remarks>
        /// This property just represents a convenient getter to be able 
        /// to query if the content is used or not.
        /// </remarks>
        /// <value>
        /// True or false depending on current content availability.
        /// </value>
        public Boolean IsContent
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Content);
            }
        }

        /// <summary>
        /// Gets and set additional options.
        /// </summary>
        /// <remarks>
        /// This property gets and set additional options. Additional 
        /// options will be used for example as a placeholder for option 
        /// parameters.
        /// </remarks>
        /// <value>
        /// The options assigned to an instance of this attribute.
        /// </value>
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
        /// <remarks>
        /// This property just represents a convenient getter to be able 
        /// to query if the options is used or not.
        /// </remarks>
        /// <value>
        /// True or false depending on current options availability.
        /// </value>
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
        /// <remarks>
        /// This overwritten method returns a string representing the 
        /// current object.
        /// </remarks>
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
