using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards
{
    public class Elve : Card, IMonster
    {
        public Elve(string name, double damage, ElementType type)
        {
            Name = name;
            Damage = damage;
            Type = type;
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public bool EvadeAttack(Card card)
        {
            if (this.Type == ElementType.Fire)
            {
                return card is Dragon;
            }
            else
            {
                return false;
            }
        }
    }
}
