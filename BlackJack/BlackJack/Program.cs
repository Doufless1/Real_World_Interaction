using BlackJack;
using System.Numerics;
using System.Text;


namespace BlackJack
{
    class Program
    {
        private static void Main(string[] args)
        {
            // Console cannot render unicode characters without this line

            Console.Title = "♠♥♣♦ Blackjack";

            Console.WriteLine("Press any key to play.");
            Console.ReadKey();

            IDeck deck = new Deck();
            IPlayer player = new Player();
            IDealer dealer = new Dealer();
            //cock = new Casino();

            var cock = new Casino(deck, player, dealer);

            cock.StartRound();
        }
    }
    
}

    

