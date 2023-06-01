using NYTimes.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace NYTimes
{ 
    public partial class MainWindow : Window
    {
        private NYVM viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModel.NYVM();
            DataContext = viewModel;
        }
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            await viewModel.Szukaj(SearchTerm);
            Tabelka.ItemsSource = viewModel.Articles;
        }
        private void SearchTerm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_Click(sender, e);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var textBlock in FindVisualChildren<TextBlock>(this))
            {
                if (textBlock.Tag is string url)
                {
                    textBlock.PreviewMouseLeftButtonUp += Tabelka_PreviewMouseLeftButtonUp;
                    textBlock.Cursor = Cursors.Hand;
                    textBlock.TextDecorations = TextDecorations.Underline;
                    textBlock.Foreground = Brushes.Blue;
                }
            }
        }
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }   
        private void Tabelka_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.Tag is string url)
            {
                try
                {
                    Process.Start(url);
                }
                catch (Exception ex)
                {
                    // Obsłuż błąd otwarcia przeglądarki lub nieprawidłowego adresu URL
                    MessageBox.Show("Wystąpił błąd podczas otwierania linku: " + ex.Message);
                }
            }
        }

    }
}
