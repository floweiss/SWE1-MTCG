using NUnit.Framework;
using Moq;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Moq.Language.Flow;
using SWE1_MTCG.Cards.Monsters;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Services;
using SWE1_MTCG.Controller;
using SWE1_MTCG.DataTransferObject;


namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class TradeTests
    {
        private Mock<ITradeService> _tradeService;
        private TradeController _tradeController;

        [SetUp]
        public void SetUp()
        {
            _tradeService = new Mock<ITradeService>();
            _tradeController = new TradeController(_tradeService.Object);
        }

        [Test]
        public void TestNotAddingInvalidTrade()
        {
            TradeDTO trade = new TradeDTO("Trade1", "Card1", "spell", 10);

            _tradeService.Setup(s => s.AddTrade(trade, "user-token")).Returns("POST ERR - Trade already exists");

            string result = _tradeController.AddTrade(trade, "user-token");

            Assert.AreEqual("POST ERR - Trade already exists", result);
        }

        [Test]
        public void TestNotTradingWhenRequestedDamageNoMatched()
        {
            Card wizard = new Wizard("wizhar", "Harry Potter", 75, ElementType.Normal);
            TradeDTO trade = new TradeDTO("Trade1", "wizhar", "dragon", 80);

            Card dragon = new Dragon("dravis", "Viserion", 70, ElementType.Fire);

            _tradeService.Setup(s => s.TradeCards("Trade1", "dravis", "user-token")).Returns("POST ERR - Wrong type or too low damage for trade");

            string result = _tradeController.TradeCards("Trade1", "dravis", "user-token");

            Assert.AreEqual("POST ERR - Wrong type or too low damage for trade", result);
        }

        [Test]
        public void TestNotTradingWhenRequestedTypeNoMatched()
        {
            Card wizard = new Wizard("wizhar", "Harry Potter", 75, ElementType.Normal);
            TradeDTO trade = new TradeDTO("Trade1", "wizhar", "knight", 80);

            Card dragon = new Dragon("dravis", "Viserion", 90, ElementType.Water);

            _tradeService.Setup(s => s.TradeCards("Trade1", "dravis", "user-token")).Returns("POST ERR - Wrong type or too low damage for trade") ;

            string result = _tradeController.TradeCards("Trade1", "dravis", "user-token");

            Assert.AreEqual("POST ERR - Wrong type or too low damage for trade", result);
        }

        [Test]
        public void TestTradingWhenRequestedMatched()
        {
            Card wizard = new Wizard("wizhar", "Harry Potter", 75, ElementType.Normal);
            TradeDTO trade = new TradeDTO("Trade1", "wizhar", "dragon", 70);

            Card dragon = new Dragon("dravis", "Viserion", 90, ElementType.Fire);

            _tradeService.Setup(s => s.TradeCards("Trade1", "dravis", "user-token")).Returns("POST OK - cards traded");

            string result = _tradeController.TradeCards("Trade1", "dravis", "user-token");

            Assert.AreEqual("POST OK - cards traded", result);
        }
    }
}
