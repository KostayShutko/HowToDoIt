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
        [Key]
        public string TagId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Instruction> Instructions { get; set; }
    }
}