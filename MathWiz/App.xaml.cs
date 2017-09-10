using System.Windows;

namespace MathWiz
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MathWizModel wizModel = new MathWizModel();
            MathWizVM vm = new MathWizVM(wizModel);

            var win = new MathWizWindow
            {
                DataContext = vm
            };

            win.Show();
        }
    }
}
