using NYTimes.Service;
using NYTimes;
using NYTimesInterfaces;
using System.Windows;
using System.Net.Http;
using System.Windows.Markup;
using System;
using System.Net.Http.Headers;

namespace NYTimes
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var httpClient = new HttpClient();
            var nyTimesService = new NYTimesService(httpClient);
            var mainWindow = new MainWindow(nyTimesService, httpClient);
            Application.Current.MainWindow = mainWindow;

            mainWindow.Show();
        }
       
    }
    public class NYTimesServiceFactory : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.nytimes.com/svc/search/v2/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string apiKey = "TlGVAG0Xbiacm9eP6VXUdUPZ8W51AJM2";
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("API Key is missing. Please provide an API Key.");
            }

            NYTimesService nyTimesService = new NYTimesService(httpClient);

            return nyTimesService;
        }
    }
}