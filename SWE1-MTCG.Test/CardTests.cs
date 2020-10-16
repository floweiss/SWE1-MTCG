using NUnit.Framework;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards.Monsters;
using SWE1_MTCG.Cards.Spells;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class CardTests
    {
        [Test]
        public void TestWizardCanControlOrk()
        {
            // Arrange
            Card wizard = new Wizard("Gandalf", 100, ElementType.Normal);
            Card orc = new Orc("Burul", 80, ElementType.Normal);

            // Act
            bool controlsOrc = ((Wizard)wizard).ControlOrc(orc);

            // Assert
            Assert.True(controlsOrc);
        }

        [Test]
        public void TestFireElfCanEvadeAttack()
        {
            Card fireElve = new Elve("Erlan Erhice", 50, ElementType.Fire);
            Card waterDragon = new Dragon("Balrog", 150, ElementType.Water);

            bool evadesAttack = ((Elve)fireElve).EvadeAttack(waterDragon);

            Assert.True(evadesAttack);
        }

        [Test]
        public void TestNormalElfCannotEvadeAttack()
        {
            Card normalElve = new Elve("Grimli Gohan", 60, ElementType.Normal);
            Card waterDragon = new Dragon("Smaug", 180, ElementType.Water);

            bool evadesAttack = ((Elve)normalElve).EvadeAttack(waterDragon);

            Assert.False(evadesAttack);
        }

        [Test]
        public void TestKnightDrownsAgainstWaterSpell()
        {
            Card knight = new Knight("Udona the Relentless", 120, ElementType.Normal);
            Card waterSpell = new WaterSpell("Water Whirl", 70);

            bool drowns = ((Knight)knight).Drown(waterSpell);

            Assert.True(drowns);
        }

        [Test]
        public void TestKrakenImmuneToSpells()
        {
            Card kraken = new Kraken("Glindat", 140, ElementType.Water);
            Card waterSpell = new WaterSpell("Static Shower", 60);
            Card fireSpell = new FireSpell("Fire Storm", 80);
            Card normalSpell = new NormalSpell("Scorch", 70);

            bool immuneWater = ((Kraken)kraken).Immune(waterSpell);
            bool immuneFire = ((Kraken)kraken).Immune(fireSpell);
            bool immuneNormal = ((Kraken)kraken).Immune(normalSpell);

            Assert.True(immuneWater && immuneFire && immuneNormal);
        }

        [Test]
        public void TestGoblinTooAfraidToAttack()
        {
            Card goblin = new Goblin("Drevrul", 55, ElementType.Normal);
            Card dragon = new Dragon("Rordirro", 70, ElementType.Fire);

            bool afraid = ((Goblin)goblin).Afraid(dragon);

            Assert.True(afraid);
        }
    }
}
