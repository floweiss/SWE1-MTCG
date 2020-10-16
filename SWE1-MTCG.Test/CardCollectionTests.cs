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
            Assert.IsNotNull(stack);
            Assert.Equals(stack._cardCollection[0].Name, "Gandalf");
        }
    }
}
