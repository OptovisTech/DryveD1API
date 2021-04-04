using System.Net;
using System.Net.Sockets;

namespace DryveD1API
{
    public interface ISocketProxy
    {
        bool Connected();
        //void Connect(EndPoint remoteEP);
        void Close();
        void Shutdown(SocketShutdown how);
        int Send(byte[] buffer);
        int Receive(byte[] buffer);
        ISocketProxy Accept();
    }
}