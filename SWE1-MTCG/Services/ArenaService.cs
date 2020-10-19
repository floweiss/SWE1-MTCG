using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public class ArenaService : IArenaService
    {
        private ElementEffectivenessService _elementEffectivenessService;

        public ArenaService(ElementEffectivenessService elementEffectivenessService)
        {
            _elementEffectivenessService = elementEffectivenessService;
        }

        public int Battle(User user1, User user2)
        {
            CardDeck deck1 = user1.Deck;
            CardDeck deck2 = user2.Deck;
            int roundNumber = 0;

            while (deck1.CardCollection.Count > 0 && deck2.CardCollection.Count > 0 && roundNumber < 100)
            {
                if (roundNumber % 2 == 0)
                {
                    Round(ref deck1, ref deck2);
                }
                else
                {
                    Round(ref deck2, ref deck1);
                }

                roundNumber++;
            }

            if (roundNumber == 100)
            {
                return 0;
            }
            return (deck1.CardCollection.Count > deck2.CardCollection.Count ? 1 : -1);
        }

        public void Round(ref CardDeck deck1, ref CardDeck deck2)
        {
            Card card1 = deck1.GetRandomCard();
            Card card2 = deck2.GetRandomCard();
            card1.Damage = card1.Damage * _elementEffectivenessService.CompareElements(card1.Type, card2.Type);

            // ToDo: implement special abilities

            bool card1wins;
            if (card1 is IMonster)
            {
                card1wins = ((IMonster)card1).CompareDamage(card2.Damage);
            }
            else
            {
                card1wins = ((ISpell)card1).CompareDamage(card2.Damage);
            }

            if (card1wins)
            {
                deck1.AddCard(card2, true);
                deck2.RemoveCard(card2);
            }
            else
            {
                deck2.AddCard(card1, true);
                deck1.RemoveCard(card1);
            }
        }

        public void VerifyWinner(User winner)
        {
            throw new NotImplementedException();
        }
    }
}
