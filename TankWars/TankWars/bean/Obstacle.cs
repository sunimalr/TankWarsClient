using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankWars.bean
{
    class Obstacle
    {
        protected Point location;

        public Obstacle()
        {
        }

        public Obstacle(string x, string y)
        {
            this.location = new Point(int.Parse(x), int.Parse(y));
        }

        public Point Location
        {
            get { return location;}
        }

    }
}
