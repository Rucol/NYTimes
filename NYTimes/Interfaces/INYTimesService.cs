using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NYTimes;
using NYTimes.Service;

namespace NYTimesInterfaces
{
    public interface INYTimesService
    {
        Task<List<MyArticle>> SearchAsync(string searchTerm);
        Task<string> GetArticleContentAsync(string articleUrl);
        void SetHttpClient(HttpClient httpClient);
    }

    public class MyArticle
    {
        public MyArticle(string headline, string snippet, string url)
        {
            Headline = headline;
            Snippet = snippet;
            Url = url;
        }

        public string Headline { get; set; }
        public string Snippet { get; set; }
        public string Url { get; set; }
        public string PublicationDate { get; set; }
        public string Content { get; internal set; }
    }

}