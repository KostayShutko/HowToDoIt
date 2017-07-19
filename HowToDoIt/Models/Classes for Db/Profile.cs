using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HowToDoIt.Models.Classes_for_Db
{
    public class Profile
    {
        [Key]
        [ForeignKey("Users")]
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string FirtstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Contacts { get; set; }
        public string Interests { get; set; }

        public virtual ApplicationUser Users { get; set; }


    }
}