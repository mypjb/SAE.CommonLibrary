using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.EventStore.Document.Memory.Test.Domain;
using SAE.CommonLibrary.EventStore.Document.Test.Dtos;
using SAE.CommonLibrary.MessageQueue;
using SAE.CommonLibrary.Test;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
namespace SAE.CommonLibrary.EventStore.Document.Memory.Test
{
    public class MemoryTest : BaseTest
    {
        private readonly IDocumentStore _documentStore;
        private readonly IStorage _storage;
        public const string Password = "111111";
        public MemoryTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            this._documentStore = this._serviceProvider.GetService<IDocumentStore>();

            this._storage = this._serviceProvider.GetService<IStorage>();
        }

        protected override void ConfigureServicesBefore(IServiceCollection services)
        {
            services.AddDataPersistenceService()
                    .AddMemoryMessageQueue();
            base.ConfigureServicesBefore(services);
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryDocument();
            
            base.ConfigureServices(services);
        }

        [Fact]
        public virtual async Task<User> Register()
        {
            var user = new User();
            user.Create(this.GetRandom(), Password);
            await _documentStore.SaveAsync(user);
            this.WriteLine(user);
            var newUser = await _documentStore.FindAsync<User>(user.Id.ToIdentity());
            Xunit.Assert.NotNull(newUser);
            Xunit.Assert.Equal(user.Id, newUser.Id);
            Xunit.Assert.Equal(user.LoginName, newUser.LoginName);
            Xunit.Assert.Equal(user.Name, newUser.Name);
            Xunit.Assert.Equal(user.Password, newUser.Password);
            Xunit.Assert.Equal(user.Sex, newUser.Sex);
            this.WriteLine(newUser);
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
            this.WriteLine(newUser);
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
            this.WriteLine(newUser);
        }

        [Fact]
        public virtual async Task Remove()
        {
            var user = await this.Register();
            await this._documentStore.DeleteAsync(user);
            Xunit.Assert.Null(await _documentStore.FindAsync<User>(user.Id.ToIdentity()));
        }
        [Fact]
        public virtual async Task Query()
        {
            var user = await this.Register();
            var dto= this._storage.AsQueryable<UserDto>()
                                  .Where(s => s.Sex == user.Sex)
                                  .FirstOrDefault();

            Xunit.Assert.NotNull(dto);
        }

    }
}
