using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Word_Wiz
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            WordWizModel wizModel = new WordWizModel();
            WordWizVM vm = new WordWizVM(wizModel);

            var win = new MainWindow
            {
                DataContext = vm
            };

            win.Show();
        }
    }
}
