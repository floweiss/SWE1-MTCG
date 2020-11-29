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
        public void AddCard(Card addedCard, bool inBattle = false)
        {
            if (CardCollection.Count < 4)
            {
                CardCollection.Add(addedCard);
            }
            else if (inBattle)
            {
                CardCollection.Add(addedCard);
            }
            else
            {
                Console.WriteLine("Deck already full, remove one card to add another one!");
            }
        }

        public void AddCardByName(CardStack stack, string cardName)
        {
            foreach (var card in stack.CardCollection)
            {
                if (card.Name == cardName)
                {
                    AddCard(card);
                }
            }
        }

        /*public void RemoveCardByName(string cardName)
        {
            foreach (var card in CardCollection)
            {
                if (card.Name == cardName)
                {
                    RemoveCard(card);
                }
                else
                {
                    Console.WriteLine("You want to remove a card that is not in your deck!");
                }
            }
        }*/

        public Card GetRandomCard()
        {
            Random rnd = new Random();
            int randomCardIndex = rnd.Next(CardCollection.Count);
            return CardCollection[randomCardIndex];
        }
    }
}
