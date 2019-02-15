using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class SoftwareVersionCommand : ICommand
    {
        public string Name { get; } = "Q2";//
        public void Execute()
        {
            throw new NotImplementedException();
        }
        private DriveController driveController;

        public SoftwareVersionCommand(DriveController controller)
        {
            driveController = controller;
        }
    }
}
