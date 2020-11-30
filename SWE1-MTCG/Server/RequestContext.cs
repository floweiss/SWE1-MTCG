using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System;
using System.Linq;
using System.Text.Json;
using Microsoft.VisualBasic.FileIO;
using SWE1_MTCG.DataTransferObject;

namespace SWE1_MTCG.Server
{
    public class RequestContext
    {
        public string HttpMethod { get; private set; }
        public string HttpVersion { get; private set; }
        public string RequestedResource { get; private set; }
        public int ContentLength { get; protected set; }
        public string Content { get; protected set; }
        public SortedList<string, string> CustomHeader { get; protected set; }

        public RequestContext(string request)
        {
            // Split Request on \r\n (linebreak)
            string[] spliced = request.Split("\r\n");

            string[] firstLine = spliced[0].Split(' ');
            HttpMethod = firstLine[0];
            HttpVersion = firstLine[2];
            RequestedResource = firstLine[1];

            
            int headerNr = 1; 
            CustomHeader = new SortedList<string, string>(); 
            while (headerNr < spliced.Length)
            {
                if (!spliced[headerNr].StartsWith("Content-Length:"))
                {
                    if (spliced[headerNr] != "")
                    {
                        int location = spliced[headerNr].IndexOf(':');
                        string key = spliced[headerNr].Substring(0, location);
                        string value = spliced[headerNr].Substring((location + 2), (spliced[headerNr].Length-location-2));
                        CustomHeader.Add(key, value);
                    }
                }
                else
                { 
                    break;
                }

                headerNr++;
            }

            if (MethodIsAllowed())
            {
                if (spliced.Length > headerNr)
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

        public string RequestToString()
        {
            string request = "";
            request += HttpMethod + " " + RequestedResource + " " + HttpVersion + "\n";
            foreach (var header in CustomHeader)
            {
                request += (header.Key + ": " + header.Value);
                if (header.Key != CustomHeader.Last().Key)
                {
                    request += "\n";
                }
            }

            if (!String.IsNullOrEmpty(Content))
            {
                request += "\nContent-Length: " + ContentLength + "\n\n";
                request += Content;
            }

            return request;
        }
    }
}
