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
    public static class ShapeMovingMethods
    {
        public static DrawWizModel.ShapeType DetermineType(UIElement targetElement)
        {
            Ellipse el = targetElement as Ellipse;
            if (el != null)
                return DrawWizModel.ShapeType.ellipseType;

            Rectangle rect = targetElement as Rectangle;
            if (rect != null)
                return DrawWizModel.ShapeType.rectangleType;

            Line line = targetElement as Line;
            if (line != null)
                return DrawWizModel.ShapeType.lineType;

            Polygon poly = targetElement as Polygon;
            if (poly != null)
                return DrawWizModel.ShapeType.triangleType;

            Polyline polyLine = targetElement as Polyline;
            if (polyLine != null)
                return DrawWizModel.ShapeType.freeHandType;

            Label label = targetElement as Label;
            if (label != null)
                return DrawWizModel.ShapeType.textType;

            TextBlock textBlock = targetElement as TextBlock;
            if (textBlock != null)
                return DrawWizModel.ShapeType.textType;

            CurveShape curve = targetElement as CurveShape;
            if (curve != null)
                return DrawWizModel.ShapeType.curveType;

            return DrawWizModel.ShapeType.unknownType;
        }

        public static DrawWizShape FindMatchingShape(UIElement targetElement, List<DrawWizShape> shapes)
        {
            DrawWizModel.ShapeType shapeType = DetermineType(targetElement);
            if (shapeType == DrawWizModel.ShapeType.unknownType)
                return null;

            double left = Canvas.GetLeft(targetElement);
            double top = Canvas.GetTop(targetElement);

            foreach (DrawWizShape rawShape in shapes)
            {
                if (rawShape.TheShapeType == shapeType &&
                    rawShape.ExternalLeft == left &&
                    rawShape.ExternalTop == top)
                    return rawShape;
            }

            return null;
        }

        public static bool DeleteMatchingShape(UIElement targetElement, List<DrawWizShape> shapes)
        {
            DrawWizModel.ShapeType shapeType = DetermineType(targetElement);
            if (shapeType == DrawWizModel.ShapeType.unknownType)
                return false;

            double left = Canvas.GetLeft(targetElement);
            double top = Canvas.GetTop(targetElement);

            foreach (DrawWizShape rawShape in shapes)
            {
                if (rawShape.TheShapeType == shapeType &&
                    rawShape.ExternalLeft == left &&
                    rawShape.ExternalTop == top)
                {
                    shapes.Remove(rawShape);
                    return true;
                }
            }

            return false;
        }

        public static UIElement ConvertMovedRawShapeToElement(DrawWizShape rawShape)
        {
            Brush brush = ShapeConversionMethods.GetBrush(rawShape);

            switch (rawShape.TheShapeType)
            {
                case DrawWizModel.ShapeType.ellipseType:
                    {
                        Shape ellipse = new Ellipse();
                        ellipse.Fill = brush;
                        ellipse.Stroke = brush;
                        ellipse.Width = rawShape.Width;
                        ellipse.Height = rawShape.Height;

                        return ellipse;
                    }
                case DrawWizModel.ShapeType.rectangleType:
                    {
                        Shape rect = new Rectangle();
                        rect.Fill = brush;
                        rect.Stroke = brush;
                        rect.Width = rawShape.Width;
                        rect.Height = rawShape.Height;

                        return rect;
                    }
                case DrawWizModel.ShapeType.lineType:
                    {
                        Line line = new Line();
                        line.Stroke = brush;
                        line.StrokeThickness = 3;
                        line.HorizontalAlignment = HorizontalAlignment.Left;
                        line.VerticalAlignment = VerticalAlignment.Top;

                        if (rawShape.X1 >= rawShape.Left && rawShape.X2 >= rawShape.Left)
                        {
                            rawShape.X1 -= rawShape.Left;
                            rawShape.Y1 -= rawShape.Top;
                            rawShape.X2 -= rawShape.Left;
                            rawShape.Y2 -= rawShape.Top;
                        }

                        line.X1 = rawShape.X1;
                        line.Y1 = rawShape.Y1;
                        line.X2 = rawShape.X2;
                        line.Y2 = rawShape.Y2;

                        line.Width = rawShape.Width + line.StrokeThickness;
                        line.Height = rawShape.Height + line.StrokeThickness;

                        return line;
                    }
                case DrawWizModel.ShapeType.triangleType:
                    {
                        Polygon poly = new Polygon();
                        poly.Fill = brush;
                        poly.Stroke = brush;
                        poly.Width = rawShape.Width;
                        poly.Height = rawShape.Height;
                        PointCollection points = new PointCollection();
                        points.Add(new Point(0, rawShape.Height));
                        points.Add(new Point(rawShape.Width / 2, 0));
                        points.Add(new Point(rawShape.Width, rawShape.Height));
                        poly.Points = points;
                        return poly;
                    }
                case DrawWizModel.ShapeType.freeHandType:
                    {
                        Polyline polyLine = new Polyline();
                        polyLine.Fill = null;
                        polyLine.Stroke = brush;
                        polyLine.StrokeThickness = DrawWizShape.DefaultStrokeThickness;
                        polyLine.Width = rawShape.Width;    
                        polyLine.Height = rawShape.Height; 
                        PointCollection pts = new PointCollection();

                        // need to load collection from rawshapes
                        foreach (Point pt in rawShape.PointList)
                        {
                            pts.Add(pt);
                        }

                        polyLine.Points = pts;

                        return polyLine;
                    }
                case DrawWizModel.ShapeType.curveType:
                    {
                        CurveShape curve = new CurveShape();
                        curve.Fill = brush;
                        curve.Stroke = brush;
                        curve.Width = rawShape.Width;
                        curve.Height = rawShape.Height;
    
                        return curve;
                    }
            }
            return null;
        }

        public static UIElement ConvertMovedShape(DrawWizShape rawShape)
        {
            UIElement shape = ConvertMovedRawShapeToElement(rawShape);

            if (shape != null)
            {
                Canvas.SetLeft(shape, rawShape.ExternalLeft);
                Canvas.SetTop(shape, rawShape.ExternalTop);
                Canvas.SetZIndex(shape, rawShape.Zindex);
            }

            return shape;
        }


    }
}
