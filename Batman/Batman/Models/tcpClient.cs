using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Batman.Models
{
    public class tcpClient
    {
      
        

        public List<Card> StartClient(string message)
        {
            try
            {
                // Set up the TcpClient
                Int32 port = 13000;
                var client = new TcpClient("127.0.0.1", port);

                // Translate the passed message into ASCII and store it as a Byte array.
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // Loop to send and receive data
                //while (true)
                //{
                    

                    

                    

                    // Receive response from the server
                    data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);
                    string responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    
                    var par = new Parser();
                    var parsedCard = par.ParseCard(responseData);
                    
                //}

                // Close everything
                stream.Close();
                client.Close();

                return parsedCard;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();

            return new List<Card>();
        }

        

        
    }
}
