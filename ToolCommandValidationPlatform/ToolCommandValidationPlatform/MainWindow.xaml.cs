using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DriveCommands;

namespace ToolCommandValidationPlatform
{
    public class DummyCommand : DriveCommands.Commands.ICommand
    {
        public string Name { get; set; }
        public void Execute()
        {
            throw new NotImplementedException();
        }

        public DummyCommand(string cmd)
        {
            Name = cmd;
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DriveController driveController;

        IUdpSocket udpSocket;

        public ObservableCollection<string> _drives = new ObservableCollection<string>();

        public ObservableCollection<string> Drives
        {
            get { return _drives; }
            set { _drives = value; }
        }
      
        public MainWindow()
        {
            InitializeComponent();

            udpSocket = new UdpSocket(new IPEndPoint(IPAddress.Parse("10.29.80.50"), 22359), false);
            udpSocket.OnDriveDataReceived += UdpSocket_OnDriveDataReceived;

            driveController = new DriveController(udpSocket);
            driveController.CommandCount = selectedIndex + 1;

           var currentAssembly = Assembly.LoadFrom("DriveCommands.dll");

           var iDisposableAssemblies = currentAssembly.DefinedTypes.Where(type => type.ImplementedInterfaces.Any(inter => inter == typeof(DriveCommands.Commands.ICommand))).ToList();

           object [] args = { driveController };

            foreach (var typeInfo in iDisposableAssemblies)
            {
                try
                {
                    Type type = Type.GetType(typeInfo.AssemblyQualifiedName, true);

                    object t = Activator.CreateInstance(type, args);

                    LstCommands.Items.Add(t);
                }
                catch (Exception ex)
                {

                }
            }
           
            if (false == _drives.Contains("10.29.80.50"))
            {
                _drives.Add("10.29.80.50");
            }
            Task.Factory.StartNew(() => { StartDiscoverDrive(); });

            DataContext = this;
        }

        private void UdpSocket_OnDriveDataReceived(object sender, byte[] e)
        {
            this.Dispatcher.Invoke(() =>
            {
                ActualResultBytes.Text = string.Join(" ", e.Select(x => x.ToString("X2"))); /*BitConverter.ToString(e);*/
                ActualResult.Text = Encoding.UTF8.GetString(e);
            });

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string ip = DriveIp.Text;
            if (string.IsNullOrEmpty(ip))
            {
                ip = "10.29.80.50";
            }
            udpSocket.DriveEndPoint = new IPEndPoint(IPAddress.Parse(ip), 22359);
            foreach (var item in LstCommands.SelectedItems)
            {
                driveController.AddCommand((DriveCommands.Commands.ICommand)item);//(DriveCommands.Commands.ICommand)LstCommands.SelectedItem
            }           
        }
        private void ManualCommand(object sender, RoutedEventArgs e)
        {
            string ip = DriveIp.Text;
            if (string.IsNullOrEmpty(ip))
            {
                ip = "10.29.80.50";
            }
            udpSocket.DriveEndPoint = new IPEndPoint(IPAddress.Parse(ip), 22359);

            driveController.AddCommand((DriveCommands.Commands.ICommand) new DummyCommand(ManualCommandText.Text) );
        }

        private int selectedIndex = 2;
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            selectedIndex = cmb.SelectedIndex;
            if (driveController != null)
            {
                driveController.CommandCount = selectedIndex + 1;
            }
        }

        private void StartDiscoverDrive()
        {
            NetworkInterfaceManager ntInterface = new NetworkInterfaceManager();
            // Get interface list
            List<string> listNetworkInterface = ntInterface.GetNetwotkInterfaceList();

            // Iterate over interface list
            foreach (string interfaceIP in listNetworkInterface)
            {
                try
                {
                    Socket socket = ntInterface.CreateSocket();
                    // Bind the udp socket to a local network interface
                    bool bindingStatus = ntInterface.BindSocketWithNetworkInterface(socket, interfaceIP, 22359);

                    if (bindingStatus)
                    {
                        DiscoverDriveManager discoverDriveManager = new DiscoverDriveManager(); 

                        discoverDriveManager.DoDiscoverDrive(socket);

                      Thread.Sleep(1000);

                        foreach (var drive in discoverDriveManager.Drives)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                _drives.Add(drive);
                            });                           
                        }
                        ntInterface.CloseSocket(socket);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void DiscoveredDrives_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = DiscoveredDrives.SelectedItem;

            if (item == null)
            {
                DriveIp.Text = "10.29.80.50";
            }
            else
            {
                DriveIp.Text = item.ToString();
            }
        }
    }
}
