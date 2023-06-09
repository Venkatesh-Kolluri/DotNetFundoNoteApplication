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

namespace FundoNoteApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }
        [HttpPost]
        [Route("register")]
        public IActionResult register(UserRegistration userRegistration)
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

        [HttpPost("login")]
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
        [HttpPost("forgetpassword")]
        public IActionResult ForgetPassword(UserLogin userLogin)
        {
            try
            {
                var result = userBL.ForgetPass(userLogin.Email);
                if(result != null)
                {
                    return Ok(new { success = true, msg = "Token sent" });
                }
                return Ok(new { success = false, msg = "Token not sent" });

            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
}

