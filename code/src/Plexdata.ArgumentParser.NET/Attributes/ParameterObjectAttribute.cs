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
    /// The parameter object attribute.
    /// </summary>
    /// <remarks>
    /// This attribute serves as the base class of all other supported property attributes 
    /// and should not be used directly. Use instead one of the derived attributes.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParameterObjectAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// The solid label field.
        /// </summary>
        /// <remarks>
        /// The field contains the solid label value.
        /// </remarks>
        private String solidLabel = String.Empty;

        /// <summary>
        /// The brief label field.
        /// </summary>
        /// <remarks>
        /// The field contains the brief label value.
        /// </remarks>
        private String briefLabel = String.Empty;

        /// <summary>
        /// The dependencies field.
        /// </summary>
        /// <remarks>
        /// The field contains the dependencies value.
        /// </remarks>
        private String dependencies = String.Empty;

        #endregion

        #region Construction

        /// <summary>
        /// Default attribute constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls its base class constructor.
        /// </remarks>
        protected ParameterObjectAttribute()
            : base()
        {
            this.IsRequired = false;
            this.IsExclusive = false;
            this.DependencyType = DependencyType.Optional;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets and gets the label to be used as solid label for this parameter 
        /// but without any prefix.
        /// </summary>
        /// <remarks>
        /// This property sets and gets the label to be used as solid label for 
        /// this parameter but without any prefix.
        /// </remarks>
        /// <value>
        /// The solid label assigned to an instance of this attribute.
        /// </value>
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

        /// <summary>
        /// Convenient getter to determine if the solid label is valid or not.
        /// </summary>
        /// <remarks>
        /// This property just represents a convenient getter to be able 
        /// to query if the solid label is used or not.
        /// </remarks>
        /// <value>
        /// True or false depending on current solid label availability.
        /// </value>
        public Boolean IsSolidLabel
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.SolidLabel);
            }
        }

        /// <summary>
        /// Sets and gets the label to be used as brief label for this parameter 
        /// but without any prefix.
        /// </summary>
        /// <remarks>
        /// This property sets and gets the label to be used as brief label for 
        /// this parameter but without any prefix.
        /// </remarks>
        /// <value>
        /// The brief label assigned to an instance of this attribute.
        /// </value>
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

        /// <summary>
        /// Convenient getter to determine if the brief label is valid or not.
        /// </summary>
        /// <remarks>
        /// This property just represents a convenient getter to be able 
        /// to query if the brief label is used or not.
        /// </remarks>
        /// <value>
        /// True or false depending on current brief label availability.
        /// </value>
        public Boolean IsBriefLabel
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.BriefLabel);
            }
        }

        /// <summary>
        /// Sets and gets the required state of this parameter.
        /// </summary>
        /// <remarks>
        /// This property sets and gets the required state of this parameter.
        /// True, if this parameter is required. Otherwise this parameter is 
        /// treated as optional parameter. The default value is <c>false</c>.
        /// </remarks>
        /// <value>
        /// True, if this parameter is required. Otherwise this parameter optional.
        /// </value>
        public Boolean IsRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Sets and gets the exclusive state of this parameter.
        /// </summary>
        /// <remarks>
        /// This property sets and gets the exclusive state of this parameter.
        /// True, if this parameter is used exclusively. Exclusive parameters cannot be 
        /// used along with other parameters. Otherwise this parameter is treated as a 
        /// non-exclusive parameter. The default value is <c>false</c>.
        /// </remarks>
        /// <value>
        /// True, if this parameter is exclusive. Otherwise this parameter non-exclusive.
        /// </value>
        public Boolean IsExclusive
        {
            get;
            set;
        }

        /// <summary>
        /// Sets and gets a comma separated list of property names that depending 
        /// on this attribute. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property sets and gets a comma separated list of property names that 
        /// depending on the property that has been tagged with this attribute. The 
        /// provided sequence must be case sensitive.
        ///</para>
        /// <para>
        /// Actually, it is possible to use one of the allowed dependency separators.
        /// </para>
        /// </remarks>
        /// <value>
        /// The dependencies assigned to an instance of this attribute.
        /// </value>
        public String DependencyList
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
        /// Sets and gets the type of the dependencies that are referenced by property 
        /// <see cref="DependencyList"/>.
        /// </summary>
        /// <remarks>
        /// This property sets and gets the type of the dependencies that are referenced 
        /// by property <see cref="DependencyList"/>. The default value is <c>Optional</c>.
        /// </remarks>
        /// <value>
        /// The dependency type assigned to an instance of this attribute.
        /// </value>
        public DependencyType DependencyType
        {
            get;
            set;
        }

        /// <summary>
        /// Convenient getter to determine if the dependency feature is used or not.
        /// </summary>
        /// <remarks>
        /// This property just represents a convenient getter to be able 
        /// to query if the dependencies are used or not.
        /// </remarks>
        /// <value>
        /// True or false depending on current dependencies availability.
        /// </value>
        public Boolean IsDependencies
        {
            get { return !String.IsNullOrWhiteSpace(this.DependencyList); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convenient method to determine if the solid label is valid or not and if it 
        /// starts with given label.
        /// </summary>
        /// <remarks>
        /// This convenient method just determines if the solid label is valid or not and 
        /// if it starts with given label.
        /// </remarks>
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
        /// <remarks>
        /// This convenient method just determines if the brief label is valid or not and 
        /// if it starts with given label.
        /// </remarks>
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
        /// A convenient method to get the used dependencies as a list.
        /// </summary>
        /// <remarks>
        /// This convenient method just gets the used dependencies as a list. The currently 
        /// used string of dependencies is split into pieces by using all of the allowed 
        /// dependency separators.
        /// </remarks>
        /// <returns>
        /// The list of available dependencies or a list of length zero if no dependencies 
        /// are used.
        /// </returns>
        public String[] GetDependencies()
        {
            if (this.IsDependencies)
            {
                return this.DependencyList.Split(DependencySeparators.AllowedSeparators, StringSplitOptions.RemoveEmptyEntries);
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
        /// <remarks>
        /// A convenient method throwing exceptions according to current instance of 
        /// a derived class.
        /// </remarks>
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

            if (this.IsSolidLabel)
            {
                result.Append($"{nameof(this.SolidLabel)}: {this.SolidLabel}, ");
            }

            if (this.IsBriefLabel)
            {
                result.Append($"{nameof(this.BriefLabel)}: {this.BriefLabel}, ");
            }

            result.Append($"{nameof(this.IsRequired)}: {this.IsRequired}, ");

            result.Append($"{nameof(this.IsExclusive)}: {this.IsExclusive}, ");

            if (this.IsDependencies)
            {
                result.Append($"{nameof(this.DependencyType)}: {this.DependencyType}, ");
                result.Append($"{nameof(this.DependencyList)}: {this.DependencyList}, ");
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
        /// Cuts off any additional data from given string that not belong to 
        /// the label, but only if current instance is of type "Option".
        /// </summary>
        /// <remarks>
        /// This method cuts off any additional data from given string that not 
        /// belong to the label, but only if current instance is of type "Option".
        /// </remarks>
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
