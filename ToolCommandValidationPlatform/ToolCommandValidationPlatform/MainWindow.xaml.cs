using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DriveCommands;

namespace ToolCommandValidationPlatform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DriveController driveController;

        IUdpSocket udpSocket = new UdpSocket(new IPEndPoint(IPAddress.Parse("10.29.80.126"), 22359), false );

       

        public MainWindow()
        {
            InitializeComponent();

            LstCommands.Items.Add("QT");
            LstCommands.Items.Add("Q1");
            LstCommands.Items.Add("QC");
            LstCommands.Items.Add("J");
            LstCommands.Items.Add("SP");

            udpSocket.OnDriveDataReceived += UdpSocket_OnDriveDataReceived;
            driveController = new DriveController(udpSocket);

            foreach (Type t in this.GetType().Assembly.GetTypes())
            {
                if (t is ICommand)
                {

                }
            }
        }

        private void UdpSocket_OnDriveDataReceived(object sender, byte[] e)
        {
            string result = Encoding.ASCII.GetString(e).Trim();

            this.Dispatcher.Invoke(() =>
            {
               TxtStatus.Text = result;
            });
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            driveController.ExecuteCommand("QT");
        }
    }
}
