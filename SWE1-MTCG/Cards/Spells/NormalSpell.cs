using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Spells
{
    public class NormalSpell : Card, ISpell
    {
        public NormalSpell(string name, double damage)
        {
            Name = name;
            Damage = damage;
            Type = ElementType.Normal;
        }

        public bool CompareDamage(double damage)
        {
            return Damage > damage;
        }
    }
}
