﻿using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards
{
    public abstract class Card
    {
        public string Name { get; protected set; }
        public double Damage { get; set; }
        public ElementType Type { get; protected set; }
    }
}
