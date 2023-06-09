using CommonLayer.Model;
using DataLayer.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interface
{
    public interface IUserDL
    {
        public UserEntity Register(UserRegistration user);
        public string Login(UserLogin userLogin);
        public string ForgetPass(string Email);
      //  public string GenerateSecurityToken(string email, long userId);

    }
}
