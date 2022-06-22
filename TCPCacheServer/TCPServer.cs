using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TCPCacheServer.Builders;

namespace TCPCacheServer
{
    public class TCPServer
    {
        private readonly TcpListener _server;
        private readonly bool _isRunning;
        private readonly CacheController cacheController;
        private StringParser stringParser;

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
                    stringParser = new StringParser(streamData);
                    var commandBuilder = stringParser.GetCommandBuilder();

                    if (commandBuilder.GetType() == typeof(SetCommandBuilder))
                    {
                        commandBuilder.BuildCommandValue(sReader.ReadLine());

                        var command = commandBuilder.BuildCommand();

                        cacheController.Add(command.Key, command.Value);

                        sWriter.WriteLine("OK");
                        sWriter.Flush();

                    }
                    else if (commandBuilder.GetType() == typeof(GetCommandBuilder))
                    {
                        var command = commandBuilder.BuildCommand();
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

