using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicassoClient
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {

            // data buffer for incoming data 
            byte[] bytes = new byte[1024];

            // connect to a Remote device 
            try
            {
                // Establish the remote end point for the socket 
                IPAddress ipAddr = Dns.GetHostEntry("127.0.0.1").AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

                Socket sender = new Socket(AddressFamily.InterNetwork,
                                           SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint 

                sender.Connect(ipEndPoint);

                Console.WriteLine("Socket connected to {0}",
                sender.RemoteEndPoint.ToString());

                string theMessage = "This is a test";

                byte[] msg = Encoding.ASCII.GetBytes(theMessage + "<theend>");

                // Send the data through the socket 
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device 
                int bytesRec = sender.Receive(bytes);

                Console.WriteLine("The Server says : {0}",
                                  Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Release the socket 
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
        }
    }
}
