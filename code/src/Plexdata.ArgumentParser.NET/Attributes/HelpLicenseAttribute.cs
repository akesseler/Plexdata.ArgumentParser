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

using Plexdata.ArgumentParser.Constants;
using System;
using System.Text;

namespace Plexdata.ArgumentParser.Attributes
{
    /// <summary>
    /// The help license attribute is intended to be used to define 
    /// a program's copyright statement.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HelpLicenseAttribute : Attribute
    {
        #region Fields

        private String content = $"Copyright © {Placeholders.Company}";

        #endregion

        #region Construction

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        public HelpLicenseAttribute()
            : base()
        {
        }

        /// <summary>
        /// Attribute constructor that takes a content parameter.
        /// </summary>
        /// <param name="content">
        /// The text to be used as content.
        /// </param>
        public HelpLicenseAttribute(String content)
            : base()
        {
            this.Content = content;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets and gets the value of the content property. Default value 
        /// is "Copyright © &lt;company&gt;".
        /// </summary>
        /// <remarks>
        /// The placeholder "&lt;company&gt;" will be automatically replaced 
        /// by the real company name that is defined in the executing assembly, 
        /// but only if it is not empty. Otherwise the placeholder remains 
        /// unchanged.
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
