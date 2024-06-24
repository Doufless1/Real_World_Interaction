using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Batman.Enums
{
    public class Casino
    {

        private IDeck deck_;// = new Deck();
        private IPlayer player_;// = new Player();
        private IDealer dealer_;

        public EventHandler<string> RoundEnded;


        public Casino(IDeck deck, IPlayer player, IDealer dealer)
        {
            deck_ = deck;
            player_ = player;
            dealer_ = dealer;
        }


        public void OnRoundEnded(string message)
        {
            RoundEnded?.Invoke(this, message);
        }

        public bool IsHandBlackjack(List<Card> hand)
        {
            try
            {

                if (hand == null)
                {
                    throw new ArgumentNullException(nameof(hand), "The hand cant be null");
                }

                if (hand.Count == Constants.VALUE_OF_2)
                {
                    if (hand[Constants.VALUE_OF_O].Face_ == Face.Ace && hand[Constants.VALUE_OF_1].Value_ == Constants.VALUE_OF_10) return true; // checks if any of the players ahnds is King Queen Jack or Ten with Ace to have a blackjack
                    else if (hand[Constants.VALUE_OF_1].Face_ == Face.Ace && hand[Constants.VALUE_OF_O].Value_ == Constants.VALUE_OF_1) return true;
                }
            }
            catch (Exception ex)
            {
                OnRoundEnded($"{ex.Message}");

            }
            return false;
        }
        public bool Is_Hand_for_Splitting(List<Card> hand)
        {
            try
            {
                if (hand.Count == Constants.VALUE_OF_2)
                {
                    if ((hand[Constants.VALUE_OF_O].Face_ == hand[Constants.VALUE_OF_1].Face_) || (hand[Constants.VALUE_OF_O].Value_ == hand[Constants.VALUE_OF_1].Value_))
                    {
                        return true;
                    }
                    return false;
                }
                else if (hand.Count == Constants.VALUE_OF_1)
                {
                    throw new InvalidProgramException("Something Went wrong in the code!!!");
                }
            }
            catch (Exception ex)
            {
                OnRoundEnded($"{ex.Message}");

            }
            return false;
        }

        private void AdjustAceValue(List<Card> hand)
        {
            if (hand.Count == Constants.VALUE_OF_2 && hand.All(card => card.Face_ == Face.Ace))
            {
                hand[Constants.VALUE_OF_1].Value_ = Constants.VALUE_OF_1;
            }
        }


        public void InitializeHand(string starting_the_round)
        {
            //TODO: I feel like you are doing it wrong the thing is that in poker only one of the dealers hands is down
            try
            {
                deck_.Initialize();

                player_.Hand_ = deck_.DealHand();
                dealer_.HiddenCards = new List<Card> { deck_.DrawCard() };
                dealer_.RevealedCards = new List<Card> { deck_.DrawCard() };

                AdjustAceValue(dealer_.HiddenCards);
                AdjustAceValue(dealer_.RevealedCards);
            }
            catch (Exception ex)
            {
                OnRoundEnded($"{ex.Message}");
            }
        }

        public bool TakeBet(string bet_in_string)
        {


            try
            {
                // why did u put it with Int32 why not basic int 
                int bet = Int32.Parse(bet_in_string);
                if (bet < Constants.VALUE_OF_10)
                {
                    throw new InvalidOperationException("The value at least shoud be equal or higher then 10");
                }
                if (player_.Chips_ >= bet)
                {
                    player_.AddBet(bet);
                    return true;
                }
                throw new InvalidOperationException("Exceeding balance");
            }
            catch (Exception ex)
            {
                OnRoundEnded($"{ex.Message}");
                return false;
            }
        }

        public void TakeActions(string action)
        {
            try
            {
                if (action.Equals(null))
                {
                    throw new ArgumentException(nameof(action), "The action taken cant be null or empty");
                }
                bool hit = false;
                do
                {
                    switch (action.ToUpper())
                    {
                        case "HIT":
                            player_.Hand_.Add(deck_.DrawCard());
                            hit = true;
                            break;
                        case "STAND":
                            break;
                        case "FOLD": // its not surrender its fold
                            player_.Hand_.Clear();
                            player_.ClearBet();
                            dealer_.RevealedCards.Clear();
                            break;
                        case "DOUBLE":
                            // Fixed: In blackjack when u double u cant double if have less then the needed chips 
                            if (player_.Chips_ >= player_.Bet_) // i think this is wrong
                            {
                                player_.AddBet(player_.Bet_);
                                player_.Hand_.Add(deck_.DrawCard());
                            }
                            else
                            {

                                throw new InvalidOperationException("You cant DOUBLE dont have Enought Chips to do that!!!");
                            }

                            break;
                        case "SPLIT":
                            if (Is_Hand_for_Splitting(player_.Hand_) && player_.Chips_ >= player_.Bet_)
                            {
                                player_.AddBet(player_.Chips_);
                                List<Card> splitHand1 = new List<Card>() { player_.Hand_[Constants.VALUE_OF_O], deck_.DrawCard() };
                                List<Card> splitHand2 = new List<Card>() { player_.Hand_[Constants.VALUE_OF_1], deck_.DrawCard() };

                                //       Take_Action_After_Spitting_The_Hand(splitHand1);
                                //     Take_Action_After_Spitting_The_Hand(splitHand2);


                                player_.SplitHands_ = new List<List<Card>> { splitHand1, splitHand2 };

                            }
                            else
                            {
                                throw new InvalidOperationException("You are not able to SPLIT u need your balance to be equal to your Bet to do that");

                            }
                            break;
                        default:
                            throw new ArgumentException("$Invalid action{action}", nameof(action));
                    }

                    if (player_.GetHandValue() > Constants.VALUE_OF_21)
                    {
                        foreach (Card card in player_.Hand_)
                        {
                            if (card.Value_ == Constants.VALUE_OF_11) // Only a soft ace can have a value of 11
                            {
                                card.Value_ = Constants.VALUE_OF_1;
                                break;
                            }
                        }
                    }
                } while (!action.ToUpper().Equals("STAND") && !action.ToUpper().Equals("DOUBLE")
                    && !action.ToUpper().Equals("FOLD") && player_.GetHandValue() <= Constants.VALUE_OF_21 && hit != true);
            }
            catch (Exception ex)
            {
                OnRoundEnded($"{ex.Message}");
                throw;
            }
        }
        /*   private void Take_Action_After_Spitting_The_Hand(List<Card> hand_splitted)
           {
               string action;
               do
               {
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
           }*/
        private int GetHandValue(List<Card> hand)
        {
            int value = Constants.VALUE_OF_O;
            int Ace_counter = Constants.VALUE_OF_O;
            foreach (Card card in hand)
            {
                if (card.Face_ == Enums.Face.Ace)
                {
                    value += Constants.VALUE_OF_11;
                    Ace_counter++;
                }
                else
                {
                    value += card.Value_;
                }
            }
            while (value > Constants.VALUE_OF_21 && Ace_counter > Constants.VALUE_OF_O)
            {
                value -= Constants.VALUE_OF_10;
                Ace_counter--;

            }
            return value;
        }
        public void ProcessHand(List<Card> hand)
        {
            if (hand.Count == Constants.VALUE_OF_O)
            {
                EndRound(RoundResult.SURRENDER);
                return;
            }
            else if (GetHandValue(hand) > Constants.VALUE_OF_21)
            {
                EndRound(RoundResult.PLAYER_BUST);
                return;
            }

            while (dealer_.GetHandValue() <= Constants.VALUE_OF_16)
            {
                dealer_.RevealedCards.Add(deck_.DrawCard());

            }

            if (IsHandBlackjack(hand) && !IsHandBlackjack(dealer_.RevealedCards))
            {
                player_.Wins_++;
                EndRound(RoundResult.PLAYER_BLACKJACK);
                return;
            }

            if (IsHandBlackjack(dealer_.RevealedCards))
            {
                EndRound(RoundResult.DEALER_WIN);
                return;
            }

            if (GetHandValue(hand) > dealer_.GetHandValue())
            {
                player_.Wins_++;
                EndRound(RoundResult.PLAYER_WIN);

            }
            else if (dealer_.GetHandValue() > Constants.VALUE_OF_21)
            {
                player_.Wins_++;
                EndRound(RoundResult.PLAYER_WIN);
            }
            else if (dealer_.GetHandValue() > GetHandValue(hand))
            {
                //UpdateCardDisplayForDealer();
                EndRound(RoundResult.DEALER_WIN);
            }
            else
            {
                EndRound(RoundResult.PUSH);
            }
        }

        public void EndRound(RoundResult result)
        {
            string message = "";
            switch (result)
            {
                case RoundResult.PUSH:
                    player_.AddChips();
                    message = "Player and Dealer Push.";
                    break;
                case RoundResult.PLAYER_WIN:
                    message = $"Player Wins {player_.WinBet(false)} chips";

                    break;
                case RoundResult.PLAYER_BUST:

                    message = $"Player Busts Loses {player_.Bet_} chips";
                    break;
                case RoundResult.PLAYER_BLACKJACK:
                    message = $"Player Wins {player_.WinBet(true)} chips";
                    break;
                case RoundResult.DEALER_WIN:

                    message = $"Dealer Wins! {player_.Bet_} chips lost";
                    player_.ClearBet();
                    break;
                case RoundResult.SURRENDER:
                    message = $"Player Surrenders {player_.Bet_ / Constants.VALUE_OF_2} chips";
                    player_.Chips_ += player_.Bet_ / Constants.VALUE_OF_2;
                    player_.ClearBet();
                    break;
                case RoundResult.INVALID_BET:
                    message = "Invalid Bet.";
                    break;
            }

            if (player_.Chips_ <= Constants.VALUE_OF_O)
            {
                message = $"Player is broke {player_.Chips_} chips left";
                player_ = new Player();
            }
            OnRoundEnded(message);
        }

    }
}
