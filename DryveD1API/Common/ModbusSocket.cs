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
        private static Dictionary<IPAddress, ControllerConnection> connectionList;


        public static ControllerConnection GetConnection(string server, int port)
        {

            if (connectionList is null)
            {
                connectionList = new Dictionary<IPAddress, ControllerConnection>();
            }
            IPAddress ipAddress = IPAddress.Parse(server);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (connectionList.ContainsKey(ipAddress))
            {
                var connection = connectionList[ipAddress];
                if (connection.socket.Connected)
                {
                    watch.Stop();
                    Debug.WriteLine($"Connected: {watch.ElapsedMilliseconds}");
                    return connection;
                }
                else
                {
                    connectionList.Remove(ipAddress);
                    connection.socket = Connect(ipAddress, port);
                    connection.MultiplicationFactor = GetMultiplicationFactor(connection.socket);
                    connectionList.Add(ipAddress, connection);
                    watch.Stop();
                    Debug.WriteLine($"Not connected: {watch.ElapsedMilliseconds}");
                    return connection;
                }
            }
            else
            {
                watch = new Stopwatch();
                watch.Start();
                var connection = new ControllerConnection();
                connection.socket = Connect(ipAddress, port);
                connection.MultiplicationFactor = GetMultiplicationFactor(connection.socket);
                connectionList.Add(ipAddress, connection);
                watch.Stop();
                Debug.WriteLine($"Not existing: {watch.ElapsedMilliseconds}");
                return connection;
            }
        }

        private static double GetMultiplicationFactor(Socket s)
        {
            var siUnitPosition = new SIUnitPosition();
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
            foreach (var controllerConnection in connectionList)
            {
                if (controllerConnection.Value.socket.Connected)
                {
                    Close(controllerConnection.Value.socket);
                }
            }
        }

        private static void Close(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }
    }

    public class ControllerConnection
    {
        public Socket socket { get; set; }

        public double MultiplicationFactor { get; set; }
    }
}
