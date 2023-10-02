using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using DryveD1API.Modules;

namespace DryveD1API.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModbusSocket
    {
        private static Dictionary<IPAddress, ControllerConnection> _connectionList;

        /// <summary>
        ///
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ControllerConnection GetConnection(string server, int port)
        {
            _connectionList ??= new Dictionary<IPAddress, ControllerConnection>();

            var ipAddress = IPAddress.Parse(server);
            var watch = new Stopwatch();
            watch.Start();
            if (_connectionList.ContainsKey(ipAddress))
            {
                var connection = _connectionList[ipAddress];
                if (connection.Socket.Connected)
                {
                    watch.Stop();
                    Debug.WriteLine($"Connected: {watch.ElapsedMilliseconds}");
                    return connection;
                }

                _connectionList.Remove(ipAddress);
                connection.Socket = Connect(ipAddress, port);
                connection.MultiplicationFactor = GetMultiplicationFactor(connection.Socket);
                _connectionList.Add(ipAddress, connection);
                watch.Stop();
                Debug.WriteLine($"Not connected: {watch.ElapsedMilliseconds}");
                return connection;
            }
            else
            {
                watch = new Stopwatch();
                watch.Start();
                var connection = new ControllerConnection
                {
                    Socket = Connect(ipAddress, port)
                };
                connection.MultiplicationFactor = GetMultiplicationFactor(connection.Socket);
                _connectionList.Add(ipAddress, connection);
                watch.Stop();
                Debug.WriteLine($"Not existing: {watch.ElapsedMilliseconds}");
                return connection;
            }
        }

        private static double GetMultiplicationFactor(Socket s)
        {
            var siUnitPosition = new SiUnitPosition();
            siUnitPosition.Read(s);
            return siUnitPosition.GetMultiplicationFactorValue();
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
            var tempSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var ipe = new IPEndPoint(iPAddress, port);
            tempSocket.Connect(ipe);

            if (tempSocket.Connected)
            {
                s = tempSocket;
            }

            return s;
        }

        /// <summary>
        ///
        /// </summary>
        public static void CLoseAll()
        {
            if (_connectionList is null)
            {
                return;
            }

            foreach (var (_, connection) in _connectionList)
            {
                if (connection?.Socket is not null && connection.Socket.Connected)
                {
                    Close(connection.Socket);
                }
            }
        }

        private static void Close(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class ControllerConnection
    {
        /// <summary>
        ///
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        ///
        /// </summary>
        public double MultiplicationFactor { get; set; }
    }
}
