using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MyMvvmLib;

namespace DrawWiz
{
    public class FontVM : ViewModelBase, IDataErrorInfo
    {
        private TextAndFontData _textFontData = new TextAndFontData();
        public TextAndFontData TextFontData
        {
            get { return _textFontData; }
        }

        private string _fontSizeText = "12";
        public string Size
        {
            get { return _fontSizeText; }
            set
            {
                SetProperty(ref _fontSizeText, value, () => Size);
            }
        }

        private string _text = "";
        public string Text
        {
            get { return _text; }
            set
            {
                SetProperty(ref _text, value, () => Text);
            }
        }

        private string _family = "";
        public string Family
        {
            get { return _family; }
            set
            {
                SetProperty(ref _family, value, () => Family);
            }
        }

        private string _style = "";
        public string Style
        {
            get { return _style; }
            set
            {
                SetProperty(ref _style, value, () => Style);
            }
        }

        private FontWeight _weight = FontWeights.Normal;
        public FontWeight Weight
        {
            get { return _weight; }
            set
            {
                SetProperty(ref _weight, value, () => Weight);
            }
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { return _textFontData.Error; }
        }

        public string this[string propertyName]
        {
            get
            {
                if (propertyName == "Size")
                {
                    double textSize;
                    string msg = this.ValidateFontTextSize(out textSize);
                    if(! string.IsNullOrEmpty(msg))
                        return msg;

                    _textFontData.Size = textSize;
                }

                if (propertyName == "Text")
                {
                    if (String.IsNullOrEmpty(_text))
                        return "Text is blank";

                    _textFontData.Text = _text;
                }

                if (propertyName == "Family")
                {
                    if (String.IsNullOrEmpty(_family))
                        return "Family is blank";

                    _textFontData.Family = new FontFamily(_family);
                }

                if (propertyName == "Style")
                {
                    if (String.IsNullOrEmpty(_style))
                        return "Style is blank";

                    if (_style != "Italic" && _style != "Oblique" && _style != "Normal")
                    {
                        return "Style must be Italic, Oblique or Normal";
                    }

                    if (_style == "Italic")
                        _textFontData.Style = FontStyles.Italic;
                    else if (_style == "Oblique")
                        _textFontData.Style = FontStyles.Oblique;
                    else
                        _textFontData.Style = FontStyles.Normal;
                }

                if (propertyName == "Weight")
                {
                    if (_weight == null)
                        return "Weight is empty";

                    _textFontData.Weight = _weight;
                }

                return _textFontData[propertyName];
            }

        }

        string ValidateFontTextSize(out double size)
        {
            size = 0.0;
            string msg = null;

            if (String.IsNullOrEmpty(_fontSizeText))
                msg = "Font size is missing";

            if (!double.TryParse(_fontSizeText, out size))
                msg = "Font size is not a number";

            return msg;
        }

        #endregion
    }
}
