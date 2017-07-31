using HowToDoIt.Models.Classes_for_Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Sort
{
    public class SortingByAlphabet: ISorting
    {
        public List<Instruction> Sorting(List<Instruction> instruction)
        {
            return instruction.OrderBy(x => x.Name).ToList();
        }


    }
}