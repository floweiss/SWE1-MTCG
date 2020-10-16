using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using Random = System.Random;

namespace SWE1_MTCG.Cards
{
    public class CardStack : ICardCollection
    {
        public List<Card> _cardCollection { get; set; }

        public CardStack()
        {
            _cardCollection = new List<Card>();
        }

        public void AddCard(Card addedCard)
        {
            _cardCollection.Add(addedCard);
        }

        public void RemoveCard(Card removedCard)
        {
            _cardCollection.Remove(removedCard);
        }

        public void GetPackage()
        {
            CardPackage pack = new CardPackage();
            foreach (var card in pack.cardCollection)
            {
                AddCard(card);
            }
        }
    }
}
