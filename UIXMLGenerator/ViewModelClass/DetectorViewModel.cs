using AutomationHelper;
using HP.LR.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using UIAutoXMLBuilder.Base;

namespace UIAutoXMLBuilder.ViewModelClass
{
    public class DetectorViewModel : ViewModelBase
    {
        #region Properties        
        public ObservableCollectionEx<DetectUIControl> UIControlCollection
        {
            get
            {
                return _uicontrolCollection;
            }
            set
            {
                if (value != _uicontrolCollection)
                {
                    _uicontrolCollection = value;
                    OnPropertyChanged("UIControlCollection");
                }
            }
        }        
        public DetectUIControl SelectedUIControl
        {
            get
            {
                return _selectedUIControl;
            }
            set
            {
                if (value != _selectedUIControl)
                {
                    _selectedUIControl = value;
                    OnPropertyChanged("SelectedUIControl");
                }
            }
        }
        public bool IsReadyForCapture { get; set; }
        public bool IsDetecting { get; set; }
        #endregion

        #region Private members
        private ObservableCollectionEx<DetectUIControl> _uicontrolCollection;
        private DetectUIControl _selectedUIControl;
        private MouseHook _mouseHook = new MouseHook();
        private KeyboardHook _keyboardHook = new KeyboardHook();

        private const string TEXT_START_DETECT = "To capture a control, press and hold RIGHT CTRL key or press ESC to quit detect mode.";
        private const string TEXT_START_INDETECTING = "Release RIGHT CTRL key when the mouse move on the control.";

        private System.Windows.Point _detectMousePoint;
        private DispatcherTimer _detectTimer;
        private Popup _popupMessage;
        
        private RelayCommand<object> detectCommand;
        private RelayCommand<object> exportCommand;
        private RelayCommand<object> insertBeforeCommand;
        private RelayCommand<object> insertAfterCommand;
        private RelayCommand<object> insertIntoCommand;
        private RelayCommand<object> replaceCommand;
        #endregion

        #region Constructor
        public DetectorViewModel()
        {
            _uicontrolCollection = new ObservableCollectionEx<DetectUIControl>();
        }
        #endregion

        #region Commands
        public System.Windows.Input.ICommand DetectCommand
        {
            get
            {
                if (detectCommand == null)
                {
                    detectCommand = new RelayCommand<object>(x => this.Detect(), x => this.CanDetect());
                }
                return detectCommand;
            }
        }

        public System.Windows.Input.ICommand ExportCommand
        {
            get
            {
                if (exportCommand == null)
                {
                    exportCommand = new RelayCommand<object>(x => this.Export(), x => this.CanExport());
                }
                return exportCommand;
            }
        }

        public System.Windows.Input.ICommand InsertIntoCommand
        {
            get
            {
                if (insertIntoCommand == null)
                {
                    insertIntoCommand = new RelayCommand<object>(x => this.InsertInto(), x => this.CanInsertInto());
                }
                return insertIntoCommand;
            }
        }

        public System.Windows.Input.ICommand InsertBeforeCommand
        {
            get
            {
                if (insertBeforeCommand == null)
                {
                    insertBeforeCommand = new RelayCommand<object>(x => this.InsertBefore(), x => this.CanInsertBefore());
                }
                return insertBeforeCommand;
            }
        }

        public System.Windows.Input.ICommand InsertAfterCommand
        {
            get
            {
                if (insertAfterCommand == null)
                {
                    insertAfterCommand = new RelayCommand<object>(x => this.InsertAfter(), x => this.CanInsertAfter());
                }
                return insertAfterCommand;
            }
        }

        public System.Windows.Input.ICommand ReplaceCommand
        {
            get
            {
                if (replaceCommand == null)
                {
                    replaceCommand = new RelayCommand<object>(x => this.Replace(), x => this.CanReplace());
                }
                return replaceCommand;
            }
        }
        #endregion

        #region Private methods
        //private ObservableCollectionEx<UIControl> FindParentControl(ObservableCollectionEx<UIControl> parent, UIControl target)
        //{
        //    ObservableCollectionEx<UIControl> result = new ObservableCollectionEx<UIControl>();
        //    result.ItemPropertyChanged += PropertyChangedHandler;
        //    if (parent == null)
        //        return null;
        //    if (parent.Count == 0)
        //        return null;

        //    if (parent.Contains(target))
        //        return parent;
        //    else
        //    {
        //        foreach (UIControl control in parent)
        //        {
        //            result = FindParentControl(control.ChildControls, target);
        //            if (result != null)
        //            {
        //                if (result.Count > 0)
        //                    break;
        //            }
        //        }
        //        return result;
        //    }
        //}
        //private void AddExportControl(ObservableCollectionEx<UIControl> xmlControls, ObservableCollectionEx<UIControl> uiControls)
        //{
        //    foreach (UIControl uicontrol in uiControls)
        //    {
        //        if (uicontrol.IsExport)
        //        {
        //            UIControl exportControl = uicontrol.Clone() as UIControl;
        //            exportControl.ChildControls = new ObservableCollectionEx<UIControl>();
        //            exportControl.ChildControls.ItemPropertyChanged += PropertyChangedHandler;
        //            if (uicontrol.ChildControls.Count > 0)
        //            {
        //                AddExportControl(exportControl.ChildControls, uicontrol.ChildControls);
        //            }
        //            uicontrol.IsExport = false;
        //            xmlControls.Add(exportControl);                    
        //        }
        //    }
        //}
        
        #endregion

        public void Initial()
        {
            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                DetectUIControl control = Capture.GetDetectUIControl(AutomationElement.RootElement);
                _uicontrolCollection.Add(control);
                foreach (object obj in Capture.GetChildrenControls(AutomationElement.RootElement))
                {
                    control.ChildControls.Add(Capture.GetDetectUIControl(obj));
                }
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                
            };

            worker.RunWorkerAsync();
        }

        #region Execute Methods
        public void Detect()
        {
            Hook();

            _detectTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _detectTimer.Tick += timer_Tick;
            _popupMessage = new Popup();
        }
        public void Replace()
        {
            //MessageBoxResult msgResult = MessageBox.Show(string.Format("Replace Control '{0}' with new Control '{1}'. Are you sure?", SelectedXMLControl.Id, SelectedUIControl.Id), "Replace Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            //if (msgResult == MessageBoxResult.Yes)
            //{
            //    SelectedXMLControl.Id = SelectedUIControl.Id;
            //    SelectedXMLControl.Index = SelectedUIControl.Index;
            //    SelectedXMLControl.Element = SelectedUIControl.Element;
            //    SelectedXMLControl.Parent = SelectedUIControl.Parent;
            //    SelectedXMLControl.Properties = SelectedUIControl.Properties;
            //    SelectedXMLControl.Scope = SelectedUIControl.Scope;
            //    SelectedXMLControl.Snapshot = SelectedUIControl.Snapshot;
            //    SelectedXMLControl.SupportedPattern = SelectedUIControl.SupportedPattern;
            //    SelectedXMLControl.ControlType = SelectedUIControl.ControlType;
            //    SelectedXMLControl.Framework = SelectedUIControl.Framework;
            //    IsModified = true;
            //}
            //else
            //{
            //    SelectedTab = 1;
            //}
        }
        
        public void Export()
        {
            //MessageBoxResult msgResult = MessageBox.Show("Bulk add selected controls?", "Bulk Add Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            //if (msgResult == MessageBoxResult.Yes)
            //{
            //    if (XMLControlCollection == null)
            //    {
            //        XMLControlCollection = new ObservableCollectionEx<UIControl>();
            //        XMLControlCollection.ItemPropertyChanged += PropertyChangedHandler;
            //    }                    

            //    AddExportControl(XMLControlCollection, UIControlCollection);
            //    SelectedTab = 0;
            //}
        }
        public void InsertInto()
        {
            //SelectedTab = 0;
            //MessageBoxResult msgResult = MessageBox.Show(string.Format("Insert Control '{0}' into Control '{1}'?", SelectedUIControl.Id, SelectedXMLControl.Id), "Insert Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            //if (msgResult == MessageBoxResult.Yes)
            //{
            //    SelectedXMLControl.ChildControls.ItemPropertyChanged += PropertyChangedHandler;
            //    SelectedXMLControl.ChildControls.Add(SelectedUIControl.Clone() as UIControl);
            //}
            //else
            //{
            //    SelectedTab = 1;
            //}
        }
        public void InsertBefore()
        {
            //SelectedTab = 0;
            //MessageBoxResult msgResult = MessageBox.Show(string.Format("Insert Control '{0}' before Control '{1}'?", SelectedUIControl.Id, SelectedXMLControl.Id), "Insert Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            //if (msgResult == MessageBoxResult.Yes)
            //{
            //    FindParentControl(XMLControlCollection, SelectedXMLControl).Insert(0, SelectedUIControl.Clone() as UIControl);
            //}
            //else
            //{
            //    SelectedTab = 1;
            //}
        }
        public void InsertAfter()
        {
            //SelectedTab = 0;
            //MessageBoxResult msgResult = MessageBox.Show(string.Format("Insert Control '{0}' after Control '{1}'?", SelectedUIControl.Id, SelectedXMLControl.Id), "Insert Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            //if (msgResult == MessageBoxResult.Yes)
            //{
            //    FindParentControl(XMLControlCollection, SelectedXMLControl).Add(SelectedUIControl.Clone() as UIControl);
            //}
            //else
            //{
            //    SelectedTab = 1;
            //}
        }
        #endregion

        #region CanExecute Methods
        public bool CanDetect()
        {
            return true;
        }        

        public bool CanExport()
        {
            //if (XMLControlCollection != null)
            //{
            //    if (XMLControlCollection.Count == 0)
            //        return true;
            //    else
            //        return false;
            //}

            //if (UIControlCollection != null)
            //{
            //    if (UIControlCollection.Where(o => o.IsExport).Count() > 0)
            //        if (!IsDetecting)
            //            if (SelectedTab == 1)
            //                return true;
            //            else
            //                return false;
            //        else
            //            return false;
            //    else
            //        return false;
            //}
            //else
            //    return false;

            return true;
        }

        public bool CanInsertInto()
        {
            //if (_selectedUIControl != null & _selectedXMLControl != null)
            //    if (!IsDetecting)
            //        if (SelectedTab == 1)
            //            return true;
            //        else
            //            return false;
            //    else
            //        return false;
            //else
            //    return false;
            return true;
        }

        public bool CanInsertBefore()
        {
            //if (_selectedUIControl != null & _selectedXMLControl != null)
            //    if (!IsDetecting)
            //        if (SelectedTab == 1)
            //            return true;
            //        else
            //            return false;
            //    else
            //        return false;
            //else
            //    return false;
            return true;
        }

        public bool CanInsertAfter()
        {
            //if (_selectedUIControl != null & _selectedXMLControl != null)
            //    if (!IsDetecting)
            //        if (SelectedTab == 1)
            //            return true;
            //        else
            //            return false;
            //    else
            //        return false;
            //else
            //    return false;

            return true;
        }


        public bool CanReplace()
        {
            //if (_selectedUIControl != null & _selectedXMLControl != null)
            //    if (!IsDetecting)
            //        if (SelectedTab == 1)
            //            return true;
            //        else
            //            return false;
            //    else
            //        return false;
            //else
            //    return false;

            return true;
        }
        #endregion        


        #region Private methods
        #region Hook
        private void Hook()
        {
            _mouseHook.MouseMove += new System.Windows.Forms.MouseEventHandler(mouseHook_MouseMove);
            _mouseHook.MouseDown += new System.Windows.Forms.MouseEventHandler(mouseHook_MouseDown);
            _mouseHook.MouseUp += new System.Windows.Forms.MouseEventHandler(mouseHook_MouseUp);
            _mouseHook.MouseWheel += new System.Windows.Forms.MouseEventHandler(mouseHook_MouseWheel);

            _keyboardHook.KeyDown += new System.Windows.Forms.KeyEventHandler(keyboardHook_KeyDown);
            _keyboardHook.KeyUp += new System.Windows.Forms.KeyEventHandler(keyboardHook_KeyUp);
            _keyboardHook.KeyPress += new System.Windows.Forms.KeyPressEventHandler(keyboardHook_KeyPress);
        }
        private void UnHook()
        {
            _mouseHook.MouseMove -= new System.Windows.Forms.MouseEventHandler(mouseHook_MouseMove);
            _mouseHook.MouseDown -= new System.Windows.Forms.MouseEventHandler(mouseHook_MouseDown);
            _mouseHook.MouseUp -= new System.Windows.Forms.MouseEventHandler(mouseHook_MouseUp);
            _mouseHook.MouseWheel -= new System.Windows.Forms.MouseEventHandler(mouseHook_MouseWheel);

            _keyboardHook.KeyDown -= new System.Windows.Forms.KeyEventHandler(keyboardHook_KeyDown);
            _keyboardHook.KeyUp -= new System.Windows.Forms.KeyEventHandler(keyboardHook_KeyUp);
            _keyboardHook.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(keyboardHook_KeyPress);
        }

        #region Mouse Hook
        private void mouseHook_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void mouseHook_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (IsReadyForCapture)
            {
                _detectTimer.Stop();
                _detectTimer.Start();
            }
        }

        private void mouseHook_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void mouseHook_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }
        #endregion

        #region Keyboard Hook
        private void keyboardHook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue == 162 && IsDetecting)
            {
                IsReadyForCapture = true;
            }
        }

        private void keyboardHook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue == 162 && _detectorVM.IsDetecting)
            {
                var worker = new BackgroundWorker();

                worker.DoWork += (s, ev) =>
                {
                    _controlTreeViewModel.UIControlCollection = Capture.CaptureControls(_currentDetectedControl);
                    if (_controlTreeViewModel.UIControlCollection != null)
                    {
                        foreach (UIControl control in _controlTreeViewModel.UIControlCollection)
                        {
                            AsyncLoadUIElement(control);
                        }
                    }
                };

                worker.RunWorkerCompleted += (s, ev) =>
                {
                    Dispatcher.BeginInvoke((Action)delegate() { AfterCapture(); });
                };

                worker.RunWorkerAsync();
            }
        }

        private void keyboardHook_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                _controlTreeViewModel.IsDetecting = false;
                _controlTreeViewModel.IsReadyForCapture = false;
                _mouseHook.Stop();
                _keyboardHook.Stop();
                ClosePopupDetectDialog();
                tabXMLControl.IsEnabled = true;
            }
        }
        #endregion
        #endregion

        #region Timer
        private void timer_Tick(object sender, EventArgs e)
        {
            _detectTimer.Stop();
            _detectMousePoint = new System.Windows.Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
            _currentDetectedControl = _uiLocator.GetElementFromPoint(_detectMousePoint);
            HighlightControl(_currentDetectedControl);
        }
        #endregion

        private void AfterCapture()
        {
            _controlTreeViewModel.IsReadyForCapture = false;
            _controlTreeViewModel.IsDetecting = false;
            _mouseHook.Stop();
            _keyboardHook.Stop();
            ClosePopupDetectDialog();
            tabXMLControl.IsEnabled = true;
        }
        private void HighlightControl(object control)
        {
            if (control != null)
            {
                _uiLocator.BaseControl = control;
                _uiLocator.HighlightElement();
            }
        }

        private void OpenMessagePopup(string message, Brush brush)
        {
            ClosePopupDetectDialog();

            var border = new Border
            {
                CornerRadius = new CornerRadius(5),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Width = 450,
                Height = 40,
                Background = brush,
            };

            var textbox = new System.Windows.Controls.TextBox
            {
                Text = message,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Background = Brushes.Transparent,
                FontSize = 12,
                TextWrapping = new TextWrapping(),
                AcceptsReturn = true,
                BorderBrush = Brushes.Transparent,
            };

            border.Child = textbox;

            _popupMessage.Child = border;

            _popupMessage.Placement = PlacementMode.Center;
            _popupMessage.PlacementTarget = this;
            _popupMessage.VerticalOffset = this.Height / 2 - 60;
            //_popupMessage.HorizontalOffset = mainWin.Width / 2 + 200 ;
            _popupMessage.AllowsTransparency = true;
            //_popupMessage.StaysOpen = true;
            _popupMessage.PopupAnimation = PopupAnimation.Fade;
            _popupMessage.IsOpen = true;
        }
        private void ClosePopupDetectDialog()
        {
            _popupMessage.StaysOpen = false;
            _popupMessage.IsOpen = false;
        }
        #endregion
    }
}
