using System;
using Encryption;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ConnectionInfo
    {
        public ConnectionInfo(string connectionID)
        {
            ConnectionID = connectionID;
        }

        public CustomAes Aes { get; set; }
        public CustomRsa Rsa { get; set; }
        public User UserInfo { get; set; }
        public string ConnectionID { get; set; }
        public enum Statuses
        {
            Connected,
            Handshaking,
            Handshaked
        }
        public Statuses Status { get; set; }

    }
}
