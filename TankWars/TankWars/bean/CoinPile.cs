using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;

namespace TankWars.bean
{
    class CoinPile : MapItem
    {
        private int price = 0;

        public CoinPile(String x, String y, String lifeTime, String value)
        {
            Position = new Point(int.Parse(x), int.Parse(y));
            LifeTime = int.Parse(lifeTime);

            aTimer = new System.Timers.Timer(LifeTime);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;

            price = int.Parse(value);
            this.Type = "coin";
            GC.KeepAlive(aTimer);
           
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Coins removed at {0}", e.SignalTime);
            GameManager.getGameManager.removeCoins(this);
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
