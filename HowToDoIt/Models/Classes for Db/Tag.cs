using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Classes_for_Db
{
    public class Tag
    {
        public Tag()
        {
            Instructions = new List<Instruction>();
        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Instruction> Instructions { get; set; }
    }
}