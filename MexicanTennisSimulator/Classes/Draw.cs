using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MexicanTennisSimulator.Classes
{
    sealed class Draw
    {
        private static Canvas _rCourt;
        private static List<Shape> _courtObjects;

        public static void DrawCourt(ref Canvas rCourt)
        {
            _rCourt = rCourt;
            _courtObjects = new List<Shape>();
            int index;
            int strokeThicknessLines = 5;
            int strokeThicknessNet = 1;
            int addNetLength = 40;
            int postDiameter = 6;
            Brush colorCourt = Brushes.DarkGreen;
            Brush colorLine = Brushes.White;
            Brush colorNet = Brushes.DimGray;
            Brush colorNetPost = Brushes.Black;

            double test = _rCourt.Width;
            double test2 = _rCourt.ActualWidth;
            double test3 = _rCourt.MaxWidth;

            index = 0;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "court";
            _courtObjects[index].Width = _rCourt.ActualWidth;
            _courtObjects[index].Height = _rCourt.ActualHeight;
            _courtObjects[index].StrokeThickness = 0;
            _courtObjects[index].Stroke = Brushes.Black;
            _courtObjects[index].Fill = colorCourt;

            index = 1;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "outerLines";
            _courtObjects[index].Width = _rCourt.ActualWidth / 2;
            _courtObjects[index].Height = _rCourt.ActualHeight / 2;
            _courtObjects[index].StrokeThickness = strokeThicknessLines;
            _courtObjects[index].Stroke = colorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _rCourt.ActualWidth / 2 - _courtObjects[index].Width / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _rCourt.ActualHeight / 2 - _courtObjects[index].Height / 2);

            index = 2;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "leftSinglesSideline";
            _courtObjects[index].Width = strokeThicknessLines;
            _courtObjects[index].Height = _rCourt.ActualHeight / 2;
            _courtObjects[index].StrokeThickness = strokeThicknessLines;
            _courtObjects[index].Stroke = colorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _rCourt.ActualWidth / 2 - _rCourt.ActualWidth / 2 / 36 * 14 - strokeThicknessLines / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _rCourt.ActualHeight / 2 - _rCourt.ActualHeight / 2 / 2);

            index = 3;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "rightSinglesSideline";
            _courtObjects[index].Width = strokeThicknessLines;
            _courtObjects[index].Height = _rCourt.ActualHeight / 2;
            _courtObjects[index].StrokeThickness = strokeThicknessLines;
            _courtObjects[index].Stroke = colorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _rCourt.ActualWidth / 2 + _rCourt.ActualWidth / 2 / 36 * 14 - strokeThicknessLines / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _rCourt.ActualHeight / 2 - _rCourt.ActualHeight / 2 / 2);

            index = 4;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "upperServiceLine";
            _courtObjects[index].Width = _rCourt.ActualWidth / 2 - _rCourt.ActualWidth / 2 / 36 * 8;
            _courtObjects[index].Height = strokeThicknessLines;
            _courtObjects[index].StrokeThickness = strokeThicknessLines;
            _courtObjects[index].Stroke = colorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _rCourt.ActualWidth / 2 - _rCourt.ActualWidth / 2 / 36 * 14);
            _courtObjects[index].SetValue(Canvas.TopProperty, _rCourt.ActualHeight / 2 - _rCourt.ActualHeight / 2 / 78 * 21 - strokeThicknessLines / 2);

            index = 5;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "bottomServiceLine";
            _courtObjects[index].Width = _rCourt.ActualWidth / 2 - _rCourt.ActualWidth / 2 / 36 * 8;
            _courtObjects[index].Height = strokeThicknessLines;
            _courtObjects[index].StrokeThickness = strokeThicknessLines;
            _courtObjects[index].Stroke = colorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _rCourt.ActualWidth / 2 - _rCourt.ActualWidth / 2 / 36 * 14);
            _courtObjects[index].SetValue(Canvas.TopProperty, _rCourt.ActualHeight / 2 + _rCourt.ActualHeight / 2 / 78 * 21 - strokeThicknessLines / 2);

            index = 6;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "centerServiceLine";
            _courtObjects[index].Width = strokeThicknessLines;
            _courtObjects[index].Height = _rCourt.ActualHeight / 2 / 78 * 42;
            _courtObjects[index].StrokeThickness = strokeThicknessLines;
            _courtObjects[index].Stroke = colorLine;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _rCourt.ActualWidth / 2 - strokeThicknessLines / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _rCourt.ActualHeight / 2 - _rCourt.ActualHeight / 2 / 78 * 21);

            index = 7;
            _courtObjects.Add(new Rectangle());
            _courtObjects[index].Name = "net";
            _courtObjects[index].Width = _rCourt.ActualWidth / 2 + addNetLength;
            _courtObjects[index].Height = strokeThicknessNet;
            _courtObjects[index].StrokeThickness = strokeThicknessNet;
            _courtObjects[index].Stroke = colorNet;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _rCourt.ActualWidth / 2 - _rCourt.ActualWidth / 2 / 36 * 18 - addNetLength / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _rCourt.ActualHeight / 2 - strokeThicknessNet / 2);

            index = 8;
            _courtObjects.Add(new Ellipse());
            _courtObjects[index].Name = "leftNetPost";
            _courtObjects[index].Width = postDiameter;
            _courtObjects[index].Height = postDiameter;
            _courtObjects[index].Fill = colorNetPost;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _rCourt.ActualWidth / 2 - _rCourt.ActualWidth / 2 / 36 * 18 - addNetLength / 2 - _courtObjects[index].Width / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _rCourt.ActualHeight / 2 - strokeThicknessNet / 2 - _courtObjects[index].Height / 2);

            index = 9;
            _courtObjects.Add(new Ellipse());
            _courtObjects[index].Name = "rightNetPost";
            _courtObjects[index].Width = postDiameter;
            _courtObjects[index].Height = postDiameter;
            _courtObjects[index].Fill = colorNetPost;
            _courtObjects[index].SetValue(Canvas.LeftProperty, _rCourt.ActualWidth / 2 + _rCourt.ActualWidth / 2 / 36 * 18 + addNetLength / 2 - _courtObjects[index].Width / 2);
            _courtObjects[index].SetValue(Canvas.TopProperty, _rCourt.ActualHeight / 2 + strokeThicknessNet / 2 - _courtObjects[index].Height / 2);

            foreach (var item in _courtObjects)
                _rCourt.Children.Add(item);
        }
    }
}
