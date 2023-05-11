using GalaSoft.MvvmLight.Command;
using NYTimesInterfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;


namespace NYTimes.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly INYTimesService _nyTimesService;
        private List<MyArticleViewModel> _articles;
        public List<MyArticleViewModel> Articles
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

        private string _parsingError;
        public string ParsingError
        {
            get => _parsingError;
            set
            {
                _parsingError = value;
                OnPropertyChanged(nameof(ParsingError));
            }
        }
        
        private string _selectedArticleContent;
        public string SelectedArticleContent
        {
            get => _selectedArticleContent;
            set
            {
                _selectedArticleContent = value;
                OnPropertyChanged(nameof(SelectedArticleContent));
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(async () => await SearchAsync(), () => true);
                }
                return _searchCommand;
            }
        }

        public MainViewModel(INYTimesService nyTimesService)
        {
            _nyTimesService = nyTimesService;
            Articles = new List<MyArticleViewModel>();
        }

        public async Task SearchAsync()
        {
            Articles.Clear();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                List<MyArticle> articles = await _nyTimesService.SearchAsync(SearchTerm);

                if (articles.Count > 0)
                {
                    foreach (MyArticle article in articles)
                    {
                        Articles.Add(new MyArticleViewModel(article));
                    }
                }
                else
                {
                    ParsingError = "No articles found for the specified search term.";
                }
            }
            else
            {
                ParsingError = "Please enter a search term.";
            }
        }

        public async Task GetArticleContentAsync(MyArticleViewModel articleViewModel)
        {
            SelectedArticleContent = await _nyTimesService.GetArticleContentAsync(articleViewModel.Url);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MyArticleViewModel
    {
        public string Headline { get; }
        public string Snippet { get; }
        public string Url { get; }

        public MyArticleViewModel(MyArticle article)
        {
            Headline = article.Headline;
            Snippet = article.Snippet;
            Url = article.Url;
        }
    }
}
