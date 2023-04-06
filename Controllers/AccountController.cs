using Intex_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Intex_app.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;

        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Admin()
        {
            var user = _userManager.FindByEmailAsync(this.User.FindFirst(ClaimTypes.Email).Value).Result;
            if (user.Admin == true)
            {
                return View(_userManager.Users);
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Remove(string userToDeleteEmail)
        {
            var user = _userManager.FindByEmailAsync(this.User.FindFirst(ClaimTypes.Email).Value).Result;
            if (user.Admin == true)
            {
                var userToDelete = _userManager.FindByEmailAsync(userToDeleteEmail).Result;
                _userManager.DeleteAsync(userToDelete);
                
                return RedirectToAction("Admin");
            }
            else
            {
                return View();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterBindingModel model)
        {
            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
            var email = user.Email;
            user.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                //if (_userManager.FindByNameAsync(user.UserName).IsCompletedSuccessfully == false)
                //{
                //the user does not already exist
                var result = await _userManager.CreateAsync(user, model.Password);
                if (email != null)
                {
                    var message = new System.Net.Mail.MailMessage();
                    message.To.Add(email);
                    message.Subject = "Verify Account";
                    message.From = new MailAddress("noreply@impetusfactor.com");

                    var emailDomain = model.Email.Substring(model.Email.LastIndexOf('@'));

                    var orgName = "@byu.edu" == emailDomain;
                    if (!orgName){
                        //you can't sign up
                        message.Body = "Sorry this is for @byu.edu email addresses only";
                    }
                    else
                    {
                        message.Body = email + ",\n" + System.Environment.NewLine + "Thank you for registering with Gamous. Click link to verify " + "http://intex-final.huynhatdoan.com/account/VerifyAccount/?userId=" + user.Id;
                    }
                   



                    message.IsBodyHtml = true;
                    var smtp = new SmtpClient("mail.impetusfactor.com", 587)
                    {
                        Credentials = new NetworkCredential("noreply@impetusfactor.com", "blurgRider1165@b"),
                        EnableSsl = true                        
                    };
                    smtp.Send(message);
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("Token", "token");
                }
                //}
            }
            return Ok(Response);
        }
        [Authorize]
        [HttpGet]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> VerifyAccount(String userId)
        {
            var user = _userManager.Users.First(u => u.Id == userId);
            user.EmailConfirmed = true;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                //take them to the token page
                return RedirectToAction("Token", "token");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
