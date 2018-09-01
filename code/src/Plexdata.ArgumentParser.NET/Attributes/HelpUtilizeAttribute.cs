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

using Plexdata.ArgumentParser.Constants;
using System;
using System.Text;

namespace Plexdata.ArgumentParser.Attributes
{
    /// <summary>
    /// The help utilize attribute is intended to be used to define 
    /// how a program's command lines arguments can be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HelpUtilizeAttribute : Attribute
    {
        #region Fields

        private String heading = "Usage:";
        private String caption = String.Empty;
        private String content = $"{Placeholders.Program} [options]";

        #endregion

        #region Construction

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        public HelpUtilizeAttribute()
            : base()
        {
        }

        /// <summary>
        /// Attribute constructor that takes a content parameter.
        /// </summary>
        /// <param name="content">
        /// The text to be used as content.
        /// </param>
        public HelpUtilizeAttribute(String content)
            : base()
        {
            this.Content = content;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets and gets the value of the heading property. Default value is "Usage:".
        /// </summary>
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
        /// Sets and gets the value of the caption property. Default value 
        /// is "empty".
        /// </summary>
        public String Section
        {
            get
            {
                return this.caption;
            }
            set
            {
                this.caption = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// Convenient getter to query if the caption is used or not.
        /// </summary>
        public Boolean IsSection
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Section);
            }
        }

        /// <summary>
        /// Sets and gets the value of the content property. Default value 
        /// is "&lt;program&gt; [options]". 
        /// </summary>
        /// <remarks>
        /// The placeholder "&lt;program&gt;" will be automatically replaced 
        /// by the real name of the executing assembly, but only if could be 
        /// determined. Otherwise the placeholder remains unchanged.
        /// </remarks>
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
                result.Append($"Caption: {this.Section}, ");
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
