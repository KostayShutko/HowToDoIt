using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Classes_for_Db
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public int Mark { get; set; }

        public int InstructionId { get; set; }
        [ForeignKey("InstructionId")]
        public virtual Instruction Instruction { get; set; }
    }
}