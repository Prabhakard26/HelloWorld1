using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DriveCommands.Commands;

namespace DriveCommands
{
    public class DriveController
    {
        private  Semaphore _pool;
        private static bool _done;
        private readonly IUdpSocket _udpSocket;

        BlockingCollection<ICommand> dataItems = new BlockingCollection<ICommand>(10);

        private Queue<ICommand> _commandsInExecution;

        
        public DriveController(IUdpSocket socket, int count)
        {
            _udpSocket = socket;

            if (false == _done)
            {
                _done = true;

                _udpSocket.OnDriveDataReceived -= UdpSocket_OnDriveDataReceived;
                _udpSocket.OnDriveDataReceived += UdpSocket_OnDriveDataReceived;

                _pool = new Semaphore(count, count);

                _commandsInExecution = new Queue<ICommand>(count);

                Consumer();
            }
        }
        
        public void AddCommand(ICommand command)
        {
            dataItems.Add(command);

            //dataItems.CompleteAdding();
        }

        public void Consumer()
        { 
            // A simple blocking consumer with no cancellation.
            Task.Run(() =>
            {
                ICommand data = null;

                while (!dataItems.IsCompleted)
                {                        
                    try
                    {
                        data = dataItems.Take();
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    if (data != null)
                    {
                        ExecuteCommand(data);
                    }
                }
            });
        }
      

        public  void ExecuteCommand(ICommand input)
        {
            _pool.WaitOne();

            byte[] command = Encoding.ASCII.GetBytes(input.Name);

            byte[] driveCommand = new byte[header.Length + command.Length];

            //Merge header and command
            header.CopyTo(driveCommand, 0);
            command.CopyTo(driveCommand, header.Length);
            
            _udpSocket.Send(driveCommand);

            _commandsInExecution.Enqueue(input);

        }
        static readonly object Resendlogic = new object();
        private void UdpSocket_OnDriveDataReceived(object sender, byte[] e)
        {
            lock (Resendlogic)
            {
                string temp = Encoding.UTF8.GetString(e);
                ICommand cmd = _commandsInExecution.Dequeue();

                if (!temp.StartsWith(cmd.Name))
                {
                    AddCommand(cmd);
                }
                _pool.Release(1);
            }
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
