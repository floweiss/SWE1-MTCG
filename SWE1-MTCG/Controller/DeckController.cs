using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class DeckController
    {
        private IDeckService _deckService;

        public DeckController(IDeckService deckService)
        {
            _deckService = deckService;
        }

        public string ShowDeck(string usertoken, bool showPlain)
        {
            return _deckService.ShowDeck(usertoken, showPlain);
        }

        public string ConfigureDeck(string usertoken, CardIdsDTO cardIds)
        {
            return _deckService.ConfigureDeck(usertoken, cardIds.CardIds);
        }
    }
}
