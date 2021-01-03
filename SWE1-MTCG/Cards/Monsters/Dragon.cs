using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Monsters
{
    public class Dragon : Card, IMonster
    {
        public Dragon(string id, string name, double damage, ElementType type)
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

        public override void EnhanceDamage(double enhancement)
        {
            Damage = Damage * enhancement;
        }

        public override string ToCardString()
        {
            return "Card ID " + ID + ": Dragon " + Name + " of Element " + Type + " with Damage " + Damage;
        }

        public override string ToBattleString()
        {
            return "Dragon " + Name + " of Element " + Type + " with Damage " + Damage;
        }
    }
}
