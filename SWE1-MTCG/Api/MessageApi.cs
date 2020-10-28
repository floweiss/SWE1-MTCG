using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            Regex regex = new Regex(@"^/messages/\d+/?$");
            switch (_request.HttpMethod)
            {
                case "POST":
                    if (_request.RequestedResource.EndsWith("messages") ||
                        _request.RequestedResource.EndsWith("messages/"))
                    {
                        string newFile = workingDir + "\\" + messageNumber.ToString() + ".txt";
                        File.WriteAllText(newFile, _request.Content);
                        messageNumber++;
                        File.WriteAllText(numberFile, messageNumber.ToString());
                        return "POST OK - ID: " + (messageNumber-1);
                    }
                    else
                    {
                        return "POST ERR";
                    }

                case "GET":
                    if (_request.RequestedResource.EndsWith("messages") ||
                        _request.RequestedResource.EndsWith("messages/"))
                    {
                        string files = "";
                        foreach (var filename in Directory.GetFiles(workingDir))
                        {
                            if (!filename.EndsWith("Number.txt"))
                            {
                                int lastSlash = filename.LastIndexOf('\\');
                                files = files + filename.Substring(lastSlash+1) + ": " + File.ReadAllText(filename) + "\n";
                            }
                        }

                        return files;
                    }
                    else
                    {
                        if (regex.IsMatch(_request.RequestedResource))
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

                            string filename = workingDir + "\\" + readMessageNumber + ".txt";
                            if (File.Exists(filename))
                            {
                                return File.ReadAllText(filename);
                            }
                            else
                            {
                                return "GET ERR";
                            }
                        }
                        else
                        {
                            return "GET ERR";
                        }
                        
                    }

                case "PUT":
                    if (regex.IsMatch(_request.RequestedResource))
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
                        int lastSlashPut =requestedResourcePut.LastIndexOf('/');
                        string readMessageNumberPut = requestedResourcePut.Substring(lastSlashPut+1);

                        string filenamePut = workingDir + "\\" + readMessageNumberPut + ".txt";
                        if (File.Exists(filenamePut))
                        {
                            File.WriteAllText(filenamePut, _request.Content);
                            return "PUT OK";
                        }
                        else
                        {
                            return "PUT ERR";
                        }
                    }
                    else
                    {
                        return "PUT ERR";
                    }

                case "DELETE":
                    if (regex.IsMatch(_request.RequestedResource))
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

                        string filenameDel = workingDir + "\\" + readMessageNumberDel + ".txt";
                        if (File.Exists(filenameDel))
                        {
                            File.Delete(filenameDel);
                            return "DELETE OK";
                        }
                        else
                        {
                            return "DELETE ERR";
                        }
                    }
                    else
                    {
                        return "DELETE ERR";
                    }

                default:
                    return "Nothing happened";
            }
        }
    }
}
