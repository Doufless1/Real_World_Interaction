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
            //Deck_ = Unshuffled();
            //Shuffle();
        }

        public List<Card> DealHand()
        {

            if (Deck_.Count < Constants.VALUE_OF_2)

            var connect=new Models.tcpClient();
            return connect.StartClient("Deal");
            /*
            if (Deck_.Count < 2)

            {
                throw new InvalidOperationException("Not enough cards to play the game");
            }
            List<Card> hand = new List<Card>();
            hand.Add(Deck_[Constants.VALUE_OF_O]);
            hand.Add(Deck_[Constants.VALUE_OF_1]);

            Deck_.RemoveRange(Constants.VALUE_OF_O, Constants.VALUE_OF_2);

            return hand;*/
        }

        public Card DrawCard()
        {

            if (Deck_.Count == Constants.VALUE_OF_O)

            var connect = new Models.tcpClient();
            var hit = connect.StartClient("hit");
            return hit[0];
            /*
            if (Deck_.Count == 0)
 refs/remotes/destination/main
            {
                throw new InvalidOperationException("There is no more cards to draw!");
            }
            Card card = Deck_[Constants.VALUE_OF_O];

            Deck_.Remove(card);

            return card;
            */
        }

    }

}
