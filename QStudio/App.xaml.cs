using System;
using System.Windows;

namespace QStudio {
    public partial class App
    {
        public App()
        {
            AppDomain.CurrentDomain.AppendPrivatePath(@"core");
            AppDomain.CurrentDomain.AppendPrivatePath(@"vendors");
            
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            string message = string.Empty;
            if (NetworkAdapter.AcesssToken.GetAccess("1", "1", out message))
            {
                if (e.Args.Length > 0 && !String.IsNullOrEmpty(e.Args[0]))
                    (new MainWindow(e.Args[0])).Show();
                else
                    (new MainWindow()).Show();
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к серверу лицензирования и/или проверить наличие обновлений!");
                App.Current.Shutdown();
            }
        }
    }
}
