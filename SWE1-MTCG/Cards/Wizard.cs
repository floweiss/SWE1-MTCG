using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards
{
    public class Wizard : Card, IMonster
    {
        public Wizard(string name, double damage, ElementType type)
        {
            Name = name;
            Damage = damage;
            Type = type;
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public bool ControlOrc(Card card)
        {
            return card is Orc;
        }
    }
}
