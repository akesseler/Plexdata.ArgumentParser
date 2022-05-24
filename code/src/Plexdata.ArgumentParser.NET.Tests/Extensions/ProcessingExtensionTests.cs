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

using NUnit.Framework;
using Plexdata.ArgumentParser.Attributes;
using Plexdata.ArgumentParser.Extensions;
using Plexdata.ArgumentParser.Interfaces;
using Plexdata.ArgumentParser.Processors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Plexdata.ArgumentParser.Tests.Extensions
{
    [TestFixture]
    [TestOf(nameof(ProcessingExtension))]
    public class ProcessingExtensionTests
    {
        #region RemovePrefix

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RemovePrefix_ParameterInvalid_ResultIsSameParameter(String parameter)
        {
            Assert.That(parameter.RemovePrefix(), Is.EqualTo(parameter));
        }

        [Test]
        [TestCase("x", "x")]
        [TestCase("xy", "xy")]
        [TestCase("xyz", "xyz")]
        [TestCase("--", "-")]
        [TestCase("--x", "x")]
        [TestCase("--xy", "xy")]
        [TestCase("--xyz", "xyz")]
        [TestCase("-", "-")]
        [TestCase("-x", "x")]
        [TestCase("-xy", "xy")]
        [TestCase("-xyz", "xyz")]
        [TestCase("/", "/")]
        [TestCase("/x", "x")]
        [TestCase("/xy", "xy")]
        [TestCase("/xyz", "xyz")]
        public void RemovePrefix_ParameterValid_ResultAsExpected(String parameter, String expected)
        {
            Assert.That(parameter.RemovePrefix(), Is.EqualTo(expected));
        }

        #endregion

        #region IsParameter

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsParameter_ParameterInvalid_ResultIsFalse(String parameter)
        {
            Assert.That(parameter.IsParameter(), Is.False);
        }

        [Test]
        [TestCase("x", false)]
        [TestCase("  x  ", false)]
        [TestCase("xy", false)]
        [TestCase("  xy  ", false)]
        [TestCase("xyz", false)]
        [TestCase("  xyz  ", false)]
        [TestCase("--", false)]
        [TestCase("  --  ", false)]
        [TestCase("-", false)]
        [TestCase("  -  ", false)]
        [TestCase("/", false)]
        [TestCase("  /  ", false)]
        [TestCase("--x", true)]
        [TestCase("  --x  ", true)]
        [TestCase("--xy", true)]
        [TestCase("  --xy  ", true)]
        [TestCase("--xyz", true)]
        [TestCase("  --xyz  ", true)]
        [TestCase("-x", true)]
        [TestCase("  -x  ", true)]
        [TestCase("-xy", true)]
        [TestCase("  -xy  ", true)]
        [TestCase("-xyz", true)]
        [TestCase("  -xyz  ", true)]
        [TestCase("/x", true)]
        [TestCase("  /x  ", true)]
        [TestCase("/xy", true)]
        [TestCase("  /xy  ", true)]
        [TestCase("/xyz", true)]
        [TestCase("  /xyz  ", true)]
        public void IsParameter_ParameterValid_ResultIsFalse(String parameter, Boolean expected)
        {
            Assert.That(parameter.IsParameter(), Is.EqualTo(expected));
        }

        #endregion

        #region IsSupportedDataType

        [Test]
        [TestCaseSource(nameof(IsSupportedDataTypeTestItems))]
        public void IsSupportedDataType_VariousDefinitions_ResultAsExpected(Object candidate)
        {
            IsSupportedDataTypeTestItem actual = (IsSupportedDataTypeTestItem)candidate;

            Assert.That(actual.Parameter.IsSupportedDataType(actual.Property), Is.EqualTo(actual.Expected));
        }

        #endregion

        #region IsConverterSupported

        [Test]
        [TestCase(typeof(SwitchParameterAttribute))]
        [TestCase(typeof(VerbalParameterAttribute))]
        [TestCase(typeof(UnsupportedParameterAttribute))]
        public void IsConverterSupported_WrongParameterObjectAttributeType_ResultIsFalse(Type type)
        {
            ParameterObjectAttribute attribute = null;

            if (type == typeof(SwitchParameterAttribute))
            {
                attribute = new SwitchParameterAttribute();
            }
            else if (type == typeof(VerbalParameterAttribute))
            {
                attribute = new VerbalParameterAttribute();
            }
            else if (type == typeof(UnsupportedParameterAttribute))
            {
                attribute = new UnsupportedParameterAttribute();
            }

            Assert.That(attribute.IsConverterSupported(null), Is.False);
        }

        [Test]
        [TestCaseSource(nameof(IsConverterSupportedTestItems))]
        public void IsConverterSupported_OptionParameterAttribute_ResultAsExpected(Object candidate)
        {
            IsConverterSupportedTestItem actual = (IsConverterSupportedTestItem)candidate;

            Assert.That(actual.Attribute.IsConverterSupported(actual.Property), Is.EqualTo(actual.Expected));
        }

        #endregion

        #region IsSwitchParameter

        [Test]
        public void IsSwitchParameter_ArgumentProcessorSettingIsNull_ResultIsFalse()
        {
            ArgumentProcessorSetting setting = null;

            Assert.That(setting.IsSwitchParameter(), Is.False);
        }

        [Test]
        [TestCase(typeof(SwitchParameterAttribute), true)]
        [TestCase(typeof(OptionParameterAttribute), false)]
        [TestCase(typeof(VerbalParameterAttribute), false)]
        [TestCase(typeof(UnsupportedParameterAttribute), false)]
        public void IsSwitchParameter_ArgumentProcessorSettingWithAttribute_ResultAsExpected(Type type, Boolean expected)
        {
            ParameterObjectAttribute attribute = null;

            if (type == typeof(SwitchParameterAttribute))
            {
                attribute = new SwitchParameterAttribute();
            }
            else if (type == typeof(OptionParameterAttribute))
            {
                attribute = new OptionParameterAttribute();
            }
            else if (type == typeof(VerbalParameterAttribute))
            {
                attribute = new VerbalParameterAttribute();
            }
            else if (type == typeof(UnsupportedParameterAttribute))
            {
                attribute = new UnsupportedParameterAttribute();
            }

            ArgumentProcessorSetting setting = new ArgumentProcessorSetting(new TestPropertyInfo(typeof(Object)), attribute);

            Assert.That(setting.IsSwitchParameter(), Is.EqualTo(expected));
        }

        #endregion

        #region IsOptionParameter

        [Test]
        public void IsOptionParameter_ArgumentProcessorSettingIsNull_ResultIsFalse()
        {
            ArgumentProcessorSetting setting = null;

            Assert.That(setting.IsOptionParameter(), Is.False);
        }

        [Test]
        [TestCase(typeof(SwitchParameterAttribute), false)]
        [TestCase(typeof(OptionParameterAttribute), true)]
        [TestCase(typeof(VerbalParameterAttribute), false)]
        [TestCase(typeof(UnsupportedParameterAttribute), false)]
        public void IsOptionParameter_ArgumentProcessorSettingWithAttribute_ResultAsExpected(Type type, Boolean expected)
        {
            ParameterObjectAttribute attribute = null;

            if (type == typeof(SwitchParameterAttribute))
            {
                attribute = new SwitchParameterAttribute();
            }
            else if (type == typeof(OptionParameterAttribute))
            {
                attribute = new OptionParameterAttribute();
            }
            else if (type == typeof(VerbalParameterAttribute))
            {
                attribute = new VerbalParameterAttribute();
            }
            else if (type == typeof(UnsupportedParameterAttribute))
            {
                attribute = new UnsupportedParameterAttribute();
            }

            ArgumentProcessorSetting setting = new ArgumentProcessorSetting(new TestPropertyInfo(typeof(Object)), attribute);

            Assert.That(setting.IsOptionParameter(), Is.EqualTo(expected));
        }

        #endregion

        #region IsVerbalParameter

        [Test]
        public void IsVerbalParameter_ArgumentProcessorSettingIsNull_ResultIsFalse()
        {
            ArgumentProcessorSetting setting = null;

            Assert.That(setting.IsVerbalParameter(), Is.False);
        }

        [Test]
        [TestCase(typeof(SwitchParameterAttribute), false)]
        [TestCase(typeof(OptionParameterAttribute), false)]
        [TestCase(typeof(VerbalParameterAttribute), true)]
        [TestCase(typeof(UnsupportedParameterAttribute), false)]
        public void IsVerbalParameter_ArgumentProcessorSettingWithAttribute_ResultAsExpected(Type type, Boolean expected)
        {
            ParameterObjectAttribute attribute = null;

            if (type == typeof(SwitchParameterAttribute))
            {
                attribute = new SwitchParameterAttribute();
            }
            else if (type == typeof(OptionParameterAttribute))
            {
                attribute = new OptionParameterAttribute();
            }
            else if (type == typeof(VerbalParameterAttribute))
            {
                attribute = new VerbalParameterAttribute();
            }
            else if (type == typeof(UnsupportedParameterAttribute))
            {
                attribute = new UnsupportedParameterAttribute();
            }

            ArgumentProcessorSetting setting = new ArgumentProcessorSetting(new TestPropertyInfo(typeof(Object)), attribute);

            Assert.That(setting.IsVerbalParameter(), Is.EqualTo(expected));
        }

        #endregion

        #region ToParameterLabel

        [Test]
        public void ToParameterLabel_ArgumentProcessorSettingIsNull_ResultIsEmpty()
        {
            ArgumentProcessorSetting setting = null;

            Assert.That(setting.ToParameterLabel(), Is.Empty);
        }

        [Test]
        [TestCaseSource(nameof(ToParameterLabelTestItems))]
        public void ToParameterLabel_ArgumentProcessorSettingWithAttribute_ResultAsExpected(Object candidate)
        {
            ToParameterLabelTestItem actual = (ToParameterLabelTestItem)candidate;

            Assert.That(actual.Setting.ToParameterLabel(), Is.EqualTo(actual.Expected));
        }

        #endregion

        #region Test helpers

        private class TestTypeConverter : ICustomConverter<Object>
        {
            public Object Convert(String parameter, String argument, String delimiter)
            {
                throw new NotImplementedException();
            }
        }

        private class TestTypeConverterWrongDataType : ICustomConverter<String>
        {
            public String Convert(String parameter, String argument, String delimiter)
            {
                throw new NotImplementedException();
            }
        }

        private class TestTypeConverterWrongInterfaceType : IComparable<Object>
        {
            public Int32 CompareTo(Object other)
            {
                throw new NotImplementedException();
            }
        }

        private class TestTypeConverterNoGenericInterface : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private class TestPropertyInfo : PropertyInfo
        {
            private readonly Type propertyType = null;

            public TestPropertyInfo(Type propertyType)
            {
                this.propertyType = propertyType;
            }

            public override Type PropertyType { get { return this.propertyType; } }

            public override PropertyAttributes Attributes
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override Boolean CanRead
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override Boolean CanWrite
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override String Name
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override Type DeclaringType
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override Type ReflectedType
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override MethodInfo[] GetAccessors(Boolean nonPublic)
            {
                throw new NotImplementedException();
            }

            public override Object[] GetCustomAttributes(Boolean inherit)
            {
                throw new NotImplementedException();
            }

            public override Object[] GetCustomAttributes(Type attributeType, Boolean inherit)
            {
                throw new NotImplementedException();
            }

            public override MethodInfo GetGetMethod(Boolean nonPublic)
            {
                throw new NotImplementedException();
            }

            public override ParameterInfo[] GetIndexParameters()
            {
                throw new NotImplementedException();
            }

            public override MethodInfo GetSetMethod(Boolean nonPublic)
            {
                throw new NotImplementedException();
            }

            public override Object GetValue(Object obj, BindingFlags invokeAttr, Binder binder, Object[] index, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public override Boolean IsDefined(Type attributeType, Boolean inherit)
            {
                throw new NotImplementedException();
            }

            public override void SetValue(Object obj, Object value, BindingFlags invokeAttr, Binder binder, Object[] index, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private class IsSupportedDataTypeTestItem
        {
            public ParameterObjectAttribute Parameter { get; set; }

            public PropertyInfo Property { get; set; }

            public Boolean Expected { get; set; }

            public override String ToString()
            {
                return $"{this.Parameter?.ToString() ?? "<null>"}, {this.Property?.PropertyType?.ToString() ?? "<null>"}, {this.Expected}";
            }
        }

        private class UnsupportedParameterAttribute : ParameterObjectAttribute { }

        public static Object[] IsSupportedDataTypeTestItems = new IsSupportedDataTypeTestItem[]
        {
            new IsSupportedDataTypeTestItem()
            {
                Parameter = null,
                Property = new TestPropertyInfo(null),
                Expected = false,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new SwitchParameterAttribute(),
                Property = new TestPropertyInfo(typeof(String)),
                Expected = false,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new SwitchParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Boolean)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new SwitchParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Boolean?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Object)),
                Expected = false,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(String)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(SByte)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(SByte?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Byte)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Byte?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Char)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Char?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Int16)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Int16?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(UInt16)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(UInt16?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Int32)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Int32?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(UInt32)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(UInt32?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Int64)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Int64?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(UInt64)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(UInt64?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(DateTime)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(DateTime?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Decimal)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Decimal?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Double)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Double?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Single)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new OptionParameterAttribute(),
                Property = new TestPropertyInfo(typeof(Single?)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new VerbalParameterAttribute(),
                Property = new TestPropertyInfo(typeof(String)),
                Expected = false,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new VerbalParameterAttribute(),
                Property = new TestPropertyInfo(typeof(List<String>)),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new VerbalParameterAttribute(),
                Property = new TestPropertyInfo(typeof(String[])),
                Expected = true,
            },
            new IsSupportedDataTypeTestItem()
            {
                Parameter = new UnsupportedParameterAttribute(),
                Property = new TestPropertyInfo(null),
                Expected = false,
            }
        };

        private abstract class IsConverterSupportedTestClassBase
        {
            public abstract Object TestProperty { get; set; }
        }

        private class IsConverterSupportedTestClassNoAttributes : IsConverterSupportedTestClassBase
        {
            public override Object TestProperty { get; set; }
        }

        private class IsConverterSupportedTestClassNoConverterAttribute : IsConverterSupportedTestClassBase
        {
            [OptionParameter]
            public override Object TestProperty { get; set; }
        }

        private class IsConverterSupportedTestClassConverterAttributeNullInstance : IsConverterSupportedTestClassBase
        {
            [OptionParameter]
            [CustomConverter(null)]
            public override Object TestProperty { get; set; }
        }

        private class IsConverterSupportedTestClassConverterWrongDataType : IsConverterSupportedTestClassBase
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverterWrongDataType))]
            public override Object TestProperty { get; set; }
        }

        private class IsConverterSupportedTestClassConverterWrongInterfaceType : IsConverterSupportedTestClassBase
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverterWrongInterfaceType))]
            public override Object TestProperty { get; set; }
        }

        private class IsConverterSupportedTestClassConverterNoGenericInterface : IsConverterSupportedTestClassBase
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverterNoGenericInterface))]
            public override Object TestProperty { get; set; }
        }

        private class IsConverterSupportedTestClassConverter : IsConverterSupportedTestClassBase
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverter))]
            public override Object TestProperty { get; set; }
        }

        private class IsConverterSupportedTestItem
        {
            public IsConverterSupportedTestItem(IsConverterSupportedTestClassBase candidate, Boolean expected)
            {
                this.TypeName = candidate.GetType().Name;
                this.Attribute = new OptionParameterAttribute();
                this.Property = candidate.GetType().GetRuntimeProperty(nameof(IsConverterSupportedTestClassBase.TestProperty));
                this.Expected = expected;
            }

            public String TypeName { get; private set; }

            public OptionParameterAttribute Attribute { get; private set; }

            public PropertyInfo Property { get; private set; }

            public Boolean Expected { get; private set; }

            public override String ToString()
            {
                return $"{this.TypeName ?? "<null>"}, {this.Property?.ToString() ?? "<null>"}, {this.Expected}";
            }
        }

        public static Object[] IsConverterSupportedTestItems = new IsConverterSupportedTestItem[]
        {
            new IsConverterSupportedTestItem(new IsConverterSupportedTestClassNoAttributes(), false),
            new IsConverterSupportedTestItem(new IsConverterSupportedTestClassNoConverterAttribute(), false),
            new IsConverterSupportedTestItem(new IsConverterSupportedTestClassConverterAttributeNullInstance(), false),
            new IsConverterSupportedTestItem(new IsConverterSupportedTestClassConverterWrongDataType(), false),
            new IsConverterSupportedTestItem(new IsConverterSupportedTestClassConverterWrongInterfaceType(), false),
            new IsConverterSupportedTestItem(new IsConverterSupportedTestClassConverterNoGenericInterface(), false),
            new IsConverterSupportedTestItem(new IsConverterSupportedTestClassConverter(), true)
        };

        private abstract class ToParameterLabelTestClassBase
        {
            public abstract Object TestProperty { get; set; }
        }

        private class ToParameterLabelTestClassSolidLabel : ToParameterLabelTestClassBase
        {
            [SwitchParameter(SolidLabel = "solid-label")]
            public override Object TestProperty { get; set; }
        }

        private class ToParameterLabelTestClassBriefLabel : ToParameterLabelTestClassBase
        {
            [SwitchParameter(BriefLabel = "brief-label")]
            public override Object TestProperty { get; set; }
        }

        private class ToParameterLabelTestClassNoLabel : ToParameterLabelTestClassBase
        {
            [SwitchParameter]
            public override Object TestProperty { get; set; }
        }

        private class ToParameterLabelTestItem
        {
            public ToParameterLabelTestItem(ToParameterLabelTestClassBase candidate, String expected)
            {
                PropertyInfo property = candidate.GetType().GetRuntimeProperty(nameof(ToParameterLabelTestClassBase.TestProperty));
                ParameterObjectAttribute attribute = (ParameterObjectAttribute)property.GetCustomAttribute(typeof(SwitchParameterAttribute));

                this.Setting = new ArgumentProcessorSetting(property, attribute);
                this.Expected = expected;
            }

            public ArgumentProcessorSetting Setting { get; private set; }

            public String Expected { get; private set; }

            public override String ToString()
            {
                return $"{this.Setting?.ToString() ?? "<null>"}, {this.Expected ?? "<null>"}";
            }
        }

        public static Object[] ToParameterLabelTestItems = new ToParameterLabelTestItem[]
        {
            new ToParameterLabelTestItem(new ToParameterLabelTestClassSolidLabel(), "--solid-label"),
            new ToParameterLabelTestItem(new ToParameterLabelTestClassBriefLabel(), "-brief-label"),
            new ToParameterLabelTestItem(new ToParameterLabelTestClassNoLabel(), nameof(ToParameterLabelTestClassBase.TestProperty))
        };

        #endregion
    }
}
