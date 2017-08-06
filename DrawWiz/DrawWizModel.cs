using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DrawWiz
{
    [Serializable]
    public class DrawWizModel
    {
        public enum ShapeType { unknownType, ellipseType, rectangleType, lineType, triangleType, freeHandType, textType, curveType };

        public enum ShapeColor { redShape, orangeShape, yellowShape, greenShape, blueShape, purpleShape, grayShape, blackShape, customShape };

        public DrawWizModel()
        {}

        private List<DrawWizShape> _shapes = new List<DrawWizShape>();
        public List<DrawWizShape> Shapes
        {
            get { return _shapes; }
            set { _shapes = value; }
        }

        private ColorWrapper _customColor = new ColorWrapper(Colors.Aquamarine);

        public Color CustomColor
        {
            get { return _customColor.StdColor; }
            set { _customColor = new ColorWrapper(value); }
        }

        public ColorWrapper WrapperColor
        {
            get { return _customColor; }
            set { _customColor = value; }
        }
    }
}
