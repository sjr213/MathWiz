using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace DrawWiz
{
    /// <summary>
    /// Interaction logic for TextDialog.xaml
    /// </summary>
    public partial class TextDialog : Window
    {
        private FontVM _fontVM = new FontVM();

        public TextDialog()
        {
            InitializeComponent();

            FillFontFamily();

            FillFontStyle();

            FillFontWeight();

            DataContext = _fontVM;
        }

        private void FillFontFamily()
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                // FontFamily.Source contains the font family name.
                _family.Items.Add(fontFamily.Source);
            }

            _family.SelectedIndex = 0;
            _fontVM.Family = _family.SelectedItem.ToString();
        }

        private void FillFontStyle()
        {
            _styles.Items.Add(FontStyles.Normal);
            _styles.Items.Add(FontStyles.Italic);
            _styles.Items.Add(FontStyles.Oblique);

            _styles.SelectedIndex = 0;
            _fontVM.Style = _styles.SelectedItem.ToString();
        }

        private void FillFontWeight()
        {
            _weight.Items.Add(FontWeights.Normal);
            _weight.Items.Add(FontWeights.Thin);
            _weight.Items.Add(FontWeights.ExtraLight);
            _weight.Items.Add(FontWeights.Light);
            _weight.Items.Add(FontWeights.Medium);
            _weight.Items.Add(FontWeights.SemiBold);
            _weight.Items.Add(FontWeights.Bold);
            _weight.Items.Add(FontWeights.ExtraBold);
            _weight.Items.Add(FontWeights.Black);
            _weight.Items.Add(FontWeights.ExtraBlack);

            _weight.SelectedIndex = 0;
        }

        public TextAndFontData TextFontData
        {
            get { return _fontVM.TextFontData; }
        }

        private void OnOk(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }


    }
}
