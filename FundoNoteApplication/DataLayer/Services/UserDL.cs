using CommonLayer.Model; 
using DataLayer.Db;
using DataLayer.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DataLayer.Services
{
    public class UserDL : IUserDL
    {
        private readonly FundoContext context;
         private readonly string secret ;
        private readonly string expDate;

        public UserDL(FundoContext context, IConfiguration config)
        {
            this.context = context;
            this.secret = config.GetSection("JwtConfig").GetSection("secret").Value;
            this.expDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
        }
        /// <summary>
        /// Register method used to register users in the application
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserEntity Register(UserRegistration user)
        {
            try
            {
                UserEntity userentity = new UserEntity();
                userentity.FirstName = user.FirstName;
                userentity.LastName = user.LastName;
                userentity.Email = user.Email;
                userentity.Password = user.Password;
                userentity.RegisteredAt = DateTime.Now;
                context.UserTable.Add(userentity);
                context.SaveChanges();
                if (userentity != null)
                {
                    return userentity;
                }
                else return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Login method is used to get access to a particular user into the application
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public string Login(UserLogin userLogin)
        {
            UserEntity userEntity = new UserEntity();
            userEntity = context.UserTable
                .FirstOrDefault(u => u.Email == userLogin.Email && u.Password == userLogin.Password);
            if (userEntity != null)
            {
                var Token = GenerateSecurityToken(userEntity.Email, userEntity.UserId);
                return Token;
            }
            return null ;
        }
        /// <summary>
        /// This method is used to generate the token whenever the method is called
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GenerateSecurityToken(string email,long userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(expDate)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        /// <summary>
        /// ForgetPass method is used to generate a token and mail the token to a particular user to reset the users password
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public string ForgetPass(string Email)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity= context.UserTable.FirstOrDefault(u => u.Email == Email);
                if (userEntity != null)
                {
                    var Token = GenerateSecurityToken(userEntity.Email, userEntity.UserId);
                    MSMQModel msmqModel = new MSMQModel();
                    msmqModel.sendData2Queue(Token);
                    return Token;
                }
                else
                    return null;

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// EmailCheck method is used to check weather the entered email is available in the database or not
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool EmailCheck(string email)
        {
            try
            {
                UserEntity userentity = new UserEntity();
                userentity = context.UserTable.FirstOrDefault(x => x.Email == email);
                if (userentity != null)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
       /// <summary>
       /// ResetPassword method is used to change the old password into new password
       /// </summary>
       /// <param name="newPassword"></param>
       /// <param name="confirmPassword"></param>
       /// <param name="email"></param>
       /// <returns></returns>
        public string ResetPassword(string newPassword,string confirmPassword, string email)
        {
            try
            {
                
                UserEntity userEntity = new UserEntity();
              
                userEntity = context.UserTable.FirstOrDefault(x => x.Email == email);
                if (userEntity != null)
                {
                    userEntity.Password = newPassword;
                    context.SaveChanges();
                    return "Done";
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// GetAllUser method will retrive all the users present in the database
        /// </summary>
        /// <returns></returns>
        public List<UserEntity> GetAllUser()
        {
            try
            {
                var AllNotes = context.UserTable.FirstOrDefault();
                if (AllNotes != null)
                {
                    return context.UserTable.ToList();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// GetbyUserId method is used to get the all the details of the user with particular userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserEntity> GetbyUserId(long userId)
        {
            try
            {
                var getUserId = context.UserTable.Where(x => x.UserId == userId).FirstOrDefault();
                if (getUserId != null)
                {
                    return context.UserTable.Where(u => u.UserId == userId).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
