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

using Plexdata.ArgumentParser.Attributes;
using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Interfaces;
using System;
using System.Linq;
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
        /// This constructor does not try to create an instance of a custom converter automatically.
        /// </remarks>
        /// <param name="property">
        /// An instance of property information to be applied.
        /// </param>
        /// <param name="attribute">
        /// An instance of a parameter object attribute to be applied.
        /// </param>
        /// <seealso cref="ArgumentProcessorSetting(PropertyInfo, ParameterObjectAttribute, Boolean)"/>
        public ArgumentProcessorSetting(PropertyInfo property, ParameterObjectAttribute attribute)
            : this(property, attribute, false)
        { }

        /// <summary>
        /// Extended construction.
        /// </summary>
        /// <remarks>
        /// This constructor tries to create an instance of the custom converter automatically, but 
        /// only in case of parameter <paramref name="enabled"/> is set to <c>true</c>.
        /// </remarks>
        /// <param name="property">
        /// An instance of property information to be applied.
        /// </param>
        /// <param name="attribute">
        /// An instance of a parameter object attribute to be applied.
        /// </param>
        /// <param name="enabled">
        /// True to enable automatic custom converter construction and false to disable this feature.
        /// </param>
        /// <seealso cref="ArgumentProcessorSetting(PropertyInfo, ParameterObjectAttribute)"/>
        public ArgumentProcessorSetting(PropertyInfo property, ParameterObjectAttribute attribute, Boolean enabled)
            : base()
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            this.CustomConverter = enabled ? this.GetCustomConverter(property) : null;
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

        /// <summary>
        /// Gets instance of <see cref="ICustomConverter{TTarget}"/>.
        /// </summary>
        /// <remarks>
        /// This property allows to get an instance of <see cref="ICustomConverter{TTarget}"/>. The 
        /// property value might be <c>null</c> either if automatic creation was disabled during class 
        /// construction or if assigned property type does not support custom type conversion or if 
        /// creation of custom type converter has failed for any reason.
        /// </remarks>
        /// <value>
        /// An instance of <see cref="ICustomConverter{TTarget}"/> or <c>null</c>.
        /// </value>
        /// <seealso cref="HasCustomConverter"/>
        public Object CustomConverter { get; private set; }

        /// <summary>
        /// Determines whether a custom converter is assigned.
        /// </summary>
        /// <remarks>
        /// This property determines whether a custom converter is assigned or not.
        /// </remarks>
        /// <value>
        /// True if a custom converter is assigned and false otherwise.
        /// </value>
        /// <seealso cref="CustomConverter"/>
        public Boolean HasCustomConverter
        {
            get
            {
                return this.CustomConverter != null;
            }
        }

        #endregion

        #region Publics

        /// <summary>
        /// Tries invoking method <see cref="ICustomConverter{TTarget}.Convert(String, String, String)"/> 
        /// of interface <see cref="ICustomConverter{TTarget}"/>.
        /// </summary>
        /// <remarks>
        /// This method tries to invoke method <see cref="ICustomConverter{TTarget}.Convert(String, String, String)"/> 
        /// of interface <see cref="ICustomConverter{TTarget}"/>.
        /// </remarks>
        /// <param name="parameter">
        /// The corresponding parameter from the command line for a property 
        /// of that type.
        /// </param>
        /// <param name="argument">
        /// The corresponding argument from the command line for a property 
        /// of that type.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to be used to split each argument value.
        /// </param>
        /// <returns>
        /// A new instance of the converted custom type.
        /// </returns>
        /// <seealso cref="ICustomConverter{TTarget}"/>
        /// <seealso cref="ICustomConverter{TTarget}.Convert(String, String, String)"/>
        /// <exception cref="CustomConverterException">
        /// This exception is thrown either if no custom converter could be determined, or in any error case 
        /// while invoking method <see cref="ICustomConverter{TTarget}.Convert(String, String, String)"/>.
        /// </exception>
        public Object InvokeCustomConverter(String parameter, String argument, String delimiter)
        {
            if (!this.HasCustomConverter)
            {
                throw new CustomConverterException(parameter, argument,
                    $"Converter invocation impossible because of missing converter for type {this.Property.PropertyType.Name}.");
            }

            try
            {
                MethodInfo method = this.CustomConverter.GetType().GetMethod(
                    nameof(ICustomConverter<Object>.Convert),
                    new Type[] { typeof(String), typeof(String), typeof(String) }
                );

                if (method is null)
                {
                    return null; // Might never happen...
                }

                return method.Invoke(this.CustomConverter, new Object[] { parameter, argument, delimiter });
            }
            catch (Exception exception)
            {
                if (exception.InnerException is CustomConverterException)
                {
                    throw exception.InnerException;
                }
                else
                {
                    throw new CustomConverterException(parameter, argument,
                        "Converter invocation has failed. See inner exception for more details.",
                        exception is TargetInvocationException ? exception.InnerException : exception);
                }
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// This method tries to create a new instance of a custom type converter and returns it.
        /// </summary>
        /// <remarks>
        /// The provided parameter <paramref name="property"/> must have an attribute of type 
        /// <see cref="CustomConverterAttribute"/> and its instance type of that attribute must 
        /// be derived from interface <see cref="ICustomConverter{TType}"/>. Furthermore, the 
        /// type of the interface implementation must have the same type as the type of the 
        /// corresponding property.
        /// </remarks>
        /// <param name="property">
        /// The property information to construct a converter instance for.
        /// </param>
        /// <returns>
        /// An instance of the custom type parser/converter or <c>null</c> if no attribute is assigned 
        /// or if applied parser type is not derived from interface <see cref="ICustomConverter{TType}"/> 
        /// or in case of a wrong interface type or in any error case (for example if no default constructor 
        /// is defined).
        /// </returns>
        private Object GetCustomConverter(PropertyInfo property)
        {
            foreach (Attribute attribute in property.GetCustomAttributes())
            {
                if (attribute is CustomConverterAttribute converter)
                {
                    if (converter.Instance is null)
                    {
                        continue;
                    }

                    foreach (Type realization in converter.Instance.GetInterfaces())
                    {
                        if (!realization.IsGenericType) { continue; }
                        if (realization.GetGenericTypeDefinition() != typeof(ICustomConverter<>)) { continue; }
                        if (!realization.GetGenericArguments().Any(x => x == property.PropertyType)) { continue; }

                        try
                        {
                            // A parameterless constructor is always returned by this method, 
                            // no matter if none is defined or the defined one is private.
                            ConstructorInfo constructor = converter.Instance.GetConstructor(Type.EmptyTypes);

                            // According to docs: Either an exceptions is thrown or an instance is returned (but not null)...
                            return constructor.Invoke(new Object[] { });
                        }
                        catch (Exception exception)
                        {
                            System.Diagnostics.Debug.WriteLine(exception);
                            return null;
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
