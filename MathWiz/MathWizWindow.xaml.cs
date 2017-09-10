using System.Windows;
using System.Windows.Input;

namespace MathWiz
{
    /// <summary>
    /// Interaction logic for DrawWizWindow.xaml
    /// </summary>
    public partial class MathWizWindow : Window
    {
        public MathWizWindow()
        {
            InitializeComponent();
        }

        private void OnResultKeyUp(object sender, KeyEventArgs e)
        {
            MathWizVM vm = (MathWizVM)this.DataContext;
            if (vm == null)
                return;

            string result = resultTextbox.Text;
            if (vm.ResultTypedCommand.CanExecute(null))
            {
                ResultParams resultParams = new ResultParams();

                // Seconds is the seconds delay before determining if the result is correct
                resultParams.Seconds = (e.Key == Key.Return) ? 0 : 3;
                resultParams.StrResult = result;

                vm.ResultTypedCommand.Execute(resultParams);
            }
        }

    }
}
