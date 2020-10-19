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
                    _arenaServiceMock.Verify(s => s.VerifyWinner(_user1), Times.Once);
                    break;
                case -1:
                    _arenaServiceMock.Verify(s => s.VerifyWinner(_user2), Times.Once);
                    break;
                case 0:
                    _arenaServiceMock.Verify(s => s.VerifyWinner(_user1), Times.Never);
                    _arenaServiceMock.Verify(s => s.VerifyWinner(_user2), Times.Never);
                    break;
            }
        }
    }
}
