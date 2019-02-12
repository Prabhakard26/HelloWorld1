using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class DriveTypeCommand : ICommand
    {
        public string Name { get; } = "Q1";
        //q2 software vers
        //q3 job number
        //q4 item num
        //qa panel name

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
