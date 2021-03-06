﻿using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Monsters
{
    public class Goblin : Card, IMonster
    {
        public Goblin(string id, string name, double damage, ElementType type)
        {
            ID = id;
            Name = name;
            Damage = damage;
            Type = type;
        }

        public bool AfraidOf(Card card)
        {
            return card is Dragon;
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
            return "Card ID " + ID + ": Goblin " + Name + " of Element " + Type + " with Damage " + Damage;
        }

        public override string ToBattleString()
        {
            return "Goblin " + Name + " of Element " + Type + " with Damage " + Damage;
        }
    }
}
