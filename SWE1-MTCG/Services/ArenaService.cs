using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Cards.Monsters;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public class ArenaService : IArenaService
    {
        private ElementEffectivenessService _elementEffectivenessService;

        public ArenaService(ElementEffectivenessService elementEffectivenessService)
        {
            _elementEffectivenessService = elementEffectivenessService;
        }

        public int Battle(User user1, User user2)
        {
            CardDeck deck1 = user1.Deck;
            CardDeck deck2 = user2.Deck;

            Random random = new Random();
            int roundNumber;
            if (random.Next(0, 2) == 0)
            {
                roundNumber = 0;
            }
            else
            {
                roundNumber = 1;
            }

            while (deck1.CardCollection.Count > 0 && deck2.CardCollection.Count > 0 && roundNumber < 100)
            {
                Console.Write(roundNumber + ": ");
                if (roundNumber % 2 == 0)
                {
                    Round(ref deck1, ref deck2);
                }
                else
                {
                    Round(ref deck2, ref deck1);
                }

                roundNumber++;
            }

            if (roundNumber == 100)
            {
                return 0;
            }
            return (deck1.CardCollection.Count > deck2.CardCollection.Count ? 1 : -1);
        }

        private void Round(ref CardDeck deck1, ref CardDeck deck2)
        {
            Card card1 = deck1.GetRandomCard();
            Card card2 = deck2.GetRandomCard();
            card1.Damage = card1.Damage * _elementEffectivenessService.CompareElements(card1.Type, card2.Type);
            card2.Damage = card2.Damage * _elementEffectivenessService.CompareElements(card2.Type, card1.Type);

            // ToDo: implement special abilities

            /*int card1wins = card1 switch
            {
                IMonster monster => monster.CompareDamage(card2.Damage) ? 1 : -1, 
                ISpell spell => spell.CompareDamage(card2.Damage) ? 1 : -1,
                _ => 0
            };*/

            int card1wins = card1 switch
            {
                IMonster _ when card2 is IMonster => FightMonsterMonster(card1, card2),
                IMonster _ when card1 is ISpell => FightMonsterSpell(card1, card2),
                ISpell _ when card2 is IMonster => FightSpellMonster(card2, card1),
                _ => FightSpellSpell(card1, card2)
            };

            if (card1wins == 1)
            {
                deck1.AddCard(card2, true);
                deck2.RemoveCard(card2);
                Console.WriteLine(card1.Name + " won against " + card2.Name);
            }
            else if (card1wins == -1)
            {
                deck2.AddCard(card1, true);
                deck1.RemoveCard(card1);
                Console.WriteLine(card1.Name + " lost against " + card2.Name);
            }
            else
            {
                Console.WriteLine(card1.Name + " drew with " + card2.Name);
            }
        }

        private int FightMonsterMonster(Card card1, Card card2)
        {
            int card1wins = card1 switch
            {
                Goblin goblin when goblin.AfraidOf(card2) => 0,
                Orc orc when card2 is Wizard wizard && wizard.CanControl(orc) => 0,
                Dragon dragon when card2 is Elve elve && elve.Type == ElementType.Fire &&
                                   elve.EvadeAttackWhen(dragon) => 0,
                _ => card1.CompareDamage(card2.Damage) ? 1 : -1
            };
            return card1wins;
        }

        private int FightSpellSpell(Card card1, Card card2)
        {
            return card1.CompareDamage(card2.Damage) ? 1 : -1;
        }

        private int FightMonsterSpell(Card card2, Card card1)
        {
            return card1.CompareDamage(card2.Damage) ? 1 : -1;
        }

        private int FightSpellMonster(Card card2, Card card1)
        {
            int card1wins = card2 switch
            {
                Knight knight when knight.DrownsWhen(card1) => 1,
                Kraken kraken when kraken.ImmuneTo(card1) => 0,
                _ => card1.CompareDamage(card2.Damage) ? 1 : -1
            };
            return card1wins;
        }

        public void UpdateUserStats(User winner, User loser)
        {
            throw new NotImplementedException();
        }
    }
}
