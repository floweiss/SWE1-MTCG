using System;
using System.Collections.Generic;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Services;

namespace SWE1_MTCG
{
    public class Trade
    {
        private User _trader;
        public Card TradedCard;
        public string RequestedCardType;
        public double RequestedMinDamage;

        public Trade(User trader, Card tradedCard, string requestedCardType, double requestedMinDamage)
        {
            _trader = trader;
            TradedCard = tradedCard;
            RequestedCardType = requestedCardType;
            RequestedMinDamage = requestedMinDamage;
        }

        public bool IsProperTrade()
        {
            return TradedCard != null && RequestedCardType != null && RequestedMinDamage > 0;
        }

        public bool IsMatchingRequest(Card cardToTrade)
        {
            return cardToTrade.GetType().ToString().ToLower().Contains(RequestedCardType.ToLower()) &&
                   cardToTrade.Damage >= RequestedMinDamage;
        }
    }
}
