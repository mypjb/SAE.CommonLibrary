using SAE.Framework.ObjectMapper.Bindings;
using System;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.Framework.ObjectMapper.Test.Mappings.Attributes
{
    public sealed class MappingWithAttributesTests: MappingBase
    {
        [Fact]
        public void Map_WithAttributes_Success()
        {
            _tinyMapper.Bind<SourceWithIgnore, TargetWithIgnore>();

            SourceWithIgnore source = CreateSourceWithIgnore();
            var actual = _tinyMapper.Map<TargetWithIgnore>(source);

            XAssert.Equal(default(DateTime), actual.DateTime);
            XAssert.Equal(source.FirstName, actual.FirstName);
            XAssert.Equal(source.LegacyString, actual.LatestString);
            XAssert.Equal(default(string), actual.ProtectedString);
        }

        private static SourceWithIgnore CreateSourceWithIgnore()
        {
            return new SourceWithIgnore
            {
                FirstName = "John",
                DateTime = DateTime.Now,
                LegacyString = "LegacyString",
                SealedString = "SealedString"
            };
        }

        [Fact]
        public void Map_WithTargetSubset_Success()
        {
            _tinyMapper.Bind<SourceWithSubset, TargetSubset1>();
            _tinyMapper.Bind<SourceWithSubset, TargetSubset2>();

            SourceWithSubset source = CreateSourceWithSubset();
            var target1 = _tinyMapper.Map<TargetSubset1>(source);

            XAssert.Equal(default(DateTime), target1.DateTime);
            XAssert.Equal(source.FirstName, target1.FirstName);
            XAssert.Equal(source.SourceForTarget1and2, target1.LatestString);
            XAssert.Equal(source.SourceForTarget1, target1.SourceString);
            XAssert.NotEqual(source.SourceForTarget2, target1.SourceString);

            var target2 = _tinyMapper.Map<TargetSubset2>(source);

            XAssert.Equal(source.DateTime, target2.DateTime);
            XAssert.Equal(source.FirstName, target2.FirstName);
            XAssert.Equal(source.SourceForTarget1and2, target2.LatestString);
            XAssert.NotEqual(source.SourceForTarget1, target2.SourceString);
            XAssert.Equal(source.SourceForTarget2, target2.SourceString);
        }

        private static SourceWithSubset CreateSourceWithSubset()
        {
            return new SourceWithSubset
            {
                FirstName = "John",
                DateTime = DateTime.Now,
                SourceForTarget1and2 = "SourceForTarget1and2",
                SourceForTarget1 = "SourceForTarget1",
                SourceForTarget2 = "SourceForTarget2"
            };
        }

        public class SourceWithIgnore
        {
            [Ignore]
            public DateTime DateTime { get; set; }

            public string FirstName { get; set; }

            [Bind(nameof(TargetWithIgnore.LatestString))]
            public string LegacyString { get; set; }

            public string SealedString { get; set; }
        }


        public class TargetWithIgnore
        {
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LatestString { get; set; }
            public string ProtectedString { get; set; }
        }


        public class SourceWithSubset
        {
            /// <summary>
            /// Ignore map for <see cref="TargetSubset1"/>, but not for <see cref="TargetSubset2"/>
            /// </summary>
            [Ignore(typeof(TargetSubset1))]
            public DateTime DateTime { get; set; }

            public string FirstName { get; set; }

            [Bind("LatestString")]
            public string SourceForTarget1and2 { get; set; }

            [Bind(nameof(TargetSubset1.SourceString), typeof(TargetSubset1))]
            public string SourceForTarget1 { get; set; }

            [Bind(nameof(TargetSubset2.SourceString), typeof(TargetSubset2))]
            public string SourceForTarget2 { get; set; }
        }


        public class TargetSubset1
        {
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LatestString { get; set; }
            public string SourceString { get; set; }
        }


        public class TargetSubset2
        {
            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LatestString { get; set; }
            public string SourceString { get; set; }
        }
    }
}
