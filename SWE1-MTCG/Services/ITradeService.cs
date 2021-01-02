using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public interface ITradeService
    {
        string GetTradesFor(string usertoken);
        string GetAllTrades();
        string AddTrade(TradeDTO trade, string usertoken);
        string TradeCards(string tradeID, string cardToTradeID, string usertoken);
        string RemoveTrade(string tradeID, string usertoken);
    }
}
