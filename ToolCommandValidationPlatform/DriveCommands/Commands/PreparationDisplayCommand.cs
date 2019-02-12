using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    /// <summary>
    /// SP
    /// </summary>
    public class PreparationDisplayCommand : ICommand
    {
        public string Name { get; } = "SP";

        public string MinVersion { get; } = "1.0";

        private DriveController driveController; 

         public PreparationDisplayCommand(DriveController controller)
        {
            driveController = controller;
        }

        private void OnDriveDataReceived(object sender, byte[] driveResponseBytes)
        {
            byte[] resultArray = new byte[8];

            Array.Copy(driveResponseBytes, 22, resultArray, 0, 8);

            Console.WriteLine(Encoding.ASCII.GetString(resultArray).Trim());                      
        }

        public new void Execute()
        {           
            byte[] command = { 81, 84 }; //Sp

            driveController.ExecuteCommand(Name);
        }        
    
    }
}
