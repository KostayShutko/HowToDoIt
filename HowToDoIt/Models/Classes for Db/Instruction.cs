using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Classes_for_Db
{
    public class Instruction
    {
        public Instruction()
        {
            Tags = new List<Tag>();
            Steps= new List<Step>();
            Ratings= new List<Rating>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }

        
        public virtual ICollection<Step> Steps { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<Tag> Tags { get; set;}
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}