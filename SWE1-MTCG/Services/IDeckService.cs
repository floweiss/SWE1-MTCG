using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;

namespace SWE1_MTCG.Services
{
    public interface IDeckService
    {
        string ShowDeck(string usertoken);

        string ConfigureDeck(string usertoken, List<string> cardIds);
    }
}
