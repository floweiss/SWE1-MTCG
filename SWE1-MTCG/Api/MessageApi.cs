using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class MessageApi : IApi
    {
        private RequestContext _request;
        private Regex _regex;
        private IFileService _fileService;
        private string _workingDir;

        public MessageApi(RequestContext request, Regex regex, IFileService fileService, string workingDir)
        {
            _request = request;
            _regex = regex;
            _fileService = fileService;
            _workingDir = workingDir;
        }

        public string Interaction()
        {
            _fileService.CreateDirectory(_workingDir);

            // HTTPMethods
            switch (_request.HttpMethod)
            {
                case "POST":
                    return PostMethod();

                case "GET":
                    return GetMethod();

                case "PUT":
                    return PutMethod();

                case "DELETE":
                    return DeleteMethod();

                default:
                    return "Method ERR";
            }
        }

        public string PostMethod()
        {
            if (_request.RequestedResource.EndsWith("messages") ||
                _request.RequestedResource.EndsWith("messages/"))
            {
                // FileNumber
                int messageNumber;
                lock (this)
                {
                    string numberFile = _workingDir + "\\Number.txt";
                    string text = null;
                    if (_fileService.FileExists(numberFile))
                    {
                        text = _fileService.ReadFromFile(numberFile);
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

                    string newFile = _workingDir + "\\" + messageNumber.ToString() + ".txt";
                    _fileService.WriteToFile(newFile, _request.Content);
                    messageNumber++;
                    _fileService.WriteToFile(numberFile, messageNumber.ToString());
                }

                return "POST OK - ID: " + (messageNumber - 1);
            }
            return "POST ERR";
        }

        public string GetMethod()
        {
            if (_request.RequestedResource.EndsWith("messages") ||
                        _request.RequestedResource.EndsWith("messages/"))
            {
                return _fileService.ReadAllFilesInDir(_workingDir);
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
                    string filename = _workingDir + "\\" + readMessageNumber + ".txt";
                    if (_fileService.FileExists(filename))
                    {
                        return _fileService.ReadFromFile(filename);
                    }
                }

                return "GET ERR";
            }
            return "GET ERR";
        }

        public string PutMethod()
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
                    string filenamePut = _workingDir + "\\" + readMessageNumberPut + ".txt";
                    if (_fileService.FileExists(filenamePut))
                    {
                        _fileService.WriteToFile(filenamePut, _request.Content);
                        return "PUT OK";
                    }
                }

                return "PUT ERR";
            }
            return "PUT ERR";
        }

        public string DeleteMethod()
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
                    string filenameDel = _workingDir + "\\" + readMessageNumberDel + ".txt";
                    if (_fileService.FileExists(filenameDel))
                    {
                        _fileService.DeleteFile(filenameDel);
                        return "DELETE OK";
                    }
                }

                return "DELETE ERR";
            }
            return "DELETE ERR";
        }
    }
}
