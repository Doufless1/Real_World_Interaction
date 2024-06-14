using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Deck : IDeck
    {
        private List<Card> Deck_;


        public Deck() { Initialize();}

        public List<Card> Unshuffled() 
        {
            List<Card> deck = new List<Card>();

            for(int i = 0; i < 13; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    deck.Add(new Card((Suit)j, (Face)i));
                }
            }
            return deck;
        }

        public void Shuffle()
        {
            Random rng = new Random();

            int n = Deck_.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card card = Deck_[k];
                Deck_[k] = Deck_[n];
                Deck_[n] = card;
            }
        }


        public void Initialize()
        {
            Deck_ = Unshuffled();
            Shuffle();
        }

        public List<Card> DealHand()
        {
            List<Card> hand = new List<Card>();
            hand.Add(Deck_[0]);
            hand.Add(Deck_[1]);

            Deck_.RemoveRange(0, 2);

            return hand;
        }

        public Card DrawCard()
        {
            Card card = Deck_[0];

            Deck_.Remove(card);

            return card;
        }

    }

}
