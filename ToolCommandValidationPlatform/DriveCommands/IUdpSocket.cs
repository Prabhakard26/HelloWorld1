using System;

namespace DriveCommands
{
    public interface IUdpSocket
    {
        event EventHandler<byte[]> OnDriveDataReceived;

        void Send(byte [] data);
    }
}
