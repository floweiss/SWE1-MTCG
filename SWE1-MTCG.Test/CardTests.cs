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
            Card wizard = new Wizard("wizgan","Gandalf", 100, ElementType.Normal);
            Card orc = new Orc("orcbur","Burul", 80, ElementType.Normal);

            // Act
            bool controlsOrc = ((Wizard)wizard).CanControl(orc);

            // Assert
            Assert.True(controlsOrc);
        }

        [Test]
        public void TestFireElfCanEvadeAttack()
        {
            Card fireElve = new Elve("elverl","Erlan Erhice", 50, ElementType.Fire);
            Card waterDragon = new Dragon("drabal","Balrog", 150, ElementType.Water);

            bool evadesAttack = ((Elve)fireElve).EvadeAttackWhen(waterDragon);

            Assert.True(evadesAttack);
        }

        [Test]
        public void TestNormalElfCannotEvadeAttack()
        {
            Card normalElve = new Elve("elvgri","Grimli Gohan", 60, ElementType.Normal);
            Card waterDragon = new Dragon("drasma","Smaug", 180, ElementType.Water);

            bool evadesAttack = ((Elve)normalElve).EvadeAttackWhen(waterDragon);

            Assert.False(evadesAttack);
        }

        [Test]
        public void TestKnightDrownsAgainstWaterSpell()
        {
            Card knight = new Knight("kniudo","Udona the Relentless", 120, ElementType.Normal);
            Card waterSpell = new WaterSpell("watwat","Water Whirl", 70);

            bool drowns = ((Knight)knight).DrownsWhen(waterSpell);

            Assert.True(drowns);
        }

        [Test]
        public void TestKrakenImmuneToSpells()
        {
            Card kraken = new Kraken("kragli","Glindat", 140, ElementType.Water);
            Card waterSpell = new WaterSpell("watsta","Static Shower", 60);
            Card fireSpell = new FireSpell("firfir","Fire Storm", 80);
            Card normalSpell = new NormalSpell("norsco","Scorch", 70);

            bool immuneWater = ((Kraken)kraken).ImmuneTo(waterSpell);
            bool immuneFire = ((Kraken)kraken).ImmuneTo(fireSpell);
            bool immuneNormal = ((Kraken)kraken).ImmuneTo(normalSpell);

            Assert.True(immuneWater && immuneFire && immuneNormal);
        }

        [Test]
        public void TestGoblinTooAfraidToAttack()
        {
            Card goblin = new Goblin("gobdre","Drevrul", 55, ElementType.Normal);
            Card dragon = new Dragon("draror","Rordirro", 70, ElementType.Fire);

            bool afraid = ((Goblin)goblin).AfraidOf(dragon);

            Assert.True(afraid);
        }
    }
}
