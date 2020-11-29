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
    public class ArenaTests
    {
        private Mock<IArenaService> _arenaServiceMock;
        private IArenaService _arenaService;
        private ArenaController _arenaController;
        private User _user1;
        private User _user2;
        private Arena _arena;

        [SetUp]
        public void SetUp()
        {
            _arenaServiceMock = new Mock<IArenaService>();
            _arenaController = new ArenaController(_arenaServiceMock.Object);
            _user1 = new User("user1", "supergeheim");
            _user2 = new User("user2", "secretsecret");
            _arena = new Arena(_user1, _user2);
        }

        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestBattleServiceReturnsRightResult(int result)
        {
            _arenaServiceMock.Setup(s => s.Battle(_user1, _user2)).Returns(result);

            _arenaController.StartBattle(_arena);

            switch (result)
            {
                case 1:
                    _arenaServiceMock.Verify(s => s.UpdateUserStats(_user1,_user2), Times.Once);
                    break;
                case -1:
                    _arenaServiceMock.Verify(s => s.UpdateUserStats(_user2, _user1), Times.Once);
                    break;
                case 0:
                    _arenaServiceMock.Verify(s => s.UpdateUserStats(_user1, _user2), Times.Never);
                    _arenaServiceMock.Verify(s => s.UpdateUserStats(_user2, _user1), Times.Never);
                    break;
            }
        }

        [Test]
        public void TestBattleLogic()
        {
            ElementEffectivenessService elementEffectivenessService = new ElementEffectivenessService();
            IArenaService arenaService = new ArenaService(elementEffectivenessService);

            _user1.AddCardsToStack();
            _user2.AddCardsToStack();

            _user1.AddCardToDeck("Gandalf");
            _user1.AddCardToDeck("Burul");
            _user1.AddCardToDeck("Balrog");
            _user1.AddCardToDeck("Water Whirl");

            _user2.AddCardToDeck("Erlan Erhice");
            _user2.AddCardToDeck("Burul");
            _user2.AddCardToDeck("Erlan Erhice");
            _user2.AddCardToDeck("Water Whirl");

            arenaService.Battle(_user1, _user2);
            int user1wins = arenaService.Battle(_user1, _user2);

            Assert.AreEqual(1, user1wins);
        }
    }
}
