using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class ItemNumberCommand : ICommand
    {
        public string Name { get; } = "Q4";
        public void Execute()
        {
            throw new NotImplementedException();
        }

        private DriveController driveController;

        public ItemNumberCommand(DriveController controller)
        {
            driveController = controller;
        }
    }
}
