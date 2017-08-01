using HowToDoIt.Models.Classes_for_Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Sort
{
    public class SortingByRating:ISorting
    {
        public List<Instruction> Sorting(List<Instruction> instruction)
        {
            var i = from p in instruction let rating = (from r in p.Ratings where r.InstructionId == p.Id select r.Mark).Sum() orderby (rating) descending select p;
            return i.ToList();
        }
    }
}