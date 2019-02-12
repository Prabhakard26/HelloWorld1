using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMEIC.TMdN.Constants;

namespace DriveCommands
{
    /// <summary>
    /// TT
    /// </summary>
    public class TestModeCommand
    {
        IUdpSocket socket;

        public TestModeCommand(IUdpSocket udpSocket)
        {
            socket = udpSocket;
            socket.OnDriveDataReceived += OnDriveDataReceived;
        }

        private void OnDriveDataReceived(object sender, byte[] driveResponseBytes)
        {
            byte[] resultArray = new byte[8];

            Array.Copy(driveResponseBytes, 22, resultArray, 0, 8);

            Console.WriteLine(Encoding.ASCII.GetString(resultArray).Trim());                      
        }

        public new void Execute()
        {           
            byte[] command = { 81, 84 }; //QT
            byte[] drivecommand = new byte[22]; //header.Length + command.Length

            CommandConstants.HEADER.CopyTo(drivecommand, 0);
            command.CopyTo(drivecommand, CommandConstants.HEADER.Length);

            socket.Send(drivecommand);
        }        
    
    }
}
