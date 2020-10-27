using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SWE1_MTCG.Server;

namespace SWE1_MTCG
{
    class Program
    {
        public static void Main(string[] args)
        {
            Webserver server = new Webserver();
            server.Start();
        }
    }
}
