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
    public class DeckApi : IApi
    {
        private RequestContext _request;
        private DeckController _deckController;
        private CardIdsDTO _cardIds;

        public DeckApi(RequestContext request)
        {
            _request = request;
            try
            {
                _cardIds = JsonSerializer.Deserialize<CardIdsDTO>(_request.Content);
            }
            catch (Exception e)
            {
                _cardIds = null;
            }
            IDeckService deckService = new DeckService();
            _deckController = new DeckController(deckService);

        }

        public string Interaction()
        {
            switch (_request.HttpMethod)
            {
                case "POST":
                    return PostMethod();

                case "GET":
                    return GetMethod();

                case "PUT":
                    return PutMethod();

                default:
                    return "Method ERR";
            }
        }

        public string PostMethod()
        {
            if (!_request.CustomHeader.ContainsKey("Content-Type") || _request.CustomHeader["Content-Type"] != "application/json" || _cardIds == null)
            {
                return "POST ERR - Request not in JSON Format";
            }
            else if (_cardIds.CardIds.Count == 0 || _cardIds.CardIds.Count > 4)
            {
                return "POST ERR - Zero or too many (max. 4) CardIDs in Request";
            }

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

            return _deckController.ConfigureDeck(usertoken, _cardIds, false);
        }

        public string GetMethod()
        {
            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "GET ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                return "GET ERR - Not logged in";
            }

            return _deckController.ShowDeck(usertoken, _request.RequestedResource.EndsWith("?format=plain"));
        }

        public string PutMethod()
        {
            if (!_request.CustomHeader.ContainsKey("Content-Type") || _request.CustomHeader["Content-Type"] != "application/json" || _cardIds == null)
            {
                return "PUT ERR - Request not in JSON Format";
            }
            else if (_cardIds.CardIds.Count == 0 || _cardIds.CardIds.Count > 4)
            {
                return "PUT ERR - Zero or too many (max. 4) CardIDs in Request";
            }

            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "PUT ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                return "PUT ERR - Not logged in";
            }

            return _deckController.ConfigureDeck(usertoken, _cardIds, true);
        }

        public string DeleteMethod()
        {
            throw new NotImplementedException();
        }
    }
}
