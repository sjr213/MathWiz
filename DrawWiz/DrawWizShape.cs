using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DrawWiz
{
    [Serializable]
    public class DrawWizShape
    {
        public const double DefaultStrokeThickness = 3;
        private double _strokeThickness = DefaultStrokeThickness;

        private DrawWizModel.ShapeType _shapeType = DrawWizModel.ShapeType.ellipseType;

        public DrawWizModel.ShapeType TheShapeType
        {
            get { return _shapeType; }
            set { _shapeType = value; }
        }

        private DrawWizModel.ShapeColor _shapeColor = DrawWizModel.ShapeColor.blueShape;

        public DrawWizModel.ShapeColor TheShapeColor
        {
            get { return _shapeColor; }
            set { _shapeColor = value; }
        }

        private ColorWrapper _customColor = new ColorWrapper(Colors.Aquamarine);
        public DrawWiz.ColorWrapper CustomColor
        {
            get { return _customColor; }
            set { _customColor = value; }
        }
       
        private double _width = 10;

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private double _height = 10;

        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        private double _left = 0;

        public double Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public double ExternalLeft
        {
            get
            {
                if (DrawWizModel.ShapeType.lineType == TheShapeType ||
                    DrawWizModel.ShapeType.freeHandType == TheShapeType) 
                {
                    return _left - _strokeThickness / 2;
                }
                else
                    return _left;
            }

            set
            {
                if (DrawWizModel.ShapeType.lineType == TheShapeType ||
                    DrawWizModel.ShapeType.freeHandType == TheShapeType) 
                {
                    _left = value + _strokeThickness / 2;
                }
                else
                    _left = value;
            }
        }

        public double ExternalTop
        {
            get
            {
                if (DrawWizModel.ShapeType.lineType == TheShapeType  ||
                    DrawWizModel.ShapeType.freeHandType == TheShapeType) 
                {
                    return _top - _strokeThickness / 2;
                }
                else
                    return _top;
            }

            set
            {
                if (DrawWizModel.ShapeType.lineType == TheShapeType ||
                    DrawWizModel.ShapeType.freeHandType == TheShapeType) 
                {
                    _top = value + _strokeThickness / 2;
                }
                else
                    _top = value;
            }
        }

        private double _top = 0;

        public double Top
        {
            get { return _top; }
            set { _top = value; }
        }

        private double _x1 = 0;
        public double X1
        {
            get { return _x1; }
            set { _x1 = value; }
        }

        private double _y1 = 0;
        public double Y1
        {
            get { return _y1; }
            set { _y1 = value; }
        }

        private double _x2 = 0;
        public double X2
        {
            get { return _x2; }
            set { _x2 = value; }
        }

        private double _y2 = 0;
        public double Y2
        {
            get { return _y2; }
            set { _y2 = value; }
        }

        private int _zIndex = 0;
        public int Zindex
        {
            get { return _zIndex; }
            set { _zIndex = value; }
        }

        private double _deltaX = 0;
        public double DeltaX
        {
            get { return _deltaX; }
            set { _deltaX = value; }
        }

        private double _deltaY = 0;
        public double DeltaY
        {
            get { return _deltaY; }
            set { _deltaY = value; }
        }

        private ArrayList _pointList = new ArrayList();
        public ArrayList PointList
        {
            get { return _pointList; }
            set { _pointList = value; }
        }

        public double StrokeThickness
        {
            get { return _strokeThickness; }
            set { _strokeThickness = value; }
        }

        private TextAndFontData _textData = new TextAndFontData();
        public TextAndFontData TextData
        {
            get { return _textData; }
            set { _textData = value;  }
        }

    }
}
