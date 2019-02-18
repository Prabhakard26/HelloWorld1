using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

namespace ToolCommandValidationPlatform
{
    /// <summary>
    /// This class used to manage discover drive related all process,
    /// like sending commands (Q1, QC, QT, Q5) to drives as well as collecting their responces,
    /// arranging in a collection and sending result to requested client.
    /// </summary>
    public class DiscoverDriveManager
    {
        private int receivedDataBytes = 0;
        private byte[] dataBuffer;
        private AsyncCallback callbackReceived = null;
        private EndPoint endpointRemote;
        private Socket m_socket = null;

        private ObservableCollection<string> listDiscoveredDrives;
        private EndPoint endpointDestination;
        public DiscoverDriveManager()
        {
            IPEndPoint endpointBroadcast = new IPEndPoint(IPAddress.Broadcast, 22359);
            endpointDestination = endpointBroadcast;

            listDiscoveredDrives = new ObservableCollection<string>();

            endpointRemote = new IPEndPoint(IPAddress.Any, 22360);
        }

        public  ObservableCollection<string> Drives => listDiscoveredDrives;

        #region "Method"

        public void DoDiscoverDrive(Socket socket)
        {
            try
            {
                // Response data buffer size
                this.dataBuffer = new byte[socket.ReceiveBufferSize];
                // UDP socket reference
                m_socket = socket;
                m_socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                m_socket.EnableBroadcast = true;
                // Send Q1 command to collect product type.
                GetProductType();
            }
            catch(Exception ex)
            {
                return;
            }
        }
        private void GetProductType()
        {
       
            try
            {
                byte[] command = Encoding.ASCII.GetBytes("QT");

                byte[] driveCommand = new byte[header.Length + command.Length];

                //Merge header and command
                header.CopyTo(driveCommand, 0);
                command.CopyTo(driveCommand, header.Length);

                // Send command packet on network
                SendData(driveCommand, endpointDestination);

            }
            catch (Exception ex)
            {
                return;
            }
        }
        static byte[] header = { 83, 78, 48, 48, 0, 0, 0, 0, 12, 0, 8, 0, 64, 1, 0, 118, 0, 0, 0, 0 };
        private void SendData(byte[] packetData, EndPoint endpointDestination)
        {
            try
            {
                m_socket.SendTo(packetData, endpointDestination);
                ReceiveData();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void ReceiveData()
        {
            try
            {
                if (null == callbackReceived)
                {
                    callbackReceived = new AsyncCallback(OnDataReceived);
                    object tempObject = new object();
                    IAsyncResult iResult = m_socket.BeginReceiveFrom(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, ref endpointRemote, OnDataReceived, tempObject);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnDataReceived(IAsyncResult a_AsyncState)
        {
            try
            {
                if (a_AsyncState.IsCompleted)
                {
                    receivedDataBytes = m_socket.EndReceiveFrom(a_AsyncState, ref endpointRemote);
                    byte[] tempBuffer = new byte[receivedDataBytes];
                    Array.Copy(dataBuffer, tempBuffer, receivedDataBytes);

                    IPEndPoint localIpEndPoint = endpointRemote as IPEndPoint;
                    if (localIpEndPoint != null)
                    {
                        var tmp =localIpEndPoint.Address.ToString();
                        if (false == Drives.Contains(tmp))
                        {
                            Drives.Add(tmp);
                        }

                        listDiscoveredDrives.Add(localIpEndPoint.Address.ToString());
                    }
                    
                    object tempObject = null;
                    IAsyncResult iResult = m_socket.BeginReceiveFrom(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, ref endpointRemote, callbackReceived, tempObject);
                }
            }
            catch (Exception ex)
            {
                StopDiscoverDriveManager();
            }
        }

        public void StopDiscoverDriveManager()
        {
            this.callbackReceived = null;
        }

        #endregion "Method"
    }
}
