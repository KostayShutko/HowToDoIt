using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Classes_for_Db
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public int InstructionId { get; set; }
        [JsonIgnore]
        [ForeignKey("InstructionId")]
        public virtual Instruction Instruction { get; set; }

        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}