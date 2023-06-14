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
    }
}
