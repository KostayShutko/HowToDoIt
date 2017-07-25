using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Classes_for_Db
{
    public class Text:IBlock
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public int StepId { get; set; }
        [ForeignKey("StepId")]
        public virtual Step Step { get; set; }
    }
}