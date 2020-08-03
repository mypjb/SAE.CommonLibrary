
using System;
using System.ComponentModel;
using System.Globalization;
using SAE.CommonLibrary.ObjectMapper;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.CommonLibrary.ObjectMapper.Test.Mappings.TypeConverters
{
    public sealed class ConvertibleTypeMappingTests : MappingBase
    {
        public ConvertibleTypeMappingTests()
        {
            TypeDescriptor.AddAttributes(typeof(Source1), new TypeConverterAttribute(typeof(SourceClassConverter)));
        }

        [Fact]
        public void Map_ConvertibleType_Success()
        {
            _tinyMapper.Bind<Source1, Target1>();
            var source = new Source1
            {
                FirstName = "First",
                LastName = "Last"
            };

            var result = _tinyMapper.Map<Target1>(source);

            XAssert.Equal(string.Format("{0} {1}", source.FirstName, source.LastName), result.FullName);
        }

        public class Source1
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public sealed class SourceClassConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(Target1);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                var concreteValue = (Source1)value;
                var result = new Target1
                {
                    FullName = string.Format("{0} {1}", concreteValue.FirstName, concreteValue.LastName)
                };
                return result;
            }
        }

        public class Target1
        {
            public string FullName { get; set; }
        }
    }
}
