using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MyMvvmLib;
using Microsoft.Win32;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DrawWiz
{
    public class DrawWizVM : ViewModelBase<DrawWizModel>, IShapeMover
    {
        public DrawWizVM(DrawWizModel model)
        {
            Model = model;
        }

        private DrawWizModel.ShapeType _theShape = DrawWizModel.ShapeType.unknownType;

        public DrawWizModel.ShapeType TheShape
        {
            get
            {
                return _theShape;
            }
            set
            {
                SetProperty(ref _theShape, value, () => TheShape);
            }
        }

        private DrawWizModel.ShapeColor _theColor = DrawWizModel.ShapeColor.redShape;

        public DrawWizModel.ShapeColor TheColor
        {
            get
            {
                return _theColor;
            }
            set
            {
                SetProperty(ref _theColor, value, () => TheColor);
            }
        }

        private SolidColorBrush _customColorBrush;
        public SolidColorBrush CustomColorBrush
        {
            get
            {
                return new SolidColorBrush(Model.CustomColor);
            }
            set
            {
                Model.CustomColor = value.Color;
                SetProperty(ref _customColorBrush, value, () => CustomColorBrush);            
            }
        }


        private UIElement _selectedElement = null;

        public UIElement SelectedElement
        {
            get
            {
                return _selectedElement;
            }
            set
            {
                SetProperty(ref _selectedElement, value, () => SelectedElement);
                ElementIsSelected = (_selectedElement != null);
            }
        }

        private bool _elementIsSelected = false;

        public bool ElementIsSelected
        {
            get
            {
                return _elementIsSelected;
            }

            set
            {
                SetProperty(ref _elementIsSelected, value, () => ElementIsSelected);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////

        private ObservableCollection<UIElement> _theShapes;

        public ObservableCollection<UIElement> TheShapes
        {
            get
            {
                if (_theShapes == null)
                {
                    _theShapes = new ObservableCollection<UIElement>();

                    SetProperty(ref _theShapes, _theShapes, () => TheShapes);
                }

                return _theShapes;
            }

            set
            {
                if(value != null)
                    SetProperty(ref _theShapes, value, () => TheShapes);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////

        protected DrawWizShape AddRawShape(Point pt1, Point pt2, UIElement tempShape, TextAndFontData textData)
        {
            if (TheShape == DrawWizModel.ShapeType.unknownType)
                return null;

            var newShape = ShapeConversionMethods.CreateRawShape(TheShape, TheColor, pt1, pt2, tempShape, textData, Model.CustomColor);

            TheShape = DrawWizModel.ShapeType.unknownType;

            if(newShape != null)
                Model.Shapes.Add(newShape);

            return newShape;
        }

        public void AddShape(Point pt1, Point pt2, UIElement tempShape, TextAndFontData textData)
        {
            DrawWizShape rawShape = AddRawShape(pt1, pt2, tempShape, textData);
            if (rawShape == null)
                return;

            UIElement realShape = ShapeConversionMethods.ConvertShape(rawShape, tempShape);
            if (realShape != null)
                TheShapes.Add(realShape);
        }



        public UIElement MakeTempShape(Point pt1, Point pt2, UIElement tempShape)
        {
            DrawWizShape rawShape = ShapeConversionMethods.CreateRawShape(TheShape, TheColor, pt1, pt2, tempShape, new TextAndFontData(), Model.CustomColor);
            if (rawShape == null)
                return null;

            return ShapeConversionMethods.ConvertShape(rawShape, tempShape);
        }

        public void MoveShape(Point pt1, Point pt2)
        {
            // find element
            if (_selectedRawShape == null)
                return;

            // recalculate X, Y
            double newX = pt2.X - pt1.X;
            double newY = pt2.Y - pt1.Y;

            _selectedRawShape.ExternalLeft = _selectedRawShape.ExternalLeft + newX;
            _selectedRawShape.ExternalTop = _selectedRawShape.ExternalTop + newY;

            // we don't need to recalculate the shapes
        }

        private void ConvertAllMovedShapes()
        {
            ObservableCollection<UIElement> newShapes = new ObservableCollection<UIElement>();
            foreach (var rawShape in Model.Shapes)
            {
                UIElement realShape = ShapeMovingMethods.ConvertMovedShape(rawShape);
                if (realShape != null)
                    newShapes.Add(realShape);
            }

            TheShapes = newShapes;
            UpdateSelectedShape();
        }

        private void ConvertAllShapes()
        {
            ObservableCollection<UIElement> newShapes = new ObservableCollection<UIElement>();
            foreach (var rawShape in Model.Shapes)
            {
                UIElement realShape = ShapeConversionMethods.ConvertShape(rawShape, null);
                if (realShape != null)
                    newShapes.Add(realShape);
            }

            TheShapes = newShapes;         
        }

        private void UpdateSelectedShape()
        {
            if(SelectedElement == null)
                return;

            foreach (var shape in TheShapes)
            {
                if( Canvas.GetLeft(shape) == Canvas.GetLeft(_selectedElement) &&
                    Canvas.GetTop(shape) == Canvas.GetTop(_selectedElement) &&
                    Canvas.GetZIndex(shape) == Canvas.GetZIndex(_selectedElement))
                {
                    SelectedElement = shape;
                    return;
                }
            }
        }

        private DrawWizShape _selectedRawShape = null;

        public bool FoundRawShape(UIElement targetElement)
        {
            _selectedRawShape = ShapeMovingMethods.FindMatchingShape(targetElement, Model.Shapes);

            SelectedElement = (_selectedRawShape == null) ? null : targetElement;

            return _selectedRawShape != null;
        }

        //////////////////////////////////////////////////////////////////////////////////

        private ICommand _saveAsCommand = null;

        public ICommand SaveAsCommand
        {
            get
            {
                return _saveAsCommand ?? (_saveAsCommand =
                    new RelayCommand(() =>
                    {
                        SaveFile();
                    }));
            }
        }


        private ICommand _openCommand = null;

        public ICommand OpenCommand
        {
            get
            {
                return _openCommand ?? (_openCommand =
                    new RelayCommand(() =>
                    {
                        var dlg = new OpenFileDialog();
                        dlg.Filter = "Draw Wiz files|*.drawWiz";
                        dlg.Title = "Open";
                        dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        if (dlg.ShowDialog() == true)
                        {
                            try
                            {
                                using (Stream stream = File.Open(dlg.FileName, FileMode.Open))
                                {
                                    BinaryFormatter bf = new BinaryFormatter();

                                    object obj = bf.Deserialize(stream);

                                    DrawWizModel tempModel = obj as DrawWizModel;

                                    if (tempModel != null)
                                    {
                                        Model = tempModel;

                                        ConvertAllShapes();
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Save As");
                            }
                        }
                    }));
            }
        }

        void SaveFile()
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "Draw Wiz files|*.drawWiz";
            dlg.Title = "Save As";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    using (Stream stream = File.Open(dlg.FileName, FileMode.Create))
                    {
                        BinaryFormatter bf = new BinaryFormatter();

                        // 1 image queue
                        bf.Serialize(stream, Model);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Save As");
                }
            }
        }


        private ICommand _newFileCommand = null;

        public ICommand NewFileCommand
        {
            get
            {
                return _newFileCommand ?? (_newFileCommand =
                    new RelayCommand(() =>
                    {
                        if(TheShapes.Count() > 0)
                        {
                            System.Windows.MessageBoxResult result = MessageBox.Show("Do you want to save your changes?", "Unsaved data", MessageBoxButton.YesNoCancel);
                            if (result == System.Windows.MessageBoxResult.Cancel)
                                return;

                            if (result == System.Windows.MessageBoxResult.Yes)
                                SaveFile();
                        }

                        Model.Shapes.Clear();
                        TheShapes.Clear();
                    }));
            }
        }

        private ICommand _deleteCommand = null;

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand =
                    new RelayCommand(() =>
                    {
                        if (ShapeMovingMethods.DeleteMatchingShape(SelectedElement, Model.Shapes))
                            SelectedElement = null;

                        ConvertAllMovedShapes();
                    }));
            }
        }

        private ICommand _forwardCommand = null;

        public ICommand ForwardCommand
        {
            get
            {
                return _forwardCommand ?? (_forwardCommand =
                    new RelayCommand(() =>
                    {
                        DrawWizShape rawShape = ShapeMovingMethods.FindMatchingShape(SelectedElement, Model.Shapes);
                        if (rawShape == null)
                            return;

                        int z = rawShape.Zindex;
                        ++z;
                        rawShape.Zindex = z;

                        ConvertAllMovedShapes();
                    }));
            }
        }

        private ICommand _backCommand = null;

        public ICommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand =
                    new RelayCommand(() =>
                    {
                        DrawWizShape rawShape = ShapeMovingMethods.FindMatchingShape(SelectedElement, Model.Shapes);
                        if (rawShape == null)
                            return;

                        int z = rawShape.Zindex;
                        --z;
                        rawShape.Zindex = z;

                        ConvertAllMovedShapes();
                    }));
            }
        }

        public void ResizeSelectedShape()
        {
            if (_selectedElement == null || _selectedRawShape == null)
                return;

            double left = Canvas.GetLeft(_selectedElement);
            double right = Canvas.GetRight(_selectedElement);
            double top = Canvas.GetTop(_selectedElement);
            double bottom = Canvas.GetBottom(_selectedElement);

            ShapeResizingMethods.ResizeRawShape(_selectedRawShape, left, right, top, bottom);

            ShapeResizingMethods.ConvertRawToUiShape(_selectedRawShape, _selectedElement);
        }
    }
}
