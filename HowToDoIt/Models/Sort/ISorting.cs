using HowToDoIt.Models.Classes_for_Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowToDoIt.Models.Sort
{
    interface ISorting
    {
        List<Instruction> Sorting(List<Instruction> instruction);
    }
}
