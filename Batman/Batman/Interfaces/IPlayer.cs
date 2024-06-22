using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batman
{
    public interface IPlayer
    {
        int Chips_ { get; set; }
        int Bet_ { get; set; }
        int Wins_ { get; set; }
        int HandsCompleted_ { get; set; }
        List<Card> Hand_ { get; set; }

        List<List<Card>> SplitHands_ { get; set; }

        void AddBet(int bet);
        //void Setbet(int bet);
        void ClearBet();
        public int GetHandValue();
       // public void WriteHand();
        public int WinBet(bool blackjack);
        public void AddChips();
    }
}
