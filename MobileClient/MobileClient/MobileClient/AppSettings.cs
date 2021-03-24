using System;
using System.Collections.Generic;
using System.Text;

namespace MobileClient
{
    public class AppSettings
    {
        public AppSettings()
        {
        }

        public AppSettings(User user, string serverURI)
        {
            User = user;
            this.serverURI = serverURI;
        }

        public User User { get; set; }
        public string serverURI { get; set; }

    }
}
