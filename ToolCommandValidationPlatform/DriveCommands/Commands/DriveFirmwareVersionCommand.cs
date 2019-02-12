using System;
using System.Text;
using Microsoft.SqlServer.Server;


namespace DriveCommands.Commands
{
    /// <summary>
    /// QC
    /// </summary>
    public class DriveFirmwareVersionCommand : ICommand
    {
        public string Name { get; } = "QC";

        public string MinVersion { get; } = "1.0";

        private DriveController driveController;

         public DriveFirmwareVersionCommand(DriveController controller)
         {
             driveController = controller;
         }

        private void OnDriveDataReceived(object sender, byte[] driveResponseBytes)
        {
            byte[] resultArray = new byte[8];

            Array.Copy(driveResponseBytes, 22, resultArray, 0, 8);

            Console.WriteLine(Encoding.ASCII.GetString(resultArray).Trim());                      
        }

        public  void Execute()
        {           
            byte[] command = { 81, 67 }; //QC

            driveController.ExecuteCommand(Name);
        }        
    }
}
