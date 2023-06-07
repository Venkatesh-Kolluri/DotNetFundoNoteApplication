using CommonLayer.Model;
using DataLayer.Db;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Services
{
    public class UserDL : IUserDL
    {
        private readonly FundoContext context;

        public UserDL(FundoContext context)
        {
            this.context = context;
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
    
        public UserEntity Login(UserLogin userLogin)
        {
            return context.UserTable
                .FirstOrDefault(u => u.Email == userLogin.Email && u.Password == userLogin.Password);
        }
    }
}
