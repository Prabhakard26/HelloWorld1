using System;
using System.Net;

namespace DriveCommands
{
    public interface IUdpSocket
    {
        EndPoint DriveEndPoint { get; set; }

        event EventHandler<byte[]> OnDriveDataReceived;

        void Send(byte [] data);
    }
}
