using NYTimes.Service;
using NYTimesInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Markup;

namespace NYTimes
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly INYTimesService _nyTimesService;
        private List<NYTimesInterfaces.MyArticle> _articles;
        public List<NYTimesInterfaces.MyArticle> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
                OnPropertyChanged(nameof(Articles));
            }
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
            }
        }
        // Jak zmienić aby domyślnie wczytywało ten konstruktor a nie domyślny
        public MainWindow(INYTimesService nyTimesService, HttpClient httpClient)
        {
            InitializeComponent();
            DataContext = this;
            _nyTimesService = nyTimesService;
            _nyTimesService.SetHttpClient(httpClient);
            Articles = new List<NYTimesInterfaces.MyArticle>();
        }


        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ListBoxArticles.ItemsSource = Articles;
            ParsingErrorTextBox.Text = "";

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Articles = await _nyTimesService.SearchAsync(SearchTerm);

                if (Articles.Count > 0)
                {
                    MessageBox.Show("Udało się!");
                }

                ListBoxArticles.Items.Refresh();
            }
        }

        private async void ListBoxArticles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            NYTimesInterfaces.MyArticle selectedArticle = ListBoxArticles.SelectedItem as NYTimesInterfaces.MyArticle;

            if (selectedArticle != null)
            {
                selectedArticle.Content = await _nyTimesService.GetArticleContentAsync(selectedArticle.Url);

                MessageBox.Show(selectedArticle.Content);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class MyArticle : NYTimesInterfaces.MyArticle
        {
            public string Content { get; set; }

            public MyArticle(string headline, string snippet, string url) : base(headline, snippet, url)
            {
            }
        }
    }
}