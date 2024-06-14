using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Casino
    {
        private int MinimumBet { get; } = 10;
        private IDeck deck_;// = new Deck();
        private IPlayer player_;// = new Player();
        private IDealer dealer_;



        public Casino(IDeck deck, IPlayer player,IDealer dealer)
        {
            deck_ = deck;
            player_ = player;
            dealer_ = dealer;
        }

        public bool IsHandBlackjack(List<Card> hand)
        {
            if (hand.Count == 2)
            {
                if (hand[0].Face_ == Face.Ace && hand[1].Value_ == 10) return true;
                else if (hand[1].Face_ == Face.Ace && hand[0].Value_ == 10) return true;
            }
            return false;
        }
        public void InitializeHand()
        {
            deck_.Initialize();

            player_.Hand_ = deck_.DealHand();
            dealer_.HiddenCards = deck_.DealHand();

            if (player_.Hand_[0].Face_==Face.Ace && player_.Hand_[1].Face_ == Face.Ace)
            {
                player_.Hand_[1].Value_ = 1;
            }

            if (dealer_.HiddenCards[0].Face_ == Face.Ace && dealer_.HiddenCards[1].Face_ == Face.Ace)
            {
                dealer_.HiddenCards[1].Value_ = 1;
            }

            dealer_.RevealCard();

            player_.WriteHand();
            dealer_.WriteHand();
        }

        public bool TakeBet()
        {
            Console.Write("Current Chip Count: ");
            Console.WriteLine(player_.Chips_);

            Console.Write("Minimum Bet: ");
            Console.WriteLine(MinimumBet);

            Console.Write("Enter bet to begin hand " + player_.HandsCompleted_ + ": ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            string s = Console.ReadLine();

            if (Int32.TryParse(s, out int bet) && bet >= MinimumBet && player_.Chips_ >= bet)
            {
                player_.AddBet(bet);
                return true;
            }
            return false;
        }

        public void TakeActions()
        {
            string action;
            do
            {
                Console.Clear();
                player_.WriteHand();
                dealer_.WriteHand();

                Console.Write("Enter Action (? for help): ");
                action = Console.ReadLine();

                switch (action.ToUpper())
                {
                    case "HIT":
                        player_.Hand_.Add(deck_.DrawCard());
                        break;
                    case "STAND":
                        break;
                    case "SURRENDER":
                        player_.Hand_.Clear();
                        break;
                    case "DOUBLE":
                        if (player_.Chips_ <= player_.Bet_)
                        {
                            player_.AddBet(player_.Chips_);
                        }
                        else
                        {
                            player_.AddBet(player_.Bet_);
                        }
                        player_.Hand_.Add(deck_.DrawCard());
                        break;
                    default:
                        Console.WriteLine("Valid Moves:");
                        Console.WriteLine("Hit, Stand, Surrender, Double");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        break;
                }

                if (player_.GetHandValue() > 21)
                {
                    foreach (Card card in player_.Hand_)
                    {
                        if (card.Value_ == 11) // Only a soft ace can have a value of 11
                        {
                            card.Value_ = 1;
                            break;
                        }
                    }
                }
            } while (!action.ToUpper().Equals("STAND") && !action.ToUpper().Equals("DOUBLE")
                && !action.ToUpper().Equals("SURRENDER") && player_.GetHandValue() <= 21);
        }
        public void StartRound()
        {
            Console.Clear();

            if (TakeBet())
            {
                EndRound(RoundResult.INVALID_BET);
                return;
            }
            Console.Clear();

            InitializeHand();
            TakeActions();

            dealer_.RevealCard();

            Console.Clear();
            player_.WriteHand();
            dealer_.WriteHand();

            player_.HandsCompleted_++;

            if (player_.Hand_.Count == 0)
            {
                EndRound(RoundResult.SURRENDER);
                return;
            }
            else if (player_.GetHandValue() > 21)
            {
                EndRound(RoundResult.PLAYER_BUST);
                return;
            }

            while (dealer_.GetHandValue() <= 16)
            {
                //Thread.Sleep(1000);
                dealer_.RevealedCards.Add(deck_.DrawCard());

                Console.Clear();
                player_.WriteHand();
                dealer_.WriteHand();
            }


            if (player_.GetHandValue() > dealer_.GetHandValue())
            {
                player_.Wins_++;
                if (IsHandBlackjack(player_.Hand_))
                {
                    EndRound(RoundResult.PLAYER_BLACKJACK);
                }
                else
                {
                    EndRound(RoundResult.PLAYER_WIN);
                }
            }
            else if (dealer_.GetHandValue() > 21)
            {
                player_.Wins_++;
                EndRound(RoundResult.PLAYER_WIN);
            }
            else if (dealer_.GetHandValue() > player_.GetHandValue())
            {
                EndRound(RoundResult.DEALER_WIN);
            }
            else
            {
                EndRound(RoundResult.PUSH);
            }

        }

        public void EndRound(RoundResult result)
        {
            switch (result)
            {
                case RoundResult.PUSH:
                    player_.AddChips();
                    Console.WriteLine("Player and Dealer Push.");
                    break;
                case RoundResult.PLAYER_WIN:
                    Console.WriteLine("Player Wins " + player_.WinBet(false) + " chips");
                    break;
                case RoundResult.PLAYER_BUST:
                    player_.ClearBet();
                    Console.WriteLine("Player Busts");
                    break;
                case RoundResult.PLAYER_BLACKJACK:
                    Console.WriteLine("Player Wins " + player_.WinBet(true) + " chips with Blackjack.");
                    break;
                case RoundResult.DEALER_WIN:
                    player_.ClearBet();
                    Console.WriteLine("Dealer Wins.");
                    break;
                case RoundResult.SURRENDER:
                    Console.WriteLine("Player Surrenders " + (player_.Bet_ / 2) + " chips");
                    player_.Chips_ += player_.Bet_ / 2;
                    player_.ClearBet();
                    break;
                case RoundResult.INVALID_BET:
                    Console.WriteLine("Invalid Bet.");
                    break;
            }

            if (player_.Chips_ <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine();
                Console.WriteLine("You ran out of Chips after " + (player_.HandsCompleted_ - 1) + " rounds.");
                Console.WriteLine("500 Chips will be added and your statistics have been reset.");

                player_ = new Player();
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            StartRound();
        }




    }
}
