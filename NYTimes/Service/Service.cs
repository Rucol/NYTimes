using NYTimesInterfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;  
using System.Text.Json;
using System.Threading.Tasks;

namespace NYTimes.Service
{
    public class NYTimesService : INYTimesService
    {
        public HttpClient _httpClient;

        public NYTimesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MyArticle>> SearchAsync(string query)
        {
            var uri = new Uri($"https://api.nytimes.com/svc/search/v2/articlesearch.json?q={query}&api-key=yourkey");
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var articles = JsonSerializer.Deserialize<NYTimesApiResponse>(responseContent)?.Response?.Docs;

                if (articles != null)
                {
                    var myArticles = new List<MyArticle>();

                    foreach (var article in articles)
                    {
                        myArticles.Add(new MyArticle(article.Headline.Main ?? "", article.Abstract ?? "", article.WebUrl ?? ""));
                    }

                    return myArticles;
                }
            }

            return new List<MyArticle>();
        }

        public async Task<string> GetArticleContentAsync(string articleUrl)
        {
            var response = await _httpClient.GetAsync(articleUrl);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            return string.Empty;
        }

        public void SetHttpClient(HttpClient httpClient)
        {
            if (_httpClient == null)
            {
                _httpClient = httpClient;
            }
        }

    }
}
