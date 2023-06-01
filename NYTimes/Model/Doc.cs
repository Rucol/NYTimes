using NYTimes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYTimes.Model
{
    public class Doc : Byline
    {
        public string Abstract { get; set; }
        public string WebUrl { get; set; }
        public string Snippet { get; set; }
        public string LeadParagraph { get; set; }
        public List<Multimedium> Multimedia { get; set; }
        public string PrintPage { get; set; }
        public string Source { get; set; }
        public List<Keyword> Keywords { get; set; }
        public string PubDate { get; set; }
        public string DocumentType { get; set; }
        public string NewsDesk { get; set; }
        public string SectionName { get; set; }
        public string SubsectionName { get; set; }
        public Byline Byline { get; set; }
        public string TypeOfMaterial { get; set; }
        public string Id { get; set; }
        public int WordCount { get; set; }
        public List<RelatedArticle> RelatedArticles { get; set; }
        public string Uri { get; set; }
    }
}
