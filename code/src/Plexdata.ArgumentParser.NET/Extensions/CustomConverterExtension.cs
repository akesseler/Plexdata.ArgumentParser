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

using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Plexdata.ArgumentParser.Extensions
{
    /// <summary>
    /// The custom type converter management extension.
    /// </summary>
    /// <remarks>
    /// This extension class provides all methods to manage 
    /// custom converter specific type conversions.
    /// </remarks>
    [Obsolete("Extension no longer supported. Use attribute `CustomConverter` instead.", false)]
    public static class CustomConverterExtension
    {
        #region Fields

        /// <summary>
        /// The converters field.
        /// </summary>
        /// <remarks>
        /// This field contains all currently registered custom 
        /// converters.
        /// </remarks>
        private static IDictionary<String, Object> converters = null;

        #endregion

        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes the static fields of this class.
        /// </remarks>
        static CustomConverterExtension()
        {
            CustomConverterExtension.converters = new Dictionary<String, Object>();
        }

        #endregion

        #region Publics

        /// <summary>
        /// Allows adding of custom converters.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method allows adding of specific custom converters.
        /// </para>
        /// <para>
        /// Very important to know! Only one converter can be registered 
        /// per type. In other words, each previously registered converter 
        /// instance will be replaced by the new one.
        /// </para>
        /// </remarks>
        /// <typeparam name="TTarget">
        /// The type to register a custom converter for.
        /// </typeparam>
        /// <param name="converter">
        /// The instance that implements the custom converter for a particular 
        /// type to be added.
        /// </param>
        [Obsolete("Method no longer supported. Use attribute `CustomConverter` instead.", false)]
        public static void AddConverter<TTarget>(this ICustomConverter<TTarget> converter)
        {
            if (converter == null)
            {
                return;
            }

            String qualifier = CustomConverterExtension.GetQualifier<TTarget>();

            if (!CustomConverterExtension.converters.ContainsKey(qualifier))
            {
                CustomConverterExtension.converters.Add(qualifier, null);
            }

            CustomConverterExtension.converters[qualifier] = converter;
        }

        /// <summary>
        /// Removes a previously added custom converter.
        /// </summary>
        /// <remarks>
        /// This method allows to remove a previously added custom converter.
        /// </remarks>
        /// <typeparam name="TTarget">
        /// The type to remove a custom converter for.
        /// </typeparam>
        /// <param name="converter">
        /// The instance that implements the custom converter for a particular 
        /// type to be removed.
        /// </param>
        [Obsolete("Method no longer supported. Use attribute `CustomConverter` instead.", false)]
        public static void RemoveConverter<TTarget>(this ICustomConverter<TTarget> converter)
        {
            if (converter == null)
            {
                return;
            }

            String qualifier = CustomConverterExtension.GetQualifier<TTarget>();

            if (CustomConverterExtension.converters.ContainsKey(qualifier))
            {
                CustomConverterExtension.converters.Remove(qualifier);
            }
        }

        /// <summary>
        /// Determines whether a custom converter is actually registered 
        /// for a particular data type.
        /// </summary>
        /// <remarks>
        /// This method determines whether a custom converter is actually 
        /// registered for a particular data type.
        /// </remarks>
        /// <param name="type">
        /// The type to check a registration for.
        /// </param>
        /// <returns>
        /// True, if a custom converter is registered for provided type, 
        /// and false otherwise.
        /// </returns>
        [Obsolete("Method no longer supported. Use attribute `CustomConverter` instead.", false)]
        public static Boolean HasConverter(this Type type)
        {
            if (type is null)
            {
                return false;
            }

            return CustomConverterExtension.converters.ContainsKey(CustomConverterExtension.GetQualifier(type));
        }

        /// <summary>
        /// Determines whether a custom converter is actually registered 
        /// for a specific user-defined type.
        /// </summary>
        /// <remarks>
        /// This method determines whether a custom converter is actually 
        /// registered for a specific user-defined type.
        /// </remarks>
        /// <typeparam name="TTarget">
        /// The affected custom type to check a registration for.
        /// </typeparam>
        /// <param name="target">
        /// The type to check a registration for.
        /// </param>
        /// <returns>
        /// True, if a custom converter is registered for provided type, 
        /// and false otherwise.
        /// </returns>
        [Obsolete("Method no longer supported. Use attribute `CustomConverter` instead.", false)]
        public static Boolean HasConverter<TTarget>(this TTarget target)
        {
            return (target as Type).HasConverter();
        }

        /// <summary>
        /// Tries to invoke method <see cref="ICustomConverter{TTarget}.Convert"/> 
        /// of interface <see cref="ICustomConverter{TTarget}"/>.
        /// </summary>
        /// <remarks>
        /// This method tries to invoke method <see cref="ICustomConverter{TTarget}.Convert"/> 
        /// of interface <see cref="ICustomConverter{TTarget}"/>.
        /// </remarks>
        /// <typeparam name="TTarget">
        /// The affected custom type to invoke its type conversion.
        /// </typeparam>
        /// <param name="target">
        /// The type to invoke its type conversion.
        /// </param>
        /// <param name="parameter">
        /// The parameter used at command line.
        /// </param>
        /// <param name="argument">
        /// The parameter's argument used at command line.
        /// </param>
        /// <param name="delimiter">
        /// The configured delimiter to split an argument into its pieces.
        /// </param>
        /// <returns>
        /// The result of a particular custom type conversion.
        /// </returns>
        /// <exception cref="CustomConverterException">
        /// This exception is thrown in any case a type conversion fails. Such cases 
        /// might be if a custom converter is not yet registered, or if invoking the 
        /// interface's method <see cref="ICustomConverter{TTarget}.Convert"/> has 
        /// caused any kind of an issue.
        /// </exception>
        [Obsolete("Method no longer supported. Use attribute `CustomConverter` instead.", false)]
        public static Object InvokeConverter<TTarget>(this TTarget target, String parameter, String argument, String delimiter)
        {
            if (!target.HasConverter())
            {
                throw new CustomConverterException(parameter, argument,
                    $"Converter invocation impossible because of missing converter for type {((target is Type) ? (target as Type).Name : "<unknown>")}.");
            }

            try
            {
                Object converter = CustomConverterExtension.converters[CustomConverterExtension.GetQualifier(target)];

                MethodInfo method = converter.GetType().GetMethod(nameof(ICustomConverter<TTarget>.Convert));

                return method.Invoke(converter, new Object[] { parameter, argument, delimiter });
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
                        "Converter invocation has failed. See inner exception for more details.", exception);
                }
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Gets the qualifier for a particular data type.
        /// </summary>
        /// <remarks>
        /// This method returns the qualifier for a particular data type.
        /// </remarks>
        /// <param name="type">
        /// The type to get the qualifier for.
        /// </param>
        /// <returns>
        /// The qualifier for a particular data type.
        /// </returns>
        private static String GetQualifier(Type type)
        {
            return type.AssemblyQualifiedName;
        }

        /// <summary>
        /// Gets the qualifier for a particular target type.
        /// </summary>
        /// <remarks>
        /// This method returns the qualifier for a particular target type.
        /// </remarks>
        /// <typeparam name="TTarget">
        /// The affected custom type to get the qualifier for.
        /// </typeparam>
        /// <param name="target">
        /// The target type to get the qualifier for.
        /// </param>
        /// <returns>
        /// The qualifier for a particular target type.
        /// </returns>
        /// <see cref="CustomConverterExtension.GetQualifier(Type)"/>
        private static String GetQualifier<TTarget>(TTarget target)
        {
            return CustomConverterExtension.GetQualifier(target as Type);
        }

        /// <summary>
        /// Gets the qualifier for a particular target type.
        /// </summary>
        /// <remarks>
        /// This method returns the qualifier for a particular target type.
        /// </remarks>
        /// <typeparam name="TTarget">
        /// The affected custom type to get the qualifier for.
        /// </typeparam>
        /// <returns>
        /// The qualifier for a particular target type.
        /// </returns>
        /// <see cref="CustomConverterExtension.GetQualifier(Type)"/>
        private static String GetQualifier<TTarget>()
        {
            return CustomConverterExtension.GetQualifier(typeof(TTarget));
        }

        #endregion
    }
}
