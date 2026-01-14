using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    internal enum CommandWord
    {
        GO,
        HELP,
        UNKNOWN,
        LOOK,
        TAKE,
        INVENTORY,
        BACK,
        DROP,
        USE,
        ATTACK,
        QUIT
    }
}
