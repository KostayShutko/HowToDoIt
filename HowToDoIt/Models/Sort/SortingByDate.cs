using HowToDoIt.Models.Classes_for_Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Sort
{
    public class SortingByDate:ISorting
    {
        public List<Instruction> Sorting(List<Instruction> instruction)
        {
            var i= instruction.OrderBy(x => x.Date).ToList();
            i.Reverse();
            return i;
        }
    }
}