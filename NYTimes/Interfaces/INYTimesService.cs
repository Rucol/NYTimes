using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYTimes;
using static Service;

namespace NYTimesInterfaces
{
    public interface INYTimesService
    {
        Task<List<Article>> SearchAsync(string searchTerm);
        Task<string> GetArticleContentAsync(string articleUrl);
    }
}
