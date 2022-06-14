using System;
using System.Net;

namespace TCPCacheServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server starting");
            int port = 10011;
            try
            {
                TCPServer tCPServer = new TCPServer(IPAddress.Any, port);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }
    }
}
