using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Classes_for_Db
{
    public class Instruction
    {
        public string InstructionId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } 
        public virtual Category Category { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}