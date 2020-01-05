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

namespace Plexdata.ArgumentParser.Attributes
{
    /// <summary>
    /// The verbal parameter attribute. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This attribute is intended to be used together with all unassignable command 
    /// line parameters. Such a parameter is for example a list of file names. 
    /// </para>
    /// <para>
    /// Be aware, the usage of only one of this attribute is supported. Additionally 
    /// keep in mind, that only a list of strings or an array of strings are supported 
    /// for properties that are tagged by this attribute. On the other hand, if you want 
    /// to get for example a list of numbers from the command line, then you have to 
    /// convert all the strings by your self.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class VerbalParameterAttribute : ParameterObjectAttribute
    {
        #region Construction

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls its base class constructor.
        /// </remarks>
        public VerbalParameterAttribute()
            : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// A getter to retrieve the solid label. Note, trying to use the setter with a string 
        /// other than empty will cause an exception.
        /// </summary>
        /// <remarks>
        /// This property getter retrieves the solid label. Note, trying to use the setter with 
        /// a string other than empty will cause an exception.
        /// </remarks>
        /// <value>
        /// The solid label assigned to an instance of this attribute.
        /// </value>
        public override String SolidLabel
        {
            get
            {
                return base.SolidLabel;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    base.ThrowException(nameof(this.SolidLabel), "A solid label is not applicable for verbal parameters.");
                }
            }
        }

        /// <summary>
        /// A getter to retrieve the brief label. Note, trying to use the setter with a string 
        /// other than empty will cause an exception.
        /// </summary>
        /// <remarks>
        /// This property getter retrieves the brief label. Note, trying to use the setter with 
        /// a string other than empty will cause an exception.
        /// </remarks>
        /// <value>
        /// The brief label assigned to an instance of this attribute.
        /// </value>
        public override String BriefLabel
        {
            get
            {
                return base.BriefLabel;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    base.ThrowException(nameof(this.BriefLabel), "A brief label is not applicable for verbal parameters.");
                }
            }
        }

        #endregion
    }
}
