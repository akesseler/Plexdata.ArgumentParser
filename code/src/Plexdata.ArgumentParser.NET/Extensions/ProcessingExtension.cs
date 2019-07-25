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
using Plexdata.ArgumentParser.Constants;
using Plexdata.ArgumentParser.Converters;
using Plexdata.ArgumentParser.Processors;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Plexdata.ArgumentParser.Extensions
{
    internal static class ProcessingExtension
    {
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
