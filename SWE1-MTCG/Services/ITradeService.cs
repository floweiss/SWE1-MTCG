using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public interface ITradeService
    {
        List<Trade> GetTradesFor(User user);
        List<Trade> GetAllTrades();
        bool AddTrade(Trade trade);
        bool TradeCards(Trade trade, Card cardToTrade);
    }
}
