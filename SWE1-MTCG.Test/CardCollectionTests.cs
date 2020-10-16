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
        [Test]
        public void TestCardStackGetsCardsFromPackage()
        {
            // Arrange
            CardStack stack = new CardStack();

            // Act
            stack.GetPackage();

            // Assert
            Assert.IsNotNull(stack.CardCollection);
            Assert.AreEqual("Gandalf", stack.CardCollection[0].Name);
        }

        [Test]
        public void TestCardStackGetsRightNumberOfCardsFromPackage()
        {
            // Arrange
            CardStack stack = new CardStack();

            // Act
            // Get 5 cards 4 times
            stack.GetPackage();
            stack.GetPackage();
            stack.GetPackage();
            stack.GetPackage();

            // Assert
            Assert.AreEqual(20, stack.CardCollection.Count);
        }

        [Test]
        public void TestCardStackGetsRightCardsFromPackage()
        {
            // Arrange
            CardStack stack = new CardStack();

            // Act
            stack.GetPackage();

            // Assert
            Assert.AreEqual("Gandalf",stack.CardCollection[0].Name);
        }

        [Test]
        public void TestCardDeckGetsMaximumFourCards()
        {
            // Arrange
            CardStack stack = new CardStack();
            CardDeck deck = new CardDeck();

            // Act
            stack.GetPackage();
            stack.GetPackage();
            deck.AddCard(stack.CardCollection[0]);
            deck.AddCard(stack.CardCollection[1]);
            deck.AddCard(stack.CardCollection[2]);
            deck.AddCard(stack.CardCollection[3]);

            deck.RemoveCard(deck.CardCollection[2]);
            deck.AddCard(stack.CardCollection[4]);

            // Assert
            Assert.AreEqual(4, deck.CardCollection.Count);
        }

        [Test]
        public void TestGetRandomCardFromDeck()
        {
            // Arrange
            CardStack stack = new CardStack();
            CardDeck deck = new CardDeck();

            // Act
            stack.GetPackage();
            deck.AddCard(stack.CardCollection[0]);
            deck.AddCard(stack.CardCollection[1]);
            deck.AddCard(stack.CardCollection[2]);
            deck.AddCard(stack.CardCollection[3]);
            Card randomCard =  deck.GetRandomCard();

            // Assert
            Assert.NotNull(randomCard);
            Console.WriteLine("The name of the random Monster/Spell is: " + randomCard.Name);
        }
    }
}
