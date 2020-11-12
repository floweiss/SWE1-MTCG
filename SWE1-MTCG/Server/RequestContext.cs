using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System;
using Microsoft.VisualBasic.FileIO;

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

            
            int headerNr = 2; 
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

            if (MethodIsAllowed())
            {
                if (HttpMethod != "GET" && HttpMethod != "DELETE")
                {
                    ContentLength = int.Parse(spliced[headerNr].Split(' ')[1]);

                    headerNr += 2;
                    Content = spliced[headerNr];
                    for (int j = headerNr+1; j < spliced.Length; j++)
                    {
                        Content += spliced[j];
                    }
                }
            }
        }

        public bool MethodIsAllowed()
        {
            return ((HttpMethod == "GET") || (HttpMethod == "POST") || (HttpMethod == "PUT") ||
                    (HttpMethod == "DELETE"));
        }
    }
}
