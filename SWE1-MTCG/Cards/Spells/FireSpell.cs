﻿using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards.Spells
{
    public class FireSpell : Card, ISpell
    {
        public FireSpell(string id, string name, double damage)
        {
            ID = id;
            Name = name;
            Damage = damage;
            Type = ElementType.Fire;
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
            return "Card ID " + ID + ": Spell " + Name + " of Element " + Type + " with Damage " + Damage;
        }

        public override string ToBattleString()
        {
            return "Spell " + Name + " of Element " + Type + " with Damage " + Damage;
        }
    }
}
