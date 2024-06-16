
using System.Numerics;
using System.Text;


/// TOOD: TEST THE CODE MORE MAKE IT MORE EFFICIENT AND PRETTY
/// TODO: ASK THE TEACHERS FOR FEEDBACK
/// TODO: IMPLIMNENT SERVER
/// TODO: MAKE THE GUI
/// TODO: IMPLEMENT INSURANCE
/// FIX: THE CODE

namespace MAUI
{
public class Program
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


