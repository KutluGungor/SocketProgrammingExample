using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    class Program
    {
        private static TcpListener listener;
        static void Main(string[] args)
        {
            //Belirtilen Ip adresi üzerindeki port dinlenmeye başlanır.
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8000);
            Console.WriteLine("Dinleme Başlatıldı...");
            Console.WriteLine();
            listener.Start();

            while (true)
            {
                
                //Client'tan gelecek istek kabul edilir.
                TcpClient client = listener.AcceptTcpClient();
                
                //Network üzerinden gelen data alınır.
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                
                //Gelen data okunur.
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                
                //Datanın string türüne çevrilmesi gerçekleştirilir.
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);


                if (dataReceived == "DATETIME")
                {
                    Console.WriteLine("Gelen Bilgi : " + dataReceived);

                    //Client'a data gönderilir.
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(DateTime.Now.ToString());
                    Console.WriteLine("İstemciye Gönderilen Bilgi : " + DateTime.Now);
                    Console.WriteLine("");
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                client.Close();
            }

            listener.Stop();
            Console.ReadLine();
        }
    }
}
