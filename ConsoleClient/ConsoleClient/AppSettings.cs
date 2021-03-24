using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient
{
    public class AppSettings
    {
        public AppSettings()
        {
        }

        public AppSettings(ServerConnector.User user, string serverURI)
        {
            User = user;
            ServerURI = serverURI;
        }

        public ServerConnector.User User { get; set; }
        public string ServerURI { get; set; }

    }
}
