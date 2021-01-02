using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SWE1_MTCG.Controller;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class TradeApi : IApi
    {
        private RequestContext _request;
        private TradeController _tradeController;
        private TradeDTO _trade;

        public TradeApi(RequestContext request)
        {
            _request = request;
            try
            {
                _trade = JsonSerializer.Deserialize<TradeDTO>(_request.Content);
            }
            catch (Exception e)
            {
                _trade = null;
            }
            ITradeService tradeService = new TradeService();
            _tradeController = new TradeController(tradeService);
        }

        public string Interaction()
        {
            switch (_request.HttpMethod)
            {
                case "POST":
                    return PostMethod();

                case "GET":
                    return GetMethod();

                case "DELETE":
                    return DeleteMethod();

                default:
                    return "Method ERR";
            }
        }

        public string PostMethod()
        {
            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "POST ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                return "POST ERR - Not logged in";
            }

            if (_request.RequestedResource.EndsWith("tradings") || _request.RequestedResource.EndsWith("tradings/"))
            {
                if (!_request.CustomHeader.ContainsKey("Content-Type") || _request.CustomHeader["Content-Type"] != "application/json")
                {
                    return "POST ERR - Request not in JSON Format";
                }
                else if (string.IsNullOrWhiteSpace(_trade.Id) || string.IsNullOrWhiteSpace(_trade.CardToTrade) || string.IsNullOrWhiteSpace(_trade.Type) || _trade.MinimumDamage <= 0)
                {
                    return "POST ERR - Invalid Trade";
                }

                return _tradeController.AddTrade(_trade, usertoken);
            }

            string tradeID =
                _request.RequestedResource.Substring(_request.RequestedResource.LastIndexOf("/") + 1);
            return _tradeController.TradeCards(tradeID, _request.Content, usertoken);
        }

        public string GetMethod()
        {
            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "POST ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                return "POST ERR - Not logged in";
            }

            return _tradeController.GetAllTrades();
        }

        public string PutMethod()
        {
            throw new NotImplementedException();
        }

        public string DeleteMethod()
        {
            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "DELETE ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                return "DELETE ERR - Not logged in";
            }

            if (_request.RequestedResource.EndsWith("tradings") || _request.RequestedResource.EndsWith("tradings/"))
            {
                return "DELETE ERR - No tradeID in request";
            }

            string tradeID =
                _request.RequestedResource.Substring(_request.RequestedResource.LastIndexOf("/") + 1);
            return _tradeController.RemoveTrade(tradeID, usertoken);
        }
    }
}
