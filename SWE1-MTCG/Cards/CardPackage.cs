using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards.Monsters;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards
{
    public class CardPackage : ICardCollection
    {
        public List<Card> CardCollection;

        public CardPackage()
        {
            CardCollection = new List<Card>();
            GetRandomCards();
        }

        public void AddCard(Card addedCard)
        {
            CardCollection.Add(addedCard);
        }

        public void RemoveCard(Card removedCard)
        {
            CardCollection.Remove(removedCard);
        }

        private void GetRandomCards()
        {
            // ToDo: Randomize Cards
            CardCollection.AddRange(new List<Card>{
                new Wizard("Gandalf", 100, ElementType.Normal),
                new Orc("Burul", 80, ElementType.Normal),
                new Elve("Erlan Erhice", 50, ElementType.Fire),
                new Dragon("Balrog", 150, ElementType.Water),
                new WaterSpell("Water Whirl", 70)
            });
        }
    }
}
