using NUnit.Framework;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards.Monsters;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class CardCollectionTests
    {
        private List<Card> _pack;
        private CardPackage _package;

        [SetUp]
        public void SetUp()
        {
            _pack = new List<Card>{
                new Wizard("wizgan","Gandalf", 100, ElementType.Normal),
                new Orc("orcbur","Burul", 80, ElementType.Normal),
                new Elve("elverl","Erlan Erhice", 50, ElementType.Fire),
                new Dragon("drabal","Balrog", 150, ElementType.Water),
                new WaterSpell("watwat","Water Whirl", 70)
            };
            _package = new CardPackage();
            foreach (var card in _pack)
            {
                _package.AddCard(card);
            }
        }

        [Test]
        public void TestCardStackGetsCardsFromPackage()
        {
            // Arrange
            CardStack stack = new CardStack();

            // Act
            stack.AddPackage(_package);

            // Assert
            Assert.IsNotNull(stack.CardCollection);
            Assert.AreEqual("Gandalf", stack.CardCollection[0].Name);
        }

        [Test]
        public void TestCardStackGetsRightNumberOfCardsFromPackage()
        {
            CardStack stack = new CardStack();

            // Get 5 cards 4 times
            stack.AddPackage(_package);
            stack.AddPackage(_package);
            stack.AddPackage(_package);
            stack.AddPackage(_package);

            Assert.AreEqual(20, stack.CardCollection.Count);
        }

        [Test]
        public void TestCardStackGetsRightCardsFromPackage()
        {
            CardStack stack = new CardStack();

            stack.AddPackage(_package);

            Assert.AreEqual("Gandalf",stack.CardCollection[0].Name);
        }

        [Test]
        public void TestCardDeckGetsMaximumFourCards()
        {
            CardStack stack = new CardStack();
            CardDeck deck = new CardDeck();

            stack.AddPackage(_package);
            stack.AddPackage(_package);
            deck.AddCard(stack.CardCollection[0]);
            deck.AddCard(stack.CardCollection[1]);
            deck.AddCard(stack.CardCollection[2]);
            deck.AddCard(stack.CardCollection[3]);

            deck.RemoveCard(deck.CardCollection[2]);
            deck.AddCard(stack.CardCollection[4]);

            Assert.AreEqual(4, deck.CardCollection.Count);
        }

        [Test]
        public void TestGetRandomCardFromDeck()
        {
            CardStack stack = new CardStack();
            CardDeck deck = new CardDeck();

            stack.AddPackage(_package);
            deck.AddCard(stack.CardCollection[0]);
            deck.AddCard(stack.CardCollection[1]);
            deck.AddCard(stack.CardCollection[2]);
            deck.AddCard(stack.CardCollection[3]);
            Card randomCard =  deck.GetRandomCard();

            Assert.NotNull(randomCard);
            Console.WriteLine("The name of the random Monster/Spell is: " + randomCard.Name);
        }
    }
}
