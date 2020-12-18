using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class ScoreController
    {
        private IScoreService _scoreService;

        public ScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        public string ShowScore()
        {
            return _scoreService.ShowScore();
        }
    }
}
