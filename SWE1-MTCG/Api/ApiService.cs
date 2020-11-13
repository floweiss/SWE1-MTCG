using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class ApiService : IApiService
    {
        private IFileService _fileService = new FileService();
        private Regex _messageRegex = new Regex(@"^/messages/?\d*$");
        private string _workingDir = Directory.GetCurrentDirectory();

        public IApi GetApi(RequestContext request)
        {
            if (_messageRegex.IsMatch(request.RequestedResource))
            {
                _workingDir += "\\messages";
                return new MessageApi(request, _messageRegex, _fileService, _workingDir);
            }

            return null;
        }
    }
}
