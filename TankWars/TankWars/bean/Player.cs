using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankWars.bean
{
    class Player
    {
        private string name;
        private string suffix;
        private Point location;
        private int direction;
        private Boolean shot = false; //initialize to default value
        private int health = 100;       //initialize to default value
        private int currentCoins = 0;          //initialize to default value
        private int currentPoints = 0;         //initialize to default value
        private bool isAlive = true;


        public Player(string name,string x,string y, string direction, string suffix)
        {
            this.name = name;
            this.direction = int.Parse(direction);
            this.location = new Point(int.Parse(x), int.Parse(y));
            this.suffix = suffix;
        }

        public Player(string name, string x, string y, string direction)
        {
            this.name = name;
            this.direction = int.Parse(direction);
            this.location = new Point(int.Parse(x), int.Parse(y));
            this.suffix = "";
        }

        public string Name
        {
            get { return name; }
            set { name = value; } 
        }

        public Point currentP
        {
            get { return location; }
            set { location = value; }
        }

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public bool firedShot
        {
            get { return shot; }
            set { shot = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int Coins
        {
            get { return currentCoins; }
            set { currentCoins = value; }
        }

        public int Points 
        {
            get { return currentPoints; }
            set { currentPoints = value; }
        }

        public string Suffix
        {
            get { return suffix; }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = false; }
        }
    
    
    }


}
