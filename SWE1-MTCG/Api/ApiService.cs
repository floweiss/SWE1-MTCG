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
        private Regex _userRegex = new Regex(@"^/users/?\d*$");
        private Regex _sessionRegex = new Regex(@"^/sessions/?\d*$");
        private Regex _cardRegex = new Regex(@"^/cards/?\d*$");
        private Regex _packageRegex = new Regex(@"^/packages/?\d*$");
        private Regex _transactionPackageRegex = new Regex(@"^/transactions/packages/?\d*$");

        public IApi GetApi(RequestContext request)
        {
            if (_messageRegex.IsMatch(request.RequestedResource))
            {
                return new MessageApi(request, _messageRegex, _fileService, Directory.GetCurrentDirectory()+ "\\messages");
            }
            if (_userRegex.IsMatch(request.RequestedResource))
            {
                return new UserApi(request);
            }
            if (_sessionRegex.IsMatch(request.RequestedResource))
            {
                return new SessionApi(request);
            }
            if (_cardRegex.IsMatch(request.RequestedResource))
            {
                return new CardApi(request);
            }
            if (_packageRegex.IsMatch(request.RequestedResource))
            {
                return new PackageApi(request);
            }
            if (_transactionPackageRegex.IsMatch(request.RequestedResource))
            {
                return new TransactionPackageApi(request);
            }

            return null;
        }
    }
}
