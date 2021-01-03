using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Monsters
{
    public class Elve : Card, IMonster
    {
        public Elve(string id, string name, double damage, ElementType type)
        {
            ID = id;
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

        public override bool CompareDamage(double damage)
        {
            return Damage >= damage;
        }

        public override string ToCardString()
        {
            return "Card ID " + ID + ": Elve " + Name + " of Element " + Type + " with Damage " + Damage;
        }

        public override string ToBattleString()
        {
            return "Elve " + Name + " of Element " + Type + " with Damage " + Damage;
        }
    }
}
