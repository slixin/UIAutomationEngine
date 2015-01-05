using System;
using System.Collections.Generic;
using System.Windows.Automation;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;

namespace UIEngine
{
    public class UILocate
    {
        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hwnd, IntPtr lpRect, bool bErase);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        #region Properties
        public object BaseControl { get; set; }
        public string ControlProperties { get; set; }
        public string ControlType { get; set; }
        public string SearchScope { get; set; }
        public int SearchIndex { get; set; }
        #endregion

        #region Public methods
        public bool IsSame(object element)
        {
            return Enumerable.SequenceEqual(((AutomationElement)element).GetRuntimeId(), ((AutomationElement)BaseControl).GetRuntimeId());
        }

        public object GetElementFromPoint(System.Windows.Point point)
        {
            object element = null;
            AutomationElement currentElement;
            try
            {
                currentElement = AutomationElement.FromPoint(point);
                if (currentElement != null)
                {
                    if (System.Diagnostics.Process.GetCurrentProcess().Id != currentElement.Current.ProcessId)
                    {
                        element = currentElement;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something wrong: " + ex.Message);
            }

            return element;
        }

        public void HighlightElement()
        {
            RefreshScreen();

            foreach (var cs in InitializeGraphics())
            {
                var highlightGraphics = cs.SurfaceGraphics;

                try
                {
                    if (((AutomationElement)BaseControl).Current.BoundingRectangle.X >= cs.SurfaceGraphics.VisibleClipBounds.X + cs.Offset.Width &&
                    ((AutomationElement)BaseControl).Current.BoundingRectangle.X <= cs.SurfaceGraphics.VisibleClipBounds.X + cs.Offset.Width + cs.SurfaceGraphics.VisibleClipBounds.Width)
                    {
                        highlightGraphics.DrawRectangle(new Pen(Color.Red, 2),
                                Convert.ToInt32(((AutomationElement)BaseControl).Current.BoundingRectangle.X - cs.Offset.Width),
                                Convert.ToInt32(((AutomationElement)BaseControl).Current.BoundingRectangle.Y - cs.Offset.Height),
                                Convert.ToInt32(((AutomationElement)BaseControl).Current.BoundingRectangle.Width),
                                Convert.ToInt32(((AutomationElement)BaseControl).Current.BoundingRectangle.Height));
                        highlightGraphics.Dispose();
                    }
                }
                catch (Exception)
                { }
            }
        }

        /// <summary>
        /// Search Element
        /// </summary>
        /// <returns></returns>
        public AutomationElement Search()
        {
            AutomationElement element;
            List<AutomationElement> elementList = new List<AutomationElement>();

            if (BaseControl == null)
                throw new Exception("Base Control cannot be null");
            if (string.IsNullOrEmpty(ControlProperties))
                throw new Exception("Control Properties cannot be empty");
            if (string.IsNullOrEmpty(ControlType))
                throw new Exception("Control Type cannot be empty");

            var parentElement = BaseControl as AutomationElement;
            var searchCondition = BuildSearchCondition(GetPropertyDictionary(ControlProperties, false), ControlType);
            var regexCondition = GetPropertyDictionary(ControlProperties, true);
            var scope = GetScope(SearchScope);

            if (SearchIndex > 0)
            {
                elementList = SearchElements(parentElement, scope, searchCondition);
                if (elementList.Count == 0)
                    throw new Exception("Cannot find any control which meet the search condition");
               
                if (regexCondition.Count > 0)
                {
                    element = FindElementByRegex(elementList, regexCondition, SearchIndex);
                }
                else
                {
                    if (SearchIndex > elementList.Count)
                        throw new Exception(string.Format("There are {0} controls which meet the search condition, but looking for #{1} control.", elementList.Count, SearchIndex));
                    else
                        element = elementList[SearchIndex];
                }
            }
            else
            {
                if (regexCondition.Count > 0)
                {
                    elementList = SearchElements(parentElement, scope, searchCondition);
                    if (elementList.Count == 0)
                        throw new Exception("Cannot find any control which meet the search condition");

                    element = FindElementByRegex(elementList, regexCondition);
                }
                else
                {
                    element = SearchElement(parentElement, scope, searchCondition);
                }
            }

            return element;
        }

        public AutomationElement GetParent()
        {
            AutomationElement element;

            if (BaseControl == null)
                throw new Exception("Base Control cannot be null");

            TreeWalker tw = new TreeWalker(Condition.TrueCondition);
            element= tw.GetParent((AutomationElement)BaseControl);

            return element;
        }
        #endregion

        #region Private methods
        private static void RefreshScreen()
        {
            InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
            Thread.Sleep(500);
        }

        private static CompatibilitySurface[] InitializeGraphics()
        {
            // Create graphics for each display using compatibility mode
            var compatibilitySurfaces = System.Windows.Forms.Screen.AllScreens.Select(s => new CompatibilitySurface
            {
                SurfaceGraphics = Graphics.FromHdc(CreateDC(null, s.DeviceName, null, IntPtr.Zero)),
                Offset = new Size(s.Bounds.Location)
            }).ToArray();

            return compatibilitySurfaces;
        }

        /// <summary>
        /// TreeWalker search, only return one element
        /// </summary>
        /// <param name="parentControl">Parent control</param>
        /// <param name="condition">Condition</param>
        /// <returns></returns>
        private AutomationElement TreeWalkerSearch(AutomationElement parentControl, Condition condition)
        {
            AutomationElement element;

            var tw = new TreeWalker(condition);
            element = tw.GetFirstChild(parentControl);

            return element;
        }

        /// <summary>
        /// Search and get Automation Element List
        /// </summary>
        /// <param name="parent">Parent Element</param>
        /// <param name="scope">Tree Scope</param>
        /// <param name="cond">Condition</param>
        /// <returns></returns>
        private List<AutomationElement> SearchElements(AutomationElement parent, TreeScope scope, Condition cond)
        {
            List<AutomationElement> elements = new List<AutomationElement>();
            AutomationElementCollection elementCollection = parent.FindAll(scope, cond);
            if (elementCollection.Count > 0)
            {
                for (var i = 0; i < elementCollection.Count; i++)
                {
                    elements.Add(elementCollection[i]);
                }
            }

            return elements;
        }

        /// <summary>
        /// Search and get Automation Element
        /// </summary>
        /// <param name="parent">Parent Element</param>
        /// <param name="scope">Tree Scope</param>
        /// <param name="cond">Condition</param>
        /// <returns></returns>
        private AutomationElement SearchElement(AutomationElement parent, TreeScope scope, Condition cond)
        {
            AutomationElement element = parent.FindFirst(scope, cond);
            // If cannot get the element via FindFirst method, call TreeWalkerSearch
            if (element == null)
            {
                element = TreeWalkerSearch(parent, cond);
            }

            return element;
        }

        /// <summary>
        /// Search the return elements and find the element by Regex
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="regex"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private AutomationElement FindElementByRegex(List<AutomationElement> elements, Dictionary<AutomationProperty, string> regex)
        {
            AutomationElement element = null;

            element = FindElementByRegex(elements, regex, 1);

            return element;
        }

        /// <summary>
        /// Search the return elements and find the element by Regex
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="regex"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private AutomationElement FindElementByRegex(List<AutomationElement> elements, Dictionary<AutomationProperty, string> regex, int index)
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

        /// <summary>
        /// Build Property Dictionnary
        /// </summary>
        /// <param name="properties">Properties</param>
        /// <param name="isRegexProperties">Only get the regex properties if true</param>
        /// <returns></returns>
        private Dictionary<AutomationProperty, string> GetPropertyDictionary(string properties, bool isRegexProperties)
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

                // If regex properties, only get the properties which meet the regex
                if (isRegexProperties)
                {
                    if (Regex.IsMatch(value, regex))
                        pro.Add(ap, value);
                }
                else // If not, only get the properties which does not meet the regex.
                {
                    if (!Regex.IsMatch(value, regex))
                        pro.Add(ap, value);
                }                
            }

            return pro;
        }

        /// <summary>
        /// Get Scope, Default is Descendants
        /// </summary>
        /// <param name="scope">child or children all supported</param>
        /// <returns></returns>
        private TreeScope GetScope(string scope)
        {
            if (string.IsNullOrEmpty(scope))
                return TreeScope.Descendants;

            if (scope.ToLower() == "children" || scope.ToLower() == "child")
                return TreeScope.Children;

            return TreeScope.Descendants;
        }

        /// <summary>
        /// Get AutomationProperty by string
        /// </summary>
        /// <param name="propertyname">property name</param>
        /// <returns></returns>
        private AutomationProperty GetAutomationProperty(string propertyname)
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

        /// <summary>
        /// Get ControlType by string
        /// </summary>
        /// <param name="type">control type</param>
        /// <returns></returns>
        private ControlType GetControlType(string type)
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

        /// <summary>
        /// Build search condition
        /// </summary>
        /// <param name="properties">properties dictionary</param>
        /// <param name="type">control type</param>
        /// <returns></returns>
        private AndCondition BuildSearchCondition(Dictionary<AutomationProperty, string> properties, string controltype)
        {
            AndCondition ac;
            var conditioncount = 2;

            try
            {
                conditioncount += properties.Count;

                // There are 3 properties are required, IsOffScreen = false, IsEnabled = true, ControlType
                var conditions = new Condition[conditioncount];
                conditions[0] = new PropertyCondition(AutomationElement.IsOffscreenProperty, false);
                conditions[1] = new PropertyCondition(AutomationElement.ControlTypeProperty, GetControlType(controltype));

                var i = 2;
                foreach (var p in properties)
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
        #endregion
    }

    public class CompatibilitySurface : IDisposable
    {
        public Graphics SurfaceGraphics;
        public Size Offset = default(System.Drawing.Size);

        public PointF[] OffsetPoints(PointF[] points)
        {
            return points.Select(p => PointF.Subtract(p, Offset)).ToArray();
        }

        public void Dispose()
        {
            if (SurfaceGraphics != null)
                SurfaceGraphics.Dispose();
        }
    }
}
