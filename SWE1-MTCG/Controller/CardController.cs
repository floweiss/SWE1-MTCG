using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class CardController
    {
        private ICardService _cardService;
        public Card Card { get; set; }

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        public string CreateCard(CardDTO cardDto)
        {
            return _cardService.CreateCard(cardDto);
        }

        public string DeleteCard(CardDTO cardDto)
        {
            return "POST ERR - No delete";
        }
    }
}
