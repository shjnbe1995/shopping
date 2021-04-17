using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo_entity.Models
{
    public class AuthenticateRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
