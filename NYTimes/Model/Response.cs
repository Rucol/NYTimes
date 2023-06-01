using NYTimes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYTimes.Model
{
    public class Response
    {
        public List<Doc> Docs { get; set; }
        public Meta meta { get; set; }
    }
}
