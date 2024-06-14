using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public interface IDeck
    {
        //Creates and shuffles a deck
        void Initialize();

      
        //returns a list of 2 cards from deck
        List<Card> DealHand();

        //returns a list of 1 cards from deck
        Card DrawCard();
    }
}
