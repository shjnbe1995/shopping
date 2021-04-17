using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo_entity.Models
{
    public class ApplicationRole:IdentityRole<Guid>
    {
    }
}
