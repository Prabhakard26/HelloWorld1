using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands
{
    public interface IUdpSocket
    {
        event EventHandler<byte[]> OnDriveDataReceived;

        void Send(byte [] data);
    }
}
