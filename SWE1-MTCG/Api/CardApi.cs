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
    public class CardApi : IApi
    {
        private RequestContext _request;
        private CardController _cardController;
        private CardDTO _card;

        public CardApi(RequestContext request)
        {
            _request = request;
            try
            {
                _card = JsonSerializer.Deserialize<CardDTO>(_request.Content);
            }
            catch (Exception e)
            {
                _card = null;
            }
            ICardService cardService = new CardService();
            _cardController = new CardController(cardService);
        }

        public string Interaction()
        {
            switch (_request.HttpMethod)
            {
                case "POST":
                    return PostMethod();

                case "GET":
                    return GetMethod();

                default:
                    return "Method ERR";
            }
        }

        public string PostMethod()
        {
            if (!_request.CustomHeader.ContainsKey("Content-Type") || _request.CustomHeader["Content-Type"] != "application/json")
            {
                return "POST ERR - Request not in JSON Format";
            }
            else if (string.IsNullOrWhiteSpace(_card.Name) || string.IsNullOrWhiteSpace(_card.Id) || string.IsNullOrWhiteSpace(_card.CardType) || _card.Damage <= 0)
            {
                return "POST ERR - No valid Card";
            }
            else if (_card.CardType.ToLower() != "dragon" && _card.CardType.ToLower() != "elve" && _card.CardType.ToLower() != "goblin" && _card.CardType.ToLower() != "knight" && _card.CardType.ToLower() != "kraken" && _card.CardType.ToLower() != "orc" && _card.CardType.ToLower() != "wizard" && _card.CardType.ToLower() != "firespell" && _card.CardType.ToLower() != "normalspell" && _card.CardType.ToLower() != "waterspell")
            {
                return "POST ERR - Invalid card type";
            }

            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "POST ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (usertoken != "admin-mtcgToken")
            {
                return "POST ERR - No admin rights";
            }

            return _cardController.CreateCard(_card);
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

            return _cardController.ShowCards(usertoken);
        }

        public string PutMethod()
        {
            throw new NotImplementedException();
        }

        public string DeleteMethod()
        {
            throw new NotImplementedException();
        }
    }
}
