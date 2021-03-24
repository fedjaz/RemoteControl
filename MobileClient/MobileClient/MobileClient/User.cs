using System;
using System.Collections.Generic;
using System.Text;

namespace MobileClient
{
    public class User
    {
        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public User()
        {
        }

        public string Login { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Login == user.Login &&
                   Password == user.Password;
        }
    }
}
