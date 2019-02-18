using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ToolCommandValidationPlatform

{
    public class NetworkInterfaceManager
    {
        // Regular expression pattern for IP address    
        private string ip_Pattern =
            @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

        public List<string> GetNetwotkInterfaceList()
        {
            List<string> listNetworkInterface = new List<string>();
            try
            {
                // Regular expression object    
                Regex check = new Regex(ip_Pattern);
                // Local pc host name
                string hostName = Dns.GetHostName();
                // Get available local pc interface IP from host entry table
                foreach (IPAddress ip in Dns.GetHostEntry(hostName).AddressList)
                {
                    // Is valid ip
                    if (check.IsMatch(ip.ToString(), 0))
                    {
                        // Add to the interface collection
                        listNetworkInterface.Add(ip.ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            return listNetworkInterface;
        }

        public void BindSocketWithBestNetworkInterface(string driveIP, string driveSubNetMask, Socket socket,
            int sourcePortNo)
        {
            try
            {
                if (!String.IsNullOrEmpty(driveIP) && !String.IsNullOrEmpty(driveSubNetMask))
                {
                    List<UnicastIPAddressInformation> lstInterfaceIPInfo = new List<UnicastIPAddressInformation>();

                    // Convert drive ip & subnet mask into byte array.
                    byte[] bytesDriveIP = (IPAddress.Parse(driveIP)).GetAddressBytes();
                    byte[] bytesDriveSubnetMaskIP = (IPAddress.Parse(driveSubNetMask)).GetAddressBytes();

                    // Do bitwise AND operation between drive ip & drive subnet mask ip.
                    byte[] bytesDriveIPBitwiseAndOutput = DoBitwiseAND(bytesDriveIP, bytesDriveSubnetMaskIP);

                    // Collect network interface
                    GetAdapters(ref lstInterfaceIPInfo);

                    // Find best network interface.
                    string bestInterface = SearchBestNetworkInterface(lstInterfaceIPInfo, bytesDriveIPBitwiseAndOutput);

                    if (bestInterface != string.Empty)
                    {
                        // Bind socket with available best network interface.
                        BindSocketWithNetworkInterface(socket, bestInterface, sourcePortNo);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string SearchBestNetworkInterface(List<UnicastIPAddressInformation> lstInterfaceIPInfo,
            byte[] bytesDriveIPBitwiseAndOutput)
        {
            string bestInterface = string.Empty;
            foreach (var interfaceInfo in lstInterfaceIPInfo)
            {
                try
                {
                    // Interface IP & subnet mask
                    byte[] bytesInterfaceIP = (IPAddress.Parse(interfaceInfo.Address.ToString())).GetAddressBytes();
                    byte[] bytesInterfaceSubnetMaskIP =
                        (IPAddress.Parse(interfaceInfo.IPv4Mask.ToString())).GetAddressBytes();

                    // Do bitwise AND operation between interface ip & subnet mask.
                    byte[] bytesInterfaceIPBitwiseAndOutput =
                        DoBitwiseAND(bytesInterfaceIP, bytesInterfaceSubnetMaskIP);

                    // Do bitwise XOR operation between "drive ip bitwise AND operation output" & "interface ip bitwise XOR output".
                    byte[] bytesInterfaceIPBitwiseXOROutput =
                        DoBitwiseXOR(bytesDriveIPBitwiseAndOutput, bytesInterfaceIPBitwiseAndOutput);

                    if (bytesInterfaceIPBitwiseXOROutput[0] == 0 && bytesInterfaceIPBitwiseXOROutput[1] == 0 &&
                        bytesInterfaceIPBitwiseXOROutput[2] == 0 && bytesInterfaceIPBitwiseXOROutput[3] == 0)
                    {
                        bestInterface = interfaceInfo.Address.ToString();
                        break;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return bestInterface;
        }

        public void GetAdapters(ref List<UnicastIPAddressInformation> lstInterfaceIPInfo)
        {
            try
            {
                foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation
                    .NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                lstInterfaceIPInfo.Add(ip);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private byte[] DoBitwiseAND(byte[] bytesA, byte[] bytesB)
        {
            byte[] bytesOutput = new byte[4];
            if (bytesA.Length == 4 && bytesB.Length == 4)
            {
                bytesOutput[0] = (byte) (bytesA[0] & bytesB[0]);
                bytesOutput[1] = (byte) (bytesA[1] & bytesB[1]);
                bytesOutput[2] = (byte) (bytesA[2] & bytesB[2]);
                bytesOutput[3] = (byte) (bytesA[3] & bytesB[3]);
            }

            return bytesOutput;
        }

        private byte[] DoBitwiseXOR(byte[] bytesA, byte[] bytesB)
        {
            byte[] bytesOutput = new byte[4];
            if (bytesA.Length == 4 && bytesB.Length == 4)
            {
                bytesOutput[0] = (byte) (bytesA[0] ^ bytesB[0]);
                bytesOutput[1] = (byte) (bytesA[1] ^ bytesB[1]);
                bytesOutput[2] = (byte) (bytesA[2] ^ bytesB[2]);
                bytesOutput[3] = (byte) (bytesA[3] ^ bytesB[3]);
            }

            return bytesOutput;
        }
        
        public bool BindSocketWithNetworkInterface(Socket socketBinding, string ipInterface, int destinationPortNo)
        {
            bool binded = false;
            try
            {
                if (socketBinding != null && socketBinding.IsBound == false)
                {
                    IPAddress ipLocalInterface = IPAddress.Parse(ipInterface);
                    IPEndPoint epLocalInterface = new IPEndPoint(ipLocalInterface, destinationPortNo);
                    socketBinding.Bind(epLocalInterface);
                    binded = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return binded;
        }
        /// <summary>
        /// Create a UDP socket for discover drive
        /// </summary>
        public Socket CreateSocket()
        {
            Socket socket = null;
            try
            {
                // Before creating udp socket, be secure that system resources(socket, port no) has free.
                // Otherwise this will throw error during socket binding
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(new IPEndPoint(IPAddress.Any, 9000));
                socket.Blocking = false;
                socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                socket.EnableBroadcast = true;
            }
            catch (Exception ex)
            {
                return socket;
            }
            return socket;
        }

        public void CloseSocket(Socket socket)
        {
            try
            {
                if (socket != null)
                {
                    socket.Blocking = true;
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
