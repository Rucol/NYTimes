using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYTimes.Model
{
    public class Person
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string Middle { get; set; }
        public string Qualifier { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string Organization { get; set; }
        public int Rank { get; set; }
    }
}
