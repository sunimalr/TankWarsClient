using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TankWars.utilities;
using System.Threading;
using TankWars.bean;
using Microsoft.Xna.Framework;
using System.Timers;

namespace TankWars
{
    class GameManager
    {

        private static GameManager instance;

        private Rectangle viewPortRect = new Rectangle(0, 0, 1200, 800);
        public static int bulletVelocity = 1;
        private int bulletUpdateTime = 250;
        protected System.Timers.Timer bulletUpdateTimer;
        public const int MAXSIZE = 20;

        public List<Point> bulletLocs = new List<Point>();
        public List<Point> impactLocs = new List<Point>();

        public static Player[] playerArray;
        List<Bullet> bulletList = new List<Bullet>();
        List<BrickWall> brickList = new List<BrickWall>();
        List<StoneWall> stoneList = new List<StoneWall>();
        List<Water> waterList = new List<Water>();
        public static List<MapItem> coinList = new List<MapItem>();
        List<MapItem> lifeList = new List<MapItem>();
        private List<MapItem> expiredMeds = new List<MapItem>();
        private List<MapItem> expiredCoins = new List<MapItem>();
        
        public static int playerIndex;
        public static int playerCount;
        public string myPlayerName;
        public string serverIP = Communicator.getCommunicator.ServerIP;
        public string cliientIP = Communicator.getCommunicator.ClientIP;

        private GameManager() {

            AIManager.getAIManager.initArray();
            

        }

        public static GameManager getGameManager
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        
        public void sendMessage(String msg)
        {

            Communicator.getCommunicator.send(msg);

        }

        public void recieveMessage()
        {

            //ThreadPool.QueueUserWorkItem(Communicator.getCommunicator.receive);
            Thread recieveThread = new Thread(Communicator.getCommunicator.receive);
            recieveThread.Start();
            // Communicator.getCommunicator.receive();

        }

        public void splitString(Object obj)
        {
            String line = (String)obj;
            String[] token = line.Split(':');

            if (token[0] == "I")
            {
                setupMap(token);
                Console.WriteLine("***************************************" + playerIndex);///////////////////
                Console.WriteLine("Map setup successful");
               
            }

            else if (token[0] == "S")
            {
                initGame(token);

                myPlayerName = playerArray[playerIndex].Name;
                updateGame();

                bulletUpdateTimer = new System.Timers.Timer(bulletUpdateTime);
                bulletUpdateTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                bulletUpdateTimer.AutoReset = true;
                bulletUpdateTimer.Enabled = true;

                Console.WriteLine(line);
            }

            else if (token[0] == "G")
            {
                globalUpdate(token);
                updateGame();
                Console.WriteLine(line);
            }

            else if (token[0] == "L")
            {
                setLifePack(token);
                updateGame();
                Console.WriteLine(line);
            }

            else if (token[0] == "C")
            {
                acquireCoin(token);
                updateGame();
                Console.WriteLine(line);
            }
            else
            {
                Console.WriteLine("Error message received###################################################");
                Console.WriteLine(line);
                errorMsgHandler(line);

            }
        }


        private void errorMsgHandler(String line)
        {

            if (line.Equals("TOO_QUICK"))
            {
               // AIManager.delay += 100;
                Console.WriteLine("delay =" + AIManager.delay);
            }
            
        }


        private void setupMap(String[] token) 
        {
            playerIndex = int.Parse(token[1].Remove(0, 1));
            createBricks(token[2]);
            createStones(token[3]);
            createWater(token[4]);
           // Thread t = new Thread(AIManager.getAIManager.calDijkstra);
            Thread t = new Thread(AIManager.getAIManager.automatedTank);

            t.Start();
            //AIManager.getAIManager.calDijkstra();
            //AIManager.getAIManager.calDijkstra();
            //AIManager.getAIManager.calDijkstra();
           // AIManager.getAIManager.findNextMove(0, 0);
            //AIManager.getAIManager.printTerrain();
            //Thread t= new Thread(AIManager.getAIManager.calDijkstra);
            //t.Start();
   
        }

        public void displayError(Object obj) 
        {
            String[] t = (String[])obj;
            Console.WriteLine(t + " : Error"); 
        }//rejected

        public void initGame(String[] token) //initialize game
        {
            playerCount = token.Length - 1;
            playerArray = new Player[playerCount];

            for (int x = 1; x < token.Length; x++)
            {
                String[] playerInfo = token[x].Split(';');
                String[] coord = playerInfo[1].Split(',');

                if((x-1)==playerIndex)  //X starts from 1
                    playerArray[x - 1] = new Player(playerInfo[0], coord[0], coord[1], playerInfo[2]);
                else
                    playerArray[x - 1] = new Player(playerInfo[0], coord[0], coord[1], playerInfo[2],"E");
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            updateBullets();
            Console.WriteLine("Bullet timer elapesd");
        }

        public void globalUpdate(String[] token) //globally broadcasted messages processed here
        {
            
            for (int x = 1; x <= playerCount; x++)
            {
                String[] playerInfo = token[x].Split(';');
                String[] loc = playerInfo[1].Split(',');
                Point coord = new Point(int.Parse(loc[0]), int.Parse(loc[1]));

                playerArray[x - 1].currentP = coord;
                playerArray[x - 1].Direction = int.Parse(playerInfo[2]);

                if (playerInfo[3].Equals("0"))
                    playerArray[x - 1].firedShot = false;
                else
                {
                    playerArray[x - 1].firedShot = true;
                    bulletList.Add(new Bullet(playerArray[x - 1], playerArray[x - 1].currentP, playerArray[x - 1].Direction));      
                }

                playerArray[x - 1].Health = int.Parse(playerInfo[4]);
                playerArray[x - 1].Coins = int.Parse(playerInfo[5]);
                playerArray[x - 1].Points = int.Parse(playerInfo[6]);

                if (playerArray[x - 1].Health <= 0)
                {
                    playerArray[x - 1].IsAlive = false;
                }
            }

            string[] brickDetails = token[token.Length - 1].Split(';');

            brickList.Clear();
            foreach (String brick in brickDetails)
            {
                String[] brickInfo = brick.Split(',');
                brickList.Add(new BrickWall(brickInfo[0], brickInfo[1], brickInfo[2]));
            }

            //Thread tt = new Thread(AIManager.getAIManager.calDijkstra);
            //tt.Start();
            //Console.WriteLine("BOOOOOOOOOOl = "+Dijkstra.usingD);
            
                //AIManager.getAIManager.calDijkstra();
            
        }


        public void acquireCoin(String[] token) //acquire coins
        {
            String[] loc = token[1].Split(',');
            coinList.Add(new CoinPile(loc[0], loc[1], token[2], token[3]));
        }

        public void setLifePack(String[] token) //set life packs
        {
            String[] loc = token[1].Split(',');
            lifeList.Add(new MapItem(loc[0], loc[1], token[2]));
        }

        public void updateGame()
        {
            try
            {
                GameBoard.getGameBoard.ClearBoard();


                if (playerArray != null)
                {
                    for (int x = 0; x < playerArray.Length; x++)
                    {
                        if ((playerArray != null) && (playerArray.Length != 0))
                        {
                            if ((playerArray[x].IsAlive))
                            {
                                GameBoard.getGameBoard.SetSquare(playerArray[x].currentP.X, playerArray[x].currentP.Y, playerArray[x].Direction.ToString(), playerArray[x].Suffix);
                                MapItem mi = new MapItem(playerArray[x].currentP.X, playerArray[x].currentP.Y);
                                if (coinList.Contains(mi))
                                {
                                    expiredCoins.Add(mi);
                                }
                                if (lifeList.Contains(mi))
                                {
                                    expiredMeds.Add(mi);
                                }
                            }
                        }
                    }
                }
                foreach (BrickWall brick in brickList)
                {
                    GameBoard.getGameBoard.SetSquare(brick.Location.X, brick.Location.Y, brick.Type);
                }

                foreach (StoneWall stone in stoneList)
                {
                    GameBoard.getGameBoard.SetSquare(stone.Location.X, stone.Location.Y, stone.Type);
                }
                foreach (Water water in waterList)
                {
                    GameBoard.getGameBoard.SetSquare(water.Location.X, water.Location.Y, water.Type);
                }


                if (expiredCoins.Count != 0)
                {
                    foreach (MapItem c in expiredCoins)
                    {
                        coinList.Remove(c);
                    }
                    expiredCoins.Clear();
                }

                foreach (MapItem coin in coinList)
                {
                    GameBoard.getGameBoard.SetSquare(coin.Position.X, coin.Position.Y, coin.Type);
                }


                if (expiredMeds.Count != 0)
                {
                    foreach (MapItem m in expiredMeds)
                    {
                        lifeList.Remove(m);
                    }
                    expiredMeds.Clear();
                }

                foreach (MapItem item in lifeList)
                {
                    GameBoard.getGameBoard.SetSquare(item.Position.X, item.Position.Y, item.Type);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("Update Game Error...........!!!!!!1");
            }
        }
            


        private void createBricks(String line)
        {
            String[] bricks = line.Split(';');
            foreach(String brick in bricks)
            {
                Console.WriteLine("brick added to " + brick);///////////////////////////////////////////////
                String[] coord = brick.Split(',');
                brickList.Add(new BrickWall(coord[0], coord[1]));

                AIManager.terrain[(int)coord[0][0]-48, (int)coord[1][0]-48].Type = "b100";

            }
        }

        private void createStones(String line)
        {
            String[] stones = line.Split(';');
            foreach (String stone in stones)
            {
                Console.WriteLine("stone added to " + stone);///////////////////////////////////////////////
                String[] coord = stone.Split(',');
                stoneList.Add(new StoneWall(coord[0], coord[1]));

                AIManager.terrain[(int)coord[0][0] - 48, (int)coord[1][0] - 48].Type = "stone";
            }
        }

        private void createWater(String line)
        {
            String[] waterElements = line.Split(';');
            foreach (String water in waterElements)
            {
                Console.WriteLine("water added to " + water);///////////////////////////////////////////////
                String[] coord = water.Split(',');
                waterList.Add(new Water(coord[0], coord[1]));

                AIManager.terrain[(int)coord[0][0] - 48, (int)coord[1][0] - 48].Type = "water";
            }
        }

        public void removeMapItem(MapItem item)
        {

            expiredMeds.Add(item);

        }
        public void removeCoins(MapItem coins)
        {

            expiredCoins.Add(coins);

        }

        public void updateBullets()
        {
            bulletLocs.Clear();
            impactLocs.Clear();

            if (bulletList.Count != 0)
            {
                foreach (Bullet bullet in bulletList)
                {
                    if (bullet.alive)
                    {
                        bullet.updateLocation();

                        if (!viewPortRect.Contains(bullet.Pos))
                        {
                            bullet.alive = false;
                            continue;
                        }
                        else
                        {
                            bulletLocs.Add(bullet.Pos);
                        }
                        Rectangle bulletRect = new Rectangle(bullet.Pos.X, bullet.Pos.Y, GamePiece.PieceWidth, GamePiece.PieceHeight);

                        foreach (Player player in playerArray)
                        {
                            if (player.IsAlive)
                            {
                                Rectangle playerRect = new Rectangle(player.currentP.X, player.currentP.Y, GamePiece.PieceWidth, GamePiece.PieceHeight);
                                if (playerRect.Intersects(bulletRect))
                                {
                                    impactLocs.Add(player.currentP);
                                    bullet.alive = false;
                                    break;
                                }
                            }
                        }
                    }

                }
            }

        }

    }

}
