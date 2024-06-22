using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Dealer :IDealer
    {
        public List<Card> HiddenCards { get; set; } = new List<Card>();
        public List<Card> RevealedCards { get; set; } = new List<Card>();

        public void RevealCard()
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
            foreach (Card card in RevealedCards)
            {
                value += card.Value_;
            }
            return value;
        }

        public void WriteHand()
        {
            Console.WriteLine($"Dealer's Hand ( { GetHandValue()} ):");
            foreach (Card card in RevealedCards)
            {
                card.Description();
            }
        }


    }
}
