using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards
{
    public class FireSpell : Card, ISpell
    {
        public FireSpell(string name, double damage)
        {
            Name = name;
            Damage = damage;
            Type = ElementType.Fire;
        }

        public void Utilize()
        {
            throw new NotImplementedException();
        }
    }
}
