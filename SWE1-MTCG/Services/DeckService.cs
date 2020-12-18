using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using System.Configuration;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Services
{
    public class DeckService : IDeckService
    {
        private IUserDataService _userDataService = new UserDataService();
        private string _cs = "Host=localhost;Username=postgres;Password=postgres123;Database=postgres";

        public string ShowDeck(string usertoken, bool showPlain)
        {
            User user = null;
            List<string> cardList = new List<string>();
            if (ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                ClientSingleton.GetInstance.ClientMap.TryGetValue(usertoken, out user);
            }

            foreach (var card in user.Deck.CardCollection)
            {
                cardList.Add(card.ToCardString());
            }

            if (showPlain)
            {
                string plainText = "";
                foreach (var card in cardList)
                {
                    plainText += (card + "\n");
                }

                return plainText;
            }
            return JsonSerializer.Serialize(cardList);
        }

        public string ConfigureDeck(string usertoken, List<string> cardIds, bool updateGranted)
        {
            User user = null;
            if (ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                ClientSingleton.GetInstance.ClientMap.TryGetValue(usertoken, out user);
            }

            if (user.Deck.CardCollection.Count != 0 && !updateGranted)
            {
                return "POST ERR - Deck already configured";
            }
            if (cardIds.Count !=4)
            {
                string message = updateGranted ? "PUT" : "POST";
                return message + " ERR - Add 4 Cards to your Deck";
            }

            Card card;
            List<Card> cardsToAdd = new List<Card>();
            foreach (var cardId in cardIds)
            {
                card = user.Stack.GetCard(cardId);
                if ((card == null)) // || user.Deck.IsCardInDeck(card)) -- does not matter if card already in Deck
                {
                    string message = updateGranted ? "PUT" : "POST";
                    return message + " ERR - At least one CardID not found in Stack"; // or Card already in Deck";
                }
                cardsToAdd.Add(card);
            }

            if (updateGranted)
            {
                user.Deck.CardCollection.Clear();
                foreach (var cardToAdd in cardsToAdd)
                {
                    user.Stack.AddCard(cardToAdd);
                }
            }

            foreach (var cardToAdd in cardsToAdd)
            {
                user.Deck.AddCard(cardToAdd);
                user.Stack.RemoveCard(cardToAdd);
            }

            _userDataService.PersistUserData(user, usertoken);
            string messageReturn = updateGranted ? "PUT" : "POST";
            return messageReturn + " OK - Cards added to Deck";
        }
    }
}
