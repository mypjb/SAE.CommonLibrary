using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Document.Test.Dtos
{
    public class UserDto
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public int Sex { get; set; }
    }
}
