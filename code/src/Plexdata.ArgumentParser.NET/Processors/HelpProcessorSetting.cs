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
using System;
using System.Reflection;

namespace Plexdata.ArgumentParser.Processors
{
    internal class HelpProcessorSetting
    {
        #region Construction

        public HelpProcessorSetting(PropertyInfo property, ParameterObjectAttribute setting, HelpSummaryAttribute summary)
            : base()
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.Setting = setting ?? throw new ArgumentNullException(nameof(setting));
            this.Summary = summary ?? throw new ArgumentNullException(nameof(summary));
        }

        #endregion

        #region Properties

        public PropertyInfo Property { get; private set; }

        public ParameterObjectAttribute Setting { get; private set; }

        public HelpSummaryAttribute Summary { get; private set; }

        public String Heading
        {
            get
            {
                if (this.IsHeading)
                {
                    return this.Summary.Heading;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public Boolean IsHeading
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Summary.Heading);
            }
        }

        public String Section
        {
            get
            {
                if (this.IsSection)
                {
                    return this.Summary.Section;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public Boolean IsSection
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Summary.Section);
            }
        }

        public String Caption { get; set; }

        public Boolean IsCaption
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Caption);
            }
        }

        public String Content { get; set; }

        public Boolean IsContent
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Content);
            }
        }

        #endregion
    }
}
