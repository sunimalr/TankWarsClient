using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankWars.bean
{
    class BrickWall : Obstacle
    {
        private int damageLevel = 0;
        

        public BrickWall(string x, string y, string damage)
        {
            this.damageLevel = int.Parse(damage);
            this.location = new Point(int.Parse(x), int.Parse(y));
        }

        public BrickWall(string x, string y) : base(x,y)
        {

        }

        public int DamageLevel
        {
            get { return damageLevel; }
            set { damageLevel = value; }
        }

        public String Type
        {
            get
            {
                if (damageLevel == 0)
                    return "b100";
                else if (damageLevel == 1)
                    return "b25";
                else if (damageLevel == 2)
                    return "b50";
                else if (damageLevel == 3)
                    return "b75";
                else if (damageLevel == 4)
                    return "b100";
                else return null;
            }
        }
    }
}
