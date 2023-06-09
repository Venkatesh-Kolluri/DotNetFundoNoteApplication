using BusinessLayer.Interface;
using CommonLayer.Model;
using DataLayer.Db;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BussinesLayer.Services
{
    public class UserBL : IUserBL
    {
        public static readonly object UserTable;
        private readonly IUserDL userDL;
        public UserBL(IUserDL userDL)
        {
            this.userDL = userDL;
        }
        public UserEntity Register(UserRegistration user)
        {
            try
            {
                return userDL.Register(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string Login(UserLogin userLogin)
        {
            try
            {
                return userDL.Login(userLogin);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public string ForgetPass(string Email)
        {
            try
            {
                return userDL.ForgetPass(Email);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
