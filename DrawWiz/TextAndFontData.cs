using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace DrawWiz
{
     [Serializable]
    public class TextAndFontData : ISerializable, IDataErrorInfo
    {
         public TextAndFontData()
         {}

         public TextAndFontData(SerializationInfo info, StreamingContext context)
         {
             _text = (string)info.GetValue("text", typeof(string));

             string strFamily = (string)info.GetValue("family", typeof(string));
             _family = new FontFamily(strFamily);

             string strStyle = (string)info.GetValue("style", typeof(string));
             SetStyle(strStyle);

             string strWeight = (string)info.GetValue("weight", typeof(string));
             SetFontWeight(strWeight);

             _fontSize = (double)info.GetValue("size", typeof(double));
         }

         public void GetObjectData(SerializationInfo info, StreamingContext context)
         {
             info.AddValue("text", _text, typeof(string));

             info.AddValue("family", _family.ToString(), typeof(string));

             info.AddValue("style", _fontStyle.ToString(), typeof(string));

             info.AddValue("weight", _fontWeight.ToString(), typeof(string));

             info.AddValue("size", _fontSize, typeof(double));
         }
    
         // Data

         private string _text = "";
         public string Text
         {
             get { return _text; }
             set { _text = value; }
         }
         
         private FontFamily _family = new FontFamily("Times New Roman");            
         public FontFamily Family
         {
             get { return _family; }
             set { _family = value;  }
         }

         private FontStyle _fontStyle = FontStyles.Normal;  
         public FontStyle Style
         {
             get{ return _fontStyle; }
             set{ _fontStyle = value; }
         }
             
         protected void SetStyle(string strStyle)
         {
             if (strStyle == "Italic")
                 _fontStyle = FontStyles.Italic;
             else if (strStyle == "Oblique")
                 _fontStyle = FontStyles.Oblique;
             else
                 _fontStyle = FontStyles.Normal;
         }

         private FontWeight _fontWeight = FontWeights.Normal; 
         public FontWeight Weight
         {
             get { return _fontWeight; }
             set { _fontWeight = value; }
         }

         protected void SetFontWeight(string strWeight)
         {
             switch (strWeight)
             {
                 case "Thin":
                     _fontWeight = FontWeights.Thin;
                     break;
                 case "ExtraLight":
                     _fontWeight = FontWeights.ExtraLight;
                     break;
                 case "Light":
                     _fontWeight = FontWeights.Light;
                      break;
                 case "Medium":
                     _fontWeight = FontWeights.Medium;
                     break;
                 case "SemiBold":
                     _fontWeight = FontWeights.SemiBold;
                     break;
                 case "Bold":
                     _fontWeight = FontWeights.Bold;
                     break;
                 case "ExtraBold":
                     _fontWeight = FontWeights.ExtraBold;
                     break;
                 case "Black":
                     _fontWeight = FontWeights.Black;
                     break;
                 case "ExtraBlack":
                     _fontWeight = FontWeights.ExtraBlack;
                     break;
                  default:
                     _fontWeight = FontWeights.Normal;
                     break;
             }
         }

         private double _fontSize = 12.0;
         public double Size
         {
             get { return _fontSize; }
             set 
             { 
                 if(value > 0.0 && value <= 1000.0 )
                    _fontSize = value; 
             }
         }

        #region IDataErrorInfo Members

         public string Error
         {
             get { return null; }
         }

         public string this[string propertyName]
         {
             get
             {
                 if (propertyName == "Size")
                 {
                     if (this._fontSize < 2)
                     {
                         return "Font size must be greater than or equal 2";
                     }
                 }

                 return null;
             }
         }

        #endregion

    }
}
