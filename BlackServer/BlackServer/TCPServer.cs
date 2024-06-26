using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

namespace BlackServer
{
    public class TCPserver
    {
        private TcpListener tcpListener_;
        private int port_ { get; set; }
        private IPAddress hostAddress_ { get; set; }
       
        private IDeck deck_;



        public TCPserver(IDeck deck)
        {
            deck_ = deck;
            port_ = 13000;
            hostAddress_ = IPAddress.Parse("127.0.0.1");
            tcpListener_ = new TcpListener(hostAddress_, port_);
            StartServerAsync().Wait();
        }

        private async Task StartServerAsync()
        {
            tcpListener_.Start();
            Console.WriteLine("I Live");

            while (true)
            {
                try
                {
                    using TcpClient client = await tcpListener_.AcceptTcpClientAsync();
                    Console.WriteLine("Client connected...");
                    _=HandleClientAsync(client);
                }
                catch (Exception ex) { Console.WriteLine($"It hit the fan: {ex.Message}"); }
            }
         
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            bool exit=false;
            using NetworkStream tcpStream_ = client.GetStream();
            byte[] buffer = new byte[byte.MaxValue];
            
            try
            {
               int readTotal = 0;
                while (( readTotal = tcpStream_.ReadAsync(buffer, 0, buffer.Length).Result) != 0)
                {
                    string text = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Trim();
                    Console.WriteLine($"Received: <{text}>");

                    List<Card> hand = new List<Card>();
                    text = text.ToUpper();

                    if (text.Contains("DEAL")) hand = deck_.DealHand();     //gives two cards
                    else if (text.Contains("HIT")) hand.Add(deck_.DrawCard());         //gives 1 card
                    else if (text.Contains("NEW")) deck_.Initialize();      //creats new deck and suffles it 
                    else if (text.Contains("Q")) exit = !exit;

                    //print out hand as Hand(cardsuit,cardface,cardvalue;)
                    var message = "Hand(";
                    foreach (Card card in hand)
                    {
                        message += card.SendDescrition();
                    }
                    message += ")";

                    byte[] response = Encoding.UTF8.GetBytes(message);
                    await tcpStream_.WriteAsync(response, 0, response.Length);
                    //clear buffer so no retaining values
                    buffer.DefaultIfEmpty();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"CLient Error:{ex.Message}");
            }
            finally
            {
                    client.Close();
                    Console.WriteLine("I DIED!!! IT Hurts. . . . ");
            }
           
}
    }
}
