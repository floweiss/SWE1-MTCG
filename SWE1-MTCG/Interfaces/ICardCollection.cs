using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;

namespace SWE1_MTCG.Interfaces
{
    public interface ICardCollection
    {
        public void AddCard(Card addedCard);
        public void RemoveCard(Card removedCard);
    }
}
