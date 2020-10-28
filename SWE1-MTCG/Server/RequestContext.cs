using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System;

namespace SWE1_MTCG.Server
{
    public class RequestContext
    {
        public string HttpMethod { get; private set; }
        public string HttpVersion { get; private set; }
        public string RequestedResource { get; private set; }
        public int ContentLength { get; protected set; }
        public string Content { get; protected set; }
        public List<string> CustomHeader { get; protected set; }

        public RequestContext(string request)
        {
            // Split Request on \r\n (linebreak)
            string[] spliced = request.Split("\r\n");

            string[] firstLine = spliced[0].Split(' ');
            HttpMethod = firstLine[0];
            HttpVersion = firstLine[2];
            RequestedResource = firstLine[1];

            if (HttpMethod != "GET")
            {
                int headerNr = 4;
                CustomHeader = new List<string>();
                while (headerNr < spliced.Length)
                {
                    if (!spliced[headerNr].StartsWith("Content-Length:"))
                    {
                        CustomHeader.Add(spliced[headerNr]);
                    }
                    else
                    {
                        break;
                    }
                    headerNr++;
                }

                ContentLength = int.Parse(spliced[headerNr].Split(' ')[1]);

                headerNr += 2;
                Content = spliced[headerNr];
                for (int j = headerNr; j < spliced.Length; j++)
                {
                    Content += spliced[j];
                }
            }

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
