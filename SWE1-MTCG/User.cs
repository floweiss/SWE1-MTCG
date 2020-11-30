﻿using System;
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
        public string HashedPW;
        public int Coins;
        public CardDeck Deck;
        public CardStack Stack;

        public User(string username, string password)
        {
            Username = username;
            _password = password;
            HashedPW = Hash(password);
            Coins = 20;
            Deck = new CardDeck();
            Stack = new CardStack();
        }

        // https://arcanecode.com/2007/03/21/encoding-strings-to-base64-in-c/
        private string Hash(string stringToHash)
        {
            byte[] bytes = new UTF8Encoding().GetBytes(stringToHash);
            using var algorithm = new System.Security.Cryptography.SHA512Managed();
            var hashBytes = algorithm.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
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
