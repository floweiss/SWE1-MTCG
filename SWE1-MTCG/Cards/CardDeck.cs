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
        public override void AddCard(Card addedCard)
        {
            if (CardCollection.Count < 4)
            {
                CardCollection.Add(addedCard);
            }
            else
            {
                Console.WriteLine("Deck already full, remove one card to add another one!");
            }
        }

        public Card GetRandomCard()
        {
            Random rnd = new Random();
            int randomCardIndex = rnd.Next(CardCollection.Count);
            return CardCollection[randomCardIndex];
        }
    }
}
