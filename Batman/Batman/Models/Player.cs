using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batman
{
    public class Player : IPlayer
    {
        public event EventHandler<int> ChipsChanged;

        private int chips_;

        public int Chips_
        {
            get => chips_;
            set
            {
  
                    chips_ = value;
                    OnChipsChanged(chips_);
             
            }
        }

        public int Bet_ { get; set; }
        public int Wins_ { get; set; }
        public int HandsCompleted_ { get; set; } = Constants.VALUE_OF_1;

        public List<Card> Hand_ { get; set; }

        public List<List<Card>> SplitHands_ { get; set; }

        public Player()
        {
            Hand_ = new List<Card>();
            SplitHands_ = new List<List<Card>>();
            Hand_.Clear();
            Chips_ = 500;
        }

        protected virtual void OnChipsChanged(int newChipCount)
        {
            ChipsChanged?.Invoke(this, newChipCount);
        }

        public void AddBet(int bet)
        {
            Bet_ += bet;
            if (Chips_ - bet >= Constants.VALUE_OF_O)
            {
                Chips_ -= bet;
            }
            else
            {
                throw new InvalidOperationException("Not enough chips to place bet.");
            }
        }

        public void Setbet(int bet) { Bet_ = bet; }

        public void ClearBet() { Bet_ = Constants.VALUE_OF_O; }

        public void AddChips()
        {
            Chips_ += Bet_;
            ClearBet();
        }

        public int GetHandValue()
        {
            int value = Constants.VALUE_OF_O;
            int Ace_counter = Constants.VALUE_OF_O;
            foreach (Card card in Hand_)
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

        public int WinBet(bool blackjack)
        {
            int chipsWon;
            if (blackjack)
            {
                chipsWon = (int)Math.Floor(Bet_ * 2.5);
            }
            else
            {
                chipsWon = Bet_ * Constants.VALUE_OF_2;
            }

            Chips_ += chipsWon;
            ClearBet();
            return chipsWon;
        }
    }
}
