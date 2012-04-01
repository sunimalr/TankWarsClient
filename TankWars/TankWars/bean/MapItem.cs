using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;

namespace TankWars.bean
{
    class MapItem 
    {
        private Point position;
        protected System.Timers.Timer aTimer;
        private int lifeTime = -1;
        private int appearTime = -1;
        private int disappearTime = -1;
        private int disappearBalance = -1;
        protected string type = "med";

        public MapItem()
        {
        }

        public MapItem(String x, String y, String life)
        {
            Position = new Point(int.Parse(x), int.Parse(y));
            LifeTime = int.Parse(life);

            aTimer = new System.Timers.Timer(lifeTime);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
            GC.KeepAlive(aTimer);
        }
        public MapItem(int x, int y)
        {
            Position = new Point(x,y);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Medpack removed at {0}", e.SignalTime);
            GameManager.getGameManager.removeMapItem(this);
        }

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public int DisappearBalance
        {
            get { return disappearBalance; }
            set { disappearBalance = value; }
        }

        public int LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        public int AppearTime
        {
            get { return appearTime; }
            set { appearTime = value; }
        }

        public int DisappearTime
        {
            get { return disappearTime; }
            set { disappearTime = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            MapItem m = obj as MapItem;
            if ((System.Object)m == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.position.X == m.position.X) && (this.position.Y == m.position.Y);
        }
        public override int GetHashCode()
        {
            return position.X ^ position.Y;
        }

    }
}
