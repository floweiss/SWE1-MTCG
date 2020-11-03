using System;
using System.Collections.Generic;
using System.Text;

namespace SWE1_MTCG.Server
{
    public class ResponseContext
    {
        public string HttpVersion { get; private set; }
        public int StatusCode { get; private set; }
        public string StatusString { get; private set; }
        public string ContentType { get; private set; }
        public int ContentLength { get; protected set; }
        public string Content { get; protected set; }
        public ResponseContext(RequestContext request, string content)
        {
            HttpVersion = request.HttpVersion;
            ContentType = "plain/text";
            ContentLength = content.Length;
            Content = content;
            if (Content.StartsWith("ERR"))
            {
                StatusCode = 400;
                StatusString = "Bad Request";
            }
            else if ((Content.StartsWith("GET") || Content.StartsWith("PUT") || Content.StartsWith("DELETE")) && Content.EndsWith("ERR"))
            {
                StatusCode = 404;
                StatusString = "Not Found";
            }
            else if (Content.StartsWith("POST OK") || Content == "PUT OK")
            {
                StatusCode = 201;
                StatusString = "Created";
            }
            else
            {
                StatusCode = 200;
                StatusString = "OK";
            }
        }

        public string ResponseToString()
        {
            string response = "";

            response += HttpVersion + " " + StatusCode + " " + StatusString + "\n";
            response += "Via: " + HttpVersion + " localhost:11000\n";
            response += "Content Type: " + ContentType + "\n";
            response += "Content Length: " + ContentLength.ToString() + "\n\n";
            response += Content;

            return response;
        }
    }
}
