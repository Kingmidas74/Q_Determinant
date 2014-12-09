using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows;

namespace Updater
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public int Percent { get; set; }

        private const string ServerUri = "qdet.kingmidas.ru";

        private bool ServerEnabled()
        {
            try
            {
                PingReply pingReply;
                using (var ping = new Ping())
                {
                    pingReply = ping.Send(ServerUri);
                }
                if (pingReply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        private void CheckUpdates(object sender, RoutedEventArgs e)
        {
            if (ServerEnabled())
            {
                MessageBox.Show(@"http://" + ServerUri + @"/files");
                var serverDirectory = new DirectoryInfo(@"http://" + ServerUri + @"/files");
            }
        }
        
    }
}
