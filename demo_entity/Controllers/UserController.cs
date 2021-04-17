using demo_entity.Helpers;
using demo_entity.Models;
using demo_entity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace demo_entity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private IUserService _userService;

        public UserController(IUserService userService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole()
        {
            var Userrole = new ApplicationRole()
            {
                Name = Helpers.UserRole.User.ToString(),
                Id = Guid.NewGuid()

            };
            var roleUser = await _roleManager.CreateAsync(Userrole);

            if (!roleUser.Succeeded)
                return BadRequest(new { message = "cannot create user " });

            var Adminrole = new ApplicationRole()
            {
                Name = Helpers.UserRole.Admin.ToString(),
                Id = Guid.NewGuid()
            };
            var roleAdmin = await _roleManager.CreateAsync(Adminrole);

            if (!roleAdmin.Succeeded)
                return BadRequest(new { message = "cannot create user " });

            return Content("OK");
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);
            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create(Account model)
        {
            if(model == null || !ModelState.IsValid)
                return Content("NOT OK");

            var user = new ApplicationUser() {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                Email = model.Email
            };

            var createUser = await _userManager.CreateAsync(user,model.Password);
            if (!createUser.Succeeded)
                return Content("NOT OK");

            var result = await _userManager.AddToRoleAsync(user, UserRole.User.ToString());
            if (!result.Succeeded)
                return Content("NOT OK");
            return Content("OK");
        }
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin(Account model)
        {
            if (model == null || !ModelState.IsValid)
                return Content("NOT OK");

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                Email = model.Email
            };


            var createUser = await _userManager.CreateAsync(user, model.Password);
            if (!createUser.Succeeded)
                return Content("NOT OK");

            var result = await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
            if (!result.Succeeded)
                return Content("NOT OK");
            return Content("OK");
        }
        [HttpDelete("delete")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(AuthenticateRequest model)
        {
            if (model == null || !ModelState.IsValid)
                return Content("Not ok");

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return Content("Not in db");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return Content("not OK");

            return Content("OK");
        }
        [HttpPut("update")]
        public async Task<IActionResult> Edit(Account model)
        {
            if (model == null || !ModelState.IsValid)
                return Content("Not ok");

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return new NotFoundObjectResult(new {
                    title = "NotFount",
                    message = "Hahaha",
                    abc ="test"
                });

            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
                return new NotFoundResult();

            return Ok(await _userManager.FindByNameAsync(model.UserName));
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Detail(AuthenticateRequest model)
        {
            if (model == null || !ModelState.IsValid)
                return Content("Not ok");

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return Content("Not in db");
            return Ok(user);
        }
        [HttpPost("list")]
        public IActionResult GetUserList(GetListQuery model)
        {
            var countData = _userManager.Users.Count();
            var totalPage = (int)Math.Ceiling((decimal)(countData / model.PageSize ));
            var a = _userManager.Users.Skip((model.CurrentPage - 1) * model.PageSize).Take(model.PageSize);
            List<ApplicationUser> data = a.ToList();

            bool prevPage = true;
            bool nextPage = true;
            if (model.CurrentPage < 1)
                prevPage = false;
            if (model.CurrentPage > totalPage)
                nextPage = false;

            var pagination = new Pagination<ApplicationUser>(model.CurrentPage, nextPage, prevPage, totalPage, data);
            return Ok(pagination);
        }
    }
}
