using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class ArenaController
    {
        private IArenaService _arenaService;

        public ArenaController(IArenaService arenaService)
        {
            _arenaService = arenaService;
        }

        public void StartBattle(Arena arena)
        {
            int battleResult = _arenaService.Battle(arena.User1, arena.User2);

            switch (battleResult)
            {
                case 1:
                    _arenaService.VerifyWinner(arena.User1);
                    break;
                case -1:
                    _arenaService.VerifyWinner(arena.User2);
                    break;
            }
        }

    }
}
