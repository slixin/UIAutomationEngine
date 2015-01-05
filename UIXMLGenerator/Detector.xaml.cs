using AutomationHelper;
using HP.LR.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using UIAutoXMLBuilder.Base;
using UIAutoXMLBuilder.ViewModelClass;
using UIEngine;

namespace UIAutoXMLBuilder
{
    /// <summary>
    /// Interaction logic for Detector.xaml
    /// </summary>
    public partial class Detector : Window
    {
        public Detector()
        {
            InitializeComponent();
        }

        private void tvDetectControl_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DetectUIControl selectedUIControl = tvDetectControl.SelectedItem as DetectUIControl;
            e.Handled = true;

            if (selectedUIControl != null)
            {
                gridControl.ItemsSource = selectedUIControl.NativeProperties;
                selectedUIControl.UpdateChildrenControls();
                Capture.Highlight(selectedUIControl.Element);
            }
        }

        private void gridControl_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 3);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            UnHook();
        }
    }
}
