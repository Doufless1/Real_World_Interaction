using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Batman
{
    public class Dealer : IDealer
    {
        public List<Card> HiddenCards { get; set; } = new List<Card>();
        public List<Card> RevealedCards { get; set; } = new List<Card>();

        public void RevealCard(List<Card> card)
        {
            if (HiddenCards.Count != Constants.VALUE_OF_O)
            {
                RevealedCards.Add(HiddenCards[Constants.VALUE_OF_O]);
                HiddenCards.RemoveAt(Constants.VALUE_OF_O);
            }
        }

        public int GetHandValue()
        {
            int value = Constants.VALUE_OF_O;
            int Ace_counter = Constants.VALUE_OF_O;
            foreach (Card card in RevealedCards)
            {
                if (card.Face_ == Enums.Face.Ace)
                {
                    value += Constants.VALUE_OF_11;
                    Ace_counter++;
                }
                else
                {
                    value += card.Value_;
                }
            }
            while (value > Constants.VALUE_OF_21 && Ace_counter > Constants.VALUE_OF_O)
            {
                value -= Constants.VALUE_OF_10;
                Ace_counter--;

            }
            return value;
        }
    }
}
