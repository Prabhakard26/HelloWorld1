using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DriveCommands
{
    /// <summary>
    /// Responsible for sending commands to Drive and reading Drive response for requested command.
    /// </summary>
    public class UdpSocket : IUdpSocket
    {        
        private readonly byte[] _mDataBuffer;
        private int _mDataBytesRead = 0;

        public EndPoint DriveEndPoint { get; set; }

        public EndPoint RemoteDriveEndPoint;

        private readonly Socket socket;
        public  UdpSocket(EndPoint driveEndPoint, bool isBroadCast)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            if (isBroadCast)
            {
                socket.Blocking = false;
                socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                socket.EnableBroadcast = true;
            }

            _mDataBuffer = new byte[socket.ReceiveBufferSize];

            DriveEndPoint = driveEndPoint;
            RemoteDriveEndPoint =  new IPEndPoint(IPAddress.Parse("10.29.80.50"), 22359);
        }
        public void Send(byte[] drivecommand)
        {
            socket.SendTo(drivecommand, DriveEndPoint);

            Task.Factory.StartNew( () => ReceiveData(drivecommand));
        }
        
        private Dictionary<int, bool> dictionary = new Dictionary<int,bool>(4);
        public void ReceiveData(byte [] request)
        {
            try
            {
               IAsyncResult result = socket.BeginReceiveFrom(_mDataBuffer, 0, _mDataBuffer.Length, SocketFlags.None, ref RemoteDriveEndPoint, OnDataReceived, request);
               bool completed = result.AsyncWaitHandle.WaitOne(5000);

               if (completed)
               {
                   Console.WriteLine("succeeded");
                }
               else
               {
                   Console.WriteLine("failed");
               }
            }
            catch (SocketException socketException)
            {
            }
            catch (Exception exception)
            {
            }
        }

        private void OnDataReceived(IAsyncResult a_AsyncState)
        {
            try
            {
                if (a_AsyncState.IsCompleted)
                {
                    _mDataBytesRead = socket.EndReceiveFrom(a_AsyncState, ref RemoteDriveEndPoint);
                  
                    if (_mDataBytesRead >= 20)
                    {
                        byte[] tempBuffer = new byte[_mDataBytesRead - 20];
                        Array.Copy(_mDataBuffer, 20, tempBuffer, 0, _mDataBytesRead - 20);

                        //IAsyncResult iResult = socket.BeginReceiveFrom(_mDataBuffer, 0, _mDataBuffer.Length,SocketFlags.None, ref RemoteDriveEndPoint, OnDataReceived, 100);

                        if (null != OnDriveDataReceived)
                        {
                            OnDriveDataReceived(this, tempBuffer);
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
            }
            catch (Exception exception)
            {
            }
        }
               
        public event EventHandler<byte[]> OnDriveDataReceived;
    }
}
