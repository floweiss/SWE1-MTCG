using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Xml;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Cards.Monsters;
using SWE1_MTCG.Cards.Spells;
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

            Dictionary<string, string> battleLog = new Dictionary<string, string>();
            battleLog.Add("Result", "");
            while (deck1.CardCollection.Count > 0 && deck2.CardCollection.Count > 0 && roundNumber < 100)
            {
                string roundLog = "";
                if (roundNumber % 2 == 0)
                {
                    roundLog += Round(ref deck1, ref deck2, user1.Username, user2.Username);
                }
                else
                {
                    roundLog += Round(ref deck2, ref deck1, user2.Username, user1.Username);
                }
                battleLog.Add("Round "+ roundNumber, roundLog);

                roundNumber++;
            }

            if (roundNumber == 100)
            {
                battleLog["Result"] = "DRAW";
                return Tuple.Create(0, JsonSerializer.Serialize(battleLog));
            }

            if (deck1.CardCollection.Count > deck2.CardCollection.Count)
            {
                battleLog["Result"] = user1.Username + " won against " + user2.Username;
                return Tuple.Create(1, JsonSerializer.Serialize(battleLog));
            }
            else
            {
                battleLog["Result"] = user2.Username + " won against " + user1.Username;
                return Tuple.Create(-1, JsonSerializer.Serialize(battleLog));
            }
        }

        private string Round(ref CardDeck deck1, ref CardDeck deck2, string username1, string username2)
        {
            Card card1Deck = deck1.GetRandomCard();
            Card card2Deck = deck2.GetRandomCard();

            // create new cards instead of references
            Card card1 = card1Deck switch
            {
                Dragon d => new Dragon(card1Deck.ID, card1Deck.Name, card1Deck.Damage, card1Deck.Type),
                Elve e => new Elve(card1Deck.ID, card1Deck.Name, card1Deck.Damage, card1Deck.Type),
                Goblin g => new Goblin(card1Deck.ID, card1Deck.Name, card1Deck.Damage, card1Deck.Type),
                Knight k => new Knight(card1Deck.ID, card1Deck.Name, card1Deck.Damage, card1Deck.Type),
                Kraken k => new Kraken(card1Deck.ID, card1Deck.Name, card1Deck.Damage, card1Deck.Type),
                Orc o => new Orc(card1Deck.ID, card1Deck.Name, card1Deck.Damage, card1Deck.Type),
                Wizard w => new Wizard(card1Deck.ID, card1Deck.Name, card1Deck.Damage, card1Deck.Type),
                FireSpell f => new FireSpell(card1Deck.ID, card1Deck.Name, card1Deck.Damage),
                NormalSpell n => new NormalSpell(card1Deck.ID, card1Deck.Name, card1Deck.Damage),
                WaterSpell w => new WaterSpell(card1Deck.ID, card1Deck.Name, card1Deck.Damage),
            };
            Card card2 = card2Deck switch
            {
                Dragon d => new Dragon(card2Deck.ID, card2Deck.Name, card2Deck.Damage, card2Deck.Type),
                Elve e => new Elve(card2Deck.ID, card2Deck.Name, card2Deck.Damage, card2Deck.Type),
                Goblin g => new Goblin(card2Deck.ID, card2Deck.Name, card2Deck.Damage, card2Deck.Type),
                Knight k => new Knight(card2Deck.ID, card2Deck.Name, card2Deck.Damage, card2Deck.Type),
                Kraken k => new Kraken(card2Deck.ID, card2Deck.Name, card2Deck.Damage, card2Deck.Type),
                Orc o => new Orc(card2Deck.ID, card2Deck.Name, card2Deck.Damage, card2Deck.Type),
                Wizard w => new Wizard(card2Deck.ID, card2Deck.Name, card2Deck.Damage, card2Deck.Type),
                FireSpell f => new FireSpell(card2Deck.ID, card2Deck.Name, card2Deck.Damage),
                NormalSpell n => new NormalSpell(card2Deck.ID, card2Deck.Name, card2Deck.Damage),
                WaterSpell w => new WaterSpell(card2Deck.ID, card2Deck.Name, card2Deck.Damage),
            };

            card1.Damage = card1.Damage * _elementEffectivenessService.CompareElements(card1.Type, card2.Type);
            card2.Damage = card2.Damage * _elementEffectivenessService.CompareElements(card2.Type, card1.Type);

            // without special abilities
            /*int card1wins = card1 switch
            {
                IMonster monster => monster.CompareDamage(card2.Damage) ? 1 : -1, 
                ISpell spell => spell.CompareDamage(card2.Damage) ? 1 : -1,
                _ => 0
            };*/

            Random random = new Random();
            bool boosterCard1 = (random.Next(1, 21) == 20);
            if (boosterCard1)
            {
                card1.Damage = card1.Damage * 10;
            }

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
                deck1.AddCard(card2Deck, true);
                deck2.RemoveCard(card2Deck);
                if (boosterCard1)
                {
                    roundLog = (card1Deck.ToBattleString() + " (" + username1 + ") won with a booster against " + card2Deck.ToBattleString() + " (" + username2 + ")");
                }
                else
                {
                    roundLog = (card1Deck.ToBattleString() + " (" + username1 + ") won against " + card2Deck.ToBattleString() + " (" + username2 + ")");
                }
            }
            else if (card1wins == -1)
            {
                deck2.AddCard(card1Deck, true);
                deck1.RemoveCard(card1Deck);
                if (boosterCard1)
                {
                    roundLog = (card1Deck.ToBattleString() + " (" + username1 + ") lost although a booster to " + card2Deck.ToBattleString() + " (" + username2 + ")");
                }
                else
                {
                    roundLog = (card1Deck.ToBattleString() + " (" + username1 + ") lost to " + card2Deck.ToBattleString() + " (" + username2 + ")");
                }
            }
            else
            {
                if (boosterCard1)
                {
                    roundLog = (card1Deck.ToBattleString() + " (" + username1 + ") drew although a booster with " + card2Deck.ToBattleString() + " (" + username2 + ")");
                }
                else
                {
                    roundLog = (card1Deck.ToBattleString() + " (" + username1 + ") drew with " + card2Deck.ToBattleString() + " (" + username2 + ")");
                }
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
