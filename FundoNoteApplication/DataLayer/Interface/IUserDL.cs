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
        public UserEntity Login(UserLogin userLogin);

    }
}
