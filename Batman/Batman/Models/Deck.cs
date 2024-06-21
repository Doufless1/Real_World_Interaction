using Batman.Enums;
namespace Batman
{
    public class Deck : IDeck
    {
        private List<Card> Deck_;


        public Deck()
        {
            Deck_ = new List<Card>();
            //    Shuffle();
            Initialize();
        }

        public List<Card> Unshuffled()
        {
            List<Card> deck = new List<Card>();
            for (int k = 0; k < 2; k++)
            {

                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Face face in Enum.GetValues(typeof(Face)))
                    {
                        deck.Add(new Card(suit, face));
                    }
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
            if (Deck_.Count < 2)
            {
                throw new InvalidOperationException("Not enough cards to play the game");
            }
            List<Card> hand = new List<Card>();
            hand.Add(Deck_[0]);
            hand.Add(Deck_[1]);

            Deck_.RemoveRange(0, 2);

            return hand;
        }

        public Card DrawCard()
        {
            if (Deck_.Count == 0)
            {
                throw new InvalidOperationException("There is no more cards to draw!");
            }
            Card card = Deck_[0];

            Deck_.Remove(card);

            return card;
        }

    }

}
