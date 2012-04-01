using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankWars
{
    class Dijkstra
    {
        public static int[,] dist=new int[AIManager.MAXSIZE,AIManager.MAXSIZE];
        Node[,] previous = new Node[AIManager.MAXSIZE, AIManager.MAXSIZE];
        public static bool usingD=false;
     //   List<Node> Q = new List<Node>();
        private static Dijkstra instance;
        Stack<Node> stk = new Stack<Node>();

        

        public static Dijkstra getDijkstra
        {
            get
            {
                if (instance == null)
                {
                    instance = new Dijkstra();
                }
                return instance;
            }
        }

        private Dijkstra()
        {
        }

        public void initPrevious()
        {
            for (int x = 0; x < AIManager.MAXSIZE; x++)
            {
                for (int y = 0; y < AIManager.MAXSIZE; y++)
                {
                    previous[x, y].X = x;
                    previous[x, y].Y = y;
                    previous[x, y].Type = "empty";
                }
                
            }
        
        }

        public void printPrevious()
        {
            for (int x = 0; x < AIManager.MAXSIZE; x++)
            {
                for (int y = 0; y < AIManager.MAXSIZE; y++)
                {
                    //previous[x, y].X = x;
                    //previous[x, y].Y = y;
                    //previous[x, y].Type = "empty";
                    try
                    {
                        Console.Write(" x y = " + previous[x, y].X + " " + previous[x, y].Y);
                    }
                    catch (Exception e)
                    { Console.WriteLine("EEEEE"); }
                }
                Console.WriteLine();

            }
        }

        public void nextMove(int sx, int sy, int x, int y)
        {
            int ct1 = 0;
            Node n = new Node();
            Node p = new Node();
            n = previous[x, y];
            if (n.X == sx)
            {
                if (n.Y == sy)
                { 
                    p.X = x;
                    p.Y = y;

                    stk.Push(p);
                }
            }
            else
            {
                p = n;
                
                while (true)
                {
                   // ct1 = 0;
                    if (ct1 > 50)
                        break;
                    Console.WriteLine("inin");
                    n = previous[n.X, n.Y];
                    try
                    {
                        if (n.X == sx)
                        {
                            if (n.Y == sy)
                                break;
                        }
                        stk.Push(n);
                    }
                    catch (Exception e)
                    { }
                    p = n;
                    ct1++;
                }
            }


            if (ct1 > 50) { }
            else
            {
                //while (stk.Count > 0)
                {
                    //p = stk.Pop();
                    Console.WriteLine(p.X + "," + p.Y);
                    Console.WriteLine("Piece = " + AIManager.terrain[p.X, p.Y].Type);
                    if (AIManager.terrain[p.X, p.Y].Type.Equals("empty"))
                    {

                        if ((p.X > sx)&&(p.Y==sy))
                            GameManager.getGameManager.sendMessage("RIGHT#");
                        if ((p.X < sx) && (p.Y == sy))
                            GameManager.getGameManager.sendMessage("LEFT#");
                        if ((p.Y > sy)&&(p.X==sx))
                            GameManager.getGameManager.sendMessage("DOWN#");
                        if ((p.Y < sy) && (p.X == sx))
                            GameManager.getGameManager.sendMessage("UP#");
                    }
                    else
                    {
                        // AIManager.terrain[p.X, p.Y].Type = "stone";
                    }
                }
            }

            
        
        }

        public void printDist()
        {

            for (int x = 0; x < AIManager.MAXSIZE; x++)
            {
                for (int y = 0; y < AIManager.MAXSIZE; y++)
                {
                    Console.Write("dist "+dist[x, y]+ " "+x+" "+y);
                    //Console.Write("prev " + previous[x, y].X + " " + previous[x,y].Y);
                }
                Console.WriteLine();

            }
        }

       

        public void calDijkstra(int sx, int sy)
        {
            usingD = true;
            
            HashSet<Node> Q=new HashSet<Node>();
           
            //HashSet<Node> t= t.
            
            int ctor = 0;

            int curx = GameManager.playerArray[GameManager.playerIndex].currentP.X;
            int cury = GameManager.playerArray[GameManager.playerIndex].currentP.Y;

                        

            foreach(Node n in AIManager.terrain)
            {
                
                if((!n.Type.Equals("water"))&&(!n.Type.Equals("stone")&&(!n.Type.Equals("b100"))))
                {
                    ctor++;
                    Console.WriteLine("Added to Q...!! "+ n.Type+" ct = "+ctor);
                    Q.Add(n);
                }

            }


            for (int x = 0; x < AIManager.MAXSIZE; x++)
            {
                for (int y = 0; y < AIManager.MAXSIZE; y++)
                {
                    //dist[x, y] = new int();
                    dist[x, y] = int.MaxValue;
                }
            }

            dist[sx, sy] = 0;

            int minx = sx, miny = sy;
            int ux = sx;
            int uy = sy;
            int ct=0;
            while(Q.Count>0)
            {
               // Console.WriteLine("Q Count = "+Q.Count);
                ct++;
           // Console.WriteLine("Executed " + ct + " times!!");
                int min = int.MaxValue;
                /*
                for (int x = 0; x < AIManager.MAXSIZE; x++)
                {
                    for (int y = 0; y < AIManager.MAXSIZE; y++)
                    {

                        if (dist[x, y] < min)
                        {
                            min = dist[x, y];
                            minx = x;
                            miny = y;
                            Console.WriteLine("min x y "+minx+" "+miny);

                        }
                    }
                }

                */

                Node u = new Node();
                //u.X = 4;
                //u.Y = 5;
               // min = 0;
                foreach(Node n in Q)
                {
                    if (dist[n.X, n.Y] < min)
                    {
                        min = dist[n.X, n.Y];
                        minx = n.X;
                        miny = n.Y;
                        u = n;
                    }
                    /*
                    if((n.X==minx)&&(n.Y==miny))
                   {

                       u = n;
                       break;
                   }
                    */
                }
          //      Console.WriteLine("n-X n-Y " + u.X + " " + u.Y);
                if(dist[u.X,u.Y]==int.MaxValue)
                {
                    break;
                }

                Q.Remove(u);

                minx=u.X;
                miny=u.Y;

                int xp = minx + 1, yp = miny + 1, xn = minx - 1, yn = miny - 1;
                int alt = 0;
                

                if ((xp < AIManager.MAXSIZE))
                {
                    alt = dist[minx, miny] + 1;
                    if (alt < dist[xp, miny])
                    {

                      //  Console.WriteLine("testing xp miny " + xp + " " + miny + " " + AIManager.terrain[xp, miny].Type);
                        switch(AIManager.terrain[xp, miny].Type)
                        {
                            case "empty":
                            dist[xp, miny] = alt;
                            previous[xp, miny] = new Node();
                            previous[xp, miny].X = u.X;
                            previous[xp, miny].Y = u.Y;
                            break;
                            /*
                            case "b100":
                            dist[xp, miny] = alt;
                            previous[xp, miny] = new Node();
                            previous[xp, miny].X = u.X;
                            previous[xp, miny].Y = u.Y;
                            break;
                             */
                    }


                    }
                }
                

                if ((yp < GameManager.MAXSIZE))
                {
                    alt = dist[minx, miny] + 1;
                    if (alt < dist[minx, yp])
                    {
                      //  Console.WriteLine("atulata awa 1");
                        switch(AIManager.terrain[minx, yp].Type)
                        {
                            case "empty":
                        dist[minx,yp]=alt;
                        previous[minx, yp]= new Node();
                        previous[minx,yp].X=u.X;
                        previous[minx, yp].Y = u.Y;
                        break;
                                /*
                            case "b100":
                        dist[minx, yp] = alt;
                        previous[minx, yp] = new Node();
                        previous[minx, yp].X = u.X;
                        previous[minx, yp].Y = u.Y;
                        break;*/
                    }
                    }
                }

                if ((xn >= 0) )
                {
                    
                    alt = dist[minx, miny] + 1;
                    if(alt < dist[xn, miny])
                    {
                        switch(AIManager.terrain[xn, miny].Type)
                        {
                            case "empty":
                        dist[xn,miny]=alt;
                        previous[xn, miny]= new Node();
                        previous[xn, miny].X = u.X;
                        previous[xn, miny].Y = u.Y;
                        break;
                                /*
                            case "b100":
                        dist[xn, miny] = alt;
                        previous[xn, miny] = new Node();
                        previous[xn, miny].X = u.X;
                        previous[xn, miny].Y = u.Y;
                        break;*/
                    }
                    }
                }

                if ((yn >= 0))
                {
                    alt = dist[minx, miny] + 1;
                    if (alt < dist[minx, yn])
                    {
                        switch(AIManager.terrain[minx, yn].Type)
                        {
                            case "empty":
                        dist[minx,yn]=alt;
                        previous[minx, yn]= new Node();
                        previous[minx, yn].X = u.X;
                        previous[minx, yn].Y = u.Y;
                        break;
                                /*
                            case "b100":
                        dist[minx, yn] = alt;
                        previous[minx, yn] = new Node();
                        previous[minx, yn].X = u.X;
                        previous[minx, yn].Y = u.Y;
                        break;*/

                        }
                    }
                }
                 

                
            }

            usingD = false;            

            }
        }
    }

