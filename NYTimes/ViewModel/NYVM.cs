using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Newtonsoft.Json;
using NYTimes.Model;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace NYTimes.ViewModel
{
    public class NYVM : INotifyPropertyChanged
    {
        private List<NYTimes.Model.Article> articles;

        public List<Article> Articles
        {
            get { return articles; }
            set
            {
                articles = value;
                OnPropertyChanged("Articles");
            }
        }
        public NYVM()
        {
            articles = new List<NYTimes.Model.Article>();
        }
        public async Task Szukaj(TextBox Wpisywanie)
        {
            var client = new HttpClient();
            string query = Wpisywanie.Text.Substring(0, Math.Min(Wpisywanie.Text.Length, 50));
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.nytimes.com/svc/search/v2/articlesearch.json?q={query}&api-key=TlGVAG0Xbiacm9eP6VXUdUPZ8W51AJM2"),
                Headers =
                {
                    { "X-RapidAPI-Key", "82e608ca0amshfb8be84d382c693p152ebejsn0ed3121d87c8" },
                    { "X-RapidAPI-Host", "ny-times-times-wire.p.rapidapi.com" },
                },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                File.WriteAllText(@"C:\Users\x390\Desktop\Moje projekty\NYTimes nowe\PROJEKT PJOTRA\nowy.txt", body);
                Root result = JsonConvert.DeserializeObject<Root>(body);
                if (result != null && result.response != null && result.response.Docs != null)
                {
                    Articles = result.response.Docs.Select(doc => new NYTimes.Model.Article
                    {
                        article = doc.Snippet,
                        Author = doc.Byline?.original ?? "",
                        Source = doc.Source,  
                        Url = doc.Uri,
                    }).ToList();
                }

            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
