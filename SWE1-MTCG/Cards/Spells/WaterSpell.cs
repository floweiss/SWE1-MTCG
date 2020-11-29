using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Spells
{
    public class WaterSpell : Card, ISpell
    {
        public WaterSpell(string id, string name, double damage)
        {
            ID = id;
            Name = name;
            Damage = damage;
            Type = ElementType.Water;
        }

        public override bool CompareDamage(double damage)
        {
            return Damage >= damage;
        }
    }
}
