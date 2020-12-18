using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class StatsController
    {
        private IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        public string ShowStats(string usertoken)
        {
            return _statsService.ShowStats(usertoken);
        }
    }
}
