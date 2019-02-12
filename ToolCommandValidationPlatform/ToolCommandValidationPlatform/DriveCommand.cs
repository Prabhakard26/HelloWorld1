using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TMEIC.TMdN.Constants;

namespace TMEIC.TMdN.DriveCommands
{
    public abstract class DriveCommand
    {
        Socket socket;
        string driveIPAddress = "255.255.255.255";
        int drivePort = 22359;

        private AsyncCallback m_ReceiveCallBack;
        private byte[] m_DataBuffer;
        private int m_DataBytesRead = 0;
        EndPoint DriveEndPoint;
        EndPoint remoteEndPoint;


        public void Execute()
        {
           
        }

        public void ReceiveData()
        {
            try
            {
                if (null == m_ReceiveCallBack)
                {
                    socket.BeginReceiveFrom(m_DataBuffer, 0, m_DataBuffer.Length, SocketFlags.None, ref DriveEndPoint,  new AsyncCallback(OnDataReceived), null);
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
                    m_DataBytesRead = socket.EndReceiveFrom(a_AsyncState, ref DriveEndPoint);
                    byte[] tempBuffer = new byte[m_DataBytesRead];
                    Array.Copy(m_DataBuffer, tempBuffer, m_DataBytesRead);
                    object tempObject = null;
                    IAsyncResult iResult = socket.BeginReceiveFrom(m_DataBuffer, 0, m_DataBuffer.Length, SocketFlags.None, ref DriveEndPoint, m_ReceiveCallBack, tempObject);

                    ProcessCommandResponse(tempBuffer);
                }
            }
            catch (SocketException socketException)
            {
            }
            catch (Exception exception)
            {
            }
        }

        private void ProcessCommandResponse(byte[] tempBuffer)
        {
            Console.WriteLine(Encoding.ASCII.GetString(tempBuffer).Trim());

        }
    }
}
