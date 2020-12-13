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

        public string ShowDeck(string usertoken)
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

            return JsonSerializer.Serialize(cardList);
        }

        public string ConfigureDeck(string usertoken, List<string> cardIds)
        {
            User user = null;
            if (ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                ClientSingleton.GetInstance.ClientMap.TryGetValue(usertoken, out user);
            }

            if (user.Deck.CardCollection.Count != 0)
            {
                return "POST ERR - Deck already configured";
            }
            if (cardIds.Count !=4)
            {
                return "POST ERR - Add 4 Cards to your Deck";
            }

            Card card;
            List<Card> cardsToAdd = new List<Card>();
            foreach (var cardId in cardIds)
            {
                card = user.Stack.GetCard(cardId);
                if ((card == null)) // || user.Deck.IsCardInDeck(card)) -- does not matter if card already in Deck
                {
                    return "POST ERR - At least one CardID not found in Stack"; // or Card already in Deck";
                }
                cardsToAdd.Add(card);
            }

            foreach (var cardToAdd in cardsToAdd)
            {
                user.Deck.AddCard(cardToAdd);
                user.Stack.RemoveCard(cardToAdd);
            }

            _userDataService.PersistUserData(user, usertoken);
            return "POST OK - Cards added to Deck";
        }
    }
}
