using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo_entity.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(ApplicationUser user, string token)
        {
            Id = user.Id;
            Token = token;
        }
    }
}
