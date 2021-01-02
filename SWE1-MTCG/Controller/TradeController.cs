using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class TradeController
    {
        private ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        public string GetAllTradesFor(string usertoken)
        {
            return _tradeService.GetTradesFor(usertoken);
        }

        public string GetAllTrades()
        {
            return _tradeService.GetAllTrades();
        }

        public string AddTrade(TradeDTO trade, string usertoken)
        {
            return _tradeService.AddTrade(trade, usertoken);
        }

        public string TradeCards(string tradeID, string cardToTradeID, string usertoken)
        {
            return _tradeService.TradeCards(tradeID, cardToTradeID, usertoken);
        }

        public string RemoveTrade(string tradeID, string usertoken)
        {
            return _tradeService.RemoveTrade(tradeID, usertoken);
        }
    }
}
