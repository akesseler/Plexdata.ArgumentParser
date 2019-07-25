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
using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plexdata.ArgumentParser.Processors
{
    internal class ArgumentProcessor<TInstance> where TInstance : class
    {
        #region Construction

        public ArgumentProcessor(TInstance instance)
            : base()
        {
            instance.ThrowIfNull(nameof(instance));
            this.Instance = instance;
        }

        public ArgumentProcessor(TInstance instance, String[] arguments)
            : this(instance)
        {
            arguments.ThrowIfNullOrEmpty(nameof(arguments));
            this.Arguments = new List<String>(arguments);
        }

        public ArgumentProcessor(TInstance instance, List<String> arguments)
            : this(instance)
        {
            arguments.ThrowIfNullOrEmpty(nameof(arguments));
            this.Arguments = arguments;
        }

        #endregion

        #region Statics

        public static void Validate(TInstance instance)
        {
            ArgumentProcessor<TInstance> processor = new ArgumentProcessor<TInstance>(instance);
            processor.Initialize();
        }

        public static void Process(TInstance instance, String[] arguments)
        {
            ArgumentProcessor<TInstance> processor = new ArgumentProcessor<TInstance>(instance, arguments);
            processor.Process();
        }

        public static void Process(TInstance instance, List<String> arguments)
        {
            ArgumentProcessor<TInstance> processor = new ArgumentProcessor<TInstance>(instance, arguments);
            processor.Process();
        }

        #endregion

        #region Properties

        public TInstance Instance { get; private set; }

        public List<String> Arguments { get; private set; }

        public List<ArgumentProcessorSetting> Settings { get; private set; }

        #endregion

        #region Methods

        public void Initialize()
        {
            try
            {
                this.Settings = new List<ArgumentProcessorSetting>();

                VerbalParameterAttribute lastVerbal = null;
                Dictionary<String, PropertyInfo> solidLabels = new Dictionary<String, PropertyInfo>();
                Dictionary<String, PropertyInfo> briefLabels = new Dictionary<String, PropertyInfo>();

                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;

                PropertyInfo[] properties = this.Instance.GetType().GetProperties(flags);

                if (properties != null)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        IEnumerable<Attribute> attributes = property.GetCustomAttributes();

                        if (attributes != null)
                        {
                            foreach (Attribute current in attributes)
                            {
                                if (current is ParameterObjectAttribute)
                                {
                                    ParameterObjectAttribute attribute = current as ParameterObjectAttribute;

                                    if (attribute is SwitchParameterAttribute || attribute is OptionParameterAttribute)
                                    {
                                        if (attribute.IsSolidLabel || attribute.IsBriefLabel)
                                        {
                                            if (attribute.IsSolidLabel)
                                            {
                                                if (!solidLabels.ContainsKey(attribute.SolidLabel))
                                                {
                                                    solidLabels[attribute.SolidLabel] = property;
                                                }
                                                else
                                                {
                                                    throw new UtilizeViolationException(
                                                        $"The solid label \"{attribute.SolidLabel}\" of property \"{property.Name}\" is already used " +
                                                        $"by property \"{solidLabels[attribute.SolidLabel].Name}\". All solid labels must be unique.");
                                                }
                                            }

                                            if (attribute.IsBriefLabel)
                                            {
                                                if (!briefLabels.ContainsKey(attribute.BriefLabel))
                                                {
                                                    briefLabels[attribute.BriefLabel] = property;
                                                }
                                                else
                                                {
                                                    throw new UtilizeViolationException(
                                                        $"The brief label \"{attribute.BriefLabel}\" of property \"{property.Name}\" is already used " +
                                                        $"by property \"{briefLabels[attribute.BriefLabel].Name}\". All brief labels must be unique.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            throw new UtilizeViolationException(
                                                $"Neither the solid label nor the brief label is used by property \"{property.Name}\". " +
                                                $"Empty labels are only supported by verbal parameter types.");
                                        }
                                    }
                                    else if (attribute is VerbalParameterAttribute)
                                    {
                                        if (lastVerbal == null)
                                        {
                                            lastVerbal = attribute as VerbalParameterAttribute;
                                        }
                                        else
                                        {
                                            throw new VerbalViolationException(
                                                $"More than one verbal parameter is not supported " +
                                                $"per instance of \"{this.Instance.GetType().Name}\".");
                                        }
                                    }
                                    else
                                    {
                                        throw new SupportViolationException(
                                            $"A property attribute of type \"{attribute.GetType().Name}\" " +
                                            $"is not supported.");
                                    }

                                    if (attribute.IsSupportedDataType(property))
                                    {
                                        this.Settings.Add(new ArgumentProcessorSetting(property, attribute));
                                    }
                                    else
                                    {
                                        throw new SupportViolationException(
                                            $"Type \"{property.PropertyType}\" of property \"{property.Name}\" " +
                                            $"is not supported.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                exception.ThrowArgumentParserException();
            }
        }

        public void Process()
        {
            try
            {
                this.Initialize();

                List<String> parameters = new List<String>(this.Arguments);

                List<ArgumentProcessorSetting> processed = new List<ArgumentProcessorSetting>();

                while (parameters.Count > 0)
                {
                    String parameter = parameters[0];
                    parameters.RemoveAt(0);

                    ArgumentProcessorSetting setting;

                    if (!parameter.IsParameter())
                    {
                        if (this.TryFindSetting(out setting))
                        {
                            List<String> items = new List<String> { (String)OptionTypeConverter.Convert(parameter, typeof(String)) };

                            while (true)
                            {
                                if (parameters.Count > 0)
                                {
                                    parameter = parameters[0];

                                    if (!parameter.IsParameter())
                                    {
                                        parameters.RemoveAt(0);
                                        items.Add((String)OptionTypeConverter.Convert(parameter, typeof(String)));
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (setting.Property.PropertyType == typeof(List<String>))
                            {
                                setting.Property.SetValue(this.Instance, items);
                                processed.Add(setting);
                            }
                            else if (setting.Property.PropertyType == typeof(String[]))
                            {
                                setting.Property.SetValue(this.Instance, items.ToArray());
                                processed.Add(setting);
                            }
                            else
                            {
                                throw new VerbalViolationException(
                                    "A verbal parameter can only be a string list or a string array.");
                            }

                            continue;
                        }
                        else
                        {
                            throw new VerbalViolationException(
                                "A property of type verbal parameter couldn't be determined.");
                        }
                    }
                    else
                    {
                        if (this.TryFindSetting(parameter, out setting))
                        {
                            if (setting.IsSwitchParameter())
                            {
                                setting.Property.SetValue(this.Instance, true);
                                processed.Add(setting);
                            }
                            else if (setting.IsOptionParameter())
                            {
                                String argument = String.Empty;
                                Int32 offset = parameter.IndexOf((setting.Attribute as OptionParameterAttribute).Separator);

                                if (offset != -1)
                                {
                                    argument = parameter.Substring(offset + 1);
                                    parameter = parameter.Substring(0, offset);
                                }
                                else if (parameters.Count > 0)
                                {
                                    argument = parameters[0];
                                    parameters.RemoveAt(0);
                                }

                                if (String.IsNullOrWhiteSpace(argument) || argument.IsParameter())
                                {
                                    throw new OptionViolationException(
                                        $"The argument of parameter \"{parameter}\" is missing.");
                                }
                                else
                                {
                                    setting.Property.SetValue(this.Instance, OptionTypeConverter.Convert(argument, setting.Property.PropertyType));
                                    processed.Add(setting);
                                }
                            }
                        }
                        else
                        {
                            throw new SupportViolationException(
                                $"The parameter \"{parameter}\" is not supported.");
                        }
                    }
                }

                this.ValidateExclusiveProperties(processed);
                this.ValidateRequiredProperties(processed);
                this.ValidateDependencyProperties(processed);
            }
            catch (Exception exception)
            {
                exception.ThrowArgumentParserException();
            }
        }

        #endregion

        #region Privates

        private Boolean TryFindSetting(out ArgumentProcessorSetting setting)
        {
            setting = null;

            foreach (ArgumentProcessorSetting current in this.Settings)
            {
                if (current.IsVerbalParameter())
                {
                    setting = current;
                    return true;
                }
            }

            return false;
        }

        private Boolean TryFindSetting(String parameter, out ArgumentProcessorSetting setting)
        {
            setting = null;

            parameter = parameter.RemovePrefix();

            foreach (ArgumentProcessorSetting current in this.Settings)
            {
                if (current.Attribute.IsSolidLabelAndStartsWith(parameter) ||
                    current.Attribute.IsBriefLabelAndStartsWith(parameter))
                {
                    setting = current;
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<ArgumentProcessorSetting> TryGetReferencedItems(ArgumentProcessorSetting source, IEnumerable<ArgumentProcessorSetting> others, out IEnumerable<ArgumentProcessorSetting> overall)
        {
            // Get list of dependencies...
            String[] affected = source.Attribute.GetDependencies();

            // Apply all affected dependencies from overall settings.
            overall = this.Settings.Where(x => affected.Contains(x.Property.Name));

            // Filter out all currently available dependencies and return them.
            return others.Where(x => affected.Contains(x.Property.Name));
        }

        private void ValidateExclusiveProperties(List<ArgumentProcessorSetting> processed)
        {
            foreach (ArgumentProcessorSetting current in processed)
            {
                if (current.Attribute.IsExclusive)
                {
                    if (processed.Count > 1)
                    {
                        throw new ExclusiveViolationException(
                            $"Exclusive parameter \"{current.ToParameterLabel()}\" cannot be used along with other parameters.");
                    }
                }
            }
        }

        private void ValidateRequiredProperties(List<ArgumentProcessorSetting> processed)
        {
            if (processed.Any())
            {
                List<ArgumentProcessorSetting> expected = this.Settings.Where(x => x.Attribute.IsRequired).ToList();
                List<ArgumentProcessorSetting> affected = processed.Where(x => x.Attribute.IsRequired).ToList();

                expected.ForEach(e =>
                {
                    if (affected.Where(a => a.Property.Name == e.Property.Name).FirstOrDefault() == null)
                    {
                        throw new RequiredViolationException(
                            $"Required parameter \"{e.ToParameterLabel()}\" couldn't be found.");
                    }
                });
            }
        }

        private void ValidateDependencyProperties(List<ArgumentProcessorSetting> processed)
        {
            if (processed.Any())
            {
                foreach (ArgumentProcessorSetting source in processed)
                {
                    if (!source.Attribute.IsDependencies) { continue; }

                    // TODO: Self reference check...

                    this.ValidateReferencedProperties(source);

                    IEnumerable<ArgumentProcessorSetting> others = this.TryGetReferencedItems(source,
                        processed.Where(x => x.Property.Name != source.Property.Name),
                        out IEnumerable<ArgumentProcessorSetting> overall);

                    if (source.Attribute.DependencyType == DependencyType.Optional)
                    {
                        this.ValidateOptionalDependencies(source, others, overall);
                    }
                    else if (source.Attribute.DependencyType == DependencyType.Required)
                    {
                        this.ValidateRequiredDependencies(source, others, overall);
                    }
                    else
                    {
                        throw new SupportViolationException(
                            $"A value of {(Int32)source.Attribute.DependencyType} used for " +
                            $"property {source.Property.Name} is not supported as dependency type.");
                    }
                }
            }
        }

        private void ValidateReferencedProperties(ArgumentProcessorSetting source)
        {
            IEnumerable<String> candidates = source.Attribute.GetDependencies();
            IEnumerable<String> properties = this.Settings.Where(x => candidates.Contains(x.Property.Name)).Select(x => x.Property.Name);

            if (candidates.Count() != properties.Count())
            {
                String[] missings = candidates.Except(properties).ToArray();

                String s = missings.Length > 1 ? "s" : String.Empty;

                throw new DependentViolationException($"Unable to confirm dependency name{s} {String.Join(" and ", missings.Select(x => $"\"{x}\""))} as property name{s}.");
            }
        }

        private void ValidateOptionalDependencies(ArgumentProcessorSetting source, IEnumerable<ArgumentProcessorSetting> others, IEnumerable<ArgumentProcessorSetting> overall)
        {
            if (overall.Any() && !others.Any())
            {
                throw new DependentViolationException($"Parameter \"{source.ToParameterLabel()}\" depends on {String.Join(" or ", overall.Select(x => $"\"{x.ToParameterLabel()}\""))}.");
            }
        }

        private void ValidateRequiredDependencies(ArgumentProcessorSetting source, IEnumerable<ArgumentProcessorSetting> others, IEnumerable<ArgumentProcessorSetting> overall)
        {
            if (overall.Count() != others.Count())
            {
                throw new DependentViolationException($"Parameter \"{source.ToParameterLabel()}\" requires {String.Join(" and ", overall.Select(x => $"\"{x.ToParameterLabel()}\""))}.");
            }
        }

        #endregion
    }
}
