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
    public class ElementTypeTests
    {
        private ElementEffectivenessService _elementService;
        private double _effective;
        private double _notEffective;
        private double _noEffect;

        [SetUp]
        public void SetUp()
        {
            _elementService = new ElementEffectivenessService();
            _effective = 2.0;
            _notEffective = 0.5;
            _noEffect = 1.0;
    }

        [Test]
        public void TestNormalIsEffectiveAgainstWater()
        {
            // Arrange
            Card wizard = new Wizard("wizgan","Gandalf", 100, ElementType.Normal);
            Card waterOrc = new Orc("orcbur","Burul", 80, ElementType.Water);

            // Act
            double normalIsEffectiveAgainstWater = _elementService.CompareElements(wizard.Type, waterOrc.Type);

            // Assert
            Assert.AreEqual(_effective, normalIsEffectiveAgainstWater);
        }

        [Test]
        public void TestWaterIsEffectiveAgainstFire()
        {
            // Arrange
            Card fireElve = new Elve("elverl","Erlan Erhice", 70, ElementType.Water);
            Card waterDragon = new Dragon("drabal","Balrog", 130, ElementType.Fire);

            double waterIsEffectiveAgainstFire = _elementService.CompareElements(fireElve.Type, waterDragon.Type);

            // Assert
            Assert.AreEqual(_effective, waterIsEffectiveAgainstFire);
        }

        [Test]
        public void TestNormalIsNotEffectiveAgainstFire()
        {
            // Arrange
            Card wizard = new Wizard("wizgan","Gandalf", 100, ElementType.Normal);
            Card fireDragon = new Dragon("drabal","Balrog", 130, ElementType.Fire);

            double normalIsNotEffectiveAgainstFire = _elementService.CompareElements(wizard.Type, fireDragon.Type);

            // Assert
            Assert.AreEqual(_notEffective, normalIsNotEffectiveAgainstFire);
        }

        [Test]
        public void TestWaterNoEffectOnWater()
        {
            // Arrange
            Card waterSpell = new WaterSpell("watwat","Water Whirl", 70);
            Card waterDragon = new Dragon("drabal","Balrog", 120, ElementType.Water);

            double normalIsNotEffectiveAgainstFire = _elementService.CompareElements(waterSpell.Type, waterDragon.Type);

            // Assert
            Assert.AreEqual(_noEffect, normalIsNotEffectiveAgainstFire);
        }
    }
}
