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
        public Suit Suit_ { get; }
        public Face Face_ { get; }
        public int Value_ { get; set; }
        public string ImageofCards => $"Images/{Suit_.ToString().ToLower()}{Face_.ToString().ToLower()}.gif";

        public Card(Suit suit, Face face)
        {
            Suit_ = suit;
            Face_ = face;


            switch (Face_)
            {
                case Face.Jack:
                case Face.Queen:
                case Face.King:
                    Value_ = 10; break;
                case Face.Ace:
                    Value_ = 11; break;
                default:
                    Value_ = (int)Face_; break;
            }
        }

   /*     public void Description()
        {
            switch (Suit_)
            {
                case Suit.Diamonds:
                    Console.WriteLine("♦ "); break;
                case Suit.Hearts:
                    Console.WriteLine("♥ "); break;
                case Suit.Spades:
                    Console.WriteLine("♠ "); break;
                case Suit.Clubs:
                    Console.WriteLine("♣ "); break;
            }

            switch (Face_)
            {
                case Face.Ace:
                    Console.Write($"Ace"); break;
                case Face.Two:
                    Console.Write($"Two"); break;
                case Face.Three:
                    Console.Write($"Three"); break;
                case Face.Four:
                    Console.Write($"Four"); break;
                case Face.Five:
                    Console.Write($"Five"); break;
                case Face.Six:
                    Console.Write($"Six"); break;
                case Face.Seven:
                    Console.Write($"Seven"); break;
                case Face.Eight:
                    Console.Write($"Eight"); break;
                case Face.Nine:
                    Console.Write($"Nine"); break;
                case Face.Ten:
                    Console.Write($"Ten"); break;
                case Face.Jack:
                    Console.Write($"Jack"); break;
                case Face.Queen:
                    Console.Write($"Queen"); break;
                case Face.King:
                    Console.Write($"King"); break;
            }

            Console.Write($" + {Value_}");
            Console.WriteLine();
        }*/
    }
}
