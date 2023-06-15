using BusinessLayer.Interface;
using BussinesLayer.Services;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace FundoNoteApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(Register))]
        public IActionResult Register(UserRegistration userRegistration)
        {

            try
            {
                var result = userBL.Register(userRegistration);
                if (result != null)
                {
                    return this.Ok(new { success = true, msg = "registration sucessfull", data = result }); //SSMD form
                }
                else
                {
                    return this.BadRequest(new { success = false, msg = "registration Unsucessfull" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public IActionResult Login(UserLogin userlogin)
        {
            try
            {
                var result = userBL.Login(userlogin);
                if (result == null)
                {
                    return Unauthorized();
                }
                return Ok(new { success = true, msg = "Login sucessfull", data = result });
            }
            catch (System.Exception)
            {

                throw;

            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("forgetpassword")]
        public IActionResult ForgetPassword(string userEmail)
        {
            try
            {
                 var check = userBL.EmailCheck(userEmail);
                 if (check != true)
                 {
                     return this.BadRequest(new { sucess = false, msg = "Email is not exist.Please change your email" });
                 }
                 var resultForgetPassword = userBL.ForgetPassword(userEmail);
                 if (resultForgetPassword != null)
                 {
                     return this.Ok(new { sucess = true, msg = "Genrate Password Sucessfull", data = resultForgetPassword }); 
                 }
                 else
                 {
                     return this.BadRequest(new { sucess = false, msg = "Genrate Password Unsucessfull" });
                 }

            }
            catch (System.Exception)
            {

                throw;
            }        
        
        }
        [Authorize]
        [HttpPut]
        [Route("resetpassword")]
        public IActionResult ResetPassword(string newPassword,string confirmPassword)
       {
            try
            {
                string email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
                var result = userBL.ResetPassword(newPassword, confirmPassword, email);
                if (result != null)
                {
                    return this.Ok(new { sucess = true, msg = "Password Reset Successfull", data = result });
                }
                else
                {
                    return this.BadRequest(new { sucess = false, msg = "Unable to Reset Password" });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

