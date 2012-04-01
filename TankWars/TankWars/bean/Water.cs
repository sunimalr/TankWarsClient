using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankWars.bean
{
    class Water : Obstacle
    {
        protected String type = "water";
        public Water(string x, string y) : base(x,y)
        {

        }
        public String Type
        {
            get { return type; }
        }
    }
}
