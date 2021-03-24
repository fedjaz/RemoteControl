using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerConnector
{
    public class KeyData
    {
        public KeyData(User user, int keyCode, bool iskeyboard)
        {
            User = user;
            KeyCode = keyCode;
            Iskeyboard = iskeyboard;
        }

        public KeyData()
        {
        }

        public User User { get; set; }
        public int KeyCode { get; set; }
        public bool Iskeyboard { get; set; }
    }
}
