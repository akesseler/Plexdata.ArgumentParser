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
    /// The argument processor setting implementation.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of an argument processor setting.
    /// </remarks>
    internal class ArgumentProcessorSetting
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
        /// <param name="attribute">
        /// An instance of a parameter object attribute to be applied.
        /// </param>
        public ArgumentProcessorSetting(PropertyInfo property, ParameterObjectAttribute attribute)
            : base()
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
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
        /// Gets the assigned attribute information.
        /// </summary>
        /// <remarks>
        /// This property gets the assigned instance of attribute information.
        /// </remarks>
        /// <value>
        /// The assigned attribute information.
        /// </value>
        public ParameterObjectAttribute Attribute { get; private set; }

        #endregion
    }
}
