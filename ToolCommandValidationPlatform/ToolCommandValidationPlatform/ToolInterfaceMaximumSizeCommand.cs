using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMEIC.TMdN.Constants;

namespace DriveCommands
{
    /// <summary>
    /// QS
    /// </summary>
    public class ToolInterfaceMaximumSizeCommand
    {
        IUdpSocket socket;

        public ToolInterfaceMaximumSizeCommand(IUdpSocket udpSocket)
        {
            socket = udpSocket;
        }

        public new void Execute()
        {           
            byte[] command = { 81, 83 }; //QS
            byte[] drivecommand = new byte[22]; //header.Length + command.Length

            CommandConstants.HEADER.CopyTo(drivecommand, 0);
            command.CopyTo(drivecommand, CommandConstants.HEADER.Length);

            socket.Send(drivecommand);
        }        
    }
}
