using System.Numerics;

namespace BlackServer 
{
    class Program
    {

        private static void Main(string[] args)
        {
            // Console cannot render unicode characters without this line

            Console.Title = "♠♥♣♦ Server";

            
            IDeck deck = new Deck();
            var server = new TCPserver(deck);
           
            
        }





    }
}