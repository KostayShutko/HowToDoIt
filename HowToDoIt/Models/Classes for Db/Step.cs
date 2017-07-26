using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Classes_for_Db
{
    public class Step
    {
        public Step()
        {
            Blocks= new List<Block>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public virtual ICollection<Block> Blocks { get; set; }

        public int InstructionId { get; set; }
        [ForeignKey("InstructionId")]
        public virtual Instruction Instruction { get; set; }
    }
}