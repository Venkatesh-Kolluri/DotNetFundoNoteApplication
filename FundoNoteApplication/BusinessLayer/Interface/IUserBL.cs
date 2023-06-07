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
        public UserEntity Login(UserLogin userLogin);
   
    }
}
