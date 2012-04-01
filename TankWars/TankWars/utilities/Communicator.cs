using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;

namespace TankWars.utilities
{
    class Communicator
    {

        private static Communicator instance;
        private  string serverIP = "127.0.0.1";
        private  string clientIP = "127.0.0.1";

        private Communicator() { }

        public static Communicator getCommunicator
        {
            get
            {
                if (instance == null)
                {
                    instance = new Communicator();
                }
                return instance;
            }
        }

        public string ServerIP
        {
            set {serverIP = value;}
            get { return serverIP; }

        }

        public string ClientIP
        {
            set { clientIP = value; }
            get { return clientIP; }

        }


        public void send(String request)
        {

            #region sending

            try
            {
                Console.OpenStandardOutput();
                TcpClient tcpClient = new TcpClient();

                Console.WriteLine("Connecting....");

                tcpClient.Connect(serverIP, 6000);

                Console.WriteLine("Connected");
                Console.WriteLine("Sending request : " + request);

                NetworkStream outStream = tcpClient.GetStream();

                BinaryWriter writer = new BinaryWriter(outStream);
                ASCIIEncoding asci = new ASCIIEncoding();
                Byte[] ba = asci.GetBytes(request);
                //outStream.Write(ba, 0, ba.Length);
                writer.Write(ba);

                Console.WriteLine("Transmitting.....");

                tcpClient.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occured while sending request  :: " + e);

            }
        }

            #endregion sending

        #region recieving
        public void receive()
        {


            IPAddress ip = IPAddress.Parse(clientIP);

            Console.WriteLine("Waiting for a connection.....");
            while (true)
            {
                TcpListener listener = new TcpListener(ip, 7000);
                listener.Start();
                Socket s = listener.AcceptSocket();
                if (s.Connected)
                {
                    Console.WriteLine("\nConnection accepted from " + s.RemoteEndPoint);
                    NetworkStream inputStream = new NetworkStream(s);
                    StreamReader reader = new StreamReader(inputStream);

                    try
                    {

                        String line = reader.ReadLine();

                        line = line.Remove(line.Length - 1, 1);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(GameManager.getGameManager.splitString), (Object)line);

                        
                        Console.WriteLine("Recieved...");
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Server feed read error!!!");
                    }
                    }
            
                    listener.Stop();
                

            }

        }
        #endregion recieving

    }
}
