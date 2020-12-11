using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Cards.Monsters;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.DataTransferObject
{
    public class CardDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CardType { get; set; }
        public string Element { get; set; }
        public double Damage { get; set; }

        public CardDTO(string id, string name, string cardType, string element, double damage)
        {
            Id = id;
            Name = name;
            CardType = cardType;
            Element = element;
            Damage = damage;
        }

        public Card ToCard()
        {
            ElementType elementType;
            try
            {
                elementType = Enum.Parse<ElementType>(Element);
            }
            catch (Exception e)
            {
                elementType = ElementType.Normal;
            }

            switch (CardType.ToLower())
            {
                case "dragon":
                    return new Dragon(Id, Name, Damage, elementType);
                case "elve":
                    return new Elve(Id, Name, Damage, elementType);
                case "goblin":
                    return new Goblin(Id, Name, Damage, elementType);
                case "knight":
                    return new Knight(Id, Name, Damage, elementType);
                case "kraken":
                    return new Kraken(Id, Name, Damage, elementType);
                case "orc":
                    return new Orc(Id, Name, Damage, elementType);
                case "wizard":
                    return new Wizard(Id, Name, Damage, elementType);
                case "normalspell":
                    return new NormalSpell(Id, Name, Damage);
                case "waterspell":
                    return new WaterSpell(Id, Name, Damage);
                case "firespell":
                    return new FireSpell(Id, Name, Damage);
                default:
                    return null;
            }
        }
    }
}
