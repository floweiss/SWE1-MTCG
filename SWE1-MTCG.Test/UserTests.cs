﻿using NUnit.Framework;
using Moq;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards.Monsters;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Services;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Server;


namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class UserTests
    {
        private User _user;
        private UserController _userController;
        private Mock<IUserService> _userService;

        [SetUp]
        public void SetUp()
        {
            _user = new User("testuser", "supergeheim");
            _userService = new Mock<IUserService>();
            _userController = new UserController(_userService.Object);
        }

        /*[Test]
        public void TestClientSingletonHasOneUserAfterLogin()
        {
            _userService.Setup(s => s.IsRegistered(_user)).Returns(true);
            _userService.Setup(s => s.Login(_user)).Returns("POST OK - Authentication-Token: testuser-mtcgToken");
            
            string result = _userController.Login("testuser", "supergeheim");

            Console.WriteLine(result);
            Assert.AreEqual(1, ClientSingleton.GetInstance.ClientMap.Count);
        }*/
    }
}
