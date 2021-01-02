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
        private Regex _messageRegex = new Regex(@"^/messages/?\w*$");
        private Regex _userRegex = new Regex(@"^/users/?\w*$");
        private Regex _sessionRegex = new Regex(@"^/sessions/?\w*$");
        private Regex _cardRegex = new Regex(@"^/cards/?\w*$");
        private Regex _packageRegex = new Regex(@"^/packages/?\w*$");
        private Regex _transactionPackageRegex = new Regex(@"^/transactions/packages/?\w*$");
        private Regex _deckRegex = new Regex(@"^/deck[/?]?[\w\D]*$");
        private Regex _statsRegex = new Regex(@"^/stats/?\w*$");
        private Regex _scoreRegex = new Regex(@"^/score/?\w*$");
        private Regex _battleRegex = new Regex(@"^/battles/?\w*$");
        private Regex _tradeRegex = new Regex(@"^/tradings/?[\w-]*$");

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
            if (_deckRegex.IsMatch(request.RequestedResource))
            {
                return new DeckApi(request);
            }
            if (_statsRegex.IsMatch(request.RequestedResource))
            {
                return new StatsApi(request);
            }
            if (_scoreRegex.IsMatch(request.RequestedResource))
            {
                return new ScoreApi(request);
            }
            if (_battleRegex.IsMatch(request.RequestedResource))
            {
                return new ArenaApi(request);
            }
            if (_tradeRegex.IsMatch(request.RequestedResource))
            {
                return new TradeApi(request);
            }

            return null;
        }
    }
}
