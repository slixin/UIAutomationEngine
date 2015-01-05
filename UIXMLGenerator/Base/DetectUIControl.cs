using HP.LR.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace UIAutoXMLBuilder.Base
{
    public class DetectUIControl : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private bool _isexport;
        private bool _isselected;
        private bool _isexpanded;
        private object _element;
        private Dictionary<string, object> _nativeProperties;
        private ObservableCollectionEx<DetectUIControl> _childrenControls;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public bool IsExport
        {
            get
            {
                return _isexport;
            }
            set
            {
                if (value != _isexport)
                {
                    _isexport = value;
                    OnPropertyChanged("IsExport");
                }
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isselected;
            }
            set
            {
                if (value != _isselected)
                {
                    _isselected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }
        public bool IsExpanded
        {
            get
            {
                return _isexpanded;
            }
            set
            {
                if (value != _isexpanded)
                {
                    _isexpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }
        public object Element
        {
            get
            {
                return _element;
            }
            set
            {
                if (value != _element)
                {
                    _element = value;
                    OnPropertyChanged("Element");
                }
            }
        }
        public ObservableCollectionEx<DetectUIControl> ChildControls
        {
            get
            {
                return _childrenControls;
            }
            set
            {
                if (value != _childrenControls)
                {
                    _childrenControls = value;
                    OnPropertyChanged("ChildControls");
                }
            }
        }
        public Dictionary<string, object> NativeProperties
        {
            get
            {
                return _nativeProperties;
            }
            set
            {
                if (value != _nativeProperties)
                {
                    _nativeProperties = value;
                    OnPropertyChanged("NativeProperties");
                }
            }
        }

        #region Public methods
        public void UpdateChildrenControls()
        {
            if (ChildControls.Count == 0)
            {
                var worker = new BackgroundWorker();

                worker.DoWork += (s, e) =>
                {
                    foreach (object obj in Capture.GetChildrenControls(_element))
                    {
                        var control = Capture.GetDetectUIControl(obj);
                        Application.Current.Dispatcher.BeginInvoke((Action)delegate()
                        {
                            _childrenControls.Add(control);
                        });
                    }
                };

                worker.RunWorkerCompleted += (s, e) =>
                {

                };

                worker.RunWorkerAsync();
            }

        }
        #endregion

        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
