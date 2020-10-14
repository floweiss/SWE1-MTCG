using System;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Card dragon = new Dragon("Balrog", 180, ElementType.Normal);
            Console.WriteLine(dragon.Name + " of the ElementType " + dragon.Type + " has a Damage value of " + dragon.Damage + ", which makes him very powerful!");
        }
    }
}