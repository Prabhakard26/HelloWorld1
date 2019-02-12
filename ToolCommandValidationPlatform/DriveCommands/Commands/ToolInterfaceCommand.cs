using System;
using System.Text;


namespace DriveCommands.Commands
{
    /// <summary>
    /// QT
    /// </summary>
    public class ToolInterfaceCommand : ICommand
    {
        private DriveController driveController;

        public string Name { get; } = "QT";

        public ToolInterfaceCommand(DriveController controller)
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
            byte[] command = { 81, 84 }; //QT

            driveController.ExecuteCommand(Name);
        }        
    
    }
}
