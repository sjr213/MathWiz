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

namespace MathWiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
