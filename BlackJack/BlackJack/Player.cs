using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Player : IPlayer
    {
        //public string Name_="";
        public int Chips_ { get ; set; } = 500;
        public int Bet_ { get; set; }
        public int Wins_ { get; set; }
        public int HandsCompleted_ { get; set; } = 1;

        public List<Card> Hand_ {  get; set; }

        public List<List<Card>> SplitHands_ { get; set; }


        public Player() 
        {
            Hand_ = new List<Card>();
            SplitHands_ = new List<List<Card>>();
            Hand_.Clear();
        }

        public void AddBet(int bet)
        {
            Bet_ += bet;
            Chips_ -= bet;
        }

        public void Setbet(int bet){Bet_ = bet; }

        public void ClearBet() { Bet_ = 0; }

        public void AddChips()
        {
            Chips_ += Bet_;
            ClearBet();
        }

        public int GetHandValue()
        {
            int value = 0;
            foreach (Card card in Hand_)
            {
                value += card.Value_;
            }
            return value;
        }

        public int WinBet(bool blackjack)
        {
            int chipsWon;
            if (blackjack)
            {
                chipsWon = (int)Math.Floor(Bet_ * 1.5);
            }
            else
            {
                chipsWon = Bet_ * 2;
            }

            Chips_ += chipsWon;
            ClearBet();
            return chipsWon;
        }

        public void WriteHand()
        {
            Console.WriteLine($"Players's Hand ( {GetHandValue()} ):");
            foreach (Card card in Hand_)
            {
                card.Description();
            }
        }




    }
}
