using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SAE.Framework.Data;
using SAE.Framework.EventStore.Document.Memory.Test.Domain;
using SAE.Framework.EventStore.Document.Test.Dtos;
using SAE.Framework.MessageQueue;
using SAE.Framework.Test;
using Xunit;
using Xunit.Abstractions;
namespace SAE.Framework.EventStore.Document.Test
{
    public class MemoryTest : BaseTest
    {
        private readonly IDocumentStore _documentStore;
        private readonly IStorage _storage;
        public const string Password = "111111";
        protected int range = new Random().Next(1000, 9999);
        public MemoryTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            this._documentStore = this._serviceProvider.GetService<IDocumentStore>();

            this._storage = this._serviceProvider.GetService<IStorage>();
        }

        protected override void ConfigureServicesBefore(IServiceCollection services)
        {
            services.AddSAEFramework()
                    .AddDataPersistenceService()
                    .AddMemoryMessageQueue();
            base.ConfigureServicesBefore(services);
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSAEFramework()
                    .AddMemoryDocument();

            base.ConfigureServices(services);
        }

        public virtual async Task<User> Register()
        {
            var user = new User();
            user.Create(this.GetRandom(), Password);
            await _documentStore.SaveAsync(user);
            var newUser = await _documentStore.FindAsync<User>(user.Id.ToIdentity());
            if (newUser == null)
            {

            }
            Xunit.Assert.NotNull(newUser);
            Xunit.Assert.Equal(user.Id, newUser.Id);
            Xunit.Assert.Equal(user.LoginName, newUser.LoginName);
            Xunit.Assert.Equal(user.Name, newUser.Name);
            Xunit.Assert.Equal(user.Password, newUser.Password);
            Xunit.Assert.Equal(user.Sex, newUser.Sex);
            return user;
        }

        [Fact]
        public virtual async Task ChangePassword()
        {
            var user = await this.Register();
            user = await _documentStore.FindAsync<User>(user.Id.ToIdentity());
            user.ChangePassword(Password, Password + "1");
            _documentStore.Save(user);
            var newUser = await _documentStore.FindAsync<User>(user.Id.ToIdentity());
            Xunit.Assert.NotNull(newUser);
            Xunit.Assert.Equal(user.Id, newUser.Id);
            Xunit.Assert.Equal(user.LoginName, newUser.LoginName);
            Xunit.Assert.Equal(user.Name, newUser.Name);
            Xunit.Assert.Equal(user.Password, newUser.Password);
            Xunit.Assert.Equal(user.Sex, newUser.Sex);
            //Xunit.Assert.NotEqual(user, newUser);

        }

        [Fact]
        public virtual async Task ChangeProperty()
        {
            var user = await this.Register();

            user = await _documentStore.FindAsync<User>(user.Id.ToIdentity());
            user.SetProperty(this.GetRandom(), Math.Abs(user.Sex - 1));
            _documentStore.Save(user);
            var newUser = await _documentStore.FindAsync<User>(user.Id.ToIdentity());
            Xunit.Assert.NotNull(newUser);
            Xunit.Assert.Equal(user.Id, newUser.Id);
            Xunit.Assert.Equal(user.LoginName, newUser.LoginName);
            Xunit.Assert.Equal(user.Name, newUser.Name);
            Xunit.Assert.Equal(user.Password, newUser.Password);
            Xunit.Assert.Equal(user.Sex, newUser.Sex);
            //Xunit.Assert.NotEqual(user, newUser);

        }

        [Fact]
        public virtual async Task Delete()
        {
            var user = await this.Register();
            await this._documentStore.DeleteAsync(user);
            Xunit.Assert.Null(await _documentStore.FindAsync<User>(user.Id.ToIdentity()));
        }
        [Fact]
        public virtual async Task Query()
        {
            var user = await this.Register();
            var dto = this._storage.AsQueryable<UserDto>()
                                  .Where(s => s.Sex == user.Sex)
                                  .FirstOrDefault();

            Xunit.Assert.NotNull(dto);
        }
        [Fact]
        public virtual void Batch()
        {
            Enumerable.Range(0, range)
                                  .AsParallel()
                                  .ForAll(s =>
                                  {
                                      this.Register().GetAwaiter().GetResult();
                                      this.Delete().GetAwaiter().GetResult();
                                      this.ChangePassword().GetAwaiter().GetResult();
                                      this.ChangeProperty().GetAwaiter().GetResult();
                                  });

            if (this.range > 999)
            {
                var count = this._storage.AsQueryable<UserDto>().Count();

                Assert.Equal(range * 3, count);
            }


        }
    }
}
