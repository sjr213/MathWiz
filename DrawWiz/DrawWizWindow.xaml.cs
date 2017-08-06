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

namespace DrawWiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point firstLeftPt = new Point(0,0);
       
        private UIElement _selectedElement;
        private UIElement _tempShape;
        ResizingAdorner _adorner;
        Point _current;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (! (sender is Canvas))
                return;

            Canvas cv = (Canvas)sender;
            if (cv == null || cv.Name != "_mainCanvas")
                return;

            var vM = DataContext as DrawWizVM;
            if (vM == null)
                return;

            if (_tempShape != null)
                cv.Children.Remove(_tempShape);

            ClearAdornerLayer(false);

            if (e.ChangedButton == MouseButton.Left)
            {
                var layer = AdornerLayer.GetAdornerLayer(cv);
                layer.IsHitTestVisible = true;
                firstLeftPt = e.GetPosition(cv);

                // If the shape type is unknown we are trying to select and 
                // not draw a new shape
                if (vM.TheShape == DrawWizModel.ShapeType.unknownType)
                {
                    _selectedElement = e.OriginalSource as UIElement;

                    if (_selectedElement is Canvas)
                    {
                        _selectedElement = null;
                        vM.SelectedElement = null;
                        return;
                    }

                    if (!vM.FoundRawShape(_selectedElement))
                    {
                        _selectedElement = null;
                        return;
                    }

                    _current = e.GetPosition(_mainCanvas);

                    _adorner = new ResizingAdorner(_selectedElement, vM);
                    layer.Add(_adorner);
                    _mainCanvas.CaptureMouse();
                }
                else // new shape
                {
                    vM.SelectedElement = _selectedElement = null;
                    _mainCanvas.CaptureMouse();
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is Canvas))
                return;

            Canvas cv = (Canvas)sender;
            if (cv == null || cv.Name != "_mainCanvas")
                return;

            var vM = DataContext as DrawWizVM;
            if (vM == null)
                return;

            var pt = e.GetPosition(cv);

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (_selectedElement != null) 
            {          
                Canvas.SetLeft(_selectedElement, Canvas.GetLeft(_selectedElement) + pt.X - _current.X );
                Canvas.SetTop(_selectedElement, Canvas.GetTop(_selectedElement) + pt.Y - _current.Y);
                _current = pt;
            }
            else if (vM.TheShape != DrawWizModel.ShapeType.unknownType)
            {
                if (_tempShape != null)
                    cv.Children.Remove(_tempShape);

                // Add _current for free shapes
                _tempShape = vM.MakeTempShape(firstLeftPt, pt, _tempShape);

                if (_tempShape != null)
                    cv.Children.Add(_tempShape);

                _current = pt;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Canvas))
                return;

            Canvas cv = (Canvas)sender;
            if (cv == null || cv.Name != "_mainCanvas")
                return;

            var vM = DataContext as DrawWizVM;
            if (vM == null)
                return;

            if (_tempShape != null)
                cv.Children.Remove(_tempShape);

            var pos = e.GetPosition(cv);

            if (e.ChangedButton == MouseButton.Left)
            {
                _mainCanvas.ReleaseMouseCapture();
                if ( _selectedElement != null)
                {            
                    vM.MoveShape(firstLeftPt, pos);
                    AdornSelectedElement();
                }
                else
                {
                    TextAndFontData textData = new TextAndFontData();
                    if (vM.TheShape == DrawWizModel.ShapeType.textType)
                    {
                        var dlg = new TextDialog();
                        dlg.Owner = this;
                        if (dlg.ShowDialog() == false)
                            return;

                        textData = dlg.TextFontData;
                    }
                    vM.AddShape(firstLeftPt, pos, _tempShape, textData);           
                }
            }

            _tempShape = null;
        }

        private void AdornSelectedElement()
        {
            var vM = DataContext as DrawWizVM;
            if (vM == null)
                return;

            if (_selectedElement == null)
                return;

            ClearAdornerLayer(true);
            
            var layer = AdornerLayer.GetAdornerLayer(_mainCanvas);

            _adorner = new ResizingAdorner(vM.SelectedElement, vM);
            layer.Add(_adorner);
        }

        private void OnMoveForward(object sender, RoutedEventArgs e)
        {
            var vM = DataContext as DrawWizVM;
            if (vM == null)
                return;

            var forwardCmd = vM.ForwardCommand;
            if (forwardCmd.CanExecute(null))
            {
                forwardCmd.Execute(null);
                AdornSelectedElement();
            }
        }

        private void OnMoveBehind(object sender, RoutedEventArgs e)
        {
            var vM = DataContext as DrawWizVM;
            if (vM == null)
                return;

            var behindCmd = vM.BackCommand;
            if (behindCmd.CanExecute(null))
            {
                behindCmd.Execute(null);
                AdornSelectedElement();
            }
        }

        void ClearAdornerLayer(bool resize)
        {
            var layer = AdornerLayer.GetAdornerLayer(_mainCanvas);
            if (_adorner != null)
            {
                if(resize)
                    _adorner.Resize();

                Adorner[] toRemoveArray = layer.GetAdorners(_adorner.AdornedElement);
                if (toRemoveArray != null)
                {
                    for (int x = 0; x < toRemoveArray.Length; x++)
                    {
                        layer.Remove(toRemoveArray[x]);
                    }
                }
                _adorner = null;
            }
        }

        private void OnCustomColorDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vM = DataContext as DrawWizVM;
            if (vM == null)
                return;

            Microsoft.Samples.CustomControls.ColorPickerDialog cPicker
                = new Microsoft.Samples.CustomControls.ColorPickerDialog();

            cPicker.StartingColor = vM.CustomColorBrush.Color;
            cPicker.Owner = this;

            bool? dialogResult = cPicker.ShowDialog();
            if (dialogResult != null && (bool)dialogResult == true)
            {
                vM.CustomColorBrush = new SolidColorBrush(cPicker.SelectedColor);
            }

        }

    }
}
