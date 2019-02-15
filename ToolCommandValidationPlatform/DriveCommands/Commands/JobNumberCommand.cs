using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class JobNumberCommand : ICommand
    {
        public string Name { get; } = "Q3";//
        public void Execute()
        {
            throw new NotImplementedException();
        }
        private DriveController driveController;

        public JobNumberCommand(DriveController controller)
        {
            driveController = controller;
        }
    }
}
