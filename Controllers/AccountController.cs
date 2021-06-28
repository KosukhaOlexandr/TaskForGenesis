using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskForGenesis.BiewModel;
using Microsoft.AspNetCore.Identity;
using TaskForGenesis.Models;

namespace TaskForGenesis.Controllers
{
    [Route("/user")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountsController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [HttpPost("create")]
        public async Task<ActionResult<RegisterViewModel>> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                //adding a user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //creating cookies
                    await _signInManager.SignInAsync(user, false);
                    return model;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return NoContent();
        }

        [HttpPost("login")]
        
        public async Task<ActionResult<string>> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                   
                    return "Sign In Succeeded";
                }
                else
                {
                    if (result.IsLockedOut)
                    {
                        return "Lockout";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong login or(and) password");
                        return "Wrong login or(and) password";
                    }
                }
            }
            return "E-mail is not valid (maybe you made a typo)";
        }
        [HttpPost("logout")]
        public async Task<ActionResult<string>> Logout()
        {
            // deleting cookies
            await _signInManager.SignOutAsync();
            return "Logout succeded";
        }
    }
}
