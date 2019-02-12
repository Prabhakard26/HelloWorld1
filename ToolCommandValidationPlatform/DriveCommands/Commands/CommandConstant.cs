using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveCommands.Commands
{
    public class CommandConstants
    {
        //Control Command - 2 bytes - 83,78 (SN : PC - Drive)
        //Result Bytes- 2 bytes, 48, 48 (00 : PC - Drive )
        //Slot 2 byte - 00 BUS, UNIT, SLOT  00
        //Station  2 byte - 00
        //Length of Data - 2 byte 12 , 0, TS-S20 header + Message 2 
        // Header 10 bytes
        public static byte[] HEADER = { 83, 78, 48, 48, 0, 0, 0, 0, 12, 0, 8, 0, 64, 1, 0, 118, 0, 0, 0, 0 };
    }
}
