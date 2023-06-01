using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYTimes.Model
{
    public class Byline
    {
        public string original { get; set; }
        public List<object> person { get; set; }
        public string organization { get; set; }
    }
}
