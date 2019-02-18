using System;


namespace DriveCommands.Commands
{
    public class DriveFaultResetCommand :ICommand
    {
        public string Name { get; } = "SR"; //0101
        public void Execute()
        {
            throw new NotImplementedException();
        }

        private DriveController driveController;

        public DriveFaultResetCommand(DriveController controller)
        {
            driveController = controller;
        }
    }
}
