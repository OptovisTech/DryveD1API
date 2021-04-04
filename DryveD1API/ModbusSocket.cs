using System;
using System.Net;
using System.Net.Sockets;

namespace DryveD1API
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModbusSocket
    {
        /// <summary>
        /// Create a socket connection with the specified server and port.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Socket Connect(string server, int port)
        {
            Socket s = null;
            IPAddress iPAddress = IPAddress.Parse(server);
            Socket tempSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(iPAddress, port);
            tempSocket.Connect(ipe);

            if (tempSocket.Connected)
            {
                s = tempSocket;
            }
            return s;
        }

        public static void Close(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }
    }
}
