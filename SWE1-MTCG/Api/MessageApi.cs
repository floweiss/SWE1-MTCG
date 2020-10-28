using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Api
{
    public class MessageApi
    {
        private RequestContext _request;

        public MessageApi(RequestContext request)
        {
            _request = request;
        }

        public string Interaction()
        {
            string workingDir = Directory.GetCurrentDirectory()+"\\messages";
            if (!Directory.Exists(workingDir))
            {
                Directory.CreateDirectory(workingDir);
            }

            // FileNumber
            string numberFile = workingDir + "\\Number.txt";
            string text = null;
            if (File.Exists(numberFile))
            {
                text = System.IO.File.ReadAllText(numberFile);
            }
            
            int messageNumber;
            if (text != null)
            {
                messageNumber = int.Parse(text);
            }
            else
            {
                messageNumber = 0;
            }


            // HTTPMethods
            switch (_request.HttpMethod)
            {
                case "POST":
                    string newFile = workingDir + "\\" + messageNumber.ToString() + ".txt";
                    System.IO.File.WriteAllText(newFile, _request.Content);
                    messageNumber++;
                    return "POST OK";
                case "GET":
                    if (_request.RequestedResource.EndsWith("messages") ||
                        _request.RequestedResource.EndsWith("messages/"))
                    {
                        string files = "";
                        foreach (var filename in Directory.GetFiles(workingDir))
                        {
                            if (filename != "Number.txt")
                            {
                                files = files + filename + "\n";
                            }
                        }

                        return files;
                    }
                    else
                    {
                        string requestedResource;
                        if (_request.RequestedResource.EndsWith("/"))
                        {
                            requestedResource = _request.RequestedResource.Remove(_request.RequestedResource.Length - 1, 1);
                        }
                        else
                        {
                            requestedResource = _request.RequestedResource;
                        }
                        string readMessageNumber = requestedResource.Substring(requestedResource.Length-1);

                        return System.IO.File.ReadAllText(workingDir + "\\" + readMessageNumber + ".txt");
                    }
                default:
                    return "Nothing happened";
            }


            System.IO.File.WriteAllText(numberFile, messageNumber.ToString());
        }
    }
}
