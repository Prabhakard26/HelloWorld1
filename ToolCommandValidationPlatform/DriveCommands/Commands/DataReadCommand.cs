using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class DataReadCommand : ICommand
    {
        public string Name { get; } = "R";

        public void Execute()
        {
            throw new NotImplementedException();
        }

        private DriveController driveController;

        public DataReadCommand(DriveController controller)
        {
            driveController = controller;
        }
    }
}
