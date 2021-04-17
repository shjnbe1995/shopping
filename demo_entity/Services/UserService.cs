using demo_entity.Helpers;
using demo_entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace demo_entity.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<ApplicationUser> GetById(string id);
    }
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            //var a = new Guid();
            //chú ý, async func đi với async, nên khi func nào dùng cái này cũng thành async func
            var b = model.UserName;
            var user = await _userManager.FindByNameAsync(b);
            var userRole = await _userManager.GetRolesAsync(user);
            
            var token = GenerateJwtToken(user, userRole);
            var response = new AuthenticateResponse(user, token);
            return response;
        }

        public Task<ApplicationUser> GetById(string id)
        {
            return _userManager.FindByIdAsync(id);
        }


        private string GenerateJwtToken(ApplicationUser user, IList<string> role)
        {
            // generate token that is valid for 7 days
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("9b3d556f1f1940ba918dfd569b5fa4a5");


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // cái payload
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Claims = new Dictionary<string, object>() {
                    [ClaimTypes.Role] = role, 
                    [ClaimTypes.Email] = user.Email },
                // signature
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}


