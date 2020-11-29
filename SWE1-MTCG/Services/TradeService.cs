using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public class TradeService : ITradeService
    {
        public List<Trade> GetTradesFor(User user)
        {
            throw new NotImplementedException();
        }

        public List<Trade> GetAllTrades()
        {
            throw new NotImplementedException();
        }

        public bool AddTrade(Trade trade)
        {
            throw new NotImplementedException();
        }

        public bool TradeCards(Trade trade, Card cardToTrade)
        {
            throw new NotImplementedException();
        }
    }
}
