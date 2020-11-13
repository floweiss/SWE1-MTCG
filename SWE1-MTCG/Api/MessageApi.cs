using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Api
{
    public class MessageApi : IApi
    {
        private RequestContext _request;
        private Regex _regex;

        public MessageApi(RequestContext request, Regex regex)
        {
            _request = request;
            _regex = regex;
        }

        public string Interaction()
        {
            string workingDir = Directory.GetCurrentDirectory()+"\\messages";
            if (!Directory.Exists(workingDir))
            {
                Directory.CreateDirectory(workingDir);
            }

            // HTTPMethods
            switch (_request.HttpMethod)
            {
                case "POST":
                    return PostMethod(workingDir);

                case "GET":
                    return GetMethod(workingDir);

                case "PUT":
                    return PutMethod(workingDir);

                case "DELETE":
                    return DeleteMethod(workingDir);

                default:
                    return "Method ERR";
            }
        }

        public string PostMethod(string workingDir)
        {
            if (_request.RequestedResource.EndsWith("messages") ||
                _request.RequestedResource.EndsWith("messages/"))
            {
                // FileNumber
                int messageNumber;
                lock (this)
                {
                    string numberFile = workingDir + "\\Number.txt";
                    string text = null;
                    if (File.Exists(numberFile))
                    {
                        text = File.ReadAllText(numberFile);
                    }

                    if (text != null)
                    {
                        try
                        {
                            messageNumber = int.Parse(text);
                        }
                        catch (Exception e)
                        {
                            messageNumber = 0;
                            //throw;
                        }
                    }
                    else
                    {
                        messageNumber = 0;
                    }

                    string newFile = workingDir + "\\" + messageNumber.ToString() + ".txt";
                    File.WriteAllText(newFile, _request.Content);
                    messageNumber++;
                    File.WriteAllText(numberFile, messageNumber.ToString());
                }

                return "POST OK - ID: " + (messageNumber - 1);
            }
            return "POST ERR";
        }

        public string GetMethod(string workingDir)
        {
            if (_request.RequestedResource.EndsWith("messages") ||
                        _request.RequestedResource.EndsWith("messages/"))
            {
                string files = "";
                lock (this)
                {
                    foreach (var filename in Directory.GetFiles(workingDir))
                    {
                        if (!filename.EndsWith("Number.txt"))
                        {
                            int lastSlash = filename.LastIndexOf('\\');
                            files = files + filename.Substring(lastSlash + 1) + ": " + File.ReadAllText(filename) +
                                    "\n";
                        }
                    }
                }

                return files;
            }
            if (_regex.IsMatch(_request.RequestedResource))
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
                int lastSlash = requestedResource.LastIndexOf('/');
                string readMessageNumber = requestedResource.Substring(lastSlash + 1);

                lock (this)
                {
                    string filename = workingDir + "\\" + readMessageNumber + ".txt";
                    if (File.Exists(filename))
                    {
                        return File.ReadAllText(filename);
                    }
                }

                return "GET ERR";
            }
            return "GET ERR";
        }

        public string PutMethod(string workingDir)
        {
            if (_regex.IsMatch(_request.RequestedResource))
            {
                string requestedResourcePut;
                if (_request.RequestedResource.EndsWith("/"))
                {
                    requestedResourcePut = _request.RequestedResource.Remove(_request.RequestedResource.Length - 1, 1);
                }
                else
                {
                    requestedResourcePut = _request.RequestedResource;
                }
                int lastSlashPut = requestedResourcePut.LastIndexOf('/');
                string readMessageNumberPut = requestedResourcePut.Substring(lastSlashPut + 1);

                lock(this) {
                    string filenamePut = workingDir + "\\" + readMessageNumberPut + ".txt";
                    if (File.Exists(filenamePut))
                    {
                        File.WriteAllText(filenamePut, _request.Content);
                        return "PUT OK";
                    }
                }

                return "PUT ERR";
            }
            return "PUT ERR";
        }

        public string DeleteMethod(string workingDir)
        {
            if (_regex.IsMatch(_request.RequestedResource))
            {
                string requestedResourceDel;
                if (_request.RequestedResource.EndsWith("/"))
                {
                    requestedResourceDel = _request.RequestedResource.Remove(_request.RequestedResource.Length - 1, 1);
                }
                else
                {
                    requestedResourceDel = _request.RequestedResource;
                }
                int lastSlashDel = requestedResourceDel.LastIndexOf('/');
                string readMessageNumberDel = requestedResourceDel.Substring(lastSlashDel + 1);

                lock (this)
                {
                    string filenameDel = workingDir + "\\" + readMessageNumberDel + ".txt";
                    if (File.Exists(filenameDel))
                    {
                        File.Delete(filenameDel);
                        return "DELETE OK";
                    }
                }

                return "DELETE ERR";
            }
            return "DELETE ERR";
        }
    }
}
