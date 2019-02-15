using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class RealTimeReadCommand : ICommand
    {
        public string Name { get; } = "ZS";
        public void Execute()
        {
            throw new NotImplementedException();
        }

        private DriveController driveController;

        public RealTimeReadCommand(DriveController controller)
        {
            driveController = controller;
        }
    }
}
