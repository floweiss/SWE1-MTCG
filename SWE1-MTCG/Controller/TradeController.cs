using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
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

        public List<Trade> GetAllTradesFor(User user)
        {
            return _tradeService.GetTradesFor(user);
        }

        public List<Trade> GetAllTrades()
        {
            return _tradeService.GetAllTrades();
        }

        public bool AddTrade(Trade trade)
        {
            if (!trade.IsProperTrade())
            {
                return false;
            }
            return _tradeService.AddTrade(trade);
        }

        public bool TradeCards(Trade trade, Card cardToTrade)
        {
            if (!trade.IsMatchingRequest(cardToTrade))
            {
                return false;
            }
            return _tradeService.TradeCards(trade, cardToTrade);
        }
    }
}
