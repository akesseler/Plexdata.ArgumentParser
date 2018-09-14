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
using Plexdata.ArgumentParser.Exceptions;
using System;
using System.Text;

namespace Plexdata.ArgumentParser.Attributes
{
    /// <summary>
    /// This attribute serves as the base class of all other supported property attributes 
    /// and should not be used directly. Use instead one of the derived attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParameterObjectAttribute : Attribute
    {
        #region Fields

        private String solidLabel = String.Empty;
        private String briefLabel = String.Empty;
        private String dependencies = String.Empty;

        #endregion

        #region Construction

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        protected ParameterObjectAttribute()
            : base()
        {
            this.IsRequired = false;
            this.IsExclusive = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A label to be used as solid label for this parameter but without any prefix.
        /// </summary>
        public virtual String SolidLabel
        {
            get
            {
                return this.solidLabel;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    this.ThrowException(nameof(this.SolidLabel), "The solid label must be a valid string.");
                }
                else
                {
                    value = value.Trim();

                    if (value.StartsWith(ParameterPrefixes.SolidPrefix))
                    {
                        value = value.Substring(ParameterPrefixes.SolidPrefix.Length);
                    }
                    else if (value.StartsWith(ParameterPrefixes.BriefPrefix))
                    {
                        value = value.Substring(ParameterPrefixes.BriefPrefix.Length);
                    }
                    else if (value.StartsWith(ParameterPrefixes.OtherPrefix))
                    {
                        value = value.Substring(ParameterPrefixes.OtherPrefix.Length);
                    }

                    if (value.Length <= 0)
                    {
                        this.ThrowException(nameof(this.SolidLabel), "The solid label must consist of a valid descriptor.");
                    }

                    this.solidLabel = value;
                }
            }
        }

        /// <summary>
        /// Convenient getter to determine if the solid label is valid or not.
        /// </summary>
        public Boolean IsSolidLabel
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.SolidLabel);
            }
        }

        /// <summary>
        /// A label to be used as brief label for this parameter but without any prefix.
        /// </summary>
        public virtual String BriefLabel
        {
            get
            {
                return this.briefLabel;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    this.ThrowException(nameof(this.BriefLabel), "The brief label must be a valid string.");
                }
                else
                {
                    value = value.Trim();

                    if (value.StartsWith(ParameterPrefixes.SolidPrefix))
                    {
                        value = value.Substring(ParameterPrefixes.SolidPrefix.Length);
                    }
                    else if (value.StartsWith(ParameterPrefixes.BriefPrefix))
                    {
                        value = value.Substring(ParameterPrefixes.BriefPrefix.Length);
                    }
                    else if (value.StartsWith(ParameterPrefixes.OtherPrefix))
                    {
                        value = value.Substring(ParameterPrefixes.OtherPrefix.Length);
                    }

                    if (value.Length <= 0)
                    {
                        this.ThrowException(nameof(this.BriefLabel), "The brief label must consist of a valid descriptor.");
                    }

                    this.briefLabel = value;
                }
            }
        }

        /// <summary>
        /// Convenient getter to determine if the brief label is valid or not.
        /// </summary>
        public Boolean IsBriefLabel
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.BriefLabel);
            }
        }

        /// <summary>
        /// True, if this parameter is required. Otherwise this parameter is treated as 
        /// optional parameter. The default value is false.
        /// </summary>
        public Boolean IsRequired
        {
            get;
            set;
        }

        /// <summary>
        /// True, if this parameter is used exclusively. Exclusive parameters cannot be 
        /// used along with other parameters. Otherwise this parameter is treated as a 
        /// non-exclusive parameter. The default value is false.
        /// </summary>
        public Boolean IsExclusive
        {
            get;
            set;
        }

        /// <summary>
        /// A comma separated list of property names that are depending on the property that 
        /// has been tagged with this attribute. The provided sequence must be case sensitive.
        /// </summary>
        /// <remarks>
        /// Actually, it is possible to use one of the allowed dependency separators.
        /// </remarks>
        public String Dependencies
        {
            get
            {
                return this.dependencies;
            }
            set
            {
                this.dependencies = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// Convenient getter to determine if the dependency feature is used or not.
        /// </summary>
        public Boolean IsDependencies
        {
            get { return !String.IsNullOrWhiteSpace(this.Dependencies); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convenient method to determine if the solid label is valid or not and if it 
        /// starts with given label.
        /// </summary>
        /// <param name="other">
        /// A label to determine if the solid label starts with.
        /// </param>
        /// <returns>
        /// True, if the solid label starts with given value, and false if not.
        /// </returns>
        public Boolean IsSolidLabelAndStartsWith(String other)
        {
            if (this.IsSolidLabel)
            {
                if (!String.IsNullOrWhiteSpace(other))
                {
                    return this.SolidLabel.Equals(this.TailorOther(other));
                }
            }

            return false;
        }

        /// <summary>
        /// Convenient method to determine if the brief label is valid or not and if it 
        /// starts with given label.
        /// </summary>
        /// <param name="other">
        /// A label to determine if the brief label starts with.
        /// </param>
        /// <returns>
        /// True, if the brief label starts with given value, and false if not.
        /// </returns>
        public Boolean IsBriefLabelAndStartsWith(String other)
        {
            if (this.IsBriefLabel)
            {
                if (!String.IsNullOrWhiteSpace(other))
                {
                    return this.BriefLabel.Equals(this.TailorOther(other));
                }
            }

            return false;
        }

        /// <summary>
        /// A convenient method to get the used dependencies as a list. The currently 
        /// used string of dependencies is split into pieces by using all of the allowed 
        /// dependency separators.
        /// </summary>
        /// <returns>
        /// The list of available dependencies or a list of length zero if no dependencies 
        /// are used.
        /// </returns>
        public String[] GetDependencies()
        {
            if (this.IsDependencies)
            {
                return this.Dependencies.Split(DependencySeparators.AllowedSeparators, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                return new String[0];
            }
        }

        #endregion

        #region Protecteds

        /// <summary>
        /// Throws an exception according to current instance of a derived class.
        /// </summary>
        /// <param name="parameter">
        /// The name of the parameter that has caused the exception.
        /// </param>
        /// <param name="message">
        /// The message to be used inside the thrown exception.
        /// </param>
        protected void ThrowException(String parameter, String message)
        {
            if (this is SwitchParameterAttribute)
            {
                throw new SwitchAttributeException(parameter, message);
            }
            else if (this is OptionParameterAttribute)
            {
                throw new OptionAttributeException(parameter, message);
            }
            else if (this is VerbalParameterAttribute)
            {
                throw new VerbalAttributeException(parameter, message);
            }
            else
            {
                throw new NotImplementedException(message);
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
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(128);

            if (this.IsSolidLabel)
            {
                result.Append($"SolidLabel: {this.SolidLabel}, ");
            }

            if (this.IsBriefLabel)
            {
                result.Append($"BriefLabel: {this.BriefLabel}, ");
            }

            result.Append($"Required: {this.IsRequired}, ");

            result.Append($"Exclusive: {this.IsExclusive}, ");

            if (this.IsDependencies)
            {
                result.Append($"Dependencies: {this.Dependencies}, ");
            }

            if (result.Length >= 2)
            {
                result.Remove(result.Length - 2, 2);
            }

            return result.Length > 0 ? result.ToString() : base.ToString();
        }

        #endregion

        #region Privates

        /// <summary>
        /// This method cuts off any additional data from given string that not 
        /// belong to the label, but only if current instance is of type "Option".
        /// </summary>
        /// <param name="label">
        /// The label to be tailored.
        /// </param>
        /// <returns>
        /// The tailored label.
        /// </returns>
        private String TailorOther(String label)
        {
            if (this is OptionParameterAttribute)
            {
                String[] pieces = label.Split(new Char[] { (this as OptionParameterAttribute).Separator }, StringSplitOptions.RemoveEmptyEntries);

                if (pieces.Length > 0) { label = pieces[0]; }
            }

            return label;
        }

        #endregion
    }
}
