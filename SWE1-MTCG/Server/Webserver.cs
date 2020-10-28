﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SWE1_MTCG.Api;

namespace SWE1_MTCG.Server
{
    public class Webserver
    {
        private TcpListener _webserver;
        private IPAddress _ipAddress = IPAddress.Loopback;
        private int _port = 11000;
        private byte[] _buffer;

        public Webserver()
        {
            try
            {
                _webserver = new TcpListener(_ipAddress, _port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Start()
        {
            /*
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
            */

            _webserver.Start();
            _buffer = new byte[1024];
            while (true)
            {
                try
                {
                    object client = _webserver.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(InteractionClient, client);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private void InteractionClient(object clientObj)
        {
            Console.WriteLine("Connected!");
            TcpClient client = (TcpClient) clientObj;
            NetworkStream networkStream = client.GetStream();
            RequestContext request;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int readCount = 0;
                do
                {
                    readCount = networkStream.Read(_buffer, 0, _buffer.Length);
                    memoryStream.Write(_buffer, 0, readCount);
                } while (networkStream.DataAvailable);

                request = new RequestContext(Encoding.ASCII.GetString(memoryStream.ToArray(), 0, (int) memoryStream.Length));
            }
            /*Console.WriteLine("Method: "+request.HttpMethod);
            Console.WriteLine("Version: " + request.HttpVersion);
            Console.WriteLine("Resource: " + request.RequestedResource);
            Console.WriteLine("Content-Length: " + request.ContentLength);
            foreach (var header in request.CustomHeader)
            {
                Console.WriteLine("Header: " + header);
            }
            Console.WriteLine("Content: " + request.Content);*/

            if (request.RequestedResource.StartsWith("/messages"))
            {
                MessageApi api = new MessageApi(request);
                string response = api.Interaction();
                Console.WriteLine(response);
            }
        }
    }
}