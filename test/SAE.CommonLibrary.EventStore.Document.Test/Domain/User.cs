using SAE.CommonLibrary.EventStore.Document.Memory.Test.Event;
using System;
using System.Collections.Generic;

namespace SAE.CommonLibrary.EventStore.Document.Memory.Test.Domain
{
    public class User : Document
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }

        public int Sex { get; set; }

        public void Create(string loginName,string password)
        {
            if (string.IsNullOrWhiteSpace(loginName))
                throw new NullReferenceException($"{nameof(loginName)}无效");

            if (string.IsNullOrWhiteSpace(password))
                throw new NullReferenceException($"{nameof(password)}无效");

            this.Apply(new CreateEvent
            {
                LoginName = loginName,
                Password = password,
                Id = Utils.GenerateId()
            });
            
            this.SetProperty("pjb", 0);
        }

        public void SetProperty(string name, int sex)
        {
            this.Apply(new SetBasicPropertyEvent
            {
                Name = name,
                Sex = sex
            });
        }

        public void ChangePassword(string originalPassword,string password)
        {
            if (this.Password != originalPassword)
            {
                throw new Exception("密码不正确");
            }
            this.Apply(new ChangePasswordEvent
            {
                Password = password
            });
        }

        protected override string GetIdentity()
        {
            return this.Id;
        }
    }
}
