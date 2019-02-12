using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using DriveCommands.Commands;

namespace DriveCommands
{
    public class DriveController
    {
        private IUdpSocket udpSocket;

        private static EndPoint remoteEndpoint;
        static AsyncCallback m_ReceiveCallBack;
        static  byte[] buffer;

        private List<ICommand> commands = new List<ICommand>();

        public DriveController(IUdpSocket socket)
        {
            udpSocket = socket;
        }
        public void AddCommand(ICommand command)
        {
            commands.Add(command);
        }

        public  void ExecuteCommand(string input)
        {
            byte[] command = Encoding.ASCII.GetBytes(input);

            byte[] driveCommand = new byte[header.Length + command.Length];

            //Merge header and command
            header.CopyTo(driveCommand, 0);
            command.CopyTo(driveCommand, header.Length);

            udpSocket.Send(driveCommand);
        }
        
        //Header 
        //Control Command - 2 bytes - 83,78 (SN : PC - Drive)
        //Result Bytes- 2 bytes, 48, 48 (00 : PC - Drive )
        //Slot 2 byte - 00 BUS, UNIT, SLOT  00
        //Station  2 byte - 00
        //Length of Data - 2 byte 12 , 0, TS-S20 header + Message 2 
        // Header 10 bytes
        static byte[] header = { 83, 78, 48, 48, 0, 0, 0, 0, 12, 0, 8, 0, 64, 1, 0, 118, 0, 0, 0, 0 };
     }
}
