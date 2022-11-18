using EAPWPFDemo.EventAsynchronusPattern;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EAPWPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MutipleCallHtmlClient _clinet;

        public ObservableCollection<string> Pages { get; set; }
        public ObservableCollection<string> Results { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _clinet = new MutipleCallHtmlClient();
            Pages = new ObservableCollection<string>()
            {
                "http://www.wp.pl",
                "http://www.netflix.com",
                "http://www.youtube.com",
                "http://www.onet.pl",
                "https://www.learn.microsoft.com"
            };

            Results = new ObservableCollection<string>();

            DataContext = this;
            DownloadPages.ItemsSource = Pages;

            _clinet.Pages = Pages.Select((x)=> new Uri(x)).ToList();
            _clinet.DownloadHtmlCompleted += (sender, e) =>
            {
                DownloadButton.IsEnabled = true;
                CancelButton.IsEnabled = false;
                DownloadBar.Value = e.Cancelled ? 0 : 100;

                var result = e.AllData.Select(x => $"page: {x.Key}, length: {x.Value}").ToList();

                Results.Clear();
                foreach(var page in result)
                {
                    Results.Add(page);
                }
            };

            _clinet.DownloadHtmlProgress += (sender, e) =>
            {
                DownloadBar.Value = e.Progeress;
            };
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            Results.Clear();
            _clinet.Pages = Pages.Select(x => new Uri(x)).ToList();
            _clinet.DownloadHtmlAsync();

            DownloadButton.IsEnabled = false;
            CancelButton.IsEnabled = true;
            DownloadBar.Value = 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _clinet.CancelAsync();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var text = Url.Text;
            if (!text.Contains("http://"))
            {
                text = "http://" + text;
            }

            if (!text.Contains("http://") && !text.Contains("https://")) 
            {
                text = "https://" + text;
            }

            Pages.Add(text);
            var list = Pages.ToList();
            var uniqe = list.Distinct();

            Pages.Clear();
            foreach (var page in uniqe)
            {
                Pages.Add(page);
            }
        }
    }
}
