using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Assert = Xunit.Assert;
using System.Threading.Tasks;
using System.Collections;

namespace SAE.CommonLibrary.Storage.MongoDB.Test
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

        /// <summary>
        /// 添加数据至Mongodb中
        /// </summary>
        [Fact]
        public async Task<ClassGrade> Add()
        {
            var classGrade = new ClassGrade();
            classGrade.Id = this.GetRandom();
            //此处设置了默认Id为10
            await _storage.AddAsync(classGrade);
            var grade = this._storage.AsQueryable<ClassGrade>().FirstOrDefault(s => s.Id == classGrade.Id);
            Xunit.Assert.True(_storage.AsQueryable<ClassGrade>()
                                .Count(s => s.Id == classGrade.Id) == 1);
            Xunit.Assert.NotNull(grade);
            return grade;
        }

        /// <summary>
        /// 更新
        /// </summary>
        [Fact]
        public async Task Update()
        {
            var classGrade = await this.Add();
            //此处设置了默认Id为10
            classGrade.Students = new List<Student>
            {
                new Student
                {
                     Age=this.GetRandom().GetHashCode(),
                     Name=this.GetRandom(),
                     Sex=this.GetRandom().GetHashCode()%2==0? Sex.Nav: Sex.Man
                }
            };
            await _storage.UpdateAsync(classGrade);

            var @class = _storage.AsQueryable<ClassGrade>()
                                 .First(s => s.Id == classGrade.Id);

            Xunit.Assert.True(@class.Students.Count() == 1 &&
                    @class.Students.First().Name == classGrade.Students.First().Name);
        }

        [Theory]
        [InlineData("大禹")]
        public async Task Query(string name)
        {
            await this.Add();

            var count = _storage.AsQueryable<ClassGrade>()
                                .Where(s => s.Name == name)
                                .Count();

            Xunit.Assert.True(count > 0);

            _output.WriteLine(count.ToString());
        }

        [Fact]
        public async Task Remove()
        {
            var classGrade = await this.Add();
            //移除   注：暂不支持Clear
            await _storage.RemoveAsync(classGrade);
            Xunit.Assert.True(_storage.AsQueryable<ClassGrade>()
                                .Count(s => s.Id == classGrade.Id) == 0);
        }
    }

}
