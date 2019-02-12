using System;
using System.Net;
using System.Net.Sockets;

namespace DriveCommands
{
    public class UdpSocket : IUdpSocket
    {
        Socket socket;

        private AsyncCallback m_ReceiveCallBack;
        private byte[] m_DataBuffer;
        private int m_DataBytesRead = 0;

        public EndPoint DriveEndPoint;

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

            m_DataBuffer = new byte[socket.ReceiveBufferSize];

            DriveEndPoint = driveEndPoint;
        }

        public void Send(byte[] drivecommand)
        {
            socket.SendTo(drivecommand, DriveEndPoint);  

            ReceiveData();
        }

        public void ReceiveData()
        {
            try
            {
               socket.BeginReceiveFrom(m_DataBuffer, 0, m_DataBuffer.Length, SocketFlags.None, ref DriveEndPoint, new AsyncCallback(OnDataReceived), null);               
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
                    m_DataBytesRead = socket.EndReceiveFrom(a_AsyncState, ref DriveEndPoint);
                  
                    if (m_DataBytesRead >= 20)
                    {
                        byte[] tempBuffer = new byte[m_DataBytesRead - 20];
                        Array.Copy(m_DataBuffer, 20, tempBuffer, 0, m_DataBytesRead - 20);
                        IAsyncResult iResult = socket.BeginReceiveFrom(m_DataBuffer, 0, m_DataBuffer.Length,
                            SocketFlags.None, ref DriveEndPoint, m_ReceiveCallBack, null);

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
