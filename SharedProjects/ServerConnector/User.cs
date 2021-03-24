using System;
using System.Collections.Generic;
using System.Text;

namespace ServerConnector
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
    }
}
