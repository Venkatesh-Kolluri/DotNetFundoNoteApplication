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
using BusinessLayer.Services;
using DataLayer.Db;
using System.Collections.Generic;
using DataLayer.Services;

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

        [AllowAnonymous]
        [Authorize]
        [HttpPut]
        [Route("resetpassword")]
        public IActionResult ResetPassword(PasswordReset passwordReset)
       {
            try
            {
                string email = User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                var result = userBL.ResetPassword(passwordReset.newPassword,passwordReset.confirmPassword, email);
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
        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetByUserID))]
        public IActionResult GetByUserID(long userId)
        {
            try
            {
                //  var userId = notesModel.UserId;
              /*  var check = userBL.CheckUserId(userId);
                if (check != true)
                {
                    return this.BadRequest(new { sucess = false, msg = "User details" });
                }*/
                List<UserEntity> result = userBL.GetbyUserId(userId);
                if (result == null)
                {
                    return this.Ok(new { Success = false, message = " user not Available" });
                }
                else
                {
                    if (result != null)
                    {
                        return this.Ok(new { Success = true, message = " Got note Successfully", data = result });
                    }
                    return this.BadRequest(new { Success = false, message = " error occured" });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetAllUser))]
        public IActionResult GetAllUser()
        {
            try
            {
                List<UserEntity> result = userBL.GetAllUser();
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = " User got Successfully", data = result });
                }
                else
                    return this.BadRequest(new { Success = false, message = "User not Available" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

