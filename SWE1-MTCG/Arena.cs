using System;
using System.Collections.Generic;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Services;

namespace SWE1_MTCG
{
    public class Arena
    {
        public User User1;
        public User User2;

        public Arena(User player1, User player2)
        {
            User1 = player1;
            User2 = player2;
        }
    }
}
