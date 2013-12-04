using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MexicanTennisSimulator.Classes
{
    sealed class Cross : Shape
    {
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Double), typeof(Cross));
        public double Size
        {
            get { return (double)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                double rSize = this.Size / 2;
                Point pNull = new Point(0.0, 0.0);
                Point pRightBottom = new Point(rSize, rSize);
                Point pRightUpper = new Point(rSize, -rSize);
                Point pLeftBottom = new Point(-rSize, rSize);
                Point pLeftUpper = new Point(-rSize, -rSize);

                List<PathSegment> segments = new List<PathSegment>(6);
                segments.Add(new LineSegment(pNull, true));
                segments.Add(new LineSegment(pLeftUpper, true));
                segments.Add(new LineSegment(pRightBottom, true));
                segments.Add(new LineSegment(pNull, true));
                segments.Add(new LineSegment(pLeftBottom, true));
                segments.Add(new LineSegment(pRightUpper, true));

                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(pNull, segments, true);
                figures.Add(pf);

                Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);

                return g;
            }
        }
    }
}
