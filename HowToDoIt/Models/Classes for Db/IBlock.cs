using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowToDoIt.Models.Classes_for_Db
{
    public interface IBlock
    {
        string Type { get; set; }
        string Name { get; set; }
    }
}
