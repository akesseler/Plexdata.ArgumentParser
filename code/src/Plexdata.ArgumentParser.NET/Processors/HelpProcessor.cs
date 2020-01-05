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
using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plexdata.ArgumentParser.Processors
{
    /// <summary>
    /// The help processor implementation.
    /// </summary>
    /// <remarks>
    /// This class represents the help processor implementation.
    /// </remarks>
    /// <typeparam name="TInstance">
    /// The type descriptor of the user-defined argument class to be processed.
    /// </typeparam>
    internal class HelpProcessor<TInstance> where TInstance : class
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
        public HelpProcessor(TInstance instance)
            : base()
        {
            instance.ThrowIfNull(nameof(instance));
            this.Instance = instance;
            this.Results = String.Empty;
        }

        #endregion

        #region Statics

        /// <summary>
        /// Generates the help string.
        /// </summary>
        /// <remarks>
        /// This method generates the string to be used to print out the 
        /// arguments help.
        /// </remarks>
        /// <param name="instance">
        /// The instance of the class to be processed.
        /// </param>
        /// <returns>
        /// The string containing the complete arguments help.
        /// </returns>
        public static String Generate(TInstance instance)
        {
            HelpProcessor<TInstance> processor = new HelpProcessor<TInstance>(instance);
            processor.Generate();
            return processor.Results;
        }

        /// <summary>
        /// Generates the help string using provided line length.
        /// </summary>
        /// <remarks>
        /// This method generates the string using provided line length to 
        /// be used to print out the arguments help.
        /// </remarks>
        /// <param name="instance">
        /// The instance of the class to be processed.
        /// </param>
        /// <param name="length">
        /// The maximum length of each line.
        /// </param>
        /// <returns>
        /// The string containing the complete arguments help.
        /// </returns>
        public static String Generate(TInstance instance, Int32 length)
        {
            HelpProcessor<TInstance> processor = new HelpProcessor<TInstance>(instance);
            processor.Generate(length);
            return processor.Results;
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
        /// Gets the generated results.
        /// </summary>
        /// <remarks>
        /// This property gets the generated results.
        /// </remarks>
        /// <value>
        /// The generated results.
        /// </value>
        public String Results { get; private set; }

        /// <summary>
        /// Gets the help license attribute.
        /// </summary>
        /// <remarks>
        /// This property gets the help license attribute.
        /// </remarks>
        /// <value>
        /// The help license attribute.
        /// </value>
        internal HelpLicenseAttribute License { get; set; }

        /// <summary>
        /// Gets the help preface attribute.
        /// </summary>
        /// <remarks>
        /// This property gets the help preface attribute.
        /// </remarks>
        /// <value>
        /// The help preface attribute.
        /// </value>
        internal HelpPrefaceAttribute Preface { get; set; }

        /// <summary>
        /// Gets the help closure attribute.
        /// </summary>
        /// <remarks>
        /// This property gets the help closure attribute.
        /// </remarks>
        /// <value>
        /// The help closure attribute.
        /// </value>
        internal HelpClosureAttribute Closure { get; set; }

        /// <summary>
        /// Gets the list of help utilize attributes.
        /// </summary>
        /// <remarks>
        /// This property gets the list of help utilize attributes.
        /// </remarks>
        /// <value>
        /// The list of help utilize attributes.
        /// </value>
        internal List<HelpUtilizeAttribute> Utilizes { get; set; }

        /// <summary>
        /// Gets the list of help processor settings.
        /// </summary>
        /// <remarks>
        /// This property gets the list of help processor settings.
        /// </remarks>
        /// <value>
        /// The list of help processor settings.
        /// </value>
        internal List<HelpProcessorSetting> Settings { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this instance with its default values.
        /// </summary>
        /// <remarks>
        /// This method initializes this instance with its default values.
        /// </remarks>
        public void Initialize()
        {
            this.License = null;
            this.Preface = null;
            this.Closure = null;
            this.Utilizes = new List<HelpUtilizeAttribute>();
            this.Settings = new List<HelpProcessorSetting>();

            foreach (Attribute current in Attribute.GetCustomAttributes(this.Instance.GetType()))
            {
                if (current is HelpLicenseAttribute)
                {
                    this.License = this.FixupLicense(current as HelpLicenseAttribute);
                }
                else if (current is HelpPrefaceAttribute)
                {
                    this.Preface = this.FixupPreface(current as HelpPrefaceAttribute);
                }
                else if (current is HelpClosureAttribute)
                {
                    this.Closure = current as HelpClosureAttribute;
                }
                else if (current is HelpUtilizeAttribute)
                {
                    this.Utilizes.Add(this.FixupUtilize(current as HelpUtilizeAttribute));
                }
            }

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;

            PropertyInfo[] properties = this.Instance.GetType().GetProperties(flags);

            if (properties != null)
            {
                foreach (PropertyInfo property in properties)
                {
                    HelpSummaryAttribute summary = null;
                    ParameterObjectAttribute parameter = null;

                    IEnumerable<Attribute> attributes = property.GetCustomAttributes();

                    if (attributes != null)
                    {
                        foreach (Attribute attribute in attributes)
                        {
                            if (attribute is ParameterObjectAttribute)
                            {
                                parameter = attribute as ParameterObjectAttribute;
                            }
                            else if (attribute is HelpSummaryAttribute)
                            {
                                summary = attribute as HelpSummaryAttribute;
                            }
                        }
                    }

                    if (property != null && parameter != null && summary != null)
                    {
                        this.Settings.Add(this.FixupSetting(new HelpProcessorSetting(property, parameter, summary)));
                    }
                }
            }
        }

        /// <summary>
        /// Performs help string generation using default line length.
        /// </summary>
        /// <remarks>
        /// This method performs help string generation using 80 characters as default line length.
        /// </remarks>
        /// <see cref="Generate(Int32)"/>
        public void Generate()
        {
            this.Generate(80);
        }

        /// <summary>
        /// Performs help string generation using provided line length.
        /// </summary>
        /// <remarks>
        /// This method performs help string generation using provided number characters as line length.
        /// </remarks>
        /// <param name="bounds">
        /// The maximum number characters used as line length.
        /// </param>
        public void Generate(Int32 bounds)
        {
            String TAB = "  ";
            String EOL = Environment.NewLine;

            bounds -= EOL.Length;

            if (bounds <= 0)
            {
                throw new HelpGeneratorException($"The minimum bounds must be greater than {EOL.Length}.");
            }

            try
            {
                this.Initialize();

                StringBuilder builder = new StringBuilder(1024);

                #region Preamble...

                if (this.License != null && this.License.IsContent)
                {
                    builder.AppendFormat($"{EOL}{this.JoinIntoLines(this.License.Content, bounds)}{EOL}");
                }

                if (this.Preface != null && this.Preface.IsContent)
                {
                    builder.AppendFormat($"{EOL}{this.JoinIntoLines(this.Preface.Content, bounds)}{EOL}");
                }

                if (this.Utilizes != null && this.Utilizes.Count > 0)
                {
                    Dictionary<String, List<HelpUtilizeAttribute>> headings = this.FilterUtilizeByHeading(this.Utilizes);

                    foreach (String heading in headings.Keys)
                    {
                        builder.Append($"{EOL}");

                        if (!String.IsNullOrWhiteSpace(heading))
                        {
                            builder.AppendFormat($"{this.JoinIntoLines(heading, bounds)}{EOL}{EOL}");
                        }

                        Dictionary<String, List<HelpUtilizeAttribute>> captions = this.FilterUtilizeByCaption(headings[heading]);

                        foreach (String caption in captions.Keys)
                        {
                            if (!String.IsNullOrWhiteSpace(caption))
                            {
                                builder.AppendFormat($"{TAB}{this.JoinIntoLines(caption, bounds)}{EOL}");
                            }

                            foreach (HelpUtilizeAttribute utilize in captions[caption])
                            {
                                if (!String.IsNullOrWhiteSpace(utilize.Content))
                                {
                                    if (String.IsNullOrWhiteSpace(caption))
                                    {
                                        builder.AppendFormat($"{TAB}{this.JoinIntoLines(utilize.Content, bounds)}{EOL}");
                                    }
                                    else
                                    {
                                        builder.AppendFormat($"{TAB}{TAB}{this.JoinIntoLines(utilize.Content, bounds)}{EOL}");
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Content...

                if (this.Settings != null && this.Settings.Count > 0)
                {
                    Int32 offset = 0;

                    foreach (var setting in this.Settings)
                    {
                        if (setting.IsCaption && offset < setting.Caption.Length)
                        {
                            offset = setting.Caption.Length;
                        }
                    }

                    if (offset > 0)
                    {
                        offset += 2 * TAB.Length;

                        Dictionary<String, List<HelpProcessorSetting>> headings = this.FilterSettingByHeading(this.Settings);

                        builder.Append($"{EOL}");

                        foreach (String heading in headings.Keys)
                        {
                            if (!String.IsNullOrWhiteSpace(heading))
                            {
                                builder.AppendFormat($"{this.JoinIntoLines(heading, bounds)}{EOL}{EOL}");
                            }

                            Dictionary<String, List<HelpProcessorSetting>> sections = this.FilterSettingBySection(headings[heading]);

                            foreach (String section in sections.Keys)
                            {
                                if (!String.IsNullOrWhiteSpace(section))
                                {
                                    builder.AppendFormat($"{TAB}{this.JoinIntoLines(section, bounds)}{EOL}{EOL}");
                                }

                                foreach (HelpProcessorSetting setting in sections[section])
                                {
                                    String caption = ($"{TAB}{setting.Caption}").PadRight(offset);

                                    Int32 indent = caption.Length;

                                    String content = setting.Content;

                                    if (bounds - indent <= 20)
                                    {
                                        caption = caption.Substring(0, Math.Min(caption.Length, bounds));

                                        indent = bounds / 5;

                                        builder.Append($"{caption}{EOL}{String.Empty.PadRight(indent)}");
                                    }
                                    else
                                    {
                                        builder.Append($"{caption}");
                                    }

                                    String padding = String.Empty.PadRight(indent);

                                    List<String> lines = this.SplitIntoLines(content, bounds - indent);

                                    for (Int32 index = 0; index < lines.Count; index++)
                                    {
                                        if (index != 0)
                                        {
                                            builder.Append(padding);
                                        }

                                        builder.Append($"{lines[index]}{EOL}");
                                    }

                                    if (lines.Count == 0) { builder.Append($"{EOL}"); }

                                    builder.Append($"{EOL}");
                                }
                            }
                        }
                    }
                }
                else
                {
                    builder.Append($"{EOL}");
                }

                #endregion

                #region Epilogue...

                if (this.Closure != null && this.Closure.IsContent)
                {
                    builder.AppendFormat($"{this.JoinIntoLines(this.Closure.Content, bounds)}{EOL}");
                }

                #endregion

                this.Results = builder.ToString();

                if (this.Results == Environment.NewLine)
                {
                    this.Results = this.Results.TrimEnd();
                }
            }
            catch (Exception exception)
            {
                throw new HelpGeneratorException("Generating the help output has failed unexpectedly.", exception);
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Cleans out provided content.
        /// </summary>
        /// <remarks>
        /// This method replaces any tab-character, carriage return and line feed by 
        /// a single space and trims the result.
        /// </remarks>
        /// <param name="content">
        /// The content to be cleaned out.
        /// </param>
        /// <returns>
        /// The cleaned out content.
        /// </returns>
        private String CleanOutContent(String content)
        {
            if (!String.IsNullOrWhiteSpace(content))
            {
                return content.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ').Trim();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Converts content into its printable representation.
        /// </summary>
        /// <remarks>
        /// This method converts provided content into its printable representation by 
        /// splitting it into lines and joining them by a carriage return and line feed 
        /// afterwards.
        /// </remarks>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <returns>
        /// The printable content.
        /// </returns>
        /// <see cref="SplitIntoLines(String, Int32)"/>
        private String JoinIntoLines(String content, Int32 length)
        {
            return String.Join(Environment.NewLine, this.SplitIntoLines(content, length));
        }

        /// <summary>
        /// Splits content into lines by taking respect on provided line length.
        /// </summary>
        /// <remarks>
        /// This method splits provided content into lines by taking respect on provided line length.
        /// </remarks>
        /// <param name="content">
        /// The content to be split.
        /// </param>
        /// <param name="length">
        /// The maximum number characters used as line length.
        /// </param>
        /// <returns>
        /// The list of resulting lines.
        /// </returns>
        private List<String> SplitIntoLines(String content, Int32 length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            List<String> result = new List<String>();
            StringBuilder line = new StringBuilder(length);
            List<String> words = new List<String>(this.CleanOutContent(content).Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            if (!String.IsNullOrWhiteSpace(content))
            {
                while (words.Count > 0)
                {
                    var word = words[0]; words.RemoveAt(0);

                    if (word.Length >= length)
                    {
                        String head = word.Substring(0, length);
                        String tail = word.Substring(length);

                        word = head;
                        words.Insert(0, tail);
                    }

                    if (line.Length + word.Length >= length)
                    {
                        result.Add(line.ToString().TrimEnd());
                        line.Clear();
                    }

                    line.Append($"{word} ");
                }

                result.Add(line.ToString().TrimEnd());
            }

            return result;
        }

        /// <summary>
        /// Fixes the license placeholder.
        /// </summary>
        /// <remarks>
        /// This method fixes the license placeholder if necessary.
        /// </remarks>
        /// <param name="value">
        /// The help license attribute to be fixed.
        /// </param>
        /// <returns>
        /// The fixed help license attribute.
        /// </returns>
        private HelpLicenseAttribute FixupLicense(HelpLicenseAttribute value)
        {
            if (value.IsContent)
            {
                if (value.Content.Contains(Placeholders.Copyright))
                {
                    try
                    {
                        AssemblyCopyrightAttribute copyright = Assembly.GetEntryAssembly().GetCustomAttributes()
                            .Where(x => x is AssemblyCopyrightAttribute).FirstOrDefault() as AssemblyCopyrightAttribute;

                        if (copyright != null && !String.IsNullOrWhiteSpace(copyright.Copyright))
                        {
                            value.Content = value.Content.Replace(Placeholders.Copyright, copyright.Copyright);
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                    }
                }

                if (value.Content.Contains(Placeholders.Company))
                {
                    try
                    {
                        AssemblyCompanyAttribute company = Assembly.GetEntryAssembly().GetCustomAttributes()
                            .Where(x => x is AssemblyCompanyAttribute).FirstOrDefault() as AssemblyCompanyAttribute;

                        if (company != null && !String.IsNullOrWhiteSpace(company.Company))
                        {
                            value.Content = value.Content.Replace(Placeholders.Company, company.Company);
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                    }
                }

                if (value.Content.Contains(Placeholders.Version))
                {
                    try
                    {
                        String version = Assembly.GetEntryAssembly().GetName().Version.ToString();

                        if (!String.IsNullOrWhiteSpace(version))
                        {
                            value.Content = value.Content.Replace(Placeholders.Version, version);
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Fixes the description placeholder.
        /// </summary>
        /// <remarks>
        /// This method fixes the description placeholder if necessary.
        /// </remarks>
        /// <param name="value">
        /// The help preface attribute to be fixed.
        /// </param>
        /// <returns>
        /// The fixed help preface attribute.
        /// </returns>
        private HelpPrefaceAttribute FixupPreface(HelpPrefaceAttribute value)
        {
            if (value.IsContent)
            {
                if (value.Content.Contains(Placeholders.Description))
                {
                    try
                    {
                        AssemblyDescriptionAttribute description = Assembly.GetEntryAssembly().GetCustomAttributes()
                            .Where(x => x is AssemblyDescriptionAttribute).FirstOrDefault() as AssemblyDescriptionAttribute;

                        if (description != null && !String.IsNullOrWhiteSpace(description.Description))
                        {
                            value.Content = value.Content.Replace(Placeholders.Description, description.Description);
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Fixes the program placeholder.
        /// </summary>
        /// <remarks>
        /// This method fixes the program placeholder if necessary.
        /// </remarks>
        /// <param name="value">
        /// The help utilize attribute to be fixed.
        /// </param>
        /// <returns>
        /// The fixed help utilize attribute.
        /// </returns>
        private HelpUtilizeAttribute FixupUtilize(HelpUtilizeAttribute value)
        {
            if (value.IsContent)
            {
                try
                {
                    if (value.Content.Contains(Placeholders.Product))
                    {
                        AssemblyProductAttribute product = Assembly.GetEntryAssembly().GetCustomAttributes()
                            .Where(x => x is AssemblyProductAttribute).FirstOrDefault() as AssemblyProductAttribute;

                        if (product != null && !String.IsNullOrWhiteSpace(product.Product))
                        {
                            value.Content = value.Content.Replace(Placeholders.Product, product.Product);
                            return value;
                        }

                        value.Content = value.Content.Replace(Placeholders.Product, Placeholders.Program);
                    }

                    if (value.Content.Contains(Placeholders.Program))
                    {
                        String filename = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

                        if (!String.IsNullOrWhiteSpace(filename))
                        {
                            value.Content = value.Content.Replace(Placeholders.Program, filename);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }
            }

            return value;
        }

        /// <summary>
        /// Fixes the help processor setting.
        /// </summary>
        /// <remarks>
        /// This method fixes the help processor setting if necessary.
        /// </remarks>
        /// <param name="value">
        /// The help processor setting to be fixed.
        /// </param>
        /// <returns>
        /// The fixed help processor setting.
        /// </returns>
        private HelpProcessorSetting FixupSetting(HelpProcessorSetting value)
        {
            if (value.Setting.IsSolidLabel)
            {
                value.Caption = $"{ParameterPrefixes.SolidPrefix}{value.Setting.SolidLabel}";

                if (value.Setting.IsBriefLabel)
                {
                    value.Caption += $" [{ParameterPrefixes.BriefPrefix}{value.Setting.BriefLabel}]";
                }
            }
            else if (value.Setting.IsBriefLabel)
            {
                value.Caption += $"{ParameterPrefixes.BriefPrefix}{value.Setting.BriefLabel}";
            }

            if (value.Summary.IsOptions)
            {
                value.Caption += $" {value.Summary.Options}";
            }

            if (String.IsNullOrWhiteSpace(value.Caption))
            {
                value.Caption = String.Empty;
            }

            value.Caption = value.Caption.Trim();
            value.Content = value.Summary.Content;

            return value;
        }

        /// <summary>
        /// Filters utilize attributes by heading.
        /// </summary>
        /// <remarks>
        /// This method filters utilize attributes by heading.
        /// </remarks>
        /// <param name="source">
        /// The source list of help utilize attributes.
        /// </param>
        /// <returns>
        /// A dictionary with merged assignments of help utilize attributes.
        /// </returns>
        private Dictionary<String, List<HelpUtilizeAttribute>> FilterUtilizeByHeading(List<HelpUtilizeAttribute> source)
        {
            Dictionary<String, List<HelpUtilizeAttribute>> result = new Dictionary<String, List<HelpUtilizeAttribute>>();

            foreach (HelpUtilizeAttribute current in source)
            {
                if (!result.ContainsKey(current.Heading))
                {
                    result.Add(current.Heading, new List<HelpUtilizeAttribute>());
                }

                result[current.Heading].Add(current);
            }

            return result;
        }

        /// <summary>
        /// Filters utilize attributes by caption.
        /// </summary>
        /// <remarks>
        /// This method filters utilize attributes by caption.
        /// </remarks>
        /// <param name="source">
        /// The source list of help utilize attributes.
        /// </param>
        /// <returns>
        /// A dictionary with merged assignments of help utilize attributes.
        /// </returns>
        private Dictionary<String, List<HelpUtilizeAttribute>> FilterUtilizeByCaption(List<HelpUtilizeAttribute> source)
        {
            Dictionary<String, List<HelpUtilizeAttribute>> result = new Dictionary<String, List<HelpUtilizeAttribute>>();

            foreach (HelpUtilizeAttribute current in source)
            {
                if (!result.ContainsKey(current.Section))
                {
                    result.Add(current.Section, new List<HelpUtilizeAttribute>());
                }

                result[current.Section].Add(current);
            }

            return result;
        }

        /// <summary>
        /// Filters processor settings by heading.
        /// </summary>
        /// <remarks>
        /// This method filters processor settings by heading.
        /// </remarks>
        /// <param name="source">
        /// The source list of help processor settings.
        /// </param>
        /// <returns>
        /// A dictionary with merged assignments of help processor settings.
        /// </returns>
        private Dictionary<String, List<HelpProcessorSetting>> FilterSettingByHeading(List<HelpProcessorSetting> source)
        {
            Dictionary<String, List<HelpProcessorSetting>> result = new Dictionary<String, List<HelpProcessorSetting>>();

            foreach (HelpProcessorSetting current in source)
            {
                if (!result.ContainsKey(current.Heading))
                {
                    result.Add(current.Heading, new List<HelpProcessorSetting>());
                }

                result[current.Heading].Add(current);
            }

            return result;
        }

        /// <summary>
        /// Filters processor settings by section.
        /// </summary>
        /// <remarks>
        /// This method filters processor settings by section.
        /// </remarks>
        /// <param name="source">
        /// The source list of help processor settings.
        /// </param>
        /// <returns>
        /// A dictionary with merged assignments of help processor settings.
        /// </returns>
        private Dictionary<String, List<HelpProcessorSetting>> FilterSettingBySection(List<HelpProcessorSetting> source)
        {
            Dictionary<String, List<HelpProcessorSetting>> result = new Dictionary<String, List<HelpProcessorSetting>>();

            foreach (HelpProcessorSetting current in source)
            {
                if (!result.ContainsKey(current.Section))
                {
                    result.Add(current.Section, new List<HelpProcessorSetting>());
                }

                result[current.Section].Add(current);
            }

            return result;
        }

        #endregion
    }
}
