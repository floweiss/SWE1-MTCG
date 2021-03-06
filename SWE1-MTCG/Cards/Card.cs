﻿using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards
{
    public abstract class Card
    {
        public string ID { get; protected set; }
        public string Name { get; protected set; }
        public double Damage { get; set; }
        public ElementType Type { get; protected set; }
        public abstract bool CompareDamage(double damage);
        public abstract void EnhanceDamage(double enhancement);
       public abstract string ToCardString();
        public abstract string ToBattleString();
    }
}
