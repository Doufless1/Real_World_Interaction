using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batman
{
    public interface IDealer
    {
        List<Card> HiddenCards { get; set; }
        List<Card> RevealedCards { get; set; }
        void RevealCard(List<Card> card); // chanching how this is working i jsut put a card
        int GetHandValue();

        void WriteHand();

    }
}
