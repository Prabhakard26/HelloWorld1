using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class DriveFaultResetCommand :ICommand
    {
        public string Name { get; } = "SR"; //0101
        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
