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

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public bool Drown(Card card)
        {
            return card is WaterSpell;
        }
    }
}
