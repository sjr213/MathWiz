using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawWiz
{
    public class ShapeResizingMethods
    {
        static Point ConvertOldPt(Point oldPt, double oldWidth, double oldHeight, double width, double height)
        {
            Point newPt = new Point();

            if (oldWidth == 0.0)
                newPt.X = 0.0;
            else
                newPt.X = width * oldPt.X / oldWidth;

            if (oldHeight == 0)
                newPt.Y = 0.0;
            else
                newPt.Y = height * oldPt.Y / oldHeight;

            return newPt;
        }

        public static void ResizeRawShape(DrawWizShape rawShape, double left, double right, double top, double bottom)
        {
            double oldWidth = rawShape.Width;
            double oldHeight = rawShape.Height;

            rawShape.Width = right - left;
            rawShape.Height = bottom - top;

            if (rawShape.TheShapeType == DrawWizModel.ShapeType.lineType)
            {
                rawShape.Width -= rawShape.StrokeThickness;
                rawShape.Height -= rawShape.StrokeThickness;
            }

            rawShape.ExternalLeft = left;
            rawShape.ExternalTop = top;

            if (rawShape.X1 < rawShape.X2)
            {
                rawShape.X1 = rawShape.Left;
                rawShape.X2 = rawShape.Left + rawShape.Width - rawShape.StrokeThickness;
            }
            else
            {
                rawShape.X1 = rawShape.Left + rawShape.Width - rawShape.StrokeThickness;
                rawShape.X2 = rawShape.Left;
            }

            if (rawShape.Y1 < rawShape.Y2)
            {
                rawShape.Y1 = rawShape.Top;
                rawShape.Y2 = rawShape.Top + rawShape.Height - rawShape.StrokeThickness;
            }
            else
            {
                rawShape.Y1 = rawShape.Top + rawShape.Height - rawShape.StrokeThickness; // these changes don't make a difference
                rawShape.Y2 = rawShape.Top;
            }

            ArrayList newPts = new ArrayList();
            foreach (Point oldPt in rawShape.PointList)
            {
                Point newPt = ConvertOldPt(oldPt, oldWidth, oldHeight, rawShape.Width, rawShape.Height);
                newPts.Add(newPt);
            }

            rawShape.PointList = newPts;
        }

        public static bool ConvertRawToUiShape(DrawWizShape rawShape, UIElement shape)
        {         
            if (shape is Ellipse)
            {
                Ellipse ellipse = (Ellipse)shape;
                ellipse.Width = rawShape.Width;
                ellipse.Height = rawShape.Height;
                return true;
            }
            
            if (shape is Rectangle)
            {
                Rectangle rect = (Rectangle)shape;
                rect.Width = rawShape.Width;
                rect.Height = rawShape.Height;
                return true;
            }
        
            if (shape is Line)
            {
                Line line = (Line)shape;

                line.X1 = rawShape.X1 - rawShape.Left;
                line.Y1 = rawShape.Y1 - rawShape.Top;
                line.X2 = rawShape.X2 - rawShape.Left;
                line.Y2 = rawShape.Y2 - rawShape.Top;

                return true;
            }
          
            if (shape is Polygon)
            {
                Polygon poly = (Polygon)shape;
                poly.Width = rawShape.Width;
                poly.Height = rawShape.Height;
                PointCollection points = new PointCollection();
                points.Add(new Point(0, rawShape.Height));
                points.Add(new Point(rawShape.Width / 2, 0));
                points.Add(new Point(rawShape.Width, rawShape.Height));
                poly.Points = points;

                return true;
            }

            if (shape is Polyline)
            {
                Polyline polyLine = (Polyline)shape;
                polyLine.Width = rawShape.Width;
                polyLine.Height = rawShape.Height;

                ArrayList pts = rawShape.PointList;
                PointCollection newPoints = new PointCollection();
                foreach (Point pt in pts)
                {
                    newPoints.Add(pt);
                }

                polyLine.Points = newPoints;

                return true;
            }

            return false;
        }


    }
}
