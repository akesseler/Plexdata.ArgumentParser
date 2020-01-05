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

using Plexdata.ArgumentParser.Attributes;
using Plexdata.ArgumentParser.Constants;
using Plexdata.ArgumentParser.Converters;
using Plexdata.ArgumentParser.Interfaces;
using Plexdata.ArgumentParser.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plexdata.ArgumentParser.Extensions
{
    /// <summary>
    /// The processing extension.
    /// </summary>
    /// <remarks>
    /// This extension class provides various methods needed to 
    /// process an applied arguments list.
    /// </remarks>
    internal static class ProcessingExtension
    {
        /// <summary>
        /// Removes the Prefix from a parameter.
        /// </summary>
        /// <remarks>
        /// This method removes the Prefix from a parameter.
        /// </remarks>
        /// <param name="parameter">
        /// The parameter to remove the prefix from.
        /// </param>
        /// <returns>
        /// The prefix-free parameter.
        /// </returns>
        public static String RemovePrefix(this String parameter)
        {
            if (!String.IsNullOrWhiteSpace(parameter))
            {
                if (parameter.Length >= ParameterPrefixes.SolidPrefix.Length)
                {
                    if (String.Compare(parameter, ParameterPrefixes.SolidPrefix) != 0)
                    {
                        if (parameter.StartsWith(ParameterPrefixes.SolidPrefix))
                        {
                            return parameter.Substring(ParameterPrefixes.SolidPrefix.Length);
                        }
                    }
                }

                if (parameter.Length >= ParameterPrefixes.BriefPrefix.Length)
                {
                    if (String.Compare(parameter, ParameterPrefixes.BriefPrefix) != 0)
                    {
                        if (parameter.StartsWith(ParameterPrefixes.BriefPrefix))
                        {
                            return parameter.Substring(ParameterPrefixes.BriefPrefix.Length);
                        }
                    }
                }

                if (parameter.Length >= ParameterPrefixes.OtherPrefix.Length)
                {
                    if (String.Compare(parameter, ParameterPrefixes.OtherPrefix) != 0)
                    {
                        if (parameter.StartsWith(ParameterPrefixes.OtherPrefix))
                        {
                            return parameter.Substring(ParameterPrefixes.OtherPrefix.Length);
                        }
                    }
                }
            }

            return parameter;
        }

        /// <summary>
        /// Convenient method to perform a parameter check.
        /// </summary>
        /// <remarks>
        /// This convenient method performs a check if provided value represents a parameter.
        /// </remarks>
        /// <param name="parameter">
        /// The value to check.
        /// </param>
        /// <returns>
        /// True, if provided value represents a parameter and false otherwise.
        /// </returns>
        public static Boolean IsParameter(this String parameter)
        {
            if (String.IsNullOrWhiteSpace(parameter))
            {
                return false;
            }
            else
            {
                String value = parameter.Trim();

                if (value.Length >= ParameterPrefixes.SolidPrefix.Length)
                {
                    if (String.Compare(value, ParameterPrefixes.SolidPrefix) == 0)
                    {
                        return false;
                    }
                    else if (value.StartsWith(ParameterPrefixes.SolidPrefix))
                    {
                        return true;
                    }
                }

                if (value.Length >= ParameterPrefixes.BriefPrefix.Length)
                {
                    if (String.Compare(value, ParameterPrefixes.BriefPrefix) == 0)
                    {
                        return false;
                    }
                    else if (value.StartsWith(ParameterPrefixes.BriefPrefix))
                    {
                        return true;
                    }
                }

                if (value.Length >= ParameterPrefixes.OtherPrefix.Length)
                {
                    if (String.Compare(value, ParameterPrefixes.OtherPrefix) == 0)
                    {
                        return false;
                    }
                    else if (value.StartsWith(ParameterPrefixes.OtherPrefix))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Convenient method to check if a parameter type is one of the supported type.
        /// </summary>
        /// <remarks>
        /// This convenient method checks if a parameter type is one of the supported type.
        /// </remarks>
        /// <param name="parameter">
        /// The parameter to check its type.
        /// </param>
        /// <param name="property">
        /// The property information that contain additional type information.
        /// </param>
        /// <returns>
        /// True, if type is supported and false otherwise.
        /// </returns>
        public static Boolean IsSupportedDataType(this ParameterObjectAttribute parameter, PropertyInfo property)
        {
            if (parameter is SwitchParameterAttribute)
            {
                return property.PropertyType == typeof(Boolean) || property.PropertyType == typeof(Boolean?);
            }
            else if (parameter is OptionParameterAttribute)
            {
                return OptionTypeConverter.IsSupportedType(property.PropertyType);
            }
            else if (parameter is VerbalParameterAttribute)
            {
                return property.PropertyType == typeof(List<String>) || property.PropertyType == typeof(String[]);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether a property contains a custom converter attribute.
        /// </summary>
        /// <remarks>
        /// This method determines whether a property contains a custom converter 
        /// attribute.
        /// </remarks>
        /// <param name="parameter">
        /// The attribute type to be checked. At the moment, this attribute must 
        /// be of type <see cref="OptionParameterAttribute"/>.
        /// </param>
        /// <param name="property">
        /// The property information that contain additional attribute information.
        /// </param>
        /// <returns>
        /// True, if a custom type converter attribute with a fitting data type 
        /// is supported and false if not.
        /// </returns>
        /// <seealso cref="OptionParameterAttribute"/>
        /// <seealso cref="CustomConverterAttribute"/>
        /// <seealso cref="ICustomConverter{TTarget}"/>
        public static Boolean IsConverterSupported(this ParameterObjectAttribute parameter, PropertyInfo property)
        {
            if (parameter is OptionParameterAttribute)
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

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Convenient method to check for switch parameter types.
        /// </summary>
        /// <remarks>
        /// This convenient method checks if a parameter is of type switch.
        /// </remarks>
        /// <param name="setting">
        /// An instance of argument processor settings.
        /// </param>
        /// <returns>
        /// True, if a parameter is of type switch and false otherwise.
        /// </returns>
        public static Boolean IsSwitchParameter(this ArgumentProcessorSetting setting)
        {
            if (setting != null)
            {
                return setting.Attribute is SwitchParameterAttribute;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Convenient method to check for option parameter types.
        /// </summary>
        /// <remarks>
        /// This convenient method checks if a parameter is of type option.
        /// </remarks>
        /// <param name="setting">
        /// An instance of argument processor settings.
        /// </param>
        /// <returns>
        /// True, if a parameter is of type option and false otherwise.
        /// </returns>
        public static Boolean IsOptionParameter(this ArgumentProcessorSetting setting)
        {
            if (setting != null)
            {
                return setting.Attribute is OptionParameterAttribute;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Convenient method to check for verbal parameter types.
        /// </summary>
        /// <remarks>
        /// This convenient method checks if a parameter is of type verbal.
        /// </remarks>
        /// <param name="setting">
        /// An instance of argument processor settings.
        /// </param>
        /// <returns>
        /// True, if a parameter is of type verbal and false otherwise.
        /// </returns>
        public static Boolean IsVerbalParameter(this ArgumentProcessorSetting setting)
        {
            if (setting != null)
            {
                return setting.Attribute is VerbalParameterAttribute;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts the applied parameter label.
        /// </summary>
        /// <remarks>
        /// This method extracts the applied parameter label (solid or brief) from 
        /// provided settings instance.
        /// </remarks>
        /// <param name="setting">
        /// An instance of argument processor settings.
        /// </param>
        /// <returns>
        /// The label value to be used. It is either the solid label or the brief 
        /// label. But if none of both is used then the property name is assumed 
        /// as parameter label.
        /// </returns>
        public static String ToParameterLabel(this ArgumentProcessorSetting setting)
        {
            if (setting != null && setting.Attribute != null && setting.Property != null)
            {
                if (setting.Attribute.IsSolidLabel)
                {
                    return $"{ParameterPrefixes.SolidPrefix}{setting.Attribute.SolidLabel}";
                }
                else if (setting.Attribute.IsBriefLabel)
                {
                    return $"{ParameterPrefixes.BriefPrefix}{setting.Attribute.BriefLabel}";
                }
                else
                {
                    return setting.Property.Name;
                }
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
