using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class DataWriteCommand: ICommand
    {
        public string Name { get; } = "W";
        public void Execute()
        {
            throw new NotImplementedException();
        }

        private DriveController driveController;

        public DataWriteCommand(DriveController controller)
        {
            driveController = controller;
        }
    }
}
