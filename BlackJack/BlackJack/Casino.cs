using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
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
                if (hand[0].Face_ == Face.Ace && hand[1].Value_ == 10) return true; // checks if any of the players ahnds is King Queen Jack or Ten with Ace to have a blackjack
                else if (hand[1].Face_ == Face.Ace && hand[0].Value_ == 10) return true;
            }
            return false;
        }
        public bool Is_Hand_for_Splitting(List<Card> hand)
        {
            if (hand.Count == 2)
            {

                HashSet<Face> Faces = new HashSet<Face> { Face.Ace,Face.King,Face.Queen,Face.Jack };
                HashSet<int> Values = new HashSet<int> { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                if (Faces.Contains(hand[0].Face_) && Faces.Contains(hand[1].Face_))
                {
                    return true;
                }

                if (Values.Contains(hand[0].Value_) && Values.Contains(hand[1].Value_))
                {
                    return true;
                }
                /*if (hand[0].Face_ == Face.Ace && hand[1].Face_ == Face.Ace) return true;
                if (hand[0].Face_ == Face.King && hand[1].Face_ == Face.King) return true;
                if (hand[0].Face_ == Face.Queen && hand[1].Face_ == Face.Queen) return true;
                if (hand[0].Face_ == Face.Jack && hand[1].Face_ == Face.Jack) return true;
                if (hand[0].Value_ == 10 && hand[1].Value_ == 10) return true;
                if (hand[0].Value_ == 9 && hand[1].Value_ == 9) return true;
                if (hand[0].Value_ == 8 && hand[1].Value_ == 8) return true;
                if (hand[0].Value_ == 7 && hand[1].Value_ == 7) return true;
                if (hand[0].Value_ == 6 && hand[1].Value_ == 6) return true;
                if (hand[0].Value_ == 5 && hand[1].Value_ == 5) return true;
                if (hand[0].Value_ == 4 && hand[1].Value_ == 4) return true;
                if (hand[0].Value_ == 3 && hand[1].Value_ == 3) return true;
                if (hand[0].Value_ == 2 && hand[1].Value_ == 2) return true;*/
            }
            else if(hand.Count == 1)
            {
                throw new InvalidProgramException("Something Went wrong in the code!!!");
            }
            
            return false;
        }
        public void InitializeHand()
        {
            //TODO: I feel like you are doing it wrong the thing is that in poker only one of the dealers hands is down

            deck_.Initialize();

            player_.Hand_ = deck_.DealHand();
            dealer_.HiddenCards = deck_.DealHand();

            if (player_.Hand_[0].Face_==Face.Ace && player_.Hand_[1].Face_ == Face.Ace)
            {
                player_.Hand_[1].Value_ = 1;
            }

            if (dealer_.HiddenCards[0].Face_ == Face.Ace && dealer_.HiddenCards[1].Face_ == Face.Ace) // TODO: You can be more cleare which is the hidden cards
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
            Console.ForegroundColor = ConsoleColor.Magenta; // this is some function that specifies colour?
            string s = Console.ReadLine();
            // why did u put it with Int32 why not basic int 

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
                    case "FOLD": // its not surrender its fold
                        player_.Hand_.Clear();
                        break;
                    case "DOUBLE":
                        // Fixed: In blackjack when u double u cant double if have less then the needed chips 
                        if (player_.Chips_ >= player_.Bet_) // i think this is wrong
                        {
                            player_.AddBet(player_.Bet_);
                        }
                        else
                        {
                            throw new InvalidOperationException("You cant DOUBLE dont have Enought Chips to do that!!!");
                        }
                        player_.Hand_.Add(deck_.DrawCard());
                        break;
                    case "SPLIT":
                        if ( Is_Hand_for_Splitting(player_.Hand_) && player_.Chips_ >= player_.Bet_)
                        {
                            player_.AddBet(player_.Chips_);
                            List<Card> splitHand1 = new List<Card>() { player_.Hand_[0], deck_.DrawCard() };
                            List<Card> splitHand2 = new List<Card>() { player_.Hand_[1], deck_.DrawCard() };

                            Take_Action_After_Spitting_The_Hand(splitHand1);
                            Take_Action_After_Spitting_The_Hand(splitHand2);


                            player_.SplitHands_ = new List<List<Card>> { splitHand1, splitHand2 };
                         
                        }
                        else
                        {
                            throw new InvalidOperationException("You are not able to SPLIT u need your balance to be equal to your Bet to do that");

                        }
                        break;
                    default:
                        Console.WriteLine("Valid Moves:");
                        Console.WriteLine("Hit, Stand, Surrender, Double,Split");
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
                && !action.ToUpper().Equals("FOLD") && player_.GetHandValue() <= 21);
        }


        private void Take_Action_After_Spitting_The_Hand(List<Card> hand_splitted)
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
                    case "FOLD": // its not surrender its fold
                        player_.Hand_.Clear();
                        break;
                    case "DOUBLE":
                        if (player_.Chips_ >= player_.Bet_)
                        {
                            player_.AddBet(player_.Bet_);
                        }
                        else
                        {
                            throw new InvalidOperationException("You cant DOUBLE dont have Enought Chips to do that!!!");
                        }
                        player_.Hand_.Add(deck_.DrawCard());
                        break;
                    default:
                        Console.WriteLine("Valid Moves:");
                        Console.WriteLine("Hit, Stand, Fold");
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
            } while (!action.ToUpper().Equals("STAND") && !action.ToUpper().Equals("FOLD") && !action.ToUpper().Equals("DOUBLE")
                && player_.GetHandValue() <= 21);
        }
        public void StartRound()
        {
            Console.Clear();

            if (!TakeBet())
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


            List<List<Card>> handsToPlay = new List<List<Card>> { player_.Hand_ };
            if (player_.SplitHands_ != null && player_.SplitHands_.Count > 0)
            {
                handsToPlay.AddRange(player_.SplitHands_); // we ensure that each item inside the SplitHands is added to handstoplay as individual items
            }
            for (int i = 0; i < handsToPlay.Count; i++)
            {
                player_.Hand_ = handsToPlay[i];
                ProcessHand(player_.Hand_);
            }
        }


        private int GetHandValue(List<Card> hand)
        {
            int value = 0;
            foreach (Card card in hand)
            {
                value += card.Value_;
            }
            return value;
        }
        private void ProcessHand(List<Card> hand)
        {
            if (hand.Count == 0)
            {
                EndRound(RoundResult.SURRENDER);
                return;
            }
            else if (GetHandValue(hand) > 21)
            {
                EndRound(RoundResult.PLAYER_BUST);
                return;
            }

            while (dealer_.GetHandValue() <= 16)
            {
                dealer_.RevealedCards.Add(deck_.DrawCard());

                Console.Clear();
                player_.WriteHand();
                dealer_.WriteHand();
            }

            if (GetHandValue(hand) > dealer_.GetHandValue())
            {
                player_.Wins_++;
                if (IsHandBlackjack(hand))
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
            else if (dealer_.GetHandValue() > GetHandValue(hand))
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
