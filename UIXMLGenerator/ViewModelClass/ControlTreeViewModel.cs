using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Collections;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using System.Text.RegularExpressions;
using UIEngine;
using HP.LR.Test;
using System.Collections.Specialized;
using AutomationHelper;

namespace UIAutoXMLBuilder
{
    public class ControlTreeViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollectionEx<UIControl> XMLControlCollection
        {
            get
            {
                return _xmlcontrolCollection;
            }
            set
            {
                if (value != _xmlcontrolCollection)
                {
                    _xmlcontrolCollection = value;
                    OnPropertyChanged("XMLControlCollection");
                }
            }
        }
        public ObservableCollectionEx<UIControl> UIControlCollection
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
        public UIControl SelectedXMLControl
        {
            get
            {
                return _selectedXMLControl;
            }
            set
            {
                if (value != _selectedXMLControl)
                {
                    _selectedXMLControl = value;
                    OnPropertyChanged("SelectedXMLControl");
                }
            }
        }
        public UIControl SelectedUIControl
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
        public UIControl LocatedControl
        {
            get
            {
                return _locatedControl;
            }
            set
            {
                if (value != _locatedControl)
                {
                    _locatedControl = value;
                    OnPropertyChanged("LocatedControl");
                }
            }
        }
        public int SelectedTab
        {
            get
            {
                return _selectedTab;
            }
            set
            {
                if (value != _selectedTab)
                {
                    _selectedTab = value;
                    OnPropertyChanged("SelectedTab");
                }
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        public System.Windows.Visibility NativePropertiesVisibility
        {
            get
            {
                return _nativePropertiesVisibility;
            }
            set
            {
                if (value != _nativePropertiesVisibility)
                {
                    _nativePropertiesVisibility = value;
                    OnPropertyChanged("NativePropertiesVisibility");
                }
            }
        }
        public bool IsModified { get; set; }
        public bool IsReadyForCapture { get; set; }
        public bool IsDetecting { get; set; }
        #endregion

        #region Private members
        private ObservableCollectionEx<UIControl> _xmlcontrolCollection;
        private ObservableCollectionEx<UIControl> _uicontrolCollection;
        private UIControl _selectedXMLControl;
        private UIControl _selectedUIControl;
        private UIControl _locatedControl;
        private int _selectedTab;
        private string _title;
        private System.Windows.Visibility _nativePropertiesVisibility;

        private RelayCommand<object> addControlCommand;     
        private RelayCommand<object> deleteControlCommand;        
        private RelayCommand<object> exportControlsCommand;
        private RelayCommand<object> insertBeforeControlCommand;
        private RelayCommand<object> insertAfterControlCommand;
        private RelayCommand<object> insertIntoControlCommand;
        private RelayCommand<object> replaceControlCommand;
        #endregion

        #region Constructor
        public ControlTreeViewModel()
        {            
        }
        #endregion

        #region ChangedHandler
        public void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("id", StringComparison.InvariantCultureIgnoreCase) ||
                e.PropertyName.Equals("properties", StringComparison.InvariantCultureIgnoreCase) ||
                e.PropertyName.Equals("controltype", StringComparison.InvariantCultureIgnoreCase) ||
                e.PropertyName.Equals("index", StringComparison.InvariantCultureIgnoreCase) ||
                e.PropertyName.Equals("scope", StringComparison.InvariantCultureIgnoreCase) ||
                e.PropertyName.Equals("framework", StringComparison.InvariantCultureIgnoreCase) ||
                e.PropertyName.Equals("ChildControls", StringComparison.InvariantCultureIgnoreCase))
                IsModified = true;
        }
        
        #endregion

        #region Commands
        public System.Windows.Input.ICommand AddControlCommand
        {
            get
            {
                if (addControlCommand == null)
                {
                    addControlCommand = new RelayCommand<object>(x => this.AddControl(), x => this.CanAddControl());
                }
                return addControlCommand;
            }
        }

        public System.Windows.Input.ICommand DeleteControlCommand
        {
            get
            {
                if (deleteControlCommand == null)
                {
                    deleteControlCommand = new RelayCommand<object>(x => this.DeleteControl(), x => this.CanDeleteControl());
                }
                return deleteControlCommand;
            }
        }

        public System.Windows.Input.ICommand ExportControlsCommand
        {
            get
            {
                if (exportControlsCommand == null)
                {
                    exportControlsCommand = new RelayCommand<object>(x => this.ExportControls(), x => this.CanExportControls());
                }
                return exportControlsCommand;
            }
        }

        public System.Windows.Input.ICommand InsertIntoControlCommand
        {
            get
            {
                if (insertIntoControlCommand == null)
                {
                    insertIntoControlCommand = new RelayCommand<object>(x => this.InsertIntoControl(), x => this.CanInsertIntoControl());
                }
                return insertIntoControlCommand;
            }
        }

        public System.Windows.Input.ICommand InsertBeforeControlCommand
        {
            get
            {
                if (insertBeforeControlCommand == null)
                {
                    insertBeforeControlCommand = new RelayCommand<object>(x => this.InsertBeforeControl(), x => this.CanInsertBeforeControl());
                }
                return insertBeforeControlCommand;
            }
        }

        public System.Windows.Input.ICommand InsertAfterControlCommand
        {
            get
            {
                if (insertAfterControlCommand == null)
                {
                    insertAfterControlCommand = new RelayCommand<object>(x => this.InsertAfterControl(), x => this.CanInsertAfterControl());
                }
                return insertAfterControlCommand;
            }
        }

        public System.Windows.Input.ICommand ReplaceControlCommand
        {
            get
            {
                if (replaceControlCommand == null)
                {
                    replaceControlCommand = new RelayCommand<object>(x => this.ReplaceControl(), x => this.CanReplaceControl());
                }
                return replaceControlCommand;
            }
        }
        #endregion

        #region Private methods
        private UIControl ManualAddControl()
        {
            var control = new UIControl
                {
                    Id = string.Format("NEW_CONTROL_{0}", TextHelper.RandomString(5).ToUpper()), 
                    Scope = "Descendants", 
                    ChildControls = new ObservableCollectionEx<UIControl>(),
                };
            control.ChildControls.ItemPropertyChanged += PropertyChangedHandler;

            return control;
        }

        private ObservableCollectionEx<UIControl> FindParentControl(ObservableCollectionEx<UIControl> parent, UIControl target)
        {
            ObservableCollectionEx<UIControl> result = new ObservableCollectionEx<UIControl>();
            result.ItemPropertyChanged += PropertyChangedHandler;
            if (parent == null)
                return null;
            if (parent.Count == 0)
                return null;

            if (parent.Contains(target))
                return parent;
            else
            {
                foreach (UIControl control in parent)
                {
                    result = FindParentControl(control.ChildControls, target);
                    if (result != null)
                    {
                        if (result.Count > 0)
                            break;
                    }
                }
                return result;
            }
        }
        private void AddExportControl(ObservableCollectionEx<UIControl> xmlControls, ObservableCollectionEx<UIControl> uiControls)
        {
            foreach (UIControl uicontrol in uiControls)
            {
                if (uicontrol.IsExport)
                {
                    UIControl exportControl = uicontrol.Clone() as UIControl;
                    exportControl.ChildControls = new ObservableCollectionEx<UIControl>();
                    exportControl.ChildControls.ItemPropertyChanged += PropertyChangedHandler;
                    if (uicontrol.ChildControls.Count > 0)
                    {
                        AddExportControl(exportControl.ChildControls, uicontrol.ChildControls);
                    }
                    uicontrol.IsExport = false;
                    xmlControls.Add(exportControl);                    
                }
            }
        }
        #endregion

        #region Execute Methods
        public void AddControl()
        {
            if (XMLControlCollection == null)
            {
                XMLControlCollection = new ObservableCollectionEx<UIControl>();
                XMLControlCollection.ItemPropertyChanged += PropertyChangedHandler;
                SelectedXMLControl = ManualAddControl();
                XMLControlCollection.Add(SelectedXMLControl);
            }
            else if (XMLControlCollection.Count == 0)
            {
                SelectedXMLControl = ManualAddControl();
                XMLControlCollection.Add(SelectedXMLControl);
            }
            else if (SelectedXMLControl != null)
            {
                var nextSelect = ManualAddControl();
                SelectedXMLControl.ChildControls.Add(nextSelect);
            }                
        }
        public void DeleteControl()
        {
            MessageBoxResult msgResult = MessageBox.Show("Delete control will cause all children controls be erased. Are you sure?", "Delete Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (msgResult == MessageBoxResult.Yes)
            {
                FindParentControl(XMLControlCollection, SelectedXMLControl).Remove(SelectedXMLControl);
            }       
        }
        public void ReplaceControl()
        {
            SelectedTab = 0;
            MessageBoxResult msgResult = MessageBox.Show(string.Format("Replace Control '{0}' with new Control '{1}'. Are you sure?", SelectedXMLControl.Id, SelectedUIControl.Id), "Replace Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (msgResult == MessageBoxResult.Yes)
            {
                SelectedXMLControl.Id = SelectedUIControl.Id;
                SelectedXMLControl.Index = SelectedUIControl.Index;
                SelectedXMLControl.Element = SelectedUIControl.Element;
                SelectedXMLControl.Parent = SelectedUIControl.Parent;
                SelectedXMLControl.Properties = SelectedUIControl.Properties;
                SelectedXMLControl.Scope = SelectedUIControl.Scope;
                SelectedXMLControl.Snapshot = SelectedUIControl.Snapshot;
                SelectedXMLControl.SupportedPattern = SelectedUIControl.SupportedPattern;
                SelectedXMLControl.ControlType = SelectedUIControl.ControlType;
                SelectedXMLControl.Framework = SelectedUIControl.Framework;
                IsModified = true;
            }
            else
            {
                SelectedTab = 1;
            }
        }
        
        public void ExportControls()
        {
            MessageBoxResult msgResult = MessageBox.Show("Bulk add selected controls?", "Bulk Add Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (msgResult == MessageBoxResult.Yes)
            {
                if (XMLControlCollection == null)
                {
                    XMLControlCollection = new ObservableCollectionEx<UIControl>();
                    XMLControlCollection.ItemPropertyChanged += PropertyChangedHandler;
                }                    

                AddExportControl(XMLControlCollection, UIControlCollection);
                SelectedTab = 0;
            }
        }
        public void InsertIntoControl()
        {
            SelectedTab = 0;
            MessageBoxResult msgResult = MessageBox.Show(string.Format("Insert Control '{0}' into Control '{1}'?", SelectedUIControl.Id, SelectedXMLControl.Id), "Insert Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (msgResult == MessageBoxResult.Yes)
            {
                SelectedXMLControl.ChildControls.ItemPropertyChanged += PropertyChangedHandler;
                SelectedXMLControl.ChildControls.Add(SelectedUIControl.Clone() as UIControl);
            }
            else
            {
                SelectedTab = 1;
            }
        }
        public void InsertBeforeControl()
        {
            SelectedTab = 0;
            MessageBoxResult msgResult = MessageBox.Show(string.Format("Insert Control '{0}' before Control '{1}'?", SelectedUIControl.Id, SelectedXMLControl.Id), "Insert Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (msgResult == MessageBoxResult.Yes)
            {
                FindParentControl(XMLControlCollection, SelectedXMLControl).Insert(0, SelectedUIControl.Clone() as UIControl);
            }
            else
            {
                SelectedTab = 1;
            }
        }
        public void InsertAfterControl()
        {
            SelectedTab = 0;
            MessageBoxResult msgResult = MessageBox.Show(string.Format("Insert Control '{0}' after Control '{1}'?", SelectedUIControl.Id, SelectedXMLControl.Id), "Insert Control", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (msgResult == MessageBoxResult.Yes)
            {
                FindParentControl(XMLControlCollection, SelectedXMLControl).Add(SelectedUIControl.Clone() as UIControl);
            }
            else
            {
                SelectedTab = 1;
            }
        }
        #endregion

        #region CanExecute Methods
        public bool CanAddControl()
        {
            if (!IsDetecting)
                if (SelectedTab == 0)
                    if (XMLControlCollection != null)
                    {
                        if (XMLControlCollection.Count == 0)
                            return true;
                        else
                            if (SelectedXMLControl == null)
                                return false;
                            else
                                return true;
                    }
                    else
                        return true;

                else
                    return false;
            else
                return false;
        }
        public bool CanDeleteControl()
        {
            if (_selectedXMLControl != null)
                if (!IsDetecting)
                    if (SelectedTab == 0)
                        return true;
                    else
                        return false;
                else
                    return false;
            else
                return false;
        }

        public bool CanExportControls()
        {
            if (XMLControlCollection != null)
            {
                if (XMLControlCollection.Count == 0)
                    return true;
                else
                    return false;
            }

            if (UIControlCollection != null)
            {
                if (UIControlCollection.Where(o => o.IsExport).Count() > 0)
                    if (!IsDetecting)
                        if (SelectedTab == 1)
                            return true;
                        else
                            return false;
                    else
                        return false;
                else
                    return false;
            }
            else
                return false;
        }

        public bool CanInsertIntoControl()
        {
            if (_selectedUIControl != null & _selectedXMLControl != null)
                if (!IsDetecting)
                    if (SelectedTab == 1)
                        return true;
                    else
                        return false;
                else
                    return false;
            else
                return false;
        }

        public bool CanInsertBeforeControl()
        {
            if (_selectedUIControl != null & _selectedXMLControl != null)
                if (!IsDetecting)
                    if (SelectedTab == 1)
                        return true;
                    else
                        return false;
                else
                    return false;
            else
                return false;
        }

        public bool CanInsertAfterControl()
        {
            if (_selectedUIControl != null & _selectedXMLControl != null)
                if (!IsDetecting)
                    if (SelectedTab == 1)
                        return true;
                    else
                        return false;
                else
                    return false;
            else
                return false;
        }

        public bool CanReplaceControl()
        {
            if (_selectedUIControl != null & _selectedXMLControl != null)
                if (!IsDetecting)
                    if (SelectedTab == 1)
                        return true;
                    else
                        return false;
                else
                    return false;
            else
                return false;
        }
        #endregion        
    }
}
