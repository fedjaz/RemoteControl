using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Server.Services
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

        public bool Equals([AllowNull] User other)
        {
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Login, Password);
        }
    }
}
