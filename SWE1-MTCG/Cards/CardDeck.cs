using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using Random = System.Random;

namespace SWE1_MTCG.Cards
{
    public class CardDeck : CardStack
    {
      
        public Card GetRandomCard()
        {
            Random rnd = new Random();
            int randomCardIndex = rnd.Next(_cardCollection.Count);
            return _cardCollection[randomCardIndex];
        }
    }
}
