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
            Trade trade = new Trade(new User("testUser", "geheim"), null, null, 0);

            _tradeService.Setup(s => s.AddTrade(trade)).Returns(true);

            _tradeController.AddTrade(trade);

            _tradeService.Verify(s => s.AddTrade(trade), Times.Never);
        }

        [Test]
        public void TestNotTradingWhenRequestedDamageNoMatched()
        {
            Card wizard = new Wizard("wizhar", "Harry Potter", 75, ElementType.Normal);
            Trade trade = new Trade(new User("me", "secret"), wizard, "dragon", 80);

            Card dragon = new Dragon("dravis", "Viserion", 70, ElementType.Fire);

            _tradeService.Setup(s => s.TradeCards(trade, dragon));

            _tradeController.TradeCards(trade, dragon);

            _tradeService.Verify(s => s.TradeCards(trade, dragon), Times.Never);
        }

        [Test]
        public void TestNotTradingWhenRequestedTypeNoMatched()
        {
            Card wizard = new Wizard("wizhar", "Harry Potter", 75, ElementType.Normal);
            Trade trade = new Trade(new User("me", "secret"), wizard, "knight", 80);

            Card dragon = new Dragon("dravis", "Viserion", 90, ElementType.Water);

            _tradeService.Setup(s => s.TradeCards(trade, dragon));

            _tradeController.TradeCards(trade, dragon);

            _tradeService.Verify(s => s.TradeCards(trade, dragon), Times.Never);
        }

        [Test]
        public void TestTradingWhenRequestedMatched()
        {
            Card wizard = new Wizard("wizhar", "Harry Potter", 75, ElementType.Normal);
            Trade trade = new Trade(new User("me", "secret"), wizard, "dragon", 70);

            Card dragon = new Dragon("dravis", "Viserion", 90, ElementType.Normal);

            _tradeService.Setup(s => s.TradeCards(trade, dragon));

            _tradeController.TradeCards(trade, dragon);

            _tradeService.Verify(s => s.TradeCards(trade, dragon), Times.Once);
        }
    }
}
