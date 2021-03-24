using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class KeyData : IDataInfo
    {
        public KeyData(User user, int keyCode)
        {
            User = user;
            KeyCode = keyCode;
        }

        public KeyData()
        {
        }

        public User User { get; set; }
        public int KeyCode { get; set; }
        public bool IsKeyboard { get; set; }
    }
}
