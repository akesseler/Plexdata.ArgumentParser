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
    /// <summary>
    /// The argument processor implementation.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of the argument processor.
    /// </remarks>
    /// <typeparam name="TInstance">
    /// The type descriptor of the user-defined argument class to be processed.
    /// </typeparam>
    internal class ArgumentProcessor<TInstance> where TInstance : class
    {
        #region Construction

        /// <summary>
        /// Standard constructor that takes the type to be processed.
        /// </summary>
        /// <remarks>
        /// This constructor takes the type descriptor of an instance of the class to be processed.
        /// </remarks>
        /// <param name="instance">
        /// The instance of the class to be processed.
        /// </param>
        /// <exception cref="Exception">
        /// All exceptions of extension <see cref="ValidationExtension.ThrowIfNull{TType}(TType, String)"/> 
        /// are possible.
        /// </exception>
        public ArgumentProcessor(TInstance instance)
            : base()
        {
            instance.ThrowIfNull(nameof(instance));
            this.Instance = instance;
        }

        /// <summary>
        /// Extended constructor that takes the type and an array of arguments to be processed.
        /// </summary>
        /// <remarks>
        /// This constructor takes the type descriptor of an instance of the class as well as 
        /// an array of arguments to be processed.
        /// </remarks>
        /// <param name="instance">
        /// The instance of the class to be processed.
        /// </param>
        /// <param name="arguments">
        /// The array of arguments to be processed.
        /// </param>
        /// <exception cref="Exception">
        /// All exceptions of extension <see cref="ValidationExtension.ThrowIfNullOrEmpty{TType}(TType, String)"/> 
        /// are possible.
        /// </exception>
        public ArgumentProcessor(TInstance instance, String[] arguments)
            : this(instance)
        {
            arguments.ThrowIfNullOrEmpty(nameof(arguments));
            this.Arguments = new List<String>(arguments);
        }

        /// <summary>
        /// Extended constructor that takes the type and a list of arguments to be processed.
        /// </summary>
        /// <remarks>
        /// This constructor takes the type descriptor of an instance of the class as well as 
        /// a list of arguments to be processed.
        /// </remarks>
        /// <param name="instance">
        /// The instance of the class to be processed.
        /// </param>
        /// <param name="arguments">
        /// The list of arguments to be processed.
        /// </param>
        /// <exception cref="Exception">
        /// All exceptions of extension <see cref="ValidationExtension.ThrowIfNullOrEmpty{TType}(TType, String)"/> 
        /// are possible.
        /// </exception>
        public ArgumentProcessor(TInstance instance, List<String> arguments)
            : this(instance)
        {
            arguments.ThrowIfNullOrEmpty(nameof(arguments));
            this.Arguments = arguments;
        }

        #endregion

        #region Statics

        /// <summary>
        /// Validates the instance to be processed.
        /// </summary>
        /// <remarks>
        /// This method validates the instance to be processed by just calling method 
        /// <see cref="Initialize"/> which in turn does the validation.
        /// </remarks>
        /// <param name="instance">
        /// The instance of the class to be processed.
        /// </param>
        /// <exception cref="ArgumentParserException">
        /// Any of an argument parser exception if necessary.
        /// </exception>
        public static void Validate(TInstance instance)
        {
            ArgumentProcessor<TInstance> processor = new ArgumentProcessor<TInstance>(instance);
            processor.Initialize();
        }

        /// <summary>
        /// Validates the instance and its array of arguments to be processed.
        /// </summary>
        /// <remarks>
        /// This method validates the instance as well as its array of arguments to be 
        /// processed by just calling method <see cref="Initialize"/> which in turn does 
        /// the validation.
        /// </remarks>
        /// <param name="instance">
        /// The instance of the class to be processed.
        /// </param>
        /// <param name="arguments">
        /// The array of arguments to be processed.
        /// </param>
        /// <exception cref="ArgumentParserException">
        /// Any of an argument parser exception if necessary.
        /// </exception>
        public static void Process(TInstance instance, String[] arguments)
        {
            ArgumentProcessor<TInstance> processor = new ArgumentProcessor<TInstance>(instance, arguments);
            processor.Process();
        }

        /// <summary>
        /// Validates the instance and its list of arguments to be processed.
        /// </summary>
        /// <remarks>
        /// This method validates the instance as well as its list of arguments to be 
        /// processed by just calling method <see cref="Initialize"/> which in turn does 
        /// the validation.
        /// </remarks>
        /// <param name="instance">
        /// The instance of the class to be processed.
        /// </param>
        /// <param name="arguments">
        /// The list of arguments to be processed.
        /// </param>
        /// <exception cref="ArgumentParserException">
        /// Any of an argument parser exception if necessary.
        /// </exception>
        public static void Process(TInstance instance, List<String> arguments)
        {
            ArgumentProcessor<TInstance> processor = new ArgumentProcessor<TInstance>(instance, arguments);
            processor.Process();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the applied instance.
        /// </summary>
        /// <remarks>
        /// This property gets the applied instance.
        /// </remarks>
        /// <value>
        /// The applied instance.
        /// </value>
        public TInstance Instance { get; private set; }

        /// <summary>
        /// Gets the applied list of arguments.
        /// </summary>
        /// <remarks>
        /// This property gets the applied list of arguments.
        /// </remarks>
        /// <value>
        /// The applied list of arguments.
        /// </value>
        public List<String> Arguments { get; private set; }

        /// <summary>
        /// Gets the list of property settings.
        /// </summary>
        /// <remarks>
        /// This property gets the list of property settings.
        /// </remarks>
        /// <value>
        /// The list of property settings.
        /// </value>
        public List<ArgumentProcessorSetting> Settings { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes an instance of this class.
        /// </summary>
        /// <remarks>
        /// This method initializes an instance of this class and implicitly 
        /// performs an argument validation.
        /// </remarks>
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
                                        this.Settings.Add(this.ApplyDefaultValue(new ArgumentProcessorSetting(property, attribute)));
                                    }
                                    else if (attribute.IsConverterSupported(property))
                                    {
                                        this.Settings.Add(new ArgumentProcessorSetting(property, attribute, true));
                                    }
                                    // TODO: Remove obsolete code later on.
                                    /* obsolete start */
                                    else if (property.PropertyType.HasConverter())
                                    {
                                        this.Settings.Add(new ArgumentProcessorSetting(property, attribute));
                                    }
                                    /* obsolete end */
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

        /// <summary>
        /// Performs command line argument processing.
        /// </summary>
        /// <remarks>
        /// This method performs the command line argument processing.
        /// </remarks>
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

                                if (setting.HasCustomConverter)
                                {
                                    setting.Property.SetValue(this.Instance, setting.InvokeCustomConverter(parameter, argument, (setting.Attribute as OptionParameterAttribute).Delimiter));
                                }
                                // TODO: Remove obsolete code later on.
                                /* obsolete start */
                                else if (setting.Property.PropertyType.HasConverter())
                                {
                                    setting.Property.SetValue(this.Instance, setting.Property.PropertyType
                                        .InvokeConverter(parameter, argument, (setting.Attribute as OptionParameterAttribute).Delimiter));
                                }
                                /* obsolete end */
                                else
                                {
                                    setting.Property.SetValue(this.Instance, OptionTypeConverter.Convert(argument, setting.Property.PropertyType));
                                }

                                processed.Add(setting);
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

        /// <summary>
        /// Try finding a particular setting.
        /// </summary>
        /// <remarks>
        /// This method tries to retrieve a particular setting value from current settings list.
        /// </remarks>
        /// <param name="setting">
        /// The found setting or <c>null</c>.
        /// </param>
        /// <returns>
        /// True, if the setting could be found and false if not.
        /// </returns>
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

        /// <summary>
        /// Try finding a particular setting by parameter name.
        /// </summary>
        /// <remarks>
        /// This method tries to retrieve a particular setting value from current settings list.
        /// </remarks>
        /// <param name="parameter">
        /// The parameter name to search for.
        /// </param>
        /// <param name="setting">
        /// The found setting or <c>null</c>.
        /// </param>
        /// <returns>
        /// True, if the setting could be found and false if not.
        /// </returns>
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

        /// <summary>
        /// Tries to get referenced items.
        /// </summary>
        /// <remarks>
        /// This method tries to extract all referenced items from current settings list.
        /// </remarks>
        /// <param name="source">
        /// The source setting.
        /// </param>
        /// <param name="others">
        /// The list of source settings.
        /// </param>
        /// <param name="overall">
        /// The list of referenced overall settings.
        /// </param>
        /// <returns>
        /// The result list of referenced settings.
        /// </returns>
        private IEnumerable<ArgumentProcessorSetting> TryGetReferencedItems(ArgumentProcessorSetting source, IEnumerable<ArgumentProcessorSetting> others, out IEnumerable<ArgumentProcessorSetting> overall)
        {
            // Get list of dependencies...
            String[] affected = source.Attribute.GetDependencies();

            // Apply all affected dependencies from overall settings.
            overall = this.Settings.Where(x => affected.Contains(x.Property.Name));

            // Filter out all currently available dependencies and return them.
            return others.Where(x => affected.Contains(x.Property.Name));
        }

        /// <summary>
        /// Validates all exclusive properties.
        /// </summary>
        /// <remarks>
        /// This method validates all properties that are marked as exclusive.
        /// </remarks>
        /// <param name="processed">
        /// The list of settings to be validated.
        /// </param>
        /// <exception cref="ExclusiveViolationException">
        /// This exception is thrown in case of an exclusive violaten takes place.
        /// </exception>
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

        /// <summary>
        /// Validates all required properties.
        /// </summary>
        /// <remarks>
        /// This method validates all properties that are marked as required.
        /// </remarks>
        /// <param name="processed">
        /// The list of settings to be validated.
        /// </param>
        /// <exception cref="RequiredViolationException">
        /// This exception is thrown in case of a required violaten takes place.
        /// </exception>
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

        /// <summary>
        /// Validates all dependency properties.
        /// </summary>
        /// <remarks>
        /// This method validates all properties that are marked as dependency.
        /// </remarks>
        /// <param name="processed">
        /// The list of settings to be validated.
        /// </param>
        /// <exception cref="RequiredViolationException">
        /// This exception is thrown in case of a required violaten takes place.
        /// </exception>
        /// <exception cref="Exception">
        /// Any of the <see cref="ArgumentParserException"/> is possible.
        /// </exception>
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

        /// <summary>
        /// Validates all referenced properties.
        /// </summary>
        /// <remarks>
        /// This method validates all properties that are referenced by other properties.
        /// </remarks>
        /// <param name="source">
        /// The source setting.
        /// </param>
        /// <exception cref="DependentViolationException">
        /// This exception is thrown in case of a dependency violation takes place.
        /// </exception>
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

        /// <summary>
        /// Validates all optional dependency properties.
        /// </summary>
        /// <remarks>
        /// This method validates all properties that are marked as optional dependency.
        /// </remarks>
        /// <param name="source">
        /// The source setting.
        /// </param>
        /// <param name="others">
        /// The list of other settings.
        /// </param>
        /// <param name="overall">
        /// The list of overall dependency settings.
        /// </param>
        /// <exception cref="DependentViolationException">
        /// This exception is thrown in case of a dependency violation takes place.
        /// </exception>
        private void ValidateOptionalDependencies(ArgumentProcessorSetting source, IEnumerable<ArgumentProcessorSetting> others, IEnumerable<ArgumentProcessorSetting> overall)
        {
            if (overall.Any() && !others.Any())
            {
                throw new DependentViolationException($"Parameter \"{source.ToParameterLabel()}\" depends on {String.Join(" or ", overall.Select(x => $"\"{x.ToParameterLabel()}\""))}.");
            }
        }

        /// <summary>
        /// Validates all required dependency properties.
        /// </summary>
        /// <remarks>
        /// This method validates all properties that are marked as required dependency.
        /// </remarks>
        /// <param name="source">
        /// The source setting.
        /// </param>
        /// <param name="others">
        /// The list of other settings.
        /// </param>
        /// <param name="overall">
        /// The list of overall dependency settings.
        /// </param>
        /// <exception cref="DependentViolationException">
        /// This exception is thrown in case of a dependency violation takes place.
        /// </exception>
        private void ValidateRequiredDependencies(ArgumentProcessorSetting source, IEnumerable<ArgumentProcessorSetting> others, IEnumerable<ArgumentProcessorSetting> overall)
        {
            if (overall.Count() != others.Count())
            {
                throw new DependentViolationException($"Parameter \"{source.ToParameterLabel()}\" requires {String.Join(" and ", overall.Select(x => $"\"{x.ToParameterLabel()}\""))}.");
            }
        }

        /// <summary>
        /// Tries to apply a default value to its related property.
        /// </summary>
        /// <remarks>
        /// This method tries to apply a default value to its related property. For 
        /// the moment, only optional parameters support a usage of default values.
        /// </remarks>
        /// <param name="setting">
        /// An instance of class <see cref="ArgumentProcessorSetting"/> to get the 
        /// default value from to be applied.
        /// </param>
        /// <returns>
        /// The same instance of class <see cref="ArgumentProcessorSetting"/> that 
        /// has been provided as parameter.
        /// </returns>
        /// <exception cref="DefaultValueException">
        /// This exception is thrown in all cases of applying a default value fails.
        /// </exception>
        private ArgumentProcessorSetting ApplyDefaultValue(ArgumentProcessorSetting setting)
        {
            if (!(setting.Attribute is OptionParameterAttribute attribute))
            {
                return setting;
            }

            if (!attribute.HasDefaultValue)
            {
                return setting;
            }

            try
            {
                setting.Property.SetValue(this.Instance, OptionTypeConverter.Convert(attribute.DefaultValue?.ToString(), setting.Property.PropertyType));
            }
            catch (Exception exception)
            {
                throw new DefaultValueException(
                    $"Could not apply default value of \"{(attribute.DefaultValue is null ? "null" : attribute.DefaultValue.ToString())}\" " +
                    $"to property \"{setting.Property.Name}\". See inner exception for more information.", exception);
            }

            return setting;
        }

        #endregion
    }
}
