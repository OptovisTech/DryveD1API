using System.Net;
using System.Net.Sockets;

namespace DryveD1API
{
    public class SocketProxy : ISocketProxy
    {
        private readonly Socket _tcpSocket;
        public SocketProxy()
        {
            _tcpSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
        }
        public SocketProxy(Socket tcpSocket)
        {
            _tcpSocket = tcpSocket;
        }
        public bool Connected()
        {
            return _tcpSocket.Connected;
        }
        public void Close()
        {
            _tcpSocket.Close();
        }
        public ISocketProxy Accept()
        {
            return new SocketProxy(_tcpSocket.Accept());
        }

        public void Shutdown(SocketShutdown how)
        {
            _tcpSocket.Shutdown(how);
        }

        public int Send(byte[] buffer)
        {
            return _tcpSocket.Send(buffer);
        }

        public int Receive(byte[] buffer)
        {
            return _tcpSocket.Receive(buffer);
        }
        /*
        public ISocketProxy Connect(EndPoint remoteEP)
        {
            return _tcpSocket.Connect(remoteEP);
        }*/
    }
}
