using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Test;
using StackExchange.Redis;
using Xunit;
using Xunit.Abstractions;
using Role = SAE.CommonLibrary.Test.Role;

namespace SAE.CommonLibrary.Data.Test
{
    public class MongoDBStorageTest : BaseTest
    {
        private readonly IStorage _storage;

        public MongoDBStorageTest(ITestOutputHelper output) : base(output)
        {
            this._storage = this._serviceProvider.GetService<IStorage>();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoDB();
            base.ConfigureServices(services);
        }

        [Fact]
        public async Task AssociatedQuery()
        {
            var student = new Student();
            var role = new Role();
            var studenRole = new StudenRole(student.Id, role.Id);
            await this._storage.SaveAsync(student);
            await this._storage.SaveAsync(role);
            await this._storage.SaveAsync(studenRole);
            var query = from s in this._storage.AsQueryable<Student>()
                        join sr in this._storage.AsQueryable<StudenRole>()
                        on s.Id equals sr.StudentId
                        where sr.RoleId == role.Id
                        select s;
            var students = query.ToArray();

            this.WriteLine(students);

        }

        public async Task<ClassGrade> Add()
        {
            var classGrade = new ClassGrade();
            classGrade.Id = this.GetRandom();
            await _storage.SaveAsync(classGrade);
            var grade = this._storage.AsQueryable<ClassGrade>().FirstOrDefault(s => s.Id == classGrade.Id);
            Xunit.Assert.True(_storage.AsQueryable<ClassGrade>()
                                .Count(s => s.Id == classGrade.Id) == 1);
            Xunit.Assert.NotNull(grade);
            return grade;
        }

        [Fact]
        public async Task Update()
        {
            var classGrade = await this.Add();
            classGrade.Students = new List<Student>
            {
                new Student
                {
                     Age=this.GetRandom().GetHashCode(),
                     Name=this.GetRandom(),
                     Sex=this.GetRandom().GetHashCode()%2==0? Sex.Nav: Sex.Man
                }
            };
            await _storage.SaveAsync(classGrade);

            var @class = _storage.AsQueryable<ClassGrade>()
                                 .First(s => s.Id == classGrade.Id);

            Xunit.Assert.True(@class.Students.Count() == 1 &&
                    @class.Students.First().Name == classGrade.Students.First().Name);
        }

        [Fact]
        public async Task Query()
        {
            var classGrade = await this.Add();

            var count = _storage.AsQueryable<ClassGrade>()
                                .Where(s => s.Name == classGrade.Name)
                                .Count();

            Xunit.Assert.True(count > 0);

            _output.WriteLine(count.ToString());
        }

        [Fact]
        public async Task Remove()
        {
            var classGrade = await this.Add();
            await _storage.DeleteAsync(classGrade);
            Xunit.Assert.True(_storage.AsQueryable<ClassGrade>()
                                .Count(s => s.Id == classGrade.Id) == 0);
        }
    }

}
