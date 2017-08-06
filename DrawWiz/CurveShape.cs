using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawWiz
{
    public class CurveShape : Shape
    {
        protected Point _begin;
        public Point Begin
        {
            get { return _begin; }
            set { _begin = value; }
        }

        protected Point _middle;
        public Point Middle
        {
            get { return _middle; }
            set { _middle = value; }
        }

        protected Point _end;
        public Point End
        {
            get { return _end; }
            set { _end = value; }
        }


        protected override Geometry DefiningGeometry
        {
            get { return GetCurveGeometry(); }
        }

        private Geometry GetCurveGeometry()
        {
            StreamGeometry geo = new StreamGeometry();
            using (StreamGeometryContext gc = geo.Open())
            {
                _begin = new Point(0, Height);
                _middle = new Point(Width / 2, 0);
                _end = new Point(Width, Height);

                gc.BeginFigure(_begin, false, false);

                // isFilled = false, isClosed = true
                gc.QuadraticBezierTo(_middle, _end, true, false);
            }

            return geo;
        }
    }
}
