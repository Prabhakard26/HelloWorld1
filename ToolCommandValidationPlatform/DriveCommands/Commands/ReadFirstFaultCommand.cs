using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class ReadFirstFaultCommand : ICommand
    {
        public string Name { get; } = "SF"; //3-0
        public void Execute()
        {
            throw new NotImplementedException();
        }

        private DriveController driveController;

        public ReadFirstFaultCommand(DriveController controller)
        {
            driveController = controller;
        }
    }
}
