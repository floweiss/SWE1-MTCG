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
            // ToDo: Create real card from DTO
            Card card = null;
            return _cardService.CreateCard(card);
        }

        public string DeleteCard(CardDTO cardDto)
        {
            return "POST ERR - No delete";
        }
    }
}
