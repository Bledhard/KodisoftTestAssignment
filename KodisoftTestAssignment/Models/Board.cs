using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public class Board
    {
        public string Name { get; set; }

        public List<Article> Articles { get; set; }
    }
}
