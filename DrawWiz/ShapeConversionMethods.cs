﻿using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawWiz
{
    public static class ShapeConversionMethods
    {
        public static DrawWizShape CreateRawShape(DrawWizModel.ShapeType shapeType, DrawWizModel.ShapeColor shapeColor, Point pt1, Point pt2, UIElement tempShape, TextAndFontData textData, Color customColor)
        {
            if (tempShape != null && shapeType == DrawWizModel.ShapeType.freeHandType)
                return CreateFreeHandRawShape(shapeColor, pt1, pt2, tempShape, customColor);

            double left = pt1.X < pt2.X ? pt1.X : pt2.X;
            double top = pt1.Y < pt2.Y ? pt1.Y : pt2.Y;
            double width = pt1.X < pt2.X ? pt2.X - pt1.X : pt1.X - pt2.X;
            double height = pt1.Y < pt2.Y ? pt2.Y - pt1.Y : pt1.Y - pt2.Y;

            if (width < 1 || height < 1)
                return null;

            var newShape = new DrawWizShape();
            newShape.TheShapeType = shapeType;
            newShape.TheShapeColor = shapeColor;
            newShape.CustomColor = new ColorWrapper(customColor);
            newShape.Width = width;
            newShape.Height = height;

            if (newShape.TheShapeType == DrawWizModel.ShapeType.lineType ||
                newShape.TheShapeType == DrawWizModel.ShapeType.freeHandType)
            {
                newShape.Width += newShape.StrokeThickness;
                newShape.Height += newShape.StrokeThickness;
            }

            newShape.Left = left;
            newShape.Top = top;
            newShape.X1 = pt1.X;
            newShape.Y1 = pt1.Y;
            newShape.X2 = pt2.X;
            newShape.Y2 = pt2.Y;

            newShape.TextData = textData;

            return newShape;
        }

        public static DrawWizShape CreateFreeHandRawShape(DrawWizModel.ShapeColor shapeColor, Point pt1, Point pt2, UIElement tempShape, Color customColor)
        {
            double oldLeft = Canvas.GetLeft(tempShape);
            double oldTop = Canvas.GetTop(tempShape);
            double oldRight = Canvas.GetRight(tempShape);
            double oldBottom = Canvas.GetBottom(tempShape);
            double oldWidth = oldRight - oldLeft;
            double oldHeight = oldBottom - oldTop;

            double newLeft = oldLeft;
            if(pt2.X < oldLeft)
                newLeft = pt2.X;

            double newTop = oldTop;
            if(pt2.Y < oldTop)
                newTop = pt2.Y;

            double newWidth = oldWidth;
            if (pt2.X < oldLeft)
                newWidth = oldWidth + (oldLeft - pt2.X);
            else if (pt2.X > oldRight)
                newWidth = oldWidth + (pt2.X - oldRight);

            double newHeight = oldHeight;
            if (pt2.Y < oldTop)
                newHeight = oldHeight + (oldTop - pt2.Y);
            else if (pt2.Y > oldBottom)
                newHeight = oldHeight + (pt2.Y - oldBottom);

            double deltaLeft = oldLeft - newLeft;
            double deltaTop = oldTop - newTop;

            // set the rawShape members appropriately
            var newShape = new DrawWizShape();
            newShape.TheShapeType = DrawWizModel.ShapeType.freeHandType;
            newShape.TheShapeColor = shapeColor;
            newShape.CustomColor = new ColorWrapper(customColor);
            newShape.Width = newWidth;
            newShape.Height = newHeight;
            newShape.ExternalLeft = newLeft;
            newShape.ExternalTop = newTop;
            newShape.X1 = pt1.X;
            newShape.Y1 = pt1.Y;
            newShape.X2 = pt2.X;
            newShape.Y2 = pt2.Y;
            newShape.DeltaX = deltaLeft;
            newShape.DeltaY = deltaTop;

            // create points
            ArrayList pts = new ArrayList();
            pts.Add(pt1);
            pts.Add(pt2);
            newShape.PointList = pts;

            return newShape;
        }

        public static Brush GetBrush(DrawWizShape rawShape)
        {
            DrawWizModel.ShapeColor color = rawShape.TheShapeColor;
            switch (color)
            {
                case DrawWizModel.ShapeColor.blackShape:
                    return Brushes.Black;
                case DrawWizModel.ShapeColor.blueShape:
                    return Brushes.Blue;
                case DrawWizModel.ShapeColor.grayShape:
                    return Brushes.Gray;
                case DrawWizModel.ShapeColor.greenShape:
                    return Brushes.Green;
                case DrawWizModel.ShapeColor.orangeShape:
                    return Brushes.Orange;
                case DrawWizModel.ShapeColor.purpleShape:
                    return Brushes.Purple;
                case DrawWizModel.ShapeColor.redShape:
                    return Brushes.Red;
                case DrawWizModel.ShapeColor.yellowShape:
                    return Brushes.Yellow;
                case DrawWizModel.ShapeColor.customShape:
                    return new SolidColorBrush(rawShape.CustomColor.StdColor);
            }

            return Brushes.AntiqueWhite;
        }

        public static PointCollection AddOldPointsToFreeHandShape(DrawWizShape rawShape, UIElement tempShape)
        {
            if (rawShape.TheShapeType != DrawWizModel.ShapeType.freeHandType ||
               ! (tempShape is Polyline) )
                return null;

            Polyline oldShape = tempShape as Polyline;

            PointCollection newPts = new PointCollection();
            foreach (Point pt in oldShape.Points)
            {
                Point newPt = new Point(pt.X + rawShape.DeltaX, pt.Y + rawShape.DeltaY);
                newPts.Add(newPt);
            }

            return newPts;
        }

        static ArrayList ConvertPointList(PointCollection ptCollection)
        {
            ArrayList ptList = new ArrayList();
            foreach (Point pt in ptCollection)
            {
                ptList.Add(pt);
            }

            return ptList;
        }

        public static UIElement ConvertRawShapeToElement(DrawWizShape rawShape, UIElement tempShape)
        {
            Brush brush = GetBrush(rawShape);

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

                        line.StrokeThickness = DrawWizShape.DefaultStrokeThickness;
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

                        rawShape.X1 -= rawShape.Left;
                        rawShape.Y1 -= rawShape.Top;
                        rawShape.X2 -= rawShape.Left;
                        rawShape.Y2 -= rawShape.Top;

                        // if tempShape
                        if (tempShape != null)
                        {
                            PointCollection oldPts = AddOldPointsToFreeHandShape(rawShape, tempShape);
                            if (oldPts != null)
                            {
                                pts = oldPts;
                            }
                            else
                            {
                                Point pt1 = new Point(rawShape.X1, rawShape.Y1);
                                pts.Add(pt1);
                            }

                            Point pt2 = new Point(rawShape.X2, rawShape.Y2);
                            pts.Add(pt2);
                        }
                        else
                        {
                            // need to load collection from rawshapes
                            foreach (Point pt in rawShape.PointList)
                            {
                                pts.Add(pt);
                            }
                        }

                        polyLine.Points = pts;
                        rawShape.PointList = ConvertPointList(pts);
                        return polyLine;
                    }
                case DrawWizModel.ShapeType.textType:
                    {
                        TextBlock label = new TextBlock();

                        label.Foreground = brush;
                        label.Text = rawShape.TextData.Text;
                        label.FontFamily = rawShape.TextData.Family;
                        label.FontStyle = rawShape.TextData.Style;
                        label.FontWeight = rawShape.TextData.Weight;
                        label.FontSize = rawShape.TextData.Size;

                        if (label.Text.Count() == 0)
                        {
                            label.Width = rawShape.Width;
                            label.Height = rawShape.Height;
                        }
                        else
                        {
                            Size maxSize = new Size(1000,700);
                            label.Measure(maxSize);
                            label.Width = label.DesiredSize.Width;    //rawShape.Width;
                            label.Height = label.DesiredSize.Height; // rawShape.Height;
                        }


                        return label;
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

        public static UIElement ConvertShape(DrawWizShape rawShape, UIElement tempShape)
        {
            UIElement shape = ConvertRawShapeToElement(rawShape, tempShape);

            if (shape != null)
            {
                Canvas.SetLeft(shape, rawShape.ExternalLeft);
                Canvas.SetTop(shape, rawShape.ExternalTop);
                Canvas.SetRight(shape, rawShape.ExternalLeft + rawShape.Width);
                Canvas.SetBottom(shape, rawShape.ExternalTop + rawShape.Height);
                Canvas.SetZIndex(shape, rawShape.Zindex);
            }

            return shape;
        }
    }
}
