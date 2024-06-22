using BlackJack;
using System.Numerics;
using System.Text;


/// TOOD: TEST THE CODE MORE MAKE IT MORE EFFICIENT AND PRETTY
/// TODO: ASK THE TEACHERS FOR FEEDBACK
/// TODO: IMPLIMNENT SERVER
/// TODO: MAKE THE GUI
/// TODO: IMPLEMENT INSURANCE
/// FIX: THE CODE

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

   

/*using System;

namespace BlackJack
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup the game environment
            Deck deck = new Deck();
            Player player = new Player();
            Dealer dealer = new Dealer();
            Casino casino = new Casino(deck, player, dealer);

            deck.Shuffle();

            // Setup the test - provide a hand that can definitely be split
            player.Hand_.Add(new Card(Suit.Hearts, Face.Ace));  // Ace of Hearts
            player.Hand_.Add(new Card(Suit.Spades, Face.Ace));  // Ace of Spades

            player.Chips_ = 100;
            player.Bet_ = 10;

            dealer.RevealedCards.Add(deck.DrawCard());
            dealer.HiddenCards.Add(deck.DrawCard());

            // Assume the player wants to split
            Console.WriteLine("Initial Setup:");
            Console.WriteLine("Player's Hand:");
            player.WriteHand();
    
            Console.WriteLine($"Chips before split: {player.Chips_}");
        ;
            Console.WriteLine("Dealer's Hand\n:");
            dealer.WriteHand();

            // Manually invoke the split if there's no method to simulate player actions
            if (casino.Is_Hand_for_Splitting(player.Hand_))
            {
                // Simulate a player action that chooses to split
                casino.TakeActions();  // You need to adapt this to accept a "SPLIT" action, or call the split logic directly if possible
            }
            Console.WriteLine("\n");
            dealer.RevealCard();  // Show the hidden card
            while (dealer.GetHandValue() < 17)
            {
                dealer.RevealedCards.Add(deck.DrawCard());
            }

            Console.WriteLine("\nUpdated Dealer's Hand:");
            dealer.WriteHand();

            // Check outcomes
            Console.WriteLine($"Chips after split: {player.Chips_}");
            Console.WriteLine($"Dealer's final hand value: {dealer.GetHandValue()}");

            // Determine outcome based on Blackjack rules
            if (dealer.GetHandValue() > 21)
            {
                Console.WriteLine("Dealer busts, player wins if not busted");a
            }
            else if (player.GetHandValue() > 21)
            {
                Console.WriteLine("Player busts");
            }
            else if (player.GetHandValue() > dealer.GetHandValue())
            {
                Console.WriteLine("Player wins");
            }
            else if (player.GetHandValue() < dealer.GetHandValue())
            {
                Console.WriteLine("Dealer wins");
            }
            else
            {
                Console.WriteLine("Push");
            }
        }
    }*/