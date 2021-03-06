﻿using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;

namespace SWE1_MTCG.Services
{
    public interface ICardService
    {
        string CreateCard(CardDTO card);

        string DeleteCard(Card card);

        string ShowCards(string usertoken);
    }
}
