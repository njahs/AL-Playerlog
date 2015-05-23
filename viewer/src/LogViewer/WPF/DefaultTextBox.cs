using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace LogViewer.WPF
{
    public class DefaultTextBox : TextBox
    {
        static DefaultTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DefaultTextBox), new FrameworkPropertyMetadata(typeof(DefaultTextBox)));
        }

        public string UserText
        {
            get { return (string)GetValue(UserTextProperty); }
            set { SetValue(UserTextProperty, value); }
        }

        public static readonly DependencyProperty UserTextProperty =
            DependencyProperty.Register("UserText", typeof(string), typeof(DefaultTextBox),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));

        public Thickness MarginEx
        {
            get { return (Thickness)GetValue(MarginExProperty); }
            set { SetValue(MarginExProperty, value); }
        }

        public static readonly DependencyProperty MarginExProperty =
            DependencyProperty.Register("MarginEx", typeof(Thickness), typeof(DefaultTextBox),
            new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 0), FrameworkPropertyMetadataOptions.AffectsRender));
    }
}
