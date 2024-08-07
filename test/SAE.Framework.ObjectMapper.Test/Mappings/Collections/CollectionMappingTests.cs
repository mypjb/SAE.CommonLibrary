﻿using System.Collections.Generic;
using System.Collections;
using System.Linq;
using SAE.Framework.ObjectMapper;
using SAE.Framework.ObjectMapper.Reflection;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.Framework.ObjectMapper.Test.Mappings.Collections
{
    public sealed class CollectionMappingTests : MappingBase
    {
        [Fact]
        public void Map_Collections_Success()
        {
            _tinyMapper.Bind<Source1, Target1>();

            var source = new Source1
            {
                Items = new List<Item1>
                {
                    new Item1
                    {
                        Int = 1,
                        String = "2",
                        List = new List<int> { 1, 2, 3 },
                        Bools = new[] { true, false }
                    },
                    new Item1
                    {
                        Int = 2,
                        String = "3",
                        List = new List<int> { 2, 3 },
                        Bools = new[] { false, false }
                    }
                }
            };
//            DynamicAssemblyBuilder.Get().Save();
            var actual = _tinyMapper.Map<Target1>(source);

            XAssert.Equal(source.Items.Count, actual.Items.Count);
            XAssert.Equal(source.Items1, actual.Items1);

            for (int i = 0; i < source.Items.Count; i++)
            {
                Item1 expectedItem = source.Items[i];
                Item2 actualItem = actual.Items[i];

                XAssert.Equal(expectedItem.Bools, actualItem.Bools);
                XAssert.Equal(expectedItem.Int, actualItem.Int);
                XAssert.Equal(expectedItem.List, actualItem.List);
                XAssert.Equal(expectedItem.String, actualItem.String);
            }
        }

        [Fact]
        public void Map_DifferentCollections_Success()
        {
            _tinyMapper.Bind<Person, PersonDto>();

            var source = new Person
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        Int = 1,
                        String = "2"
                    }
                }
            };

            var actual = _tinyMapper.Map<PersonDto>(source);

            XAssert.Equal(source.Contacts.Count, actual.Contacts.Count);
            for (int i = 0; i < source.Contacts.Count; i++)
            {
                Contact expectedItem = source.Contacts[i];
                ContactDto actualItem = actual.Contacts[i];

                XAssert.Equal(expectedItem.Int, actualItem.Int);
                XAssert.Equal(expectedItem.String, actualItem.String);
            }
        }

        [Fact]
        public void Map_InterfaceToCollection_Success()
        {
            _tinyMapper.Bind<Source3, Target3>();

            var source = new Source3
            {
                List = new List<int> { 1, 2, 3 }
            };

            var actual = _tinyMapper.Map<Target3>(source);
            XAssert.Equal(source.List, actual.List);
        }

        [Fact]
        public void Map_NullCollection_Success()
        {
            var source = new Source2
            {
                Int = 1
            };

            _tinyMapper.Bind<Source2, Target2>();

            var actual = _tinyMapper.Map<Target2>(source);

            XAssert.Equal(source.Ints, actual.Ints);
            XAssert.Equal(source.Int, actual.Int);
        }

        [Fact]
        public void Map_IEnumerable_T_Success()
        {
            var target = new PersonComplex
            {
                Emails = new[] {"none1@none.com", "none2@none.com"},
                FirstName = "John",
                LastName = "Doe"
            };

            _tinyMapper.Bind<PersonComplex, PersonComplexTarget>();

            var actual = _tinyMapper.Map<PersonComplexTarget>(target);

            XAssert.Equal(target.FirstName, actual.FirstName);
            XAssert.Equal(target.LastName, actual.LastName);
            XAssert.Equal(target.Emails, actual.Emails);
        }

        [Fact]
        public void Map_IEnumerable_Success()
        {
            var target = new PersonComplex
            {
                Emails = new[] { "none1@none.com", "none2@none.com" },
                FirstName = "John",
                LastName = "Doe"
            };

            _tinyMapper.Bind<PersonComplex, PersonComplexTarget2>();

            var actual = _tinyMapper.Map<PersonComplexTarget2>(target);

            XAssert.Equal(target.FirstName, actual.FirstName);
            XAssert.Equal(target.LastName, actual.LastName);
            XAssert.Equal(target.Emails, actual.Emails.Cast<string>());
        }

        [Fact]
        public void Map_To_IEnumerable_T_Success()
        {
            var target = new PersonComplex3
            {
                Emails = new ArrayList(new [] { "none1@none.com", "none2@none.com" }),
                FirstName = "John",
                LastName = "Doe"
            };

            _tinyMapper.Bind<PersonComplex3, PersonComplexTarget3>();

            var actual = _tinyMapper.Map<PersonComplexTarget3>(target);

            XAssert.Equal(target.FirstName, actual.FirstName);
            XAssert.Equal(target.LastName, actual.LastName);
            XAssert.Equal(target.Emails.Cast<string>(), actual.Emails);
        }

        [Fact]
        public void Map_To_IEnumerable_Success()
        {
            var target = new PersonComplex
            {
                Emails = new[] { "none1@none.com", "none2@none.com" },
                FirstName = "John",
                LastName = "Doe"
            };

            _tinyMapper.Bind<PersonComplex, PersonComplexTarget4>();

            var actual = _tinyMapper.Map<PersonComplexTarget4>(target);

            XAssert.Equal(target.FirstName, actual.FirstName);
            XAssert.Equal(target.LastName, actual.LastName);
            XAssert.Equal(target.Emails, actual.Emails.Cast<string>());
        }

        public class Contact
        {
            public int Int { get; set; }
            public string String { get; set; }
        }

        public class ContactDto
        {
            public int Int { get; set; }
            public string String { get; set; }
        }

        public sealed class Item1
        {
            public bool[] Bools { get; set; }
            public int Int { get; set; }
            public List<int> List { get; set; }
            public string String { get; set; }
        }

        public sealed class Item2
        {
            public bool[] Bools { get; set; }
            public int Int { get; set; }
            public List<int> List { get; set; }
            public string String { get; set; }
        }

        public class Person
        {
            public List<Contact> Contacts { get; set; }
        }

        public class PersonDto
        {
            public List<ContactDto> Contacts { get; set; }
        }

        public class PersonComplex
        {
            public IEnumerable<string> Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplexTarget
        {
            public IList<string> Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplexTarget4
        {
            public IEnumerable Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplexTarget2
        {
            public ArrayList Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplex3
        {
            public ArrayList Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class PersonComplexTarget3
        {
            public IEnumerable<string> Emails { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Source1
        {
            public IList<Item1> Items { get; set; }
            public List<Item1> Items1 { get; set; }
        }

        public class Source2
        {
            public int Int { get; set; }
            public List<int> Ints { get; set; }
        }

        public class Source3
        {
            public IReadOnlyList<int> List { get; set; }
        }

        public class Target1
        {
            public List<Item2> Items { get; set; }
            public List<Item1> Items1 { get; set; }
        }

        public class Target2
        {
            public int Int { get; set; }
            public List<int> Ints { get; set; }
        }

        public class Target3
        {
            public List<int> List { get; set; }
        }
    }
}
