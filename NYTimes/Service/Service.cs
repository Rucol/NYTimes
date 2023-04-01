using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NYTimesInterfaces;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;



public class Service : INYTimesService
{
    private readonly IConfiguration _config;

    public Service()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        _config = builder.Build();
    }
    public async Task<string> GetArticleContentAsync(string url)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(responseBody);

                return json.response.content.docs[0].content[0].value;
            }
            else
            {
                throw new Exception($"Wystąpił błąd: {response.StatusCode}");
            }
        }
    }


    public async Task<List<Article>> SearchAsync(string searchTerm)
    {
        IConfigurationSection nytimesSection = _config.GetSection("NYTimes");
        var apiKey = nytimesSection["ApiKey"];

        List<Article> articles = new List<Article>();

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string url = $"https://api.nytimes.com/svc/search/v2/articlesearch.json?q={searchTerm}&api-key={apiKey}&sort=newest";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(responseBody);

                foreach (var article in json.response.docs)
                {
                    articles.Add(new Article
                    {
                        Headline = article.headline.main,
                        Snippet = article.snippet,
                        PublicationDate = article.pub_date
                    });
                }
            }
            else
            {
                throw new Exception($"Wystąpił błąd: {response.StatusCode}");
            }
        }

        return articles;
    }

    public class Article
    {
        public string Headline { get; set; }
        public string Snippet { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
