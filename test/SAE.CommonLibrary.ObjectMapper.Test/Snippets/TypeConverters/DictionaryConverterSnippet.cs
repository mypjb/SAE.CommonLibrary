#if !COREFX
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using SAE.CommonLibrary.ObjectMapper;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.CommonLibrary.ObjectMapper.Test.Snippets.TypeConverters
{
    public sealed class DictionaryConverterSnippet : MappingBase
    {

        public DictionaryConverterSnippet()
        {
            var converterAttribute = new TypeConverterAttribute(typeof(DictionaryTypeConverter<string, string>));
            TypeDescriptor.AddAttributes(typeof(Dictionary<string, string>), converterAttribute);
        }

        [Fact]
        public void Converter()
        {
            _tinyMapper.Bind<SourceClass, TargetClass>();

            var source = new SourceClass
            {
                Dictionary = new Dictionary<string, string> { { "key", "Value" } }
            };

            var target = _tinyMapper.Map<TargetClass>(source);

            XAssert.Equal(source.Dictionary, target.Dictionary);
        }

        public class SourceClass
        {
            public Dictionary<string, string> Dictionary { get; set; }
        }

        public class TargetClass
        {
            public Dictionary<string, string> Dictionary { get; set; }
        }

        private sealed class DictionaryTypeConverter<TKey, TValue> : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(Dictionary<TKey, TValue>);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var concreteValue = (Dictionary<TKey, TValue>)value;
                var result = new Dictionary<TKey, TValue>();

                foreach (KeyValuePair<TKey, TValue> pair in concreteValue)
                {
                    result[pair.Key] = pair.Value;
                }

                return result;
            }
        }
    }
}
#endif