using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI
{
    public interface IDealer
    {
        List<Card> HiddenCards { get; set; }
        List<Card> RevealedCards { get; set; }
        void RevealCard();
        int GetHandValue();

        void WriteHand();

    }
}
