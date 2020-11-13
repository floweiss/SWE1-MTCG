using System;
using System.Collections.Generic;
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

        public IApi GetApi(RequestContext request)
        {
            if (_messageRegex.IsMatch(request.RequestedResource))
            {
                return new MessageApi(request, _messageRegex, _fileService);
            }

            return null;
        }
    }
}
