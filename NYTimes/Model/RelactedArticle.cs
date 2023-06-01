using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYTimes.Model
{
    public class RelatedArticle
    {
        public string Relation { get; set; }
        public string Url { get; set; }
        public string Anchor { get; set; }
    }
}