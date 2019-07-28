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
    /// The help preface attribute.
    /// </summary>
    /// <remarks>
    /// The help preface attribute is intended to be used to define a program's 
    /// general description that is inserted above of the usage statement.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HelpPrefaceAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// The content field.
        /// </summary>
        /// <remarks>
        /// The field contains the help preface value.
        /// </remarks>
        private String content = Placeholders.Description;

        #endregion

        #region Construction

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls its base class constructor.
        /// </remarks>
        public HelpPrefaceAttribute()
            : base()
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
        /// The text to be used as content.
        /// </param>
        public HelpPrefaceAttribute(String content)
            : base()
        {
            this.Content = content;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets and gets the value of the content property. Default value 
        /// is "&lt;description&gt;". 
        /// </summary>
        /// <remarks>
        /// The placeholder "&lt;description&gt;" will be automatically replaced 
        /// by the real description of the executing assembly, but only if it is 
        /// not empty. Otherwise the placeholder remains unchanged.
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
