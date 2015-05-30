using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

using Microsoft.Research.DynamicDataDisplay; // Core functionality
using Microsoft.Research.DynamicDataDisplay.DataSources; // EnumerableDataSource
using Microsoft.Research.DynamicDataDisplay.PointMarkers; // CirclePointMarker

/* NOT USED IN THIS VERSION */

namespace Playerlog.Utils
{
    class PointMarkerWithText : ShapeElementPointMarker
    {
        private double markerSize = 10;
        private Brush fillColor = Brushes.Black;

        public override System.Windows.UIElement CreateMarker()
        {
            StackPanel result = new StackPanel() { Background = Brushes.Transparent };

            Ellipse el = new Ellipse() { Width = markerSize, Height = markerSize, Fill = fillColor };

            result.Children.Add(el);

            if (String.IsNullOrEmpty(ToolTipText) == false)
            {
                TextBlock txtBlock = new TextBlock() { Text = ToolTipText, Margin = new Thickness(0, 6, 0, 0) };
                result.Children.Add(txtBlock);
            }

            return result;
        }

        public override void SetPosition(System.Windows.UIElement marker, System.Windows.Point screenPoint)
        {
            Canvas.SetLeft(marker, screenPoint.X - markerSize / 2);
            Canvas.SetTop(marker, screenPoint.Y - markerSize / 2);
        }

        public static PointMarkerWithText Create(string text, double size, Brush fill)
        { return new PointMarkerWithText() { ToolTipText = text, markerSize = size, fillColor = fill }; }
    }

    class PointMarkerWithTooltip : ShapeElementPointMarker
    {
        public override UIElement CreateMarker()
        {
            Ellipse result = new Ellipse();
            result.Width = Size;
            result.Height = Size;
            //result.Stroke = Brush;
            result.Fill = Fill;
            if (!String.IsNullOrEmpty(ToolTipText))
            {
                ToolTip tt = new ToolTip();
                tt.Content = ToolTipText;
                result.ToolTip = tt;
            }
            return result;
        }

        public override void SetPosition(UIElement marker, Point screenPoint)
        {
            Canvas.SetLeft(marker, screenPoint.X - Size / 2);
            Canvas.SetTop(marker, screenPoint.Y - Size / 2);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        protected override bool ShouldSerializeProperty(DependencyProperty dp)
        {
            return base.ShouldSerializeProperty(dp);
        }
    }
}
