using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class PanelNameCommand : ICommand
    {
        public string Name { get; } = "QA1";
        public void Execute()
        {
            throw new NotImplementedException();
        }

        private DriveController driveController;

        public PanelNameCommand(DriveController controller)
        {
            driveController = controller;
        }
    }
}
