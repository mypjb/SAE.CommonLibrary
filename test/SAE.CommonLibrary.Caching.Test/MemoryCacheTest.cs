using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Cache.Test
{
    public class MemoryCacheTest : BaseTest
    {
        private readonly IDistributedCache _distributedCache;

        public MemoryCacheTest(ITestOutputHelper output) : base(output)
        {
            this._distributedCache = this._serviceProvider.GetService<IDistributedCache>();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSAEMemoryDistributedCache();
        }

        [Fact]
        public async Task<Student> Add()
        {
            var student = new Student();
            Xunit.Assert.True(await _distributedCache.AddAsync(student.Name, student));
            var value = await this._distributedCache.GetAsync<Student>(student.Name);
            Xunit.Assert.NotNull(value);
            Xunit.Assert.Equal(student.Age, value.Age);
            Xunit.Assert.Equal(student.CreateTime, value.CreateTime);
            Xunit.Assert.Equal(student.Name, value.Name);
            Xunit.Assert.Equal(student.Sex, value.Sex);
            return student;
        }
        [Fact]
        public async Task<IEnumerable<string>> Adds()
        {
            Dictionary<string, Student> dic = new Dictionary<string, Student>();

            Enumerable.Range(0, 10)
                      .ForEach(s =>
                      {
                          var student = new Student();
                          dic.Add(student.Name, student);
                      });

            var results = await _distributedCache.AddAsync(dic, CacheTime.OneMinute);
            Xunit.Assert.DoesNotContain(results, s => !s);
            var values = await this._distributedCache.GetAsync<Student>(dic.Select(s => s.Key).ToArray());

            Xunit.Assert.Equal(dic.Count(), values.Count());
            foreach (var value in values)
            {
                var student = dic[value.Name];
                Xunit.Assert.Equal(student.Age, value.Age);
                Xunit.Assert.Equal(student.CreateTime, value.CreateTime);
                Xunit.Assert.Equal(student.Name, value.Name);
                Xunit.Assert.Equal(student.Sex, value.Sex);
            }

            return dic.Select(s => s.Key).ToArray();
        }


        [Fact]
        public async Task Delete()
        {
            var student = await this.Add();

            Xunit.Assert.True(_distributedCache.Delete(student.Name));
            Xunit.Assert.Null(_distributedCache.Get<Student>(student.Name));
        }
        [Fact]
        public async Task DeleteAll()
        {
            var keys = await this.Adds();
            Xunit.Assert.True(keys.Count() > 0);
            await this._distributedCache.DeleteAsync(keys);
            var students = await this._distributedCache.GetAsync<Student>(keys);
            Xunit.Assert.True(students.Count(s => s != null) == 0);
        }

        [Fact]
        public virtual async Task Deleteattern()
        {
            var student = await this.Add();

            Xunit.Assert.True(await _distributedCache.DeletePatternAsync($"^{student.Name[..(student.Name.Length / 2)]}"));
            Xunit.Assert.Null(_distributedCache.Get<Student>(student.Name));
        }

        [Fact]
        public async Task Clear()
        {
            await this.Adds();
            await _distributedCache.ClearAsync();
            var keys = await _distributedCache.GetKeysAsync();
            Xunit.Assert.True(keys.Count() == 0);
        }

    }
}
