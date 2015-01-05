using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Collections.ObjectModel;
using AutomationHelper;
using System.Windows.Threading;
using UIEngine;
using HP.LR.Test;
using UIAutoXMLBuilder.Base;

namespace UIAutoXMLBuilder
{
    public class Capture
    {
        #region Capture Controls
        #region Public Methods
        public static ObservableCollectionEx<UIControl> CaptureControls(object control)
        {
            var uiControlTree = new ObservableCollectionEx<UIControl>();

            if (control != null)
            {
                var rootControl = GetUIControl(control);
                uiControlTree.Add(rootControl);
            }

            return uiControlTree;
        }        
        public static string GetSnapshotString(object obj)
        {
            AutomationElement element = obj as AutomationElement;
            var rect = element.Current.BoundingRectangle;

            if (!rect.IsEmpty)
            {
                var size = new Size(Convert.ToInt32(rect.Width), Convert.ToInt32(rect.Height));

                var bmp = ImageHelper.TakeSnapshot(Convert.ToInt32(rect.Width), Convert.ToInt32(rect.Height), Convert.ToInt32(rect.X), Convert.ToInt32(rect.Y), size);
                var image = bmp as Image;

                if (image != null)
                {
                    return ImageHelper.ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);
                }
            }

            return null;
        }
        public static string GetElementID(object obj)
        {
            var sbId = new StringBuilder();
            string name = null;
            string autoid = null;
            AutomationElement element = obj as AutomationElement;
            var automationIdValue = element.Current.AutomationId;
            var nameValue = element.Current.Name;
            var classNameValue = element.Current.ClassName;

            try
            {
                #region Get name of the control
                if (!string.IsNullOrEmpty(nameValue))
                {
                    name = new Regex("(?:[^a-z0-9 ]|(?<=['\"]))", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled).Replace(nameValue.ToString().Trim().ToUpper(), string.Empty).Replace(" ", "_");                    
                }
                else
                {
                    object namevalueobj;
                    if (element.TryGetCurrentPattern(ValuePattern.Pattern, out namevalueobj))
                    {
                        var valuePattern = (ValuePattern)element.GetCurrentPattern(ValuePattern.Pattern);
                        var namevalue = valuePattern.Current.Value;
                        if (!string.IsNullOrEmpty(namevalue))
                            name = new Regex("(?:[^a-z0-9 ]|(?<=['\"]))", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled).Replace(namevalue.ToString().Trim().ToUpper(), string.Empty).Replace(" ", "_");
                    }
                }

                if (string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(classNameValue))
                {
                    name = new Regex("(?:[^a-z0-9 ]|(?<=['\"]))", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled).Replace(classNameValue.ToString().Trim().ToUpper(), string.Empty).Replace(" ", "_");
                }
                #endregion

                #region Get Automation Id of the control
                if (!string.IsNullOrEmpty(automationIdValue))
                {
                    autoid = new Regex("(?:[^a-z0-9 ]|(?<=['\"]))", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled).Replace(automationIdValue.ToString().Trim().ToUpper(), string.Empty).Replace(" ", "_");
                }
                #endregion

                sbId.Append(element.Current.ControlType.ProgrammaticName.ToUpper().Replace("CONTROLTYPE.", ""));
                if (!string.IsNullOrEmpty(autoid))
                    sbId.Append("_" + autoid);
                else if (!string.IsNullOrEmpty(name))
                    sbId.Append("_" + name);
                else
                    sbId.Append("_" + TextHelper.RandomString(10).ToUpper());

                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var elementId = sbId.ToString();

            return elementId;
        }
        public static string GetElementSupportedPatterns(object obj)
        {
            string pattern = null;
            AutomationElement element = obj as AutomationElement;
            foreach (var apt in element.GetSupportedPatterns().OrderBy(o => o.ProgrammaticName))
            {
                string spattern = string.Format("{0}", apt.ProgrammaticName.Substring(0, apt.ProgrammaticName.IndexOf(".")).Replace("PatternIdentifiers", string.Empty));

                if (string.IsNullOrEmpty(pattern))
                    pattern += spattern;
                else
                    pattern += string.Format(";{0}", spattern);
            }

            return pattern;
        }
        public static string GetElementProperties(object obj)
        {
            string properties = null;
            AutomationElement element = obj as AutomationElement;
            if (!string.IsNullOrEmpty(element.Current.AutomationId) && !string.IsNullOrWhiteSpace(element.Current.AutomationId))
            {
                properties = string.Format("AutomationId={0}", element.Current.AutomationId.Trim());
            }

            if (!string.IsNullOrEmpty(element.Current.Name) && !string.IsNullOrWhiteSpace(element.Current.Name) && string.IsNullOrEmpty(properties))
            {
                properties = string.Format("Name={0}", element.Current.Name.Trim());
                if (!string.IsNullOrEmpty(element.Current.LocalizedControlType))
                {
                    properties = string.Format("{0}|LocalizedControlType={1}", properties, element.Current.LocalizedControlType);
                }
            }

            if (!string.IsNullOrEmpty(element.Current.ClassName) && !string.IsNullOrWhiteSpace(element.Current.ClassName) && string.IsNullOrEmpty(properties))
            {
                properties = string.Format("ClassName={0}", element.Current.ClassName.Trim());
                if (!string.IsNullOrEmpty(element.Current.LocalizedControlType))
                {
                    properties = string.Format("{0}|LocalizedControlType={1}", properties, element.Current.LocalizedControlType);
                }
            }

            return properties;
        }
        public static Dictionary<string, object> GetNativeProperties(object obj)
        {
            Dictionary<string, object> uicProperties = new Dictionary<string, object>();
            AutomationElement element = obj as AutomationElement;
            try
            {
                foreach (AutomationProperty ap in element.GetSupportedProperties().Where(o=>Regex.IsMatch(o.ProgrammaticName, "AutomationElementIdentifiers.*Property")).OrderBy(o => o.ProgrammaticName))
                {
                    string proName = ap.ProgrammaticName.Substring(ap.ProgrammaticName.LastIndexOf(".") + 1).Replace("Property", string.Empty);
                    if (!string.IsNullOrEmpty(proName))
                    {
                        try
                        {
                            Type valueType = element.GetCurrentPropertyValue(ap).GetType();
                            if (valueType.Equals(typeof(string)) || valueType.Equals(typeof(int)) || valueType.Equals(typeof(bool)))
                            {
                                string proValue = element.GetCurrentPropertyValue(ap).ToString();

                                if (!string.IsNullOrEmpty(proValue))
                                {
                                    if (proName == "ControlType")
                                    {
                                        proValue = element.Current.ControlType.LocalizedControlType;
                                    }

                                    uicProperties.Add(proName, proValue);
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return uicProperties;
        }

        public static List<object> GetChildrenControls(object parent)
        {
            var controlList = new List<object>();
            AutomationElement element = parent as AutomationElement;

            var elementList = element.FindAll(TreeScope.Children, Condition.TrueCondition);
            for (var i = 0; i < elementList.Count; i++)
            {
                controlList.Add(elementList[i]);
            }

            return controlList;
        }

        public static DetectUIControl GetDetectUIControl(object control)
        {
            AutomationElement element = control as AutomationElement;
            DetectUIControl detectUIControl = new DetectUIControl
            {
                Name = string.Format("\"{0}\" {1}", element.Current.Name, element.Current.ControlType.LocalizedControlType),
                IsSelected = false,
                IsExport = false,
                Element = element,
                IsExpanded = true,
                NativeProperties = GetNativeProperties(element),
                ChildControls = new ObservableCollectionEx<DetectUIControl>(),
            };

            return detectUIControl;
        }

        public static void Highlight(object control)
        {
            UILocate uiLocate = new UILocate();
            uiLocate.BaseControl = control;
            uiLocate.HighlightElement();
        }
        #endregion
        #region private Methods
        private static ObservableCollectionEx<UIControl> GetUIControlChildren(object parent)
        {
            var controlList = new ObservableCollectionEx<UIControl>();
            AutomationElement element = parent as AutomationElement;
            var ctype = element.Current.ControlType.LocalizedControlType.Replace(" ", "").ToLower();

            if (ctype.Equals("datagrid"))
                return controlList;

            var tw = new TreeWalker(new AndCondition(new PropertyCondition(AutomationElement.IsOffscreenProperty, false), Condition.TrueCondition));
            var childElement = tw.GetFirstChild(element);

            if (childElement != null)
            {
                var tvm = GetUIControl(childElement);
                if (tvm != null)
                    controlList.Add(tvm);

                var currentElement = childElement;
                AutomationElement nextSiblingElement;
                while ((nextSiblingElement = tw.GetNextSibling(currentElement)) != null)
                {
                    var tvmNext = GetUIControl(nextSiblingElement);
                    if (tvmNext != null)
                        controlList.Add(tvmNext);

                    currentElement = nextSiblingElement;
                }
            }

            return controlList;
        }
        private static UIControl GetUIControl(object element)
        {
            var control = new UIControl()
            {
                Element = element as AutomationElement,
                IsDetecting = true,
                ChildControls = GetUIControlChildren(element),
            };

            return control;
        }
        #endregion
        #endregion


        #region Locate Control
        public static AutomationElement LocateControl(UIControl control)
        {
            var element = Search(
                control.Parent == null ?  null : control.Parent.Element, 
                control.Properties, 
                control.ControlType, 
                control.Scope, 
                control.Index == null ? -1 : control.Index.Value);

            return element;
        }

        private static AutomationElement Search(object parentControl, string searchProperties, string controlType, string Scope, int searchIndex)
        {
            AutomationElement currentElement;
            AutomationElement parentElement;

            if (parentControl == null)
                parentElement = AutomationElement.RootElement;
            else
                parentElement = parentControl as AutomationElement;

            var searchCondition = BuildSearchCondition(GetPropertyDictionary(searchProperties, false), controlType);
            var regexProperties = GetPropertyDictionary(searchProperties, true);

            var scope = GetScope(Scope);

            var foundElements = GetElements(parentElement, scope, searchCondition, searchIndex < 0 ? (regexProperties.Count > 0 ? false : true) : false);
            if (foundElements.Count == 0)
                throw new Exception("No control be found by the search condition.");

            // When index is specified and searchIndex larger than found control and without Regex Properties 
            if (searchIndex > 0 && searchIndex > foundElements.Count && regexProperties.Count == 0)
                throw new Exception("The searching index is larger than found controls total.");

            // When no regex properties
            if (regexProperties.Count == 0)
            {
                currentElement = searchIndex < 0 ? foundElements[0] : foundElements[searchIndex - 1];
            }
            else
            {
                currentElement = SearchWithinRegexValue(foundElements, regexProperties, searchIndex);
            }

            return currentElement;
        }

        private static AndCondition BuildSearchCondition(Dictionary<AutomationProperty, string> properties, string type)
        {
            AndCondition ac;
            var conditioncount = 3;

            try
            {
                conditioncount += properties.Count;

                var conditions = new Condition[conditioncount];
                conditions[0] = new PropertyCondition(AutomationElement.IsOffscreenProperty, false);
                conditions[1] = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
                conditions[2] = new PropertyCondition(AutomationElement.ControlTypeProperty, GetControlType(type));

                var i = 3;
                foreach (KeyValuePair<AutomationProperty, string> p in properties)
                {
                    var pc = new PropertyCondition(p.Key, p.Value);
                    conditions[i] = pc;
                    i++;
                }

                ac = new AndCondition(conditions);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ac;
        }

        private static AutomationElement SearchWithinRegexValue(List<AutomationElement> elements, Dictionary<AutomationProperty, string> regex, int index)
        {
            AutomationElement element = null;
            var foundElements = new List<AutomationElement>();

            foreach (var e in elements)
            {
                foreach (var r in regex)
                {
                    var ap = r.Key;
                    var value = r.Value;

                    var eAutoproCount = e.GetSupportedProperties().Where(o => o == ap).Count();
                    var eValue = e.GetCurrentPropertyValue(ap) == null ? string.Empty : Convert.ToString(e.GetCurrentPropertyValue(ap));
                    var regexstring = value.Substring(1, value.Length - 2);

                    if (eAutoproCount == 1 && Regex.IsMatch(eValue, regexstring, RegexOptions.IgnoreCase))
                    {
                        foundElements.Add(e);
                    }
                }
            }

            if (foundElements.Count > 0)
            {
                if (index > 0)
                {
                    if (foundElements.Count >= index)
                        element = foundElements[index - 1];
                }
                else
                {
                    element = foundElements[0];
                }
            }

            return element;
        }

        private static Dictionary<AutomationProperty, string> GetPropertyDictionary(string properties, bool isLoadRegexProperties)
        {
            var pro = new Dictionary<AutomationProperty, string>();
            const string regex = @"^\[.*\]$";

            if (string.IsNullOrEmpty(properties))
                throw new Exception("No Properties for searching the control.");

            foreach (var property in properties.Split(new[] { '|' }))
            {
                var keyvaluepair = property.Split(new[] { '=' });
                if (keyvaluepair.Length != 2)
                    throw new Exception(string.Format("The property <{0}> is not meet the format:key=value!", property));

                var key = keyvaluepair[0];
                var value = keyvaluepair[1];
                var ap = GetAutomationProperty(key);

                if (ap == null)
                    throw new Exception(string.Format("Cannot find the property <{0}>.", key));

                // If load regex properties, only get the properties which meet the regex
                if (isLoadRegexProperties)
                {
                    if (Regex.IsMatch(value, regex))
                        pro.Add(ap, value);
                }
                else // If regex is null, only get the properties which does not meet the regex.
                {
                    if (!Regex.IsMatch(value, regex))
                        pro.Add(ap, value);
                }
            }

            return pro;
        }

        private static List<AutomationElement> GetElements(AutomationElement parent, TreeScope scope, Condition cond, bool findFirst)
        {
            var elementCollection = new List<AutomationElement>();
            if (findFirst)
            {
                var ae = parent.FindFirst(scope, cond);
                if (ae == null)
                    throw new Exception("Cannot locate the control.");

                elementCollection.Add(ae);
            }
            else
            {
                var aec = parent.FindAll(scope, cond);
                for (var i = 0; i < aec.Count; i++)
                {
                    elementCollection.Add(aec[i]);
                }
            }

            return elementCollection;
        }

        private static TreeScope GetScope(string scope)
        {
            if (string.IsNullOrEmpty(scope))
                return TreeScope.Descendants;

            if (scope.ToLower() == "children")
                return TreeScope.Children;

            return TreeScope.Descendants;
        }

        private static AutomationProperty GetAutomationProperty(string propertyname)
        {
            AutomationProperty ap = null;

            var autoProp = typeof(AutomationElement);

            foreach (var fi in autoProp.GetFields())
            {
                if (fi.FieldType.FullName == "System.Windows.Automation.AutomationProperty" && fi.Name.ToLower() == string.Format("{0}Property", propertyname).ToLower())
                {
                    ap = fi.GetValue(fi) as AutomationProperty;
                    break;
                }
            }

            if (ap == null)
            {
                throw new Exception(string.Format("Automation Property '{0}' is invalid.", propertyname));
            }

            return ap;
        }

        private static ControlType GetControlType(string type)
        {
            ControlType ct = null;

            var ctrl = typeof(ControlType);

            foreach (var fi in ctrl.GetFields())
            {
                if (fi.FieldType.FullName == "System.Windows.Automation.ControlType" && fi.Name.ToLower() == type.ToLower())
                {
                    ct = fi.GetValue(fi) as ControlType;
                    break;
                }
            }

            if (ct == null)
                throw new Exception(string.Format("Control Type '{0}' is invalid.", type));

            return ct;
        }
        #endregion
    }
}
