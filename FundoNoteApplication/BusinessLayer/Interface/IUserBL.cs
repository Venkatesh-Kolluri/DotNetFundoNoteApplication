using CommonLayer.Model;
using DataLayer.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public UserEntity Register(UserRegistration user);
        public string Login(UserLogin userLogin);
        public string ForgetPassword(string Email);
        public bool EmailCheck(string email);
        public string ResetPassword(string NewPassword, string ConfirmPassword, string email);
        public List<UserEntity> GetbyUserId(long userId);
        public List<UserEntity> GetAllUser();

    }
}
