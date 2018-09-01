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

using Plexdata.ArgumentParser.Attributes;
using Plexdata.ArgumentParser.Constants;
using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plexdata.ArgumentParser.Processors
{
    internal class HelpProcessor<TInstance> where TInstance : class
    {
        #region Construction

        public HelpProcessor(TInstance instance)
            : base()
        {
            instance.ThrowIfNull(nameof(instance));
            this.Instance = instance;
            this.Results = String.Empty;
        }

        #endregion

        #region Statics

        public static String Generate(TInstance instance)
        {
            HelpProcessor<TInstance> processor = new HelpProcessor<TInstance>(instance);
            processor.Generate();
            return processor.Results;
        }

        public static String Generate(TInstance instance, Int32 length)
        {
            HelpProcessor<TInstance> processor = new HelpProcessor<TInstance>(instance);
            processor.Generate(length);
            return processor.Results;
        }

        #endregion

        #region Properties

        public TInstance Instance { get; private set; }

        public String Results { get; private set; }

        internal HelpLicenseAttribute License { get; set; }

        internal HelpPrefaceAttribute Preface { get; set; }

        internal HelpClosureAttribute Closure { get; set; }

        internal List<HelpUtilizeAttribute> Utilizes { get; set; }

        internal List<HelpProcessorSetting> Settings { get; set; }

        #endregion

        #region Methods

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

        public void Generate()
        {
            this.Generate(80);
        }

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

        private String JoinIntoLines(String content, Int32 length)
        {
            return String.Join(Environment.NewLine, this.SplitIntoLines(content, length));
        }

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
                        System.Diagnostics.Debug.WriteLine(exception);
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
                        System.Diagnostics.Debug.WriteLine(exception);
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
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }
            }

            return value;
        }

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
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }
            }

            return value;
        }

        private HelpUtilizeAttribute FixupUtilize(HelpUtilizeAttribute value)
        {
            if (value.IsContent)
            {
                if (value.Content.Contains(Placeholders.Program))
                {
                    try
                    {
                        String filename = Path.GetFileName(Assembly.GetEntryAssembly().Location);

                        if (!String.IsNullOrWhiteSpace(filename))
                        {
                            value.Content = value.Content.Replace(Placeholders.Program, filename);
                        }
                    }
                    catch (Exception exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }
            }

            return value;
        }

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

            value.Caption = value.Caption.Trim();
            value.Content = value.Summary.Content;

            return value;
        }

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
