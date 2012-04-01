using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankWars.bean
{
    class Bullet
    {
        private Player shooter;
        private Point pos;
        private int direction;
        private int[] dirData = new Int32[2];
        public bool alive;
        //int id = 0;

        //public int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}

        public int[] DirData
        {
            get { return dirData; }
            set { dirData = value; }
        }

        public Player Shooter
        {
            get { return shooter; }
            set { shooter = value; }
        }

        public Point Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public int Direction
        {
            get { return direction; }
            set
            {
                direction = value;

                switch (direction)
                {
                    case 0:
                        dirData[0] = 0;
                        dirData[1] = -1;
                        break;
                    case 1:
                        dirData[0] = 1;
                        dirData[1] = 0;
                        break;
                    case 2:
                        dirData[0] = 0;
                        dirData[1] = 1;
                        break;
                    case 3:
                        dirData[0] = -1;
                        dirData[1] = 0;
                        break;
                    default:
                        break;
                }


            }
        }

        public Bullet(Player con, Point origin, int dir)
        {
            shooter = con;
            pos = origin;
            Direction = dir;
            Console.WriteLine("Bullet shot at " + pos.X + "," + pos.Y + "," + direction);
        //    RandomGen r = new RandomGen();
        //    id = r.randomD(1000);
        }
        public void updateLocation()
        {
            this.pos.X += GameManager.bulletVelocity * dirData[0];
            this.pos.Y += GameManager.bulletVelocity * dirData[1];
        }
    }
}
