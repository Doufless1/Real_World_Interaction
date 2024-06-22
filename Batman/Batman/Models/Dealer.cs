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
            if (HiddenCards.Count != 0)
            {
                RevealedCards.Add(HiddenCards[0]);
                HiddenCards.RemoveAt(0);
            }
        }

        public int GetHandValue()
        {
            int value = 0;
            int Ace_counter = 0;
            foreach (Card card in RevealedCards)
            {
                if (card.Face_ == Enums.Face.Ace)
                {
                    value += 11;
                    Ace_counter++;
                }
                else
                {
                    value += card.Value_;
                }
            }
            while (value > 21 && Ace_counter > 0)
            {
                value -= 10;
                Ace_counter--;

            }
            return value;
        }

            public void WriteHand()
        {
            Console.WriteLine($"Dealer's Hand ( {GetHandValue()} ):");
            foreach (Card card in RevealedCards)
            {
             //   card.Description();
            }
        }


    }
}
