using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Services
{
    public class MouseData : IDataInfo
    {
        public User User { get; set; }
        public int MouseX { get; set; }
        public int MouseY { get; set; }

        public MouseData()
        {
        }  

        public MouseData(User user, int mouseX, int mouseY)
        {
            User = user;
            MouseX = mouseX;
            MouseY = mouseY;
        }
    }
}
