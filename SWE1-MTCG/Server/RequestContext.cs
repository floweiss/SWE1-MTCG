using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SWE1_MTCG.Server
{
    public class RequestContext
    {
        public string HttpMethod { get; private set; }
        public string HttpVersion { get; private set; }
        public string RequestedResource { get; private set; }

        public RequestContext(string request)
        {
            // Split Request on \r\n (linebreak)
            string[] spliced = request.Split("\r\n");

            string[] firstLine = spliced[0].Split(' ');
            HttpMethod = firstLine[0];
            HttpVersion = firstLine[2];
            RequestedResource = firstLine[1];

            /*
            int firstSpace = request.IndexOf(' ');
            HttpMethod = request.Remove(firstSpace, request.Length-firstSpace);

            int firstH = request.IndexOf('H');
            HttpVersion = request.Remove(0, firstH);
            int firstReturn = HttpVersion.IndexOf('\r');
            HttpVersion = HttpVersion.Remove(firstReturn);*/
        }
    }
}
