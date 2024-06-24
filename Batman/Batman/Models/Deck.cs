using Batman.Enums;
namespace Batman
{
    public class Deck : IDeck
    {
        private List<Card> Deck_;
        public Deck()
        {
            Deck_ = new List<Card>();
            Initialize();
        }

        public List<Card> Unshuffled()
        {
            List<Card> deck = new List<Card>();
            for (int k = Constants.VALUE_OF_O; k < Constants.VALUE_OF_2; k++)
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
            while (n > Constants.VALUE_OF_1)
            {
                n--;
                int k = rng.Next(n + Constants.VALUE_OF_1);
                Card card = Deck_[k];
                Deck_[k] = Deck_[n];
                Deck_[n] = card;
            }
        }


        public void Initialize()
        {
            var connect = new Models.tcpClient();
            connect.StartClient("NEW");
        }

        public List<Card> DealHand()
        {
            var connect=new Models.tcpClient();
            return connect.StartClient("Deal");
        }

        public Card DrawCard()
        {
            var connect = new Models.tcpClient();
            var hit = connect.StartClient("hit");
            return hit[0];
        }

    }

}
