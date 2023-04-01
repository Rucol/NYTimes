using NYTimesInterfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using NYTimes;

namespace NYTimes
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly INYTimesService _nyTimesService;
        private List<Article> _articles;
        public List<Article> Articles
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

        public MainWindow(INYTimesService nyTimesService)
        {
            InitializeComponent();
            DataContext = this;
            _nyTimesService = nyTimesService;
            Articles = new List<Article>();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ListBoxArticles.ItemsSource = Articles;
            ParsingErrorTextBox.Text = "";

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Articles = await _nyTimesService.GetArticleContentAsync(SearchTerm);

                if (Articles.Count > 0)
                {
                    MessageBox.Show("Udalo sie!");
                }

                ListBoxArticles.Items.Refresh();
            }
        }

        private async void ListBoxArticles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Article selectedArticle = ListBoxArticles.SelectedItem as Article;

            if (selectedArticle != null)
            {
                selectedArticle.Content = await _nyTimesService.GetArticleContentAsync(selectedArticle.Content);

                MessageBox.Show(selectedArticle.Content);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class Article
        {
            public string Headline { get; set; }
            public string Snippet { get; set; }
            public string Content { get; set; }
            public string PublicationDate { get; set; }

            public Article(NYTimesService.Article article)
            {
                Headline = article.Headline;
                Snippet = article.Snippet;
                Content = article.Content;
                PublicationDate = article.PublicationDate;
            }
        }
    }
}
