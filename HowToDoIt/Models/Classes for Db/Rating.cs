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
        [ForeignKey("Instruction")]
        public string RatingId { get; set; }
        public string UserLogin { get; set; }
        public string Mark { get; set; }

        public virtual Instruction Instruction { get; set; }
    }
}