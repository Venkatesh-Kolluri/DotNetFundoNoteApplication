using BusinessLayer.Interface;
using CommonLayer.Model;
using DataLayer.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundoNoteApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
  
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserBL userBL;
        private readonly FundoContext context;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        public UserController(IUserBL userBL,FundoContext context, ILogger<UserController> logger, IMemoryCache  memoryCache,IDistributedCache distributedCache)
        {
            this.userBL = userBL;
            this.context = context;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }
       
        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserRegistration userRegistration)
        {

            try
            {
                var result = userBL.Register(userRegistration);
                if (result != null)
                {
                    logger.LogInformation("registration successfull");
                    return this.Ok(new { success = true, msg = "registration sucessfull", data = result }); //SSMD form
                }
                else
                {
                    logger.LogInformation("registration Unsuccessfull");
                    return this.BadRequest(new { success = false, msg = "registration Unsucessfull" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
  
        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserLogin userlogin)
        {
            try
            {
                var result = userBL.Login(userlogin);
                if (result == null)
                {
                    logger.LogInformation("User login successfully");
                    return Unauthorized();
                }
                else
                {
                    logger.LogInformation("User unable to login");
                    return Ok(new { success = true, msg = "Login sucessfull", data = result });
                }   
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
      
        [HttpPost]
        [Route("forgetpassword")]
        public IActionResult ForgetPassword(string userEmail)
        {
            try
            {
                 var check = userBL.EmailCheck(userEmail);
                 if (check != true)
                 {
                    logger.LogInformation("Email is not exist.Please change your email");
                    return this.BadRequest(new { sucess = false, msg = "Email is not exist.Please change your email" });
                 }
                 var resultForgetPassword = userBL.ForgetPassword(userEmail);
                 if (resultForgetPassword != null)
                 {
                    logger.LogInformation("Genrate Password Sucessfull");
                    return this.Ok(new { sucess = true, msg = "Genrate Password Sucessfull", data = resultForgetPassword }); 
                 }
                 else
                 {
                    logger.LogInformation("Genrate Password UnSucessfull");
                    return this.BadRequest(new { sucess = false, msg = "Genrate Password Unsucessfull" });
                 }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }        
        
        }

       
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
                    logger.LogInformation("Password Reset successfull");
                    return this.Ok(new { sucess = true, msg = "Password Reset Successfull", data = result });
                }
                else
                {
                    logger.LogInformation("Unable to reset password");
                    return this.BadRequest(new { sucess = false, msg = "Unable to Reset Password" });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        
        [HttpGet]
        [Route("getbyuserID")]
        public IActionResult GetByUserID(long userId)
        {
            try
            {
              
                List<UserEntity> result = userBL.GetbyUserId(userId);
                if (result == null)
                {
                    logger.LogInformation(" user not Available");
                    return this.Ok(new { Success = false, message = " user not Available" });
                }
                else
                {
                    if (result != null)
                    {
                        logger.LogInformation(" User found Successfully");
                        return this.Ok(new { Success = true, message = "User found", data = result });
                    }
                    else
                    {
                        logger.LogInformation("error eccord in UserId");
                        return this.BadRequest(new { Success = false, message = " error occured" });

                    }
                   

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("getalluser")]
        public IActionResult GetAllUser()
        {
            try
            {
                List<UserEntity> result = userBL.GetAllUser();
                if (result != null)
                {
                    logger.LogInformation("Got all users");
                    return this.Ok(new { Success = true, message = " User got Successfully", data = result });
                }
                else
                {
                    logger.LogInformation("Users are not available");
                    return this.BadRequest(new { Success = false, message = "User not Available" });
                }
                    
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "UsersList";
            string serializedUsersList;
            var usersList = new List<UserEntity>();
            var redisUsersList = await distributedCache.GetAsync(cacheKey);
            if (redisUsersList != null)
            {
                serializedUsersList = Encoding.UTF8.GetString(redisUsersList);
                usersList = JsonConvert.DeserializeObject<List<UserEntity>>(serializedUsersList);
            }
            else
            {
                usersList = await context.UserTable.ToListAsync();
                serializedUsersList = JsonConvert.SerializeObject(usersList);
                redisUsersList = Encoding.UTF8.GetBytes(serializedUsersList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisUsersList, options);
            }
            return Ok(usersList);

        }
    }
}

