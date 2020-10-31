using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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
            Console.WriteLine("Connected with client!");
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
                Console.WriteLine("Method called: " + request.HttpMethod);
            }

            byte[] responseBuffer;
            string responseString;
            Regex regex = new Regex(@"^/messages/?\d*$");
            if (regex.IsMatch(request.RequestedResource))
            {
                MessageApi api = new MessageApi(request);
                responseString = api.Interaction();
            }
            else
            {
                responseString = "ERR";
            }

            ResponseContext response = new ResponseContext(request, responseString);
            using StreamWriter writer = new StreamWriter(networkStream) { AutoFlush = true };
            client.Client.Send(Encoding.ASCII.GetBytes(response.ResponseToString()));
            Console.WriteLine("Client disconnected!\n");

            client.Close();
        }
    }
}
