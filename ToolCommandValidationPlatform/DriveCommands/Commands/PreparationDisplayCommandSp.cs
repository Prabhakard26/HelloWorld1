using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class PreparationDisplayCommandSp : ICommand
    {
        
        public string Name { get; } = "Sp";

        public void Execute()
        {
            throw new NotImplementedException();
        }

        private DriveController driveController;
        public PreparationDisplayCommandSp(DriveController controller)
        {
            driveController = controller;
        }
    }
}
