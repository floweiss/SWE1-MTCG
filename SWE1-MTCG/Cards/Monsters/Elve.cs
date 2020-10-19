using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Monsters
{
    public class Elve : Card, IMonster
    {
        public Elve(string name, double damage, ElementType type)
        {
            Name = name;
            Damage = damage;
            Type = type;
        }

        public bool EvadeAttackWhen(Card card)
        {
            if (Type == ElementType.Fire)
            {
                return card is Dragon;
            }
            else
            {
                return false;
            }
        }

        public bool CompareDamage(double damage)
        {
            return Damage > damage;
        }
    }
}
