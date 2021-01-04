using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWE1_MTCG.Server
{
    public class ResponseContext
    {
        public string HttpVersion { get; private set; }
        public int StatusCode { get; private set; }
        public string StatusString { get; private set; }
        public string Host { get; protected set; }
        public string ContentType { get; private set; }
        public int ContentLength { get; protected set; }
        public string Content { get; protected set; }

        public ResponseContext(RequestContext request, string content)
        {
            HttpVersion = request.HttpVersion;
            ContentType = ((content.StartsWith("{") && content.EndsWith("}")) || (content.StartsWith("[") && content.EndsWith("]"))) ? "application/json" : "text/plain";
            ContentLength = content.Length;
            Content = content;
            if (Content.EndsWith("ERR - No DB connection"))
            {
                StatusCode = 500;
                StatusString = "Internal Server Error";
            }
            else if (Content.StartsWith("ERR") || Content.StartsWith("POST ERR"))
            {
                StatusCode = 400;
                StatusString = "Bad Request";
            }
            else if ((Content.StartsWith("GET") || Content.StartsWith("PUT") || Content.StartsWith("DELETE")) && Content.EndsWith("ERR"))
            {
                StatusCode = 404;
                StatusString = "Not Found";
            }
            else if (Content.EndsWith("ERR - No authorization header") || Content.EndsWith("ERR - Not logged in") || Content.EndsWith("ERR - Login failed") || Content.EndsWith("ERR - No admin rights"))
            {
                StatusCode = 401;
                StatusString = "Unauthorized";
            }
            else if (Content.Equals("Method ERR") || Content.Equals("Resource ERR"))
            {
                StatusCode = 501;
                StatusString = "Not Implemented";
            }
            else if (Content.StartsWith("POST OK"))
            {
                StatusCode = 201;
                StatusString = "Created";
            }
            else
            {
                StatusCode = 200;
                StatusString = "OK";
            }

            Host = "localhost:11000";
            string value = "";
            if (request.CustomHeader.TryGetValue("Host", out value))
            {
                Host = value;
            }
        }

        public string ResponseToString()
        {
            string response = "";

            response += HttpVersion + " " + StatusCode + " " + StatusString + "\n";
            response += "Via: Florian Weiss SWE1-MTCG-Server\n";
            response += "Content-type: " + ContentType + "\n";
            response += "Content-length: " + ContentLength.ToString() + "\n\n";
            response += Content;

            return response;
        }
    }
}
