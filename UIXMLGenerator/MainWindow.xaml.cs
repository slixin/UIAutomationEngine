using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Xml;
using UIEngine;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using AutomationHelper;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using HP.LR.Test;
using System.Windows.Media.Animation;
using UIAutoXMLBuilder.ViewModelClass;

namespace UIAutoXMLBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string TEXT_TITLE_DEFAULT = "Polaris - UI Automation XML Builder"; 
        private string _currentFile;
        private object _currentDetectedControl;
        private UILocate _uiLocator;
        
        private ControlTreeViewModel _controlTreeViewModel;
        private DetectorViewModel _detectorViewModel;
        private UIAutomationEngine _uiAutoEngine;        


        public MainWindow()
        {
            InitializeComponent();

            _detectorViewModel = new DetectorViewModel();
            InitialDetectorViewModel();            

            _controlTreeViewModel = new ControlTreeViewModel { 
                Title =  TEXT_TITLE_DEFAULT, 
                IsDetecting = false, 
                IsModified = false, 
                IsReadyForCapture = false, 
                SelectedTab = 0, 
                NativePropertiesVisibility = System.Windows.Visibility.Collapsed, 
            };
            this.DataContext = _controlTreeViewModel;


            _uiLocator = new UILocate();
            _uiAutoEngine = new UIAutomationEngine();
        }
        private void InitialDetectorViewModel()
        {
            _detectorViewModel.Initial();            
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (popupInsert.IsOpen)
            {
                popupInsert.IsOpen = false;
                popupInsert.Closed -= (senderClosed, eClosed) => { btnInsert.IsChecked = false; };
            }
            else
            {
                popupInsert.IsOpen = true;
                popupInsert.Closed += (senderClosed, eClosed) => { btnInsert.IsChecked = false; };
            }
        }

        private void popup_click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Primitives.Popup popup = sender as System.Windows.Controls.Primitives.Popup;
            if (popup != null)
                popup.IsOpen = false;
        }

        

        #region Private methods
        private void Save(string filename)
        {
            string filetosave = filename;
            bool isPrefixParentId = false;

            try
            {
                if (_controlTreeViewModel.UIControlCollection.Count > 0)
                {
                    if (string.IsNullOrEmpty(filetosave))
                    {
                        if (Common.SaveFileDialog(out filetosave, "XML", "UIControls.xml"))
                        {
                            _currentFile = filetosave;
                            MessageBoxResult mbr = System.Windows.MessageBox.Show("Are you going to add parent Id as prefix on each control Id?", "Save file", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                            if (mbr == MessageBoxResult.Yes)
                                isPrefixParentId = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(filetosave))
                    {
                        if (_uiAutoEngine.Controls == null)
                        {
                            _uiAutoEngine.Controls = new UIControls();
                            _uiAutoEngine.Controls.UIControlCollection = new ObservableCollectionEx<UIControl>();
                            _uiAutoEngine.Controls.UIControlDictionary = new Dictionary<string, UIControl>();
                        }
                        _uiAutoEngine.Controls.UIControlCollection = _controlTreeViewModel.XMLControlCollection;
                        if (_uiAutoEngine.Controls.Save(filetosave, isPrefixParentId))
                        {
                            _controlTreeViewModel.Title = string.Format("{0} - {1}", TEXT_TITLE_DEFAULT, _currentFile);
                            _controlTreeViewModel.IsModified = false;
                            _uiAutoEngine.Controls.UIControlCollection = new ObservableCollectionEx<UIControl>();
                            _uiAutoEngine.Controls.UIControlDictionary = new Dictionary<string, UIControl>();
                            _uiAutoEngine.Controls.Load(_currentFile);
                        }
                    }
                } 
            }
            catch(Exception ex)
            {
                OpenMessagePopup(string.Format("Save failed, {0}", ex.Message), Brushes.Red);
            }
                       
        }
        private void Open()
        {
            string[] filenames = null;

            if (Common.OpenFileDialog(out filenames, false, "XML"))
            {
                if (filenames.Length > 0)
                {
                    try
                    {
                        _currentFile = filenames[0];
                        _uiAutoEngine = new UIAutomationEngine(_currentFile);
                        _controlTreeViewModel.XMLControlCollection = _uiAutoEngine.Controls.UIControlCollection;
                        _controlTreeViewModel.XMLControlCollection.ItemPropertyChanged += _controlTreeViewModel.PropertyChangedHandler;
                        _controlTreeViewModel.Title = string.Format("{0} - {1}", TEXT_TITLE_DEFAULT, _currentFile);
                    }
                    catch
                    {
                        OpenMessagePopup(string.Format("Load failed, Please check the XML file."), Brushes.Red);
                        return;
                    }
                }
            }
        }
        #endregion

        #region Control Events
        private void tvXMLControl_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UIControl selectedXMLControl = tvXMLControl.SelectedItem as UIControl;
            e.Handled = true;

            if (selectedXMLControl != null)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(
                 delegate
                 {
                     if (!selectedXMLControl.IsSelected)
                         selectedXMLControl.IsSelected = true;
                     _controlTreeViewModel.SelectedXMLControl = selectedXMLControl;
                     _controlTreeViewModel.SelectedXMLControl.ChildControls.ItemPropertyChanged += _controlTreeViewModel.PropertyChangedHandler;
                     gridControl.ItemsSource = selectedXMLControl.NativeProperties;
                     controlDefinition.Control = selectedXMLControl;
                     if (selectedXMLControl.NativeProperties != null)
                         _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Visible;
                     else
                         _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;

                 }));
            }
            else
            {
                _controlTreeViewModel.SelectedXMLControl = null;
                gridControl.ItemsSource = null;
                controlDefinition.Control = null;
                _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void gridControl_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 3);
        }
        #endregion
        #region Execute Methods
        private void OpenExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Open();   
        }

        private void SaveExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Save(_currentFile);
        }

        private void SaveAsExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Save(null);
        }

        private void CloseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (_controlTreeViewModel.XMLControlCollection != null)
            {
                if (_controlTreeViewModel.XMLControlCollection.Count > 0 && _controlTreeViewModel.IsModified)
                {
                    MessageBoxResult msgResult = System.Windows.MessageBox.Show("Do you want to save file before close it?", "Close file", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                    if (msgResult == MessageBoxResult.Yes)
                    {
                        Save(_currentFile);
                    }
                }
                _controlTreeViewModel.XMLControlCollection.ItemPropertyChanged -= _controlTreeViewModel.PropertyChangedHandler;
                _controlTreeViewModel.XMLControlCollection = null;
                _controlTreeViewModel.IsModified = false;
                _currentFile = null;
                _controlTreeViewModel.Title = TEXT_TITLE_DEFAULT;
            }
        }

        private void AddControlToDictionary(ObservableCollectionEx<UIControl> controls, UIControl parent)
        {
            foreach (UIControl control in controls)
            {
                control.Parent = parent;
                _uiAutoEngine.Controls.UIControlDictionary.Add(control.Id, control);
                if (control.ChildControls.Count > 0)
                    AddControlToDictionary(control.ChildControls, control);
            }
        }

        private void TranslateControlCollectionToDictionary()
        {
            try
            {
                _uiAutoEngine.Controls.UIControlCollection = _controlTreeViewModel.XMLControlCollection;
                if (_uiAutoEngine.Controls.UIControlCollection != null)
                {
                    _uiAutoEngine.Controls.UIControlDictionary = new Dictionary<string, UIControl>();
                    AddControlToDictionary(_uiAutoEngine.Controls.UIControlCollection, null);
                }
            }
            catch(Exception ex)
            {
                OpenMessagePopup(ex.Message, Brushes.Red);
                //System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void AsyncLocateControl(UIControl control)
        {
            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                try
                {
                    _controlTreeViewModel.LocatedControl = _uiAutoEngine.Find(control.Id, 10);
                }
                catch (Exception ex)
                {
                    _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;
                    OpenMessagePopup(ex.Message, Brushes.Red);
                    //System.Windows.MessageBox.Show(ex.Message, "Locate Control", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                if (_controlTreeViewModel.LocatedControl == null)
                {
                    _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;
                    OpenMessagePopup("Cannot locate the control, please verify the control definition.", Brushes.Red);
                    //System.Windows.MessageBox.Show("Cannot locate the control, please verify the control definition.", "Locate Control", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _controlTreeViewModel.SelectedXMLControl.NativeProperties = Capture.GetNativeProperties(_controlTreeViewModel.LocatedControl.Element);
                    _controlTreeViewModel.SelectedXMLControl.Snapshot = Capture.GetSnapshotString(_controlTreeViewModel.LocatedControl.Element);

                    gridControl.ItemsSource = _controlTreeViewModel.SelectedXMLControl.NativeProperties;
                    _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Visible;
                    OpenMessagePopup("Control is found and highlighted.", Brushes.LightGreen);
                    //System.Windows.MessageBox.Show("Control is found and highlighted.", "Locate Control", MessageBoxButton.OK, MessageBoxImage.Information);
                    _uiLocator.BaseControl = _controlTreeViewModel.LocatedControl.Element;
                    _uiLocator.HighlightElement();
                    _controlTreeViewModel.LocatedControl = null;
                }
                Dispatcher.BeginInvoke((Action)delegate() { _controlTreeViewModel.SelectedXMLControl.IsLocating = false; });
            };

            worker.RunWorkerAsync();
        }

        private void LocateExecute(object sender, ExecutedRoutedEventArgs e)
        {
            UIControl selectedXMLControl = tvXMLControl.SelectedItem as UIControl;

            if (selectedXMLControl != null)
            {
                try
                {
                    _controlTreeViewModel.SelectedXMLControl = selectedXMLControl;
                    selectedXMLControl.IsLocating = true;
                    AsyncLocateControl(selectedXMLControl);
                }
                catch (Exception ex)
                {
                    _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;
                    OpenMessagePopup(ex.Message, Brushes.Red);
                    //System.Windows.MessageBox.Show(ex.Message, "Locate Control", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void OpenDetector()
        {
            Detector detectorWin = new Detector();
            detectorWin.DataContext = _detectorViewModel;
            detectorWin.Show();
        }

        private void DetectExecute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenDetector();

            //_controlTreeViewModel.IsDetecting = true;

            //OpenMessagePopup(TEXT_START_DETECT, Brushes.LightGray);
            //_keyboardHook.Start();
            //_mouseHook.Start();
            //tabDetectControl.IsSelected = true;
            //tabXMLControl.IsEnabled = false;
            return;
        }


        private void OperateExecute(object sender, ExecutedRoutedEventArgs e)
        {
            return;
        }

        private void ExitExecute(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(0);
        }
        #endregion

        #region CanExecute methods
        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_controlTreeViewModel != null)
            {
                if (_controlTreeViewModel.XMLControlCollection == null)
                    e.CanExecute = false;
                else
                {
                    if (_controlTreeViewModel.XMLControlCollection.Count == 0)
                        e.CanExecute = false;
                    else
                        if (!_controlTreeViewModel.IsDetecting)
                            if (_controlTreeViewModel.IsModified)
                                e.CanExecute = true;
                            else
                                e.CanExecute = false;
                        else
                            e.CanExecute = false;
                }
            }
            else
                e.CanExecute = false;
        }

        private void OpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_controlTreeViewModel != null)
            {
                if (_controlTreeViewModel.XMLControlCollection == null)
                    if (!_controlTreeViewModel.IsDetecting)
                        e.CanExecute = true;
                    else
                        e.CanExecute = false;
                else
                    if (_controlTreeViewModel.XMLControlCollection.Count == 0)
                        e.CanExecute = true;
                    else
                        e.CanExecute = false;
            }
            else
                e.CanExecute = false;
        }

        private void SaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_controlTreeViewModel != null)
            {
                if (_controlTreeViewModel.XMLControlCollection == null)
                    e.CanExecute = false;
                else
                {
                    if (_controlTreeViewModel.XMLControlCollection.Count == 0)
                        e.CanExecute = false;
                    else
                        if (!_controlTreeViewModel.IsDetecting)
                            e.CanExecute = true;
                        else
                            e.CanExecute = false;
                }
            }
            else
                e.CanExecute = false;
        }

        private void CloseCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_controlTreeViewModel != null)
            {
                if (_controlTreeViewModel.XMLControlCollection == null)
                    e.CanExecute = false;
                else
                    if (_controlTreeViewModel.XMLControlCollection.Count == 0)
                        e.CanExecute = false;
                    else
                        if (!_controlTreeViewModel.IsDetecting)
                            e.CanExecute = true;
                        else
                            e.CanExecute = false;
            }
            else
                e.CanExecute = false;
        }

        private void LocateCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_controlTreeViewModel != null)
            {
                if (_controlTreeViewModel.XMLControlCollection == null)
                    e.CanExecute = false;
                else
                    if (!_controlTreeViewModel.IsDetecting)
                        if (tabControl.SelectedIndex == 0)
                            if (_controlTreeViewModel.SelectedXMLControl != null)
                                if (_controlTreeViewModel.SelectedXMLControl.IsLocating)
                                    e.CanExecute = false;
                                else
                                    e.CanExecute = true;
                            else
                                e.CanExecute = false;
                        else
                            e.CanExecute = false;
                    else
                        e.CanExecute = false;
            }
            else
                e.CanExecute = false;            
        }

        private void DetectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_controlTreeViewModel != null)
            {
                if (!_controlTreeViewModel.IsDetecting)
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
        }


        private void OperateCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_controlTreeViewModel != null)
            {
                if (!_controlTreeViewModel.IsDetecting)
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
        }
        #endregion

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {                
                controlDefinition.Control = _controlTreeViewModel.SelectedXMLControl == null ? null : _controlTreeViewModel.SelectedXMLControl;
                if (_controlTreeViewModel.SelectedXMLControl != null)
                {
                    if (_controlTreeViewModel.SelectedXMLControl.NativeProperties != null)
                    {
                        gridControl.ItemsSource = _controlTreeViewModel.SelectedXMLControl.NativeProperties;
                        _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Visible;
                    }
                    else
                        _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                controlDefinition.Control = _controlTreeViewModel.SelectedUIControl == null ? null : _controlTreeViewModel.SelectedUIControl;
                gridControl.ItemsSource = _controlTreeViewModel.SelectedUIControl == null ? null : _controlTreeViewModel.SelectedUIControl.NativeProperties;
                if (_controlTreeViewModel.UIControlCollection != null)
                {
                    if (_controlTreeViewModel.SelectedUIControl != null)
                    {
                        gridControl.ItemsSource = _controlTreeViewModel.SelectedUIControl.NativeProperties;
                        _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Visible;
                    }
                    else
                        _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    _controlTreeViewModel.NativePropertiesVisibility = System.Windows.Visibility.Collapsed;
                }   
            }
        }
    }

    public class IngStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object result = null;

            if ((bool)value)
            {
                System.Windows.Controls.Control ctrl = new System.Windows.Controls.Control();
                ctrl.Template = System.Windows.Application.Current.MainWindow.FindResource("loadingAnimation") as ControlTemplate;
                ctrl.Visibility = Visibility.Visible;
                ctrl.Width = 16;
                ctrl.Height = 16;
                result = ctrl;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InvalidElementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush forecolor;

            if ((bool)value)
                forecolor = Brushes.Red;
            else
                forecolor = Brushes.Black;

            return forecolor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
