using System;
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

        private void textBoxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !TextBoxTextAllowed(e.Text);
        }

        private void textBoxValue_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String Text1 = (String)e.DataObject.GetData(typeof(String));
                if (!TextBoxTextAllowed(Text1)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool TextBoxTextAllowed(string Text2)
        {
            return Array.TrueForAll<Char>(Text2.ToCharArray(),
                delegate (Char c) { return Char.IsDigit(c) || Char.IsControl(c); });
        }

    }
}
