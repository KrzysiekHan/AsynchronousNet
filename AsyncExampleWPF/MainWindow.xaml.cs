using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Http;
using System.Net;
using System.IO;

namespace AsyncExampleWPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            resultsTextBox.Clear();
            SumPageSizes();
            resultsTextBox.Text += "\r\nControl returned to startButton_Click.";
        }

        private async void asyncButtonClick(object sender, RoutedEventArgs e)
        {
            asyncButton.IsEnabled = false;
            resultsTextBox.Clear();
            await SumPageSizesAsync();
            resultsTextBox.Text += "\r\nControl returned to startButton_Click.";
            asyncButton.IsEnabled = true;

        }

        private async Task SumPageSizesAsync()
        {
            List<string> urlList = SetUpURLList();
            var total = 0;
            foreach (var url in urlList)
            {
                byte[] urlContents = await GetURLContentsAsync(url);
                DisplayResults(url, urlContents);
                total += urlContents.Length;
            }
            resultsTextBox.Text += $"\r\n\r\nTotal bytes returned:  {total}\r\n";
        }

        private async Task<byte[]> GetURLContentsAsync(string url)
        {
            var content = new MemoryStream();
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            using (WebResponse response = await webReq.GetResponseAsync())
            {
                using(Stream responseStream = response.GetResponseStream())
                {
                    await responseStream.CopyToAsync(content);
                }
            }
            return content.ToArray();
        }

        private void SumPageSizes()
        {
            List<string> urlList = SetUpURLList();
            var total = 0;
            foreach (var url in urlList)
            {
                byte[] urlContents = GetURLContents(url);
                DisplayResults(url, urlContents);
                total += urlContents.Length;
            }
            resultsTextBox.Text += $"\r\n\r\nTotal bytes returned: {total} \r\n";
        }

        private List<string> SetUpURLList()
        {
            var urls = new List<string>
            {
                "https://msdn.microsoft.com/library/windows/apps/br211380.aspx",
                "https://msdn.microsoft.com",
                "https://msdn.microsoft.com/library/hh290136.aspx",
                "https://msdn.microsoft.com/library/ee256749.aspx",
                "https://msdn.microsoft.com/library/hh290138.aspx",
                "https://msdn.microsoft.com/library/hh290140.aspx",
                "https://msdn.microsoft.com/library/dd470362.aspx",
                "https://msdn.microsoft.com/library/aa578028.aspx",
                "https://msdn.microsoft.com/library/ms404677.aspx",
                "https://msdn.microsoft.com/library/ff730837.aspx"
            };
            return urls;
        }

        private byte[] GetURLContents(string url)
        {
            var content = new MemoryStream();
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            using (WebResponse response = webReq.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    responseStream.CopyTo(content);
                }
                return content.ToArray();
            }
        }

        private void DisplayResults(string url, byte[] content)
        {
            var bytes = content.Length;
            var displayURL = url.Replace("https://", "");
            resultsTextBox.Text += $"\n{displayURL,-58} {bytes,8}";
        }

    }
}
