using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Monsters
{
    public class Orc : Card, IMonster
    {
        public Orc(string id, string name, double damage, ElementType type)
        {
            ID = id;
            Name = name;
            Damage = damage;
            Type = type;
        }

        public override bool CompareDamage(double damage)
        {
            return Damage >= damage;
        }

        public override string ToCardString()
        {
            return "Card ID " + ID + ": Orc of Element " + Type + " with Damage " + Damage;
        }
    }
}
