﻿using System;
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

            return _cardController.CreateCard(_card);
        }

        public string GetMethod()
        {
            throw new NotImplementedException();
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
