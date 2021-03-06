﻿using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public interface IArenaService
    {
        Tuple<int, string> Battle(User user1, User user2);
        void UpdateUserStats(User winner, User loser);
    }
}
