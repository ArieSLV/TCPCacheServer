using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPCacheServer
{
    public class TCPServer
    {
        private readonly TcpListener _server;
        private readonly bool _isRunning;
        private readonly CacheController cacheController;

        public TCPServer(IPAddress ipAddress, int port)
        {
            _server = new TcpListener(ipAddress, port);
            _server.Start();

            _isRunning = true;

            Console.WriteLine($"Server listening at {port} port");

            cacheController = new CacheController();

            LoopClients();
        }

        public void LoopClients()
        {
            while (_isRunning)
            {
                TcpClient newClient = _server.AcceptTcpClient();

                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public void HandleClient(object obj)
        {

            TcpClient client = (TcpClient)obj;

            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.UTF8);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.UTF8);

            bool isClientConnected = true;

            while (isClientConnected)
            {
#nullable enable
                string? streamData = sReader.ReadLine();
#nullable disable
                try
                {
                    //Wrapping a string into the command to make it easier to work with her
                    var command = new Command(streamData);

                    switch (command.CommandType)
                    {
                        case CommandType.Set:
                            command.Value = sReader.ReadLine();                //Catching the second line of set command

                            cacheController.Add(command.Key, command.Value);

                            sWriter.WriteLine("OK");
                            sWriter.Flush();
                            break;
                        case CommandType.Get:
                            
                            var value = cacheController.Get(command.Key);

                            if (value != null)
                            {
                                sWriter.WriteLine($"OK {Encoding.UTF8.GetByteCount(value)}");
                                sWriter.WriteLine(value);
                                sWriter.Flush();
                            }
                            else
                            {
                                sWriter.WriteLine("MISSING");
                                sWriter.Flush();
                            }

                            break;
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }
    }
}

