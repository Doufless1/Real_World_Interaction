using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Batman.Enums;
namespace Batman
{
    public class Card
    {
        public Suit Suit_ { get; set; }
        public Face Face_ { get; }
        public int Value_ { get; set; }

        public Card(Suit suit, Face face)
        {
            Suit_ = suit;
            Face_ = face;


            switch (Face_)
            {
                case Face.Jack:
                case Face.Queen:
                case Face.King:
                    Value_ = Constants.VALUE_OF_10; break;
                case Face.Ace:
                    Value_ = Constants.VALUE_OF_11; break;
                default:
                    Value_ = (int)Face_; break;
            }
        }
    }
}
