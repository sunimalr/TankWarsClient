using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using TankWars.bean;

namespace TankWars
{
    class AIManager
    {
        private static AIManager instance;
        public static int delay=2000;
        private List<Node> coins = new List<Node>();
        //int mx=GameManager.playerArray[GameManager.playerIndex].currentP.X;
        //int my = GameManager.playerArray[GameManager.playerIndex].currentP.Y;
        public const int MAXSIZE = GameManager.MAXSIZE;
        public static Node[,] terrain = new Node[MAXSIZE,MAXSIZE];
        Dijkstra d;

        public void automatedTank()
        {

            System.Timers.Timer aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
           // System.Timers.Timer bTimer = new System.Timers.Timer(1000);
            //bTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent1);
            //bTimer.Enabled = true;

        }

        public int getDelay()
        {
            return delay;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine("Timer timer timer ", e.SignalTime);
                
                Console.WriteLine(GameManager.playerArray[GameManager.playerIndex].currentP.X);
                Console.WriteLine(GameManager.playerArray[GameManager.playerIndex].currentP.Y);
               this.calDijkstra();
            }
            catch (Exception r)
            {
                Console.WriteLine("player location error!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! "+r);
            }
        }

        private void OnTimedEvent1(object source, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine("Timer1 timer1 timer1 ", e.SignalTime);

                Console.WriteLine(GameManager.playerArray[GameManager.playerIndex].currentP.X);
                Console.WriteLine(GameManager.playerArray[GameManager.playerIndex].currentP.Y);
                //this.calDijkstra();
               // this.findNextMove(mx,my);
            }
            catch (Exception r)
            {
                Console.WriteLine("player location error!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + r);
            }
        }

        private AIManager()
        {

           // initArray();
            
        
        }

        public static AIManager getAIManager
        {
            get
            {
                if (instance == null)
                {
                    instance = new AIManager();
                }
                return instance;
            }
        }

        

        
        public void initArray()
        {
            for(int x=0;x<MAXSIZE;x++)
            {
                for(int y=0;y<MAXSIZE;y++)
                {
                    terrain[x, y] = new Node();
                    terrain[x,y].X=x;
                    terrain[x,y].Y=y;
                    terrain[x, y].Type = "empty";
                }
                
            }
            
        }
        //********************************************************************************************************************************************************
       // private System.Object lockThis = new System.Object();




        public void calDijkstra()
        {
            bool shoot = false;
            for (int k = 0; k < GameManager.playerCount; k++)
            {

                if (k != GameManager.playerIndex)
                {

                    int ux = GameManager.playerArray[k].currentP.X;
                    int uy = GameManager.playerArray[k].currentP.Y;
                    int mx = GameManager.playerArray[GameManager.playerIndex].currentP.X;
                    int my = GameManager.playerArray[GameManager.playerIndex].currentP.Y;


                    if (mx == ux)
                    {
                        if ((my < uy) && (GameManager.playerArray[GameManager.playerIndex].Direction == 2))
                        {
                            shootPlayer();
                            shoot = true;
                            break;
                        }
                        else if ((my > uy) && (GameManager.playerArray[GameManager.playerIndex].Direction == 0))
                        {
                            shootPlayer();
                            shoot = true;
                            break;
                        }

                    }
                    else if(my==uy)
                    {
                        if ((mx < ux) && (GameManager.playerArray[GameManager.playerIndex].Direction == 1))
                        {
                            shootPlayer();
                            shoot = true;
                            break;
                        }
                        else if ((mx > ux) && (GameManager.playerArray[GameManager.playerIndex].Direction == 3))
                        {
                            shootPlayer();
                            shoot = true;
                            break;
                        }
                    }
                }

            }

            if (!shoot)
            {

                //lock (lockThis)
                Monitor.Enter(this);
                {
                    Console.WriteLine("Dij started   !!");
                    // Dijkstra.getDijkstra.calDijkstra(0,0);
                    // Dijkstra.getDijkstra.printDist();
                    try
                    {
                        Dijkstra.getDijkstra.calDijkstra(GameManager.playerArray[GameManager.playerIndex].currentP.X, GameManager.playerArray[GameManager.playerIndex].currentP.Y);



                        int min = int.MaxValue;
                        int mx = GameManager.playerArray[GameManager.playerIndex].currentP.X;
                        int my = GameManager.playerArray[GameManager.playerIndex].currentP.Y;
                        MapItem[] coins = new MapItem[GameManager.coinList.Count];
                        GameManager.coinList.CopyTo(coins);
                        foreach (MapItem m in coins)
                        {
                            if (Dijkstra.dist[m.Position.X, m.Position.Y] < min)
                            {
                                min = Dijkstra.dist[m.Position.X, m.Position.Y];
                                mx = m.Position.X;
                                my = m.Position.Y;
                            }

                        }


                        AIManager.getAIManager.findNextMove(mx, my);

                        //Console.WriteLine("Player x= ................ "+GameManager.playerArray[GameManager.playerIndex].currentP.X);
                        //Dijkstra.getDijkstra.printDist();
                        //Dijkstra.getDijkstra.calDijkstra(9,0);
                        //Dijkstra.getDijkstra.printDist();
                        //Dijkstra.getDijkstra.printPrevious();
                        //Thread.CurrentThread.Start();


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Coin Pile Error at calDijkstra...........!!!");
                    }

                    // this.findNextMove(1,0);
                }
                Monitor.Exit(this);
            }
        }

        public void shootPlayer()
        {

            GameManager.getGameManager.sendMessage("SHOOT#");
        
        }

        public void printTerrain()
        {

            for (int x = 0; x < MAXSIZE; x++)
            {
                for (int y = 0; y < MAXSIZE; y++)
                {
                    Console.Write(terrain[x, y].Type+" ");
                    
                }
                Console.WriteLine();

            }
        
        }

        public void findNextMove(int x,int y)
        {
            Dijkstra.getDijkstra.nextMove(GameManager.playerArray[GameManager.playerIndex].currentP.X, GameManager.playerArray[GameManager.playerIndex].currentP.Y, x, y);
           // calDijkstra();
        }


        

      
    }
}
