using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace MexicanTennisSimulator.Classes
{
    sealed class Draw
    {
        public static readonly int StrokeThicknessLines = 5;
        public static readonly int StrokeThicknessNet = 1;
        public static readonly int AddNetLength = 40;
        public static readonly int PostDiameter = 6;
        public static readonly Brush ColorCourt = Brushes.DarkGreen;
        public static readonly Brush ColorLine = Brushes.White;
        public static readonly Brush ColorNet = Brushes.DimGray;
        public static readonly Brush ColorNetPost = Brushes.Black;
        private Grid _drawingArea;
        private ColumnDefinition _colCenter;
        private List<Shape> _courtObjects;

        public Draw(ref Grid drawingArea, int colCenter)
        {
            _drawingArea = drawingArea;
            _colCenter = drawingArea.ColumnDefinitions[colCenter];
            _courtObjects = new List<Shape>();
        }

        public void DrawCourt()
        {
            int index;

            index = 0;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "court";
            _courtObjects[index].Width = _drawingArea.ActualWidth;
            _courtObjects[index].Height = _drawingArea.ActualHeight;
            _courtObjects[index].StrokeThickness = 0;
            _courtObjects[index].Stroke = Brushes.Black;
            _courtObjects[index].Fill = ColorCourt;

            index = 1;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "outerLines";
            _courtObjects[index].Width = _colCenter.ActualWidth / 2;
            _courtObjects[index].Height = _drawingArea.ActualHeight / 2;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;

            index = 2;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "leftSinglesSideline";
            _courtObjects[index].Width = StrokeThicknessLines;
            _courtObjects[index].Height = _drawingArea.ActualHeight / 2;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;
            _courtObjects[index].SetValue(Grid.MarginProperty, new Thickness(0, 0, _colCenter.ActualWidth / 2 / 36 * 13.5, 0));

            index = 3;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "rightSinglesSideline";
            _courtObjects[index].Width = StrokeThicknessLines;
            _courtObjects[index].Height = _drawingArea.ActualHeight / 2;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;
            _courtObjects[index].SetValue(Grid.MarginProperty, new Thickness(_colCenter.ActualWidth / 36 * 13.5, 0, 0, 0));

            index = 4;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "upperServiceLine";
            _courtObjects[index].Width = _colCenter.ActualWidth / 72 * 27;
            _courtObjects[index].Height = StrokeThicknessLines;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;
            _courtObjects[index].SetValue(Grid.MarginProperty, new Thickness(0, 0, 0, _drawingArea.ActualHeight / 2 / 78 * 21));

            //index = 5;
            //_courtObjects.Add(new Rectangle());
            //_courtObjects[index].Name = "bottomServiceLine";
            //_courtObjects[index].Width = _colCenter.ActualWidth / 72 * 27;
            //_courtObjects[index].Height = StrokeThicknessLines;
            //_courtObjects[index].StrokeThickness = StrokeThicknessLines;
            //_courtObjects[index].Stroke = ColorLine;
            //_courtObjects[index].SetValue(Grid.MarginProperty, new Thickness(0, _drawingArea.ActualHeight / 2 / 78 * 21, 0, 0));

            //index = 6;
            //_courtObjects.Add(new Rectangle());
            //_courtObjects[index].Name = "centerServiceLine";
            //_courtObjects[index].Width = StrokeThicknessLines;
            //_courtObjects[index].Height = _drawingArea.ActualHeight / 2 / 78 * 42;
            //_courtObjects[index].StrokeThickness = StrokeThicknessLines;
            //_courtObjects[index].Stroke = ColorLine;
            //_courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - StrokeThicknessLines / 2);
            //_courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - _drawingArea.ActualHeight / 2 / 78 * 21);

            //index = 7;
            //_courtObjects.Add(new Rectangle());
            //_courtObjects[index].Name = "net";
            //_courtObjects[index].Width = _drawingArea.ActualWidth / 2 + AddNetLength + StrokeThicknessLines;
            //_courtObjects[index].Height = StrokeThicknessNet;
            //_courtObjects[index].StrokeThickness = StrokeThicknessNet;
            //_courtObjects[index].Stroke = ColorNet;
            //_courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - _drawingArea.ActualWidth / 2 / 36 * 18 - AddNetLength / 2 - StrokeThicknessLines / 2);
            //_courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - StrokeThicknessNet / 2);

            //index = 8;
            //_courtObjects.Add(new Ellipse());
            //_courtObjects[index].Name = "leftNetPost";
            //_courtObjects[index].Width = PostDiameter;
            //_courtObjects[index].Height = PostDiameter;
            //_courtObjects[index].Fill = ColorNetPost;
            //_courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - _drawingArea.ActualWidth / 2 / 36 * 18 - AddNetLength / 2 - StrokeThicknessLines / 2 - _courtObjects[index].Width / 2);
            //_courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - StrokeThicknessNet / 2 - _courtObjects[index].Height / 2);

            //index = 9;
            //_courtObjects.Add(new Ellipse());
            //_courtObjects[index].Name = "rightNetPost";
            //_courtObjects[index].Width = PostDiameter;
            //_courtObjects[index].Height = PostDiameter;
            //_courtObjects[index].Fill = ColorNetPost;
            //_courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 + _drawingArea.ActualWidth / 2 / 36 * 18 + AddNetLength / 2 + StrokeThicknessLines / 2 - _courtObjects[index].Width / 2);
            //_courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 + StrokeThicknessNet / 2 - _courtObjects[index].Height / 2);

            foreach (var item in _courtObjects)
            {
                _drawingArea.Children.Add(item);
                item.SetValue(Grid.ZIndexProperty, 1);
                item.SetValue(Grid.ColumnProperty, 1);
                item.Opacity = 0.05;
            }
        }

        public void ResizeCourt()
        {
            int index;

            index = 0;
            _courtObjects[index].Name = "court";
            _courtObjects[index].Width = _drawingArea.ActualWidth;
            _courtObjects[index].Height = _drawingArea.ActualHeight;
            _courtObjects[index].StrokeThickness = 0;
            _courtObjects[index].Stroke = Brushes.Black;
            _courtObjects[index].Fill = ColorCourt;

            index = 1;
            _courtObjects[index].Name = "outerLines";
            _courtObjects[index].Width = _drawingArea.ActualWidth / 2 + StrokeThicknessLines;
            _courtObjects[index].Height = _drawingArea.ActualHeight / 2 + StrokeThicknessLines;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - _courtObjects[index].Width / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - _courtObjects[index].Height / 2);

            index = 2;
            _courtObjects[index].Name = "leftSinglesSideline";
            _courtObjects[index].Width = StrokeThicknessLines;
            _courtObjects[index].Height = _drawingArea.ActualHeight / 2;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - _drawingArea.ActualWidth / 2 / 36 * 13.5 - StrokeThicknessLines / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - _drawingArea.ActualHeight / 2 / 2);

            index = 3;
            _courtObjects[index].Name = "rightSinglesSideline";
            _courtObjects[index].Width = StrokeThicknessLines;
            _courtObjects[index].Height = _drawingArea.ActualHeight / 2;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 + _drawingArea.ActualWidth / 2 / 36 * 13.5 - StrokeThicknessLines / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - _drawingArea.ActualHeight / 2 / 2);

            index = 4;
            _courtObjects[index].Name = "upperServiceLine";
            _courtObjects[index].Width = _drawingArea.ActualWidth / 2 - _drawingArea.ActualWidth / 2 / 36 * 9;
            _courtObjects[index].Height = StrokeThicknessLines;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - _drawingArea.ActualWidth / 2 / 36 * 13.5);
            _courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - _drawingArea.ActualHeight / 2 / 78 * 21 - StrokeThicknessLines / 2);

            index = 5;
            _courtObjects[index].Name = "bottomServiceLine";
            _courtObjects[index].Width = _drawingArea.ActualWidth / 2 - _drawingArea.ActualWidth / 2 / 36 * 9;
            _courtObjects[index].Height = StrokeThicknessLines;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - _drawingArea.ActualWidth / 2 / 36 * 13.5);
            _courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 + _drawingArea.ActualHeight / 2 / 78 * 21 - StrokeThicknessLines / 2);

            index = 6;
            _courtObjects[index].Name = "centerServiceLine";
            _courtObjects[index].Width = StrokeThicknessLines;
            _courtObjects[index].Height = _drawingArea.ActualHeight / 2 / 78 * 42;
            _courtObjects[index].StrokeThickness = StrokeThicknessLines;
            _courtObjects[index].Stroke = ColorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - StrokeThicknessLines / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - _drawingArea.ActualHeight / 2 / 78 * 21);

            index = 7;
            _courtObjects[index].Name = "net";
            _courtObjects[index].Width = _drawingArea.ActualWidth / 2 + AddNetLength + StrokeThicknessLines;
            _courtObjects[index].Height = StrokeThicknessNet;
            _courtObjects[index].StrokeThickness = StrokeThicknessNet;
            _courtObjects[index].Stroke = ColorNet;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - _drawingArea.ActualWidth / 2 / 36 * 18 - AddNetLength / 2 - StrokeThicknessLines / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - StrokeThicknessNet / 2);

            index = 8;
            _courtObjects[index].Name = "leftNetPost";
            _courtObjects[index].Width = PostDiameter;
            _courtObjects[index].Height = PostDiameter;
            _courtObjects[index].Fill = ColorNetPost;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 - _drawingArea.ActualWidth / 2 / 36 * 18 - AddNetLength / 2 - StrokeThicknessLines / 2 - _courtObjects[index].Width / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 - StrokeThicknessNet / 2 - _courtObjects[index].Height / 2);

            index = 9;
            _courtObjects[index].Name = "rightNetPost";
            _courtObjects[index].Width = PostDiameter;
            _courtObjects[index].Height = PostDiameter;
            _courtObjects[index].Fill = ColorNetPost;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _drawingArea.ActualWidth / 2 + _drawingArea.ActualWidth / 2 / 36 * 18 + AddNetLength / 2 + StrokeThicknessLines / 2 - _courtObjects[index].Width / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _drawingArea.ActualHeight / 2 + StrokeThicknessNet / 2 - _courtObjects[index].Height / 2);

        }
    }
}
