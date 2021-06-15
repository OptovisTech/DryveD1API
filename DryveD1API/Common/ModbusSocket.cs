using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace DryveD1API.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModbusSocket
    {
        private static Dictionary<IPAddress, Socket> socketList;


        public static Socket GetConnection(string server, int port)
        {

            if (socketList is null)
            {
                socketList = new Dictionary<IPAddress, Socket>();
            }
            IPAddress ipAddress = IPAddress.Parse(server);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (socketList.ContainsKey(ipAddress))
            {
                var socket = socketList[ipAddress];
                if (socket.Connected)
                {
                    watch.Stop();
                    Debug.WriteLine($"Connected: {watch.ElapsedMilliseconds}");
                    return socket;
                }
                else
                {
                    socketList.Remove(ipAddress);
                    socket = Connect(ipAddress, port);
                    socketList.Add(ipAddress, socket);
                    watch.Stop();
                    Debug.WriteLine($"Not connected: {watch.ElapsedMilliseconds}");
                    return socket;
                }
            }
            else
            {
                watch = new Stopwatch();
                watch.Start();
                var socket = Connect(ipAddress, port);
                watch.Stop();
                Debug.WriteLine($"Not existing: {watch.ElapsedMilliseconds}");
                socketList.Add(ipAddress, socket);
                return socket;
            }
        }

        /// <summary>
        /// Create a socket connection with the specified server and port.
        /// </summary>
        /// <param name="iPAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private static Socket Connect(IPAddress iPAddress, int port)
        {
            Socket s = null;
            Socket tempSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(iPAddress, port);
            tempSocket.Connect(ipe);

            if (tempSocket.Connected)
            {
                s = tempSocket;
            }
            return s;
        }

        public static void CLoseAll()
        {
            foreach (var socket in socketList)
            {
                if (socket.Value.Connected)
                {
                    Close(socket.Value);
                }
            }
        }

        private static void Close(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }
    }
}
