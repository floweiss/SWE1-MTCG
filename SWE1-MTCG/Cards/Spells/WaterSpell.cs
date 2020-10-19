using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Spells
{
    public class WaterSpell : Card, ISpell
    {
        public WaterSpell(string name, double damage)
        {
            Name = name;
            Damage = damage;
            Type = ElementType.Water;
        }

        public bool CompareDamage(double damage)
        {
            return Damage > damage;
        }
    }
}
