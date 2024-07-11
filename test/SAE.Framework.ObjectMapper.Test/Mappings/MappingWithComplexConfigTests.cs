using SAE.Framework.ObjectMapper;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.Framework.ObjectMapper.Test.Mappings
{
    public class MappingWithComplexConfigTests : MappingBase
    {
        [Fact]
        public void Map_ComplexBind_Success()
        {
            _tinyMapper.Bind<PersonDto, Person>(
                config =>
                {
                    config.Bind(source => source.Address.Street, target => target.Street);
                    config.Bind(source => source.Address.Phone, target => target.Phone);
                }
            );

            var dto = new PersonDto
            {
                Address = new AddressDto
                {
                    Street = "Street",
                    Phone = "123123"
                },
                Code = "Code",
                Identity = 1,
                Name = "Alex"
            };

            var person = _tinyMapper.Map<Person>(dto);

            XAssert.Equal(dto.Identity, person.Identity);
            XAssert.Equal(dto.Code, person.Code);
            XAssert.Equal(dto.Name, person.Name);
            XAssert.Equal(dto.Address.Street, person.Street);
            XAssert.Equal(dto.Address.Phone, person.Phone);
        }

        [Fact]
        public void Map_ComplexBind_Success1()
        {
            _tinyMapper.Bind<PersonDto, Person>(
                config =>
                {
                    config.Bind(source => source.Address.Street, target => target.Street);
                    config.Bind(source => source.Address.Phone, target => target.Phone);
                }
            );

            var dto = new PersonDto
            {
                Address = new AddressDto
                {
                    Street = "Street",
                    Phone = "123123"
                },
                Code = "Code",
                Identity = 1,
                Name = "Alex"
            };

            var person = (Person)_tinyMapper.Map(typeof(PersonDto), typeof(Person), dto);

            XAssert.Equal(dto.Identity, person.Identity);
            XAssert.Equal(dto.Code, person.Code);
            XAssert.Equal(dto.Name, person.Name);
            XAssert.Equal(dto.Address.Street, person.Street);
            XAssert.Equal(dto.Address.Phone, person.Phone);
        }


        public class Person
        {
            public int Identity { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Street { get; set; }
            public string Phone { get; set; }
        }

        public class PersonDto
        {
            public int Identity { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public AddressDto Address { get; set; }
        }

        public class AddressDto
        {
            public string Street { get; set; }
            public string Phone { get; set; }
        }
    }
}
