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
        public List<Card> CardCollection { get; set; }

        public CardStack()
        {
            CardCollection = new List<Card>();
        }

        public virtual void AddCard(Card addedCard)
        {
            CardCollection.Add(addedCard);
        }

        public void RemoveCard(Card removedCard)
        {
            CardCollection.Remove(removedCard);
        }

        public void RemoveCardByName(string cardName)
        {
            foreach (var card in CardCollection)
            {
                if (card.Name == cardName)
                {
                    RemoveCard(card);
                }
                else
                {
                    Console.WriteLine("The card you want to remove is not in your stack!");
                }
            }
        }

        public void AddPackage()
        {
            CardPackage pack = new CardPackage();
            foreach (var card in pack.CardCollection)
            {
                AddCard(card);
            }
        }
    }
}
