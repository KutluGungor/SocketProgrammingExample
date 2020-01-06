using System;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace ClientApp
{
    class Program
    {
        private static Timer _timerTwentySeconds;
        private static TcpClient client;
        private static NetworkStream nwStream;
        static void Main(string[] args)
        {
            //Timer tanımlaması yapılır.
            _timerTwentySeconds = new Timer(20000);
            _timerTwentySeconds.Elapsed += new ElapsedEventHandler(StateTwentySecond);
            _timerTwentySeconds.Start();

            Console.ReadLine();
            client.Close();
        }

        private static void StateTwentySecond(object sender, ElapsedEventArgs e)
        {
            string textToSend = "DATETIME";
            
            //Bu Ip üzerindeki portta Client oluşturulur.
            client = new TcpClient("127.0.0.1", 8000);
            nwStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
            

            Console.WriteLine("Gönderilen Bilgi : " + textToSend);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            
            //Serverdan dönen değer okunur.
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            var received = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            if (received == DateTime.Now.ToString())
            {
                Console.WriteLine("Sunucu ile zamanımız aynı.");
            }
            else
            {
                Console.WriteLine("Sunucu ile zamanımız farklı");
            }

            Console.WriteLine("Sunucudan Gelen Bilgi : " + received);
            Console.WriteLine();
        }
    }
}
