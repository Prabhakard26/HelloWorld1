using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DriveCommands;

namespace ToolCommandValidationPlatform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DriveController driveController;

        IUdpSocket udpSocket;
        
        public MainWindow()
        {
            InitializeComponent();

            udpSocket = new UdpSocket(new IPEndPoint(IPAddress.Parse("10.29.80.150"), 22359), false);
            udpSocket.OnDriveDataReceived += UdpSocket_OnDriveDataReceived;

            driveController = new DriveController(udpSocket, Count);

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
            DataContext = LstCommands;
        }

        private void UdpSocket_OnDriveDataReceived(object sender, byte[] e)
        {
            //string result = Encoding.ASCII.GetString(e).Trim();

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

        private int Count = 2;
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            int selectedIndex = cmb.SelectedIndex;
            Count = selectedIndex + 1;
        }
    }
}
