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
using Plexdata.ArgumentParser.Exceptions;
using System;
using System.Text;

namespace Plexdata.ArgumentParser.Attributes
{
    /// <summary>
    /// The option parameter attribute.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This attribute is intended to be used together with command line parameters that 
    /// include further options. Such a parameter is for example the parameter "username" 
    /// that is followed by the name of the affected user.
    /// </para>
    /// <para>
    /// This attribute supports various simple data types such as String, Integers, Date-Time, 
    /// Decimals, and so forth.The nullable version of all supported types are also supported. 
    /// The type of Boolean and any kind of lists or arrays are not supported.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class OptionParameterAttribute : ParameterObjectAttribute
    {
        #region Fields

        /// <summary>
        /// The separator field.
        /// </summary>
        /// <remarks>
        /// The field contains the separator value.
        /// </remarks>
        private Char separator = ParameterSeparators.DefaultSeparator;

        #endregion

        #region Construction

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls its base class constructor.
        /// </remarks>
        public OptionParameterAttribute()
            : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets and gets the delimiter to be used to split the parameter 
        /// from its option value.
        /// </summary>
        /// <remarks>
        /// This property sets and gets The delimiter to be used to split 
        /// the parameter from its option value.
        /// </remarks>
        /// <value>
        /// The separator assigned to an instance of this attribute.
        /// </value>
        public Char Separator
        {
            get
            {
                return this.separator;
            }
            set
            {
                if (Char.IsControl(value))
                {
                    throw new OptionAttributeException(nameof(this.Separator), "The separator must not be a control character.");
                }

                this.separator = value;
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
            StringBuilder result = new StringBuilder(128);

            result.Append($"{base.ToString()}, Separator: {this.Separator}");

            return result.ToString();
        }

        #endregion
    }
}
