using System;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Automation;
using System.Threading;
using System.ComponentModel;
using System.Collections.ObjectModel;
using AutomationHelper;
using HP.LR.Test;

namespace UIEngine
{
    public class UIControl : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private members
        private object _element;
        private string _id;
        private string _properties;
        private string _controltype;
        private int? _index;
        private string _scope;
        private string _framework;
        private string _snapshot;
        private bool _isexport;
        private bool _isselected;
        private bool _isexpanded;
        private bool _isdetecting;
        private bool _islocating;
        private bool _isinvalid;
        private UIControl _parent;
        private string _supportedPattern;
        private Dictionary<string, object> _nativeProperties;
        private ObservableCollectionEx<UIControl> _childrenControls;
        #endregion

        #region Properties
        public bool IsInvalid
        {
            get
            {
                return _isinvalid;
            }
            set
            {
                if (value != _isinvalid)
                {
                    _isinvalid = value;
                    OnPropertyChanged("IsInvalid");
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
        public bool IsDetecting
        {
            get
            {
                return _isdetecting;
            }
            set
            {
                if (value != _isdetecting)
                {
                    _isdetecting = value;
                    OnPropertyChanged("IsDetecting");
                }
            }
        }
        public bool IsLocating
        {
            get
            {
                return _islocating;
            }
            set
            {
                if (value != _islocating)
                {
                    _islocating = value;
                    OnPropertyChanged("IsLocating");
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
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    IsInvalid = true;
                else
                    IsInvalid = false;
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        public string Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    IsInvalid = true;
                else
                    IsInvalid = false;
                if (value != _properties)
                {
                    _properties = value;
                    OnPropertyChanged("Properties");
                }
            }
        }
        public string ControlType
        {
            get
            {
                return _controltype;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    IsInvalid = true;
                else
                    IsInvalid = false;
                if (value != _controltype)
                {
                    _controltype = value;
                    OnPropertyChanged("ControlType");
                }
            }
        }
        public string Scope
        {
            get
            {
                return _scope;
            }
            set
            {
                if (value != _scope)
                {
                    _scope = value;
                    OnPropertyChanged("Scope");
                }
            }
        }
        public string Framework
        {
            get
            {
                return _framework;
            }
            set
            {
                if (value != _framework)
                {
                    _framework = value;
                    OnPropertyChanged("Framework");
                }
            }
        }
        public string Snapshot
        {
            get
            {
                return _snapshot;
            }
            set
            {
                if (value != _snapshot)
                {
                    _snapshot = value;
                    OnPropertyChanged("Snapshot");
                }
            }
        }
        public string SupportedPattern
        {
            get
            {
                return _supportedPattern;
            }
            set
            {
                if (value != _supportedPattern)
                {
                    _supportedPattern = value;
                    OnPropertyChanged("SupportedPattern");
                }
            }
        }
        public int? Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (value != _index)
                {                    
                    _index = value;
                    OnPropertyChanged("Index");
                }
            }
        }
        public UIControl Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (value != _parent)
                {
                    _parent = value;
                    OnPropertyChanged("Parent");
                }
            }
        }
        public ObservableCollectionEx<UIControl> ChildControls
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
        #endregion

        #region Public methods
        public object Clone()
        {
            var control = new UIControl
            {
                Id = Id,
                Framework = Framework,
                Index = Index,
                ControlType = ControlType,
                Element = Element,
                Scope = Scope,
                Snapshot = Snapshot,
                SupportedPattern = SupportedPattern,
                Parent = Parent,
                Properties = Properties,
                ChildControls = new ObservableCollectionEx<UIControl>(),
            };

            foreach(UIControl cc in ChildControls)
            {
                control.ChildControls.Add(cc.Clone() as UIControl);
            }

            return control;
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

    public class UIControls : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private members
        private ObservableCollectionEx<UIControl> _uicontrolcollection;
        private Dictionary<string, UIControl> _uicontrolDictionary;
        #endregion

        #region Properties
        public ObservableCollectionEx<UIControl> UIControlCollection
        {
            get
            {
                return _uicontrolcollection;
            }
            set
            {
                if (value != _uicontrolcollection)
                {
                    _uicontrolcollection = value;
                    OnPropertyChanged("UIControlCollection");
                }
            }
        }
        public Dictionary<string, UIControl> UIControlDictionary
        {
            get
            {
                return _uicontrolDictionary;
            }
            set
            {
                if (value != _uicontrolDictionary)
                {
                    _uicontrolDictionary = value;
                    OnPropertyChanged("UIControlDictionary");
                }
            }
        }
        #endregion

        #region Public methods
        public void Load(string file)
        {
            try
            {
                XmlDocument xmldoc = XMLHelper.Load(file);
                var controlsNode = xmldoc.SelectSingleNode("Controls");

                if (_uicontrolcollection == null)
                    _uicontrolcollection = new ObservableCollectionEx<UIControl>();

                if (_uicontrolDictionary == null)
                    _uicontrolDictionary = new Dictionary<string, UIControl>();

                foreach (UIControl c in LoadControlTree(controlsNode, null))
                {
                    _uicontrolcollection.Add(c);
                }
                foreach (KeyValuePair<string, UIControl> kv in LoadControlDefinition(controlsNode, null))
                {
                    _uicontrolDictionary.Add(kv.Key, kv.Value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }
        public bool Save(string file, bool isPrefixParentId)
        {
            bool result = false;
            try
            {
                XmlDocument doc = XMLHelper.CreateXmlDocument("Controls");
                XmlNode rootNode = doc.LastChild;
                if (CreateXMLNodes(rootNode, _uicontrolcollection, isPrefixParentId, null))
                {
                    doc.Save(file);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }
        #endregion

        #region Constructor
        public UIControls()
        {
        }
        #endregion

        #region Private methods
        private UIControl GetUIControl(XmlNode node, UIControl parent)
        {
            UIControl control = new UIControl
            {
                Parent = parent,
                Id = XMLHelper.GetAttributeValue(node, "id"),
                Properties = XMLHelper.GetAttributeValue(node, "properties"),
                ControlType = XMLHelper.GetAttributeValue(node, "controltype"),
                Scope = XMLHelper.GetAttributeValue(node, "scope"),
                Index = XMLHelper.GetAttributeValue(node, "index") == null ? null : (int?)Convert.ToInt32(XMLHelper.GetAttributeValue(node, "index")),
                Framework = XMLHelper.GetAttributeValue(node, "framework") == null ? string.Empty : XMLHelper.GetAttributeValue(node, "framework"),
                SupportedPattern = XMLHelper.GetAttributeValue(node, "supportedPattern") == null ? string.Empty : XMLHelper.GetAttributeValue(node, "supportedPattern"),
                Snapshot = string.Empty,
                Element = null,
                IsDetecting = false,
                IsExport = false,
                IsInvalid = false,
                IsSelected = false,
                NativeProperties = null,
            };

            return control;
        }
        private ObservableCollectionEx<UIControl> LoadControlTree(XmlNode ctrlnode, UIControl parent)
        {
            ObservableCollectionEx<UIControl> controls = new ObservableCollectionEx<UIControl>();

            try
            {
                var nodelist = XMLHelper.SelectNodeList(ctrlnode, "Control", null);
                if (nodelist != null)
                {
                    foreach (XmlNode node in nodelist)
                    {
                        UIControl uicontrol = GetUIControl(node, parent);
                        uicontrol.ChildControls = new ObservableCollectionEx<UIControl>();
                        if (node.SelectNodes("Control").Count > 0)
                        {
                            uicontrol.ChildControls = LoadControlTree(node, uicontrol);
                        }

                        controls.Add(uicontrol);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return controls;
        }
        private Dictionary<string, UIControl> LoadControlDefinition(XmlNode ctrlnode, UIControl parent)
        {
            Dictionary<string, UIControl> controls = new Dictionary<string,UIControl>();

            try
            {
                var nodelist = XMLHelper.SelectNodeList(ctrlnode, "Control", null);
                if (nodelist != null)
                {
                    foreach (XmlNode node in nodelist)
                    {
                        UIControl uicontrol = GetUIControl(node, parent);

                        controls.Add(uicontrol.Id, uicontrol);

                        if (node.SelectNodes("Control").Count > 0)
                        {
                            foreach(KeyValuePair<string, UIControl> kv in LoadControlDefinition(node, uicontrol))
                            {
                                controls.Add(kv.Key, kv.Value);
                            }
                        }
                            
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return controls;
        }       
        private bool CreateXMLNodes(XmlNode parentNode, ObservableCollectionEx<UIControl> controls, bool IsPrefixParentId, string parentId)
        {
            bool isSuccess = false;

            foreach (UIControl control in controls)
            {
                if (control.IsInvalid)
                {
                    control.IsSelected = true;
                    throw new Exception("Some control definition is invalid.");
                }
                string id = control.Id;               

                XmlNode node = XMLHelper.CreateChildNode(parentNode, "Control");
                if (IsPrefixParentId && !string.IsNullOrEmpty(parentId))
                    id = string.Format("{0}_{1}", parentId, control.Id);

                XMLHelper.CreateAttribute(node, "id", id);
                XMLHelper.CreateAttribute(node, "properties", control.Properties);
                XMLHelper.CreateAttribute(node, "controltype", control.ControlType);
                if (!string.IsNullOrEmpty(control.Framework))
                    XMLHelper.CreateAttribute(node, "framework", control.Framework);
                if (!string.IsNullOrEmpty(control.Scope))
                    XMLHelper.CreateAttribute(node, "scope", control.Scope);
                if (control.Index !=  null)
                    XMLHelper.CreateAttribute(node, "index", control.Index.ToString());
                XMLHelper.CreateAttribute(node, "supportedPattern", control.SupportedPattern);
                CreateXMLNodes(node, control.ChildControls, IsPrefixParentId, id);
                isSuccess = true;
            }

            return isSuccess;
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

    public class UIAutomationEngine
    {
        public UIControls Controls { get; set; }        

        public UIControl this[string controlName]
        {
            get
            {
                return Find(controlName, 10);
            }
        }

        public UIControl this[string controlName, int timeout]
        {
            get
            {
                return Find(controlName, timeout);
            }
        }

        #region Construct
        public UIAutomationEngine()
        {
        }

        public UIAutomationEngine(string xmlFile)
        {
            Controls = new UIControls();
            Controls.Load(xmlFile);
        }

        public UIAutomationEngine(params string[] xmlFiles)
        {
            Controls = new UIControls();
                    
            if (xmlFiles != null)
            {
                foreach (var file in xmlFiles)
                {
                    Controls.Load(file);
                }
            }
        }
        #endregion

        #region Find behavior
        public UIControl TryFind(string controlName)
        {
            UIControl control = null;

            try
            {
                control = Find(controlName, 10);
            }
            catch { }

            return control;
        }

        public UIControl TryFind(string controlName, int timeout)
        {
            UIControl control = null;

            try
            {
                control = Find(controlName, timeout);
            }
            catch { }

            return control;
        }

        public UIControl Find(string controlName, int timeout)
        {
            object element = null;
            string errorMsg = null;

            if (Controls == null)
                throw new Exception("Please load controls first.");

            if (Controls.UIControlDictionary.Count == 0)
                throw new Exception("No controls be loaded.");

            if (!Controls.UIControlDictionary.ContainsKey(controlName))
                throw new Exception(string.Format("The control '{0}' is not existed.", controlName));

            var control = Controls.UIControlDictionary[controlName];
            Console.WriteLine("Looking for control {0}, Type={1}, Properties={2}, Scope={3}, Index={4}", controlName, control.ControlType, control.Properties, control.Scope, control.Index);
            
            var dtStart = DateTime.UtcNow;
            double consumedTime = 0;
            while (consumedTime < timeout)
            {
                try
                {
                    element = FindAutomationElement(control);
                    if (element != null)
                    {
                        control.Element = element;
                        break;
                    }
                    else
                    {
                        throw new Exception("Does not found the control yet.");
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }

                consumedTime = DateTime.UtcNow.Subtract(dtStart).TotalSeconds;
                Console.Write(".");
                Thread.Sleep(1000);
            }
            Console.WriteLine();

            if (element == null)
                throw new Exception(string.Format("Cannot locate control '{0}' in current UI. Exception: {1}", controlName, errorMsg));

            return control;
        }
        private object FindAutomationElement(UIControl control)
        {
            var parent = GetParentAutomationElementObject(control);
            object element = new UILocate()
            {
                ControlProperties = control.Properties,
                BaseControl = parent,
                ControlType = control.ControlType,
                SearchScope = control.Scope,
                SearchIndex = control.Index == null ? -1 : control.Index.Value,
            }.Search();

            return element;
        }
        private object GetParentAutomationElementObject(UIControl control)
        {
            object parent;
            if (control.Parent == null)
            {
                parent = AutomationElement.RootElement;
            }
            else
            {
                parent = FindAutomationElement(control.Parent);
            }            
            
            return parent;
        }
        #endregion
    }
}
