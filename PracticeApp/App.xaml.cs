using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PracticeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            NamedImageModel imageModel = new NamedImageModel();
            NamedImageVM vm = new NamedImageVM(imageModel);

            var win = new MainWindow{
                DataContext = vm
            };

            win.Show();
        }
    }
}
