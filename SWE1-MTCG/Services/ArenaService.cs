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
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Services
{
    public class ArenaService : IArenaService
    {
        private ElementEffectivenessService _elementEffectivenessService;
        private IUserDataService _userDataService;

        public ArenaService(ElementEffectivenessService elementEffectivenessService)
        {
            _elementEffectivenessService = elementEffectivenessService;
            _userDataService = new UserDataService();
        }

        public Tuple<int, string> Battle(User user1, User user2)
        {
            CardDeck deck1 = new CardDeck();
            foreach (var card in user1.Deck.CardCollection)
            {
                deck1.AddCard(card);
            }
            CardDeck deck2 = new CardDeck();
            foreach (var card in user2.Deck.CardCollection)
            {
                deck2.AddCard(card);
            }

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

            string battleLog = "";
            while (deck1.CardCollection.Count > 0 && deck2.CardCollection.Count > 0 && roundNumber < 100)
            {
                battleLog += ("Round " + roundNumber + ": ");
                if (roundNumber % 2 == 0)
                {
                    battleLog += Round(ref deck1, ref deck2, user1.Username, user2.Username) + "\n";
                }
                else
                {
                    battleLog += Round(ref deck2, ref deck1, user2.Username, user1.Username) + "\n";
                }

                roundNumber++;
            }

            if (roundNumber == 100)
            {
                battleLog = "DRAW\n\n" + battleLog;
                return Tuple.Create(0, battleLog);
            }
            return (deck1.CardCollection.Count > deck2.CardCollection.Count ? Tuple.Create(1, user1.Username + " WON\n\n" + battleLog) : Tuple.Create(-1, user2.Username + " WON\n\n" + battleLog));
        }

        private string Round(ref CardDeck deck1, ref CardDeck deck2, string username1, string username2)
        {
            Card card1 = deck1.GetRandomCard();
            Card card2 = deck2.GetRandomCard();
            card1.Damage = card1.Damage * _elementEffectivenessService.CompareElements(card1.Type, card2.Type);
            card2.Damage = card2.Damage * _elementEffectivenessService.CompareElements(card2.Type, card1.Type);

            // without special abilities
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

            string roundLog;
            if (card1wins == 1)
            {
                deck1.AddCard(card2, true);
                deck2.RemoveCard(card2);
                roundLog = (card1.Name + " won against " + card2.Name);
            }
            else if (card1wins == -1)
            {
                deck2.AddCard(card1, true);
                deck1.RemoveCard(card1);
                roundLog = (card1.Name + " lost against " + card2.Name);
            }
            else
            {
                roundLog = (card1.Name + " drew with " + card2.Name);
            }

            return roundLog;
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
            winner.ELO = winner.ELO + 3;
            loser.ELO = loser.ELO - 5;

            ClientSingleton.GetInstance.ClientMap.AddOrUpdate(winner.Username+"-mtcgToken", winner, (key, oldValue) => winner);
            ClientSingleton.GetInstance.ClientMap.AddOrUpdate(loser.Username + "-mtcgToken", loser, (key, oldValue) => loser);
            _userDataService.PersistUserData(winner, winner.Username+"-mtcgToken");
            _userDataService.PersistUserData(loser, loser.Username + "-mtcgToken");
        }
    }
}
