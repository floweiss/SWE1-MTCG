using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Monsters
{
    public class Knight : Card, IMonster
    {
        public Knight(string name, double damage, ElementType type)
        {
            Name = name;
            Damage = damage;
            Type = type;
        }

        public bool DrownsWhen(Card card)
        {
            return card is WaterSpell;
        }

        public bool CompareDamage(double damage)
        {
            return Damage > damage;
        }
    }
}
