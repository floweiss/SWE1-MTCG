using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Services;

namespace SWE1_MTCG
{
    public class User
    {
        public string Username;
        private string _password;
        public int Coins;
        public CardDeck Deck;
        public CardStack Stack;

        public User(string username, string password)
        {
            Username = username;
            _password = password;
            Coins = 20;
            Deck = new CardDeck();
            Stack = new CardStack();
        }

        public void AddCardsToStack()
        {
            if (Coins > 0)
            {
                Stack.AddPackage();
                Coins -= 5;
            }
        }

        public void RemoveCardFromStack(string cardName)
        {
            Stack.RemoveCardByName(cardName);
        }

        public void AddCardToDeck(string cardName)
        {
            Deck.AddCardByName(Stack, cardName);
        }

        public void RemoveCardFromDeck(string cardName)
        {
            Deck.RemoveCardByName(cardName);
        }
    }
}
