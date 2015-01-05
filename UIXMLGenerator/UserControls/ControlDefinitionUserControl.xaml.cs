using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutomationHelper;
using UIEngine;

namespace UIAutoXMLBuilder
{
    /// <summary>
    /// Interaction logic for ControlDefinitionUserControl.xaml
    /// </summary>
    public partial class ControlDefinitionUserControl : UserControl
    {
        public static readonly DependencyProperty ControlProperty =
            DependencyProperty.Register("Control", typeof(UIControl), typeof(ControlDefinitionUserControl), new PropertyMetadata(null));

        public UIControl Control
        {
            get { return (UIControl)GetValue(ControlProperty); }
            set { SetValue(ControlProperty, value); }
        }

        private Window parentWindow;

        public ControlDefinitionUserControl()
        {
            InitializeComponent();
            DataContext = this;

            cmbScope.ItemsSource = new List<string>()
            {
                "Descendants",
                "Children",
            };
            cmbScope.SelectedIndex = 0;

            Loaded += (sender, args) =>
            {
                parentWindow = Window.GetWindow(this);
            };
        }
    }

    public class SnapshotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            System.Drawing.Bitmap image = ImageHelper.Base64ToImage(value.ToString()) as System.Drawing.Bitmap;

            return ImageHelper.ToBitmapSource(image);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
