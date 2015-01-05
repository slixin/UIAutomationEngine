using System;
using System.Linq;
using System.Windows.Automation;
using System.Threading;
using System.Windows.Forms;
using AutomationHelper;
using WindowsInput;

namespace HP.LR.Test.UIOperation
{
    /// <summary>
    /// Public to Customized UIOperation with AutomationElement parameter
    /// </summary>
    public class ClassicBase
    {
        /// <summary>
        /// Get the classic properties for automation element
        /// </summary>
        public class Properties
        {
            /// <summary>
            /// Indicates whether the control is in enabled status.
            /// </summary>
            /// <returns>Boolean - True or False</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static bool IsEnabled(AutomationElement element)
            {
                var isEnabled = false;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    isEnabled = element.Current.IsEnabled;
                }
                catch { }

                return isEnabled;
            }

            /// <summary>
            /// Indicates whether the control is password control.
            /// </summary>
            /// <returns>Boolean - True or False</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static bool IsPassword(AutomationElement element)
            {
                var isPassword = false;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    isPassword = element.Current.IsPassword;
                }
                catch { }

                return isPassword;
            }

            /// <summary>
            /// Indicates whether the control is out of screen.
            /// </summary>        
            /// <returns>Boolean - True or False</returns>    
            /// <remarks>Support Control Type: All</remarks>
            public static bool IsOffScreen(AutomationElement element)
            {
                var isOffScreen = false;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    isOffScreen = element.Current.IsOffscreen;
                }
                catch { }

                return isOffScreen;
            }

            /// <summary>
            /// Check whether the control is required control in current form.
            /// </summary>        
            /// <returns>Boolean - True or False</returns>    
            /// <remarks>Support Control Type: All</remarks>
            public static bool IsRequiredforForm(AutomationElement element)
            {
                var isRequiredforForm = false;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    isRequiredforForm = element.Current.IsRequiredForForm;
                }
                catch { }

                return isRequiredforForm;
            }

            /// <summary>
            /// Indicates whether the entered or selected value is valid for the form rule associated.
            /// </summary>        
            /// <returns>Boolean - True or False</returns>    
            /// <remarks>Support Control Type: All</remarks>
            //public static bool IsDataValidForForm(AutomationElement element)
            //{
            //    var isDataValidForForm = false;

            //    if (element == null)
            //        throw new Exception("Element is null!");

            //    try
            //    {
            //        isDataValidForForm = element.Current.IsDataValidForForm;
            //    }
            //    catch { }

            //    return isDataValidForForm;
            //}

            /// <summary>
            /// Indicates whether the keyboard is focusable for the control.
            /// </summary>        
            /// <returns>True or False</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static bool IsKeyboardFocusable(AutomationElement element)
            {
                var isKeyboardFocusable = false;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    isKeyboardFocusable = element.Current.IsKeyboardFocusable;
                }
                catch { }

                return isKeyboardFocusable;
            }

            /// <summary>
            /// Return Name property value for the control.
            /// </summary>        
            /// <returns>String - Control Name</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static string GetName(AutomationElement element)
            {
                string name = null;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    name = element.Current.Name;
                }
                catch { }

                return name;
            }

            /// <summary>
            /// Return the runtime id for the control.
            /// </summary>        
            /// <returns>String - Runtime ID</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static string GetRuntimeId(AutomationElement element)
            {
                string runtimeid = null;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    foreach (var id in element.GetRuntimeId())
                    {
                        if (string.IsNullOrEmpty(runtimeid))
                            runtimeid += string.Format("{0}", id);
                        else
                            runtimeid += string.Format(",{0}", id);
                    }
                }
                catch { }

                return runtimeid;
            }

            /// <summary>
            /// Return the runtime id for the control.
            /// </summary>
            /// <returns>String - Runtime ID</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static string GetFrameworkId(AutomationElement element)
            {
                string frameworkid = null;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    frameworkid = element.Current.FrameworkId;
                }
                catch { }

                return frameworkid;
            }

            /// <summary>
            /// Return the access key for the control.
            /// </summary>
            /// <returns>String - Access Key</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static string GetAccessKey(AutomationElement element)
            {
                string accesskey = null;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    accesskey = element.Current.AccessKey;
                }
                catch { }

                return accesskey;
            }

            /// <summary>
            /// Return the help text for the control.
            /// </summary>        
            /// <returns>String - Help Text</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static string GetHelpText(AutomationElement element)
            {
                string helptext = null;

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    helptext = element.Current.HelpText;
                }
                catch { }

                return helptext;
            }

            /// <summary>
            /// Return the BoundingRectangle (width/height/x, y position) for the control.
            /// </summary>        
            /// <returns>System.Windows.Rect - BoundingRectangle</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static System.Windows.Rect GetBoundingRectangle(AutomationElement element)
            {
                var bounding = new System.Windows.Rect();

                if (element == null)
                    throw new Exception("Element is null!");

                try
                {
                    bounding = element.Current.BoundingRectangle;
                }
                catch { }

                return bounding;
            }

            /// <summary>
            /// Capture the snapshot of the control
            /// </summary>
            /// <param name="element"></param>
            /// <returns>Bitmap</returns>
            public static System.Drawing.Bitmap GetSnapshot(AutomationElement element)
            {
                if (element == null)
                    throw new Exception("Element is null!");

                if (element.Current.BoundingRectangle == null)
                    throw new Exception("Element without Bounding Rectangle!");

                int width = Convert.ToInt32(element.Current.BoundingRectangle.Width);
                int height = Convert.ToInt32(element.Current.BoundingRectangle.Height);
                int x = Convert.ToInt32(element.Current.BoundingRectangle.X);
                int y = Convert.ToInt32(element.Current.BoundingRectangle.Y);

                return ImageHelper.TakeSnapshot(width, height, x, y, new System.Drawing.Size(width, height));
            }
        }

        /// <summary>
        /// Methods for ExpandCollapsePattern
        /// </summary>
        public class ExpandCollapse
        {
            /// <summary>
            /// Expand the control.
            /// </summary>        
            /// <returns>void</returns>
            /// <remarks>Support Control Type: MenuItem | TreeItem | ListItem</remarks>
            public static void Expand(AutomationElement element)
            {
                if (IsSupportPattern(element, ExpandCollapsePattern.Pattern))
                {
                    var expandcollapsePattern = (ExpandCollapsePattern)element.GetCurrentPattern(ExpandCollapsePattern.Pattern);
                    if (expandcollapsePattern.Current.ExpandCollapseState != ExpandCollapseState.Expanded &&
                        expandcollapsePattern.Current.ExpandCollapseState != ExpandCollapseState.LeafNode)
                    {
                        expandcollapsePattern.Expand();
                        Thread.Sleep(1000);
                    }
                }
            }

            /// <summary>
            /// Collapse the control.
            /// </summary>        
            /// <returns>void</returns>
            /// <remarks>Support Control Type: MenuItem | TreeItem | ListItem</remarks>
            public static void Collapse(AutomationElement element)
            {
                if (IsSupportPattern(element, ExpandCollapsePattern.Pattern))
                {
                    var expandcollapsePattern = (ExpandCollapsePattern)element.GetCurrentPattern(ExpandCollapsePattern.Pattern);
                    if (expandcollapsePattern.Current.ExpandCollapseState != ExpandCollapseState.Collapsed &&
                        expandcollapsePattern.Current.ExpandCollapseState != ExpandCollapseState.LeafNode)
                    {
                        expandcollapsePattern.Collapse();
                        Thread.Sleep(1000);
                    }
                }
            }

            /// <summary>
            /// Get state (isexpand or iscollapse) on the control.
            /// </summary>        
            /// <returns>String - ExpandCollapseState</returns>
            /// <remarks>Support Control Type: MenuItem | TreeItem | ListItem</remarks>
            public static string GetExpandCollapseState(AutomationElement element)
            {
                string state = null;

                if (IsSupportPattern(element, ExpandCollapsePattern.Pattern))
                {
                    var expandcollapsePattern = (ExpandCollapsePattern)element.GetCurrentPattern(ExpandCollapsePattern.Pattern);
                    state = expandcollapsePattern.Current.ExpandCollapseState.ToString();
                }

                return state;
            }
        }

        /// <summary>
        /// Methods for InvokePattern
        /// </summary>
        public class Invoke
        {
            /// <summary>
            /// Invoke the control (Click).
            /// </summary>        
            /// <returns>void</returns>
            /// <remarks>Support Control Type: Button | MenuItem | TreeItem | ListItem | etc.</remarks>
            public static void Click(AutomationElement element)
            {
                if (IsSupportPattern(element, InvokePattern.Pattern))
                {
                    var invokePattern = (InvokePattern)element.GetCurrentPattern(InvokePattern.Pattern);
                    invokePattern.Invoke();
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Methods for TogglePattern
        /// </summary>
        public class Toggle
        {
            /// <summary>
            /// Toggle the control (Check / UnCheck).
            /// </summary>
            /// <param name="element"></param>
            /// <param name="state">toggle on / off</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: CheckBox | Button</remarks>
            public static void Check(AutomationElement element, PatternEnum.ToggleState state)
            {
                if (IsSupportPattern(element, TogglePattern.Pattern))
                {
                    var togglePattern = (TogglePattern)element.GetCurrentPattern(TogglePattern.Pattern);
                    if (state == PatternEnum.ToggleState.ToggleOn && togglePattern.Current.ToggleState != ToggleState.On)
                        togglePattern.Toggle();
                    else if (state == PatternEnum.ToggleState.ToggleOff && togglePattern.Current.ToggleState != ToggleState.Off)
                        togglePattern.Toggle();

                    Thread.Sleep(1000);
                }
            }

            /// <summary>
            /// Get Toggle status of the control.
            /// </summary>        
            /// <returns>Toggled | Non-Toggled | NA</returns>
            /// <remarks>Support Control Type: CheckBox | Button</remarks>
            public static bool? GetToggleStatus(AutomationElement element)
            {
                if (IsSupportPattern(element, TogglePattern.Pattern))
                {
                    var itemPattern = (TogglePattern)element.GetCurrentPattern(TogglePattern.Pattern);

                    if (itemPattern.Current.ToggleState == ToggleState.On)
                        return true;
                    if (itemPattern.Current.ToggleState == ToggleState.Off)
                        return false;
                    return null;
                }

                return null;
            }
        }

        /// <summary>
        /// Methods for SelectionItemPattern
        /// </summary>
        public class SelectionItem
        {
            /// <summary>
            /// Select the control
            /// </summary>        
            /// <returns>void</returns>
            /// <remarks>Support Control Type: MenuItem | TreeItem | ListItem</remarks>
            public static void SelectItem(AutomationElement element)
            {
                if (IsSupportPattern(element, SelectionItemPattern.Pattern))
                {
                    var selectPattern = (SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern);
                    selectPattern.Select();

                    Thread.Sleep(1000);
                }
            }

            /// <summary>
            /// Select current control when the parent control supports multiple selection
            /// </summary>        
            /// <returns>void</returns>
            /// <remarks>Support Control Type: ComboBox | TreeItem | ListItem</remarks>
            public static void AddItemToSelection(AutomationElement element)
            {
                if (IsSupportPattern(element, SelectionItemPattern.Pattern))
                {
                    var selectPattern = (SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern);
                    selectPattern.AddToSelection();

                    Thread.Sleep(1000);
                }
            }

            /// <summary>
            /// Unselect current control when the parent control supports multiple selection
            /// </summary>        
            /// <returns>void</returns>
            /// <remarks>Support Control Type: TreeItem | ListItem</remarks>
            public static void RemoveItemFromSelection(AutomationElement element)
            {
                if (IsSupportPattern(element, SelectionItemPattern.Pattern))
                {
                    var selectPattern = (SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern);
                    selectPattern.RemoveFromSelection();

                    Thread.Sleep(1000);
                }
            }

            /// <summary>
            /// Indicates current control is selected
            /// </summary>        
            /// <returns>void</returns>
            /// <remarks>Support Control Type: MenuItem | TreeItem | ListItem</remarks>
            public static bool IsItemSelected(AutomationElement element)
            {
                var isSelected = false;

                if (IsSupportPattern(element, SelectionItemPattern.Pattern))
                {
                    var selectPattern = (SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern);
                    isSelected = selectPattern.Current.IsSelected;
                }

                return isSelected;
            }

            /// <summary>
            /// Get container of the selection
            /// </summary>        
            /// <returns>AutomationElement - contrainer</returns>
            /// <remarks>Support Control Type: MenuItem | TreeItem | ListItem</remarks>
            public static AutomationElement GetSelectionContainer(AutomationElement element)
            {
                AutomationElement container;

                object obj;
                if (element.TryGetCurrentPattern(SelectionItemPattern.Pattern, out obj))
                {
                    var selectPattern = (SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern);
                    container = selectPattern.Current.SelectionContainer;
                }
                else
                    throw new Exception("Current element does not support SelectionItemPattern");

                return container;
            }
        }

        /// <summary>
        /// Methods for ValuePattern
        /// </summary>
        public class Value
        {
            /// <summary>
            /// Set value on the control
            /// </summary>
            /// <param name="element"></param>
            /// <param name="value">String - value</param>
            /// <returns>AutomationElement - Parent control</returns>
            /// <remarks>Support Control Type: Edit</remarks>
            public static void SetValue(AutomationElement element, string value)
            {
                if (IsSupportPattern(element, ValuePattern.Pattern))
                {
                    var valuePattern = (ValuePattern)element.GetCurrentPattern(ValuePattern.Pattern);
                    valuePattern.SetValue(value);

                    Thread.Sleep(1000);
                }
            }

            /// <summary>
            /// Get value on current control
            /// </summary>        
            /// <returns>String - value in the control</returns>
            /// <remarks>Support Control Type: Edit | TextBox</remarks>
            public static string GetValue(AutomationElement element)
            {
                string value = null;

                if (IsSupportPattern(element, ValuePattern.Pattern))
                {
                    var valuePattern = (ValuePattern)element.GetCurrentPattern(ValuePattern.Pattern);
                    value = valuePattern.Current.Value;

                    Thread.Sleep(1000);
                }

                return value;
            }
        }

        /// <summary>
        /// Methods for SelectionPattern
        /// </summary>
        public class Selection
        {
            /// <summary>
            /// Indicates whether require some selection for the control
            /// </summary>        
            /// <returns>Bool - True or False</returns>
            /// <remarks>Support Control Type: List | Tree | Menu</remarks>
            public static bool IsSelectionRequired(AutomationElement element)
            {
                var isrequired = false;

                if (IsSupportPattern(element, SelectionPattern.Pattern))
                {
                    var pattern = (SelectionPattern)element.GetCurrentPattern(SelectionPattern.Pattern);
                    isrequired = pattern.Current.IsSelectionRequired;
                }

                return isrequired;
            }

            /// <summary>
            /// Indicates whether support multiple selection for the control
            /// </summary>        
            /// <returns>Bool - True or False</returns>
            /// <remarks>Support Control Type: List | Tree | Menu</remarks>
            public static bool IsSelectionCanSelectMultiple(AutomationElement element)
            {
                var ismultiple = false;

                if (IsSupportPattern(element, SelectionPattern.Pattern))
                {
                    var pattern = (SelectionPattern)element.GetCurrentPattern(SelectionPattern.Pattern);
                    ismultiple = pattern.Current.CanSelectMultiple;
                }

                return ismultiple;
            }

            /// <summary>
            /// Get all child selection name of the control
            /// </summary>        
            /// <returns>String[] - selection names</returns>
            /// <remarks>Support Control Type: List | Tree | Menu</remarks>
            public static string[] GetSelectionItems(AutomationElement element)
            {
                string[] selectedItemsName = null;

                if (IsSupportPattern(element, SelectionPattern.Pattern))
                {
                    var pattern = (SelectionPattern)element.GetCurrentPattern(SelectionPattern.Pattern);
                    var selectedItems = pattern.Current.GetSelection();
                    selectedItemsName = new string[selectedItems.Count()];
                    const int i = 0;
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItemsName[i] = selectedItem.Current.Name;
                    }
                }

                return selectedItemsName;
            }
        }

        /// <summary>
        /// Methods for DockPattern
        /// </summary>
        public class Dock
        {
            /// <summary>
            /// Set Dock control position
            /// </summary>
            /// <param name="element"></param>
            /// <param name="dockPosition">dock Position: Fill | Bottom | Top | Left | Right | None</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: Dock</remarks>
            public static void SetDockPosition(AutomationElement element, PatternEnum.DockPosition dockPosition)
            {
                if (IsSupportPattern(element, DockPattern.Pattern))
                {
                    var dockPattern = (DockPattern)element.GetCurrentPattern(DockPattern.Pattern);
                    dockPattern.SetDockPosition(ParsePosition(dockPosition));

                    Thread.Sleep(1000);
                }
            }

            /// <summary>
            /// Get Dock control position
            /// </summary>        
            /// <returns>String - dock Position: Fill | Bottom | Top | Left | Right | None</returns>
            /// <remarks>Support Control Type: Dock</remarks>
            public static string GetDockPosition(AutomationElement element)
            {
                var position = "None";

                if (IsSupportPattern(element, DockPattern.Pattern))
                {
                    var dockPattern = (DockPattern)element.GetCurrentPattern(DockPattern.Pattern);
                    position = dockPattern.Current.DockPosition.ToString();

                    Thread.Sleep(1000);
                }

                return position;
            }
        }

        /// <summary>
        /// Methods for WindowPattern
        /// </summary>
        public class Window
        {
            /// <summary>
            /// Close Window control
            /// </summary>        
            /// <returns>void</returns>
            /// <remarks>Support Control Type: Window</remarks>
            public static void CloseWindow(AutomationElement element)
            {
                if (IsSupportPattern(element, WindowPattern.Pattern))
                {
                    var winPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
                    winPattern.Close();

                    Thread.Sleep(1000);
                }
            }

            /// <summary>
            /// Set visual state of the window control
            /// </summary>
            /// <param name="element"></param>
            /// <param name="windowState">State: Minimized | Maximize | Normal</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: Window</remarks>
            public static void SetWindowVisualState(AutomationElement element, PatternEnum.WindowVisualState windowState)
            {
                if (IsSupportPattern(element, WindowPattern.Pattern))
                {
                    var winPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
                    winPattern.SetWindowVisualState(ParseWindowVisualState(windowState));

                    Thread.Sleep(1000);
                }
            }

            /// <summary>
            /// Indicates whether the window control is timeout for waiting input
            /// </summary>
            /// <param name="element"></param>
            /// <param name="seconds">Seconds for the timeout</param>
            /// <returns>Boolean - True or False</returns>
            /// <remarks>Support Control Type: Window</remarks>
            public static bool IsWindowWaitForInputIdleTimeout(AutomationElement element, int seconds)
            {
                var isTimeout = false;

                if (IsSupportPattern(element, WindowPattern.Pattern))
                {
                    var winPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
                    isTimeout = winPattern.WaitForInputIdle(seconds * 1000);
                }

                return isTimeout;
            }

            /// <summary>
            /// Get visual state of the window control
            /// </summary>        
            /// <returns>String - State: Minimized | Maximize | Normal</returns>
            /// <remarks>Support Control Type: Window</remarks>
            public static string GetWindowVisualState(AutomationElement element)
            {
                string visualstate = null;

                if (IsSupportPattern(element, WindowPattern.Pattern))
                {
                    var winPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
                    visualstate = winPattern.Current.WindowVisualState.ToString();
                }

                return visualstate;
            }

            /// <summary>
            /// Get interaction state of the window control
            /// </summary>        
            /// <returns>String - State: BlockedByModalWindow | Closing | NotResponding | ReadyForUserInteraction | Running</returns>
            /// <remarks>Support Control Type: Window</remarks>
            public static string GetWindowInteractionState(AutomationElement element)
            {
                string ineractionstate = null;

                if (IsSupportPattern(element, WindowPattern.Pattern))
                {
                    var winPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
                    ineractionstate = winPattern.Current.WindowInteractionState.ToString();
                }

                return ineractionstate;
            }

            /// <summary>
            /// Indicates whether the control is topmost
            /// </summary>        
            /// <returns>Boolean - True or false</returns>
            /// <remarks>Support Control Type: Window</remarks>
            public static bool IsWindowTopmost(AutomationElement element)
            {
                var istopmost = false;

                if (IsSupportPattern(element, WindowPattern.Pattern))
                {
                    var winPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
                    istopmost = winPattern.Current.IsTopmost;
                }

                return istopmost;
            }

            /// <summary>
            /// Indicates whether the control is maximize
            /// </summary>        
            /// <returns>Boolean - True or false</returns>
            /// <remarks>Support Control Type: Window</remarks>
            public static bool IsWindowCanMaximize(AutomationElement element)
            {
                var canmax = false;

                if (IsSupportPattern(element, WindowPattern.Pattern))
                {
                    var winPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
                    canmax = winPattern.Current.CanMaximize;
                }

                return canmax;
            }

            /// <summary>
            /// Indicates whether the control is minimize
            /// </summary>
            /// <returns>Boolean - True or false</returns>
            /// <remarks>Support Control Type: Window</remarks>
            public static bool IsWindowCanMinimize(AutomationElement element)
            {
                var canmin = false;

                if (IsSupportPattern(element, WindowPattern.Pattern))
                {
                    var winPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
                    canmin = winPattern.Current.CanMinimize;
                }

                return canmin;
            }

            /// <summary>
            /// Indicates whether the control is modal window
            /// </summary>        
            /// <returns>Boolean - True or false</returns>
            /// <remarks>Support Control Type: Window</remarks>
            public static bool IsWindowModal(AutomationElement element)
            {
                var isModal = false;

                if (IsSupportPattern(element, WindowPattern.Pattern))
                {
                    var winPattern = (WindowPattern)element.GetCurrentPattern(WindowPattern.Pattern);
                    isModal = winPattern.Current.IsModal;
                }

                return isModal;
            }
        }

        /// <summary>
        /// Methods for ScrollPattern
        /// </summary>
        public class Scroll
        {
            /// <summary>
            /// Scroll the control by amount
            /// </summary>
            /// <param name="element"></param>
            /// <param name="scrollHorizontalAmount">Amount: LargeDecrement | LargeIncrement | SmallDecrement | SmallIncrement | NoAmount</param>
            /// <param name="scrollVerticalAmount">Horizontal or Vertical</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: ScrollBar</remarks>
            public static void ScrollByAmount(AutomationElement element, PatternEnum.ScrollAmount scrollHorizontalAmount, PatternEnum.ScrollAmount scrollVerticalAmount)
            {
                if (IsSupportPattern(element, ScrollPattern.Pattern))
                {
                    var scrollPattern = (ScrollPattern)element.GetCurrentPattern(ScrollPattern.Pattern);

                    if (scrollHorizontalAmount != PatternEnum.ScrollAmount.NoAmount)
                    {
                        var sah = ParseScrollAmount(scrollHorizontalAmount);
                        scrollPattern.ScrollHorizontal(sah);
                    }

                    if (scrollVerticalAmount != PatternEnum.ScrollAmount.NoAmount)
                    {
                        var sav = ParseScrollAmount(scrollVerticalAmount);
                        scrollPattern.ScrollVertical(sav);
                    }
                }
            }

            /// <summary>
            /// Scroll the control by percentage
            /// </summary>
            /// <param name="element"></param>
            /// <param name="scrollHorizontalPercentage"> Horizontal percentage, double type</param>
            /// <param name="scrollVerticalPercentage">Vertical percentage, double type</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: ScrollBar</remarks>
            public static void ScrollByPercentage(AutomationElement element, double scrollHorizontalPercentage, double scrollVerticalPercentage)
            {
                if (IsSupportPattern(element, ScrollPattern.Pattern))
                {
                    var scrollPattern = (ScrollPattern)element.GetCurrentPattern(ScrollPattern.Pattern);
                    scrollPattern.SetScrollPercent(scrollHorizontalPercentage, scrollVerticalPercentage);
                }
            }

            /// <summary>
            /// Indicates whether the controll support horizontally scroll
            /// </summary>
            /// <param name="element"></param>
            /// <param name="scrollAxis">PatternEnum.ScrollAxis: Horizontal or Veritical</param>
            /// <returns>Boolean - True or false</returns>
            /// <remarks>Support Control Type: ScrollBar</remarks>
            public static bool IsScrollable(AutomationElement element, PatternEnum.ScrollAxis scrollAxis)
            {
                var isScrollable = false;

                if (IsSupportPattern(element, ScrollPattern.Pattern))
                {
                    var scrollPattern = (ScrollPattern)element.GetCurrentPattern(ScrollPattern.Pattern);
                    isScrollable = scrollAxis == PatternEnum.ScrollAxis.Horizontal ? scrollPattern.Current.HorizontallyScrollable : scrollPattern.Current.VerticallyScrollable;
                }

                return isScrollable;
            }

            /// <summary>
            /// Get Scroll view size
            /// </summary>
            /// <param name="element"></param>
            /// <param name="scrollAxis">PatternEnum.ScrollAxis: Horizontal or Veritical</param>
            /// <returns>Double - size of scroll view</returns>
            /// <remarks>Support Control Type: ScrollBar</remarks>
            public static double GetScrollViewSize(AutomationElement element, PatternEnum.ScrollAxis scrollAxis)
            {
                double viewSize = 0;

                if (IsSupportPattern(element, ScrollPattern.Pattern))
                {
                    var scrollPattern = (ScrollPattern)element.GetCurrentPattern(ScrollPattern.Pattern);
                    viewSize = scrollAxis == PatternEnum.ScrollAxis.Horizontal ? scrollPattern.Current.HorizontalViewSize : scrollPattern.Current.VerticalViewSize;
                }

                return viewSize;
            }

            /// <summary>
            /// Get Scroll percentage
            /// </summary>
            /// <param name="element"></param>
            /// <param name="scrollAxis">PatternEnum.ScrollAxis: Horizontal or Veritical</param>
            /// <returns>Double - scroll percentage</returns>
            /// <remarks>Support Control Type: ScrollBar</remarks>
            public static double GetScrollPercentage(AutomationElement element, PatternEnum.ScrollAxis scrollAxis)
            {
                double scrollPercent = 0;

                if (IsSupportPattern(element, ScrollPattern.Pattern))
                {
                    var scrollPattern = (ScrollPattern)element.GetCurrentPattern(ScrollPattern.Pattern);
                    scrollPercent = scrollAxis == PatternEnum.ScrollAxis.Horizontal ? scrollPattern.Current.HorizontalScrollPercent : scrollPattern.Current.VerticalScrollPercent;
                    Console.WriteLine("Current H Percentage: {0}", scrollPattern.Current.HorizontalScrollPercent);
                    Console.WriteLine("Current V Percentage: {0}", scrollPattern.Current.VerticalScrollPercent);
                }

                return scrollPercent;
            }
        }

        /// <summary>
        /// Methods for ScrollItemPattern
        /// </summary>
        public class ScrollItem
        {
            /// <summary>
            /// Scroll specified control to current view 
            /// </summary>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: ScrollItem</remarks>
            public static void ScrollItemIntoView(AutomationElement element)
            {
                if (IsSupportPattern(element, ScrollItemPattern.Pattern))
                {
                    var scrollItemPattern = (ScrollItemPattern)element.GetCurrentPattern(ScrollItemPattern.Pattern);
                    scrollItemPattern.ScrollIntoView();
                }
            }
        }

        /// <summary>
        /// Methods for TransformPattern
        /// </summary>
        public class Transform
        {
            /// <summary>
            /// Move control to specified position
            /// </summary>
            /// <param name="element"></param>
            /// <param name="x">X axis of position</param>
            /// <param name="y">Y axis of position</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: Moveable control</remarks>
            public static void Move(AutomationElement element, double x, double y)
            {
                if (IsSupportPattern(element, TransformPattern.Pattern))
                {
                    var transformPattern = (TransformPattern)element.GetCurrentPattern(TransformPattern.Pattern);
                    transformPattern.Move(x, y);
                }
            }

            /// <summary>
            /// Resize control to specified position
            /// </summary>
            /// <param name="element"></param>
            /// <param name="x">X axis of position</param>
            /// <param name="y">Y axis of position</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: Resizeable control</remarks>
            public static void Resize(AutomationElement element, double x, double y)
            {
                if (IsSupportPattern(element, TransformPattern.Pattern))
                {
                    var transformPattern = (TransformPattern)element.GetCurrentPattern(TransformPattern.Pattern);
                    transformPattern.Resize(x, y);
                }
            }

            /// <summary>
            /// Rotate control to specified position
            /// </summary>
            /// <param name="element"></param>
            /// <param name="degrees">degrees of the rotation</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: Rotatable control</remarks>
            public static void Rotate(AutomationElement element, double degrees)
            {
                if (IsSupportPattern(element, TransformPattern.Pattern))
                {
                    var transformPattern = (TransformPattern)element.GetCurrentPattern(TransformPattern.Pattern);
                    transformPattern.Rotate(degrees);
                }
            }

            /// <summary>
            /// Indicates if the control can be moved
            /// </summary>        
            /// <returns>Boolean - True or false</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static bool CanMove(AutomationElement element)
            {
                var canMove = false;

                if (IsSupportPattern(element, TransformPattern.Pattern))
                {
                    var transformPattern = (TransformPattern)element.GetCurrentPattern(TransformPattern.Pattern);
                    canMove = transformPattern.Current.CanMove;
                }

                return canMove;
            }

            /// <summary>
            /// Indicates if the control can be resized
            /// </summary>        
            /// <returns>Boolean - True or false</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static bool CanResize(AutomationElement element)
            {
                var canResize = false;

                if (IsSupportPattern(element, TransformPattern.Pattern))
                {
                    var transformPattern = (TransformPattern)element.GetCurrentPattern(TransformPattern.Pattern);
                    canResize = transformPattern.Current.CanResize;
                }

                return canResize;
            }

            /// <summary>
            /// Indicates if the control can be rotated
            /// </summary>        
            /// <returns>Boolean - True or false</returns>
            /// <remarks>Support Control Type: All</remarks>
            public static bool CanRotate(AutomationElement element)
            {
                var canRotate = false;

                if (IsSupportPattern(element, TransformPattern.Pattern))
                {
                    var transformPattern = (TransformPattern)element.GetCurrentPattern(TransformPattern.Pattern);
                    canRotate = transformPattern.Current.CanRotate;
                }

                return canRotate;
            }
        }

        /// <summary>
        /// Methods for GridPattern
        /// </summary>
        public class Grid
        {
            /// <summary>
            /// Get item in grid
            /// </summary>
            /// <param name="element"></param>
            /// <param name="row">row index of grid item</param>
            /// <param name="column">column index of grid item</param>
            /// <returns>AutomationElement - Item</returns>
            /// <remarks>Support Control Type: Grid</remarks>
            public static AutomationElement GetGridItem(AutomationElement element, int row, int column)
            {
                AutomationElement gridItem = null;
                if (IsSupportPattern(element, GridPattern.Pattern))
                {
                    var gridPattern = (GridPattern)element.GetCurrentPattern(GridPattern.Pattern);
                    gridItem = gridPattern.GetItem(row, column);
                }

                return gridItem;
            }

            /// <summary>
            /// Get count of item in grid
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>Int - item count</returns>
            /// <remarks>Support Control Type: Grid</remarks>
            public static int GetGridItemCount(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                var count = 0;
                if (IsSupportPattern(element, GridPattern.Pattern))
                {
                    var gridPattern = (GridPattern)element.GetCurrentPattern(GridPattern.Pattern);
                    count = gridAxis == PatternEnum.Axis.Row ? gridPattern.Current.RowCount : gridPattern.Current.ColumnCount;
                }

                return count;
            }
        }

        /// <summary>
        /// Methods for GridItemPattern
        /// </summary>
        public class GridItem
        {
            /// <summary>
            /// Get container of grid item
            /// </summary>
            /// <param name="element"></param>
            /// <param name="row">row index of grid item</param>
            /// <param name="column">column index of grid item</param>
            /// <returns>AutomationElement - Container</returns>
            /// <remarks>Support Control Type: GridItem</remarks>
            public static AutomationElement GetContainingGrid(AutomationElement element, int row, int column)
            {
                AutomationElement grid = null;
                if (IsSupportPattern(element, GridItemPattern.Pattern))
                {
                    var gridItemPattern = (GridItemPattern)element.GetCurrentPattern(GridItemPattern.Pattern);
                    grid = gridItemPattern.Current.ContainingGrid;
                }

                return grid;
            }

            /// <summary>
            /// Get item index on row/column in grid
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>Int - item count</returns>
            /// <remarks>Support Control Type: GridItem</remarks>
            public static int GetGridRowColumnIndex(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                var number = 0;
                if (IsSupportPattern(element, GridItemPattern.Pattern))
                {
                    var gridItemPattern = (GridItemPattern)element.GetCurrentPattern(GridItemPattern.Pattern);
                    number = gridAxis == PatternEnum.Axis.Row ? gridItemPattern.Current.Row : gridItemPattern.Current.Column;
                }

                return number;
            }

            /// <summary>
            /// Get row/column span in grid
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>Int - item count</returns>
            /// <remarks>Support Control Type: GridItem</remarks>
            public static int GetGridRowColumnSpan(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                var span = 0;
                if (IsSupportPattern(element, GridItemPattern.Pattern))
                {
                    var gridItemPattern = (GridItemPattern)element.GetCurrentPattern(GridItemPattern.Pattern);
                    span = gridAxis == PatternEnum.Axis.Row ? gridItemPattern.Current.RowSpan : gridItemPattern.Current.ColumnSpan;
                }

                return span;
            }
        }

        /// <summary>
        /// Methods for TablePattern
        /// </summary>
        public class Table
        {
            /// <summary>
            /// Get item in table
            /// </summary>
            /// <param name="element"></param>
            /// <param name="row">row index of table item</param>
            /// <param name="column">column index of table item</param>
            /// <returns>AutomationElement - Item</returns>
            /// <remarks>Support Control Type: Table</remarks>
            public static AutomationElement GetTableItem(AutomationElement element, int row, int column)
            {
                AutomationElement tableItem = null;
                if (IsSupportPattern(element, TablePattern.Pattern))
                {
                    var tablePattern = (TablePattern)element.GetCurrentPattern(TablePattern.Pattern);
                    tableItem = tablePattern.GetItem(row, column);
                }

                return tableItem;
            }

            /// <summary>
            /// Get count of item in table
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>Int - item count</returns>
            /// <remarks>Support Control Type: Table</remarks>
            public static int GetTableItemCount(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                var count = 0;
                if (IsSupportPattern(element, TablePattern.Pattern))
                {
                    var tablePattern = (TablePattern)element.GetCurrentPattern(TablePattern.Pattern);
                    count = gridAxis == PatternEnum.Axis.Row ? tablePattern.Current.RowCount : tablePattern.Current.ColumnCount;
                }

                return count;
            }

            /// <summary>
            /// Get headers name of row/column from table
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>String[] - header names</returns>
            /// <remarks>Support Control Type: Table</remarks>
            public static string[] GetTableHeadersName(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                var headers = GetTableHeaders(element, gridAxis);
                if (headers != null)
                    return null;

                if (headers.Length == 0)
                    return null;

                var headerNames = new string[headers.Length];

                var i = 0;
                foreach (var ae in headers)
                {
                    headerNames[i] = ae.Current.Name;
                    i++;
                }

                return headerNames;
            }


            /// <summary>
            /// Get headers of row/column from table
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>AutomationElement[] - headers</returns>
            /// <remarks>Support Control Type: Table</remarks>
            public static AutomationElement[] GetTableHeaders(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                AutomationElement[] headers = null;

                if (IsSupportPattern(element, TablePattern.Pattern))
                {
                    var tablePattern = (TablePattern)element.GetCurrentPattern(TablePattern.Pattern);
                    headers = gridAxis == PatternEnum.Axis.Row ? tablePattern.Current.GetRowHeaders() : tablePattern.Current.GetColumnHeaders();
                }

                return headers;
            }

            /// <summary>
            /// Get major of row/column from table
            /// </summary>
            /// <param name="element"></param>
            /// <returns>String - Major name</returns>
            /// <remarks>Support Control Type: Table</remarks>
            public static string GetTableRowColumnMajor(AutomationElement element)
            {
                string major = null;

                if (IsSupportPattern(element, TablePattern.Pattern))
                {
                    var tablePattern = (TablePattern)element.GetCurrentPattern(TablePattern.Pattern);
                    major = tablePattern.Current.RowOrColumnMajor.ToString();
                }

                return major;
            }
        }

        /// <summary>
        /// Methods for TableItemPattern
        /// </summary>
        public class TableItem
        {
            /// <summary>
            /// Get container of table item
            /// </summary>
            /// <param name="element"></param>
            /// <param name="row">row index of table item</param>
            /// <param name="column">column index of table item</param>
            /// <returns>AutomationElement - Container</returns>
            /// <remarks>Support Control Type: TableItem</remarks>
            public static AutomationElement GetContainingTableGrid(AutomationElement element, int row, int column)
            {
                AutomationElement tableGrid = null;
                if (IsSupportPattern(element, TableItemPattern.Pattern))
                {
                    var tableItemPattern = (TableItemPattern)element.GetCurrentPattern(TableItemPattern.Pattern);
                    tableGrid = tableItemPattern.Current.ContainingGrid;
                }

                return tableGrid;
            }

            /// <summary>
            /// Get index of row/column from table item
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>Int - item count</returns>
            /// <remarks>Support Control Type: TableItem</remarks>
            public static int GetTableItemRowColumnIndex(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                var number = 0;
                if (IsSupportPattern(element, TableItemPattern.Pattern))
                {
                    var tableItemPattern = (TableItemPattern)element.GetCurrentPattern(TableItemPattern.Pattern);
                    number = gridAxis == PatternEnum.Axis.Row ? tableItemPattern.Current.Row : tableItemPattern.Current.Column;
                }

                return number;
            }

            /// <summary>
            /// Get row/column span in table item
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>Int - item count</returns>
            /// <remarks>Support Control Type: GridItem</remarks>
            public static int GetTableItemRowColumnSpan(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                var span = 0;
                if (IsSupportPattern(element, TableItemPattern.Pattern))
                {
                    var tableItemPattern = (TableItemPattern)element.GetCurrentPattern(TableItemPattern.Pattern);
                    span = gridAxis == PatternEnum.Axis.Row ? tableItemPattern.Current.RowSpan : tableItemPattern.Current.ColumnSpan;
                }

                return span;
            }

            /// <summary>
            /// Get headers name of row/column from table item
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>String[] - header names</returns>
            /// <remarks>Support Control Type: TableItem</remarks>
            public static string[] GetTableItemHeadersName(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                var headers = GetTableItemHeaders(element, gridAxis);
                if (headers != null)
                    return null;

                if (headers.Length == 0)
                    return null;

                var headerNames = new string[headers.Length];

                var i = 0;
                foreach (var ae in headers)
                {
                    headerNames[i] = ae.Current.Name;
                    i++;
                }

                return headerNames;
            }

            /// <summary>
            /// Get headers of row/column from table item
            /// </summary>
            /// <param name="element"></param>
            /// <param name="gridAxis">Indicates whether calculate in row or column</param>
            /// <returns>AutomationElement[] - headers</returns>
            /// <remarks>Support Control Type: TableItem</remarks>
            public static AutomationElement[] GetTableItemHeaders(AutomationElement element, PatternEnum.Axis gridAxis)
            {
                AutomationElement[] headers = null;
                if (IsSupportPattern(element, TableItemPattern.Pattern))
                {
                    var tableItemPattern = (TableItemPattern)element.GetCurrentPattern(TableItemPattern.Pattern);
                    headers = gridAxis == PatternEnum.Axis.Row ? tableItemPattern.Current.GetRowHeaderItems() : tableItemPattern.Current.GetColumnHeaderItems();
                }

                return headers;
            }
        }

        /// <summary>
        /// Methods for RangeValuePattern
        /// </summary>
        public class RangeValue
        {
            /// <summary>
            /// Set Range value
            /// </summary>
            /// <param name="element"></param>
            /// <param name="value">Double - value for range</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: TBD</remarks>
            public static void SetRangeValue(AutomationElement element, double value)
            {
                if (IsSupportPattern(element, RangeValuePattern.Pattern))
                {
                    var rangeValuePattern = (RangeValuePattern)element.GetCurrentPattern(RangeValuePattern.Pattern);
                    rangeValuePattern.SetValue(value);
                }
            }

            /// <summary>
            /// Indicates whether range value is readonly
            /// </summary>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: TBD</remarks>
            public static bool IsRangeValueReadOnly(AutomationElement element)
            {
                var isReadOnly = false;

                if (IsSupportPattern(element, RangeValuePattern.Pattern))
                {
                    var rangeValuePattern = (RangeValuePattern)element.GetCurrentPattern(RangeValuePattern.Pattern);
                    isReadOnly = rangeValuePattern.Current.IsReadOnly;
                }

                return isReadOnly;
            }

            /// <summary>
            /// Get Range value
            /// </summary>
            /// <param name="element"></param>
            /// <param name="rvi">Type: value, largechange, smallchange, max, min</param>
            /// <returns>double</returns>
            /// <remarks>Support Control Type: TBD</remarks>
            public static double GetRangeValue(AutomationElement element, PatternEnum.RangeValueInformation rvi)
            {
                double value = 0;
                if (IsSupportPattern(element, RangeValuePattern.Pattern))
                {
                    var rangeValuePattern = (RangeValuePattern)element.GetCurrentPattern(RangeValuePattern.Pattern);
                    switch (rvi)
                    {
                        case PatternEnum.RangeValueInformation.Value:
                            value = rangeValuePattern.Current.Value;
                            break;
                        case PatternEnum.RangeValueInformation.LargeChange:
                            value = rangeValuePattern.Current.LargeChange;
                            break;
                        case PatternEnum.RangeValueInformation.SmallChange:
                            value = rangeValuePattern.Current.SmallChange;
                            break;
                        case PatternEnum.RangeValueInformation.Max:
                            value = rangeValuePattern.Current.Maximum;
                            break;
                        case PatternEnum.RangeValueInformation.Min:
                            value = rangeValuePattern.Current.Minimum;
                            break;
                    }
                }

                return value;
            }
        }

        /// <summary>
        /// Methods for MultipleViewPattern
        /// </summary>
        public class MultipleView
        {
            /// <summary>
            /// Set specified view as current view
            /// </summary>
            /// <param name="element"></param>
            /// <param name="viewId">Specified View ID</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: MultipleView</remarks>
            public static void SetCurrentView(AutomationElement element, int viewId)
            {
                if (IsSupportPattern(element, MultipleViewPattern.Pattern))
                {
                    var multipleViewPattern = (MultipleViewPattern)element.GetCurrentPattern(MultipleViewPattern.Pattern);
                    multipleViewPattern.SetCurrentView(viewId);
                }
            }

            /// <summary>
            /// Get specified view name
            /// </summary>
            /// <param name="element"></param>
            /// <param name="viewId">Specified View ID</param>
            /// <returns>String - view name</returns>
            /// <remarks>Support Control Type: MultipleView</remarks>
            public static string GetViewName(AutomationElement element, int viewId)
            {
                string viewname = null;

                if (IsSupportPattern(element, MultipleViewPattern.Pattern))
                {
                    var multipleViewPattern = (MultipleViewPattern)element.GetCurrentPattern(MultipleViewPattern.Pattern);
                    viewname = multipleViewPattern.GetViewName(viewId);
                }

                return viewname;
            }

            /// <summary>
            /// Get specified view id
            /// </summary>
            /// <param name="element"></param>
            /// <returns>Int - view id</returns>
            /// <remarks>Support Control Type: MultipleView</remarks>
            public static int GetCurrentViewId(AutomationElement element)
            {
                var currentviewId = 0;

                if (IsSupportPattern(element, MultipleViewPattern.Pattern))
                {
                    var multipleViewPattern = (MultipleViewPattern)element.GetCurrentPattern(MultipleViewPattern.Pattern);
                    currentviewId = multipleViewPattern.Current.CurrentView;
                }

                return currentviewId;
            }

            /// <summary>
            /// Get all view ids
            /// </summary>
            /// <returns>Int[] - view ids</returns>
            /// <remarks>Support Control Type: MultipleView</remarks>
            public static int[] GetSupportedViewsId(AutomationElement element)
            {
                int[] supportedViewsId = null;

                if (IsSupportPattern(element, MultipleViewPattern.Pattern))
                {
                    var multipleViewPattern = (MultipleViewPattern)element.GetCurrentPattern(MultipleViewPattern.Pattern);
                    supportedViewsId = multipleViewPattern.Current.GetSupportedViews();
                }

                return supportedViewsId;
            }
        }

        /// <summary>
        /// Methods for TextPattern
        /// </summary>
        public class Text
        {
            /// <summary>
            /// Get selected text
            /// </summary>
            /// <returns>String - selected text</returns>
            /// <remarks>Support Control Type: Text</remarks>
            public static string GetSupportedTextSelection(AutomationElement element)
            {
                string supportedTextSelection = null;

                if (IsSupportPattern(element, TextPattern.Pattern))
                {
                    var textPattern = (TextPattern)element.GetCurrentPattern(TextPattern.Pattern);
                    supportedTextSelection = textPattern.SupportedTextSelection.ToString();
                }

                return supportedTextSelection;
            }
        }

        /// <summary>
        /// Methods for Emulation
        /// </summary>
        public class Emulation
        {
            /// <summary>
            /// Mouse click control
            /// </summary>
            /// <returns>void</returns>
            public static void RightClick(AutomationElement element)
            {
                var rect = element.Current.BoundingRectangle;
                RightClick(element, Convert.ToInt32(rect.Width / 2), Convert.ToInt32(rect.Height / 2));
            }

            /// <summary>
            /// Mouse click control with offset
            /// </summary>
            /// <param name="element"></param>
            /// <param name="xOffset">offset in X axis</param>
            /// <param name="yOffset">offset in Y axis</param>
            public static void RightClick(AutomationElement element, int xOffset, int yOffset)
            {
                var rect = element.Current.BoundingRectangle;
                var xpoint = Convert.ToInt32(rect.X + xOffset);
                var ypoint = Convert.ToInt32(rect.Y + yOffset);
                Console.WriteLine("Clicking on X axis: {0}, and on Y axix {1}", xpoint, ypoint);
                MouseEmulate.Move(xpoint, ypoint);
                MouseEmulate.Click(MouseButton.Right);
                Thread.Sleep(500);
            }

            /// <summary>
            /// Wheeling the mouse
            /// </summary>
            /// <param name="element"></param>
            /// <param name="delta">Delta of 120 wheels up once normally, -120 wheels down once normally</param>
            public static void Wheeling(AutomationElement element, int delta)
            {
                var rect = element.Current.BoundingRectangle;
                MouseEmulate.Move(Convert.ToInt32(rect.X + rect.Width / 2), Convert.ToInt32(rect.Y + rect.Height / 2));
                MouseEmulate.MouseWheel(delta);
                Thread.Sleep(500);
            }

            /// <summary>
            /// Mouse click control
            /// </summary>
            /// <returns>void</returns>
            public static void MouseClick(AutomationElement element)
            {
                var rect = element.Current.BoundingRectangle;
                MouseClick(element, Convert.ToInt32(rect.Width / 2), Convert.ToInt32(rect.Height / 2));
            }

            /// <summary>
            /// Mouse click control with offset
            /// </summary>
            /// <returns>void</returns>
            /// <param name="element"></param>
            /// <param name="xOffset">offset in X axis</param>
            /// <param name="yOffset">offset in Y axis</param>
            public static void MouseClick(AutomationElement element, int xOffset, int yOffset)
            {
                var rect = element.Current.BoundingRectangle;
                var xpoint = Convert.ToInt32(rect.X + xOffset);
                var ypoint = Convert.ToInt32(rect.Y + yOffset);
                Console.WriteLine("Clicking on X axis: {0}, and on Y axix {1}", xpoint, ypoint);
                MouseEmulate.Move(xpoint, ypoint);
                MouseEmulate.Click(MouseButton.Left);
                Thread.Sleep(500);
            }

            /// <summary>
            /// Mouse click control
            /// </summary>
            /// <returns>void</returns>
            public static void DoubleClick(AutomationElement element)
            {
                var rect = element.Current.BoundingRectangle;
                DoubleClick(element, Convert.ToInt32(rect.Width / 2), Convert.ToInt32(rect.Height / 2));
            }

            /// <summary>
            /// Mouse click control with offset
            /// </summary>
            /// <returns>void</returns>
            /// <param name="element"></param>
            /// <param name="xOffset">offset in X axis</param>
            /// <param name="yOffset">offset in Y axis</param>
            public static void DoubleClick(AutomationElement element, int xOffset, int yOffset)
            {
                var rect = element.Current.BoundingRectangle;
                var xpoint = Convert.ToInt32(rect.X + xOffset);
                var ypoint = Convert.ToInt32(rect.Y + yOffset);
                Console.WriteLine("Clicking on X axis: {0}, and on Y axix {1}", xpoint, ypoint);
                MouseEmulate.Move(xpoint, ypoint);
                MouseEmulate.DoubleClick(MouseButton.Left);
                Thread.Sleep(500);
            }

            /// <summary>
            /// Mouse over control
            /// </summary>
            /// <returns>void</returns>
            public static void MouseOver(AutomationElement element)
            {
                var rect = element.Current.BoundingRectangle;
                MouseOver(element, Convert.ToInt32(rect.Width / 2), Convert.ToInt32(rect.Height / 2));
            }

            /// <summary>
            /// Mouse over control
            /// </summary>
            /// <returns>void</returns>
            /// <param name="element"></param>
            /// <param name="xOffset">offset in X axis</param>
            /// <param name="yOffset">offset in Y axis</param>
            public static void MouseOver(AutomationElement element, int xOffset, int yOffset)
            {
                var rect = element.Current.BoundingRectangle;
                int xpoint = Convert.ToInt32(rect.X + xOffset);
                int ypoint = Convert.ToInt32(rect.Y + yOffset);
                Console.WriteLine("Clicking on X axis: {0}, and on Y axix {1}", xpoint, ypoint);
                MouseEmulate.Move(xpoint, ypoint);
                Thread.Sleep(500);
            }

            /// <summary>
            /// Drag and drop the object.
            /// </summary>
            /// <param name="element"></param>
            /// <param name="fromXOffset"></param>
            /// <param name="fromYOffset"></param>
            /// <param name="toXOffset"></param>
            /// <param name="toYOffset"></param>
            public static void DragDrop(AutomationElement element, int fromXOffset, int fromYOffset, int toXOffset, int toYOffset)
            {
                var rect = element.Current.BoundingRectangle;
                var fromXpoint = Convert.ToInt32(rect.X + fromXOffset);
                var fromYpoint = Convert.ToInt32(rect.Y + fromYOffset);
                var toXpoint = Convert.ToInt32(rect.X + toXOffset);
                var toYpoint = Convert.ToInt32(rect.Y + toYOffset);

                Console.WriteLine("Drag something on X axis: {0}, and on Y axix {1}, drop to X axis: {2}, and on Y axix {3}", fromXpoint, fromYpoint, toXpoint, toYpoint);
                MouseEmulate.Move(fromXpoint, fromYpoint);
                MouseEmulate.MouseDown(MouseButton.Left);
                Thread.Sleep(200);
                MouseEmulate.Move(toXpoint, toYpoint);
                Thread.Sleep(200);
                MouseEmulate.MouseUp(MouseButton.Left);
                Thread.Sleep(500);
            }

            /// <summary>
            /// Send value to control
            /// </summary>
            /// <param name="element"></param>
            /// <param name="value">value which to be sent</param>
            /// <returns>void</returns>
            /// <remarks>Support Control Type: Edit</remarks>
            public static void SendValue(AutomationElement element, string value)
            {
                MouseClick(element);
                SendKeys.SendWait(value);
                Thread.Sleep(500);
            }

            /// <summary>
            /// Show / no show cursor of the mouse
            /// </summary>
            /// <param name="element"></param>
            /// <param name="isShow"></param>
            public static void ShowCursor(AutomationElement element, bool isShow)
            {
                var rect = element.Current.BoundingRectangle;
                MouseEmulate.Move(Convert.ToInt32(rect.X + rect.Width / 2), Convert.ToInt32(rect.Y + rect.Height / 2));
                if (isShow)
                    MouseEmulate.Show();
                else
                    MouseEmulate.Hide();
                Thread.Sleep(500);
            }

            /// <summary>
            /// Enter the string by keyboard emulation
            /// </summary>
            /// <param name="value">string value</param>
            public static void KeyBoardEnter(string value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    InputSimulator.SimulateTextEntry(value);
                    Thread.Sleep(500);
                }
            }

            /// <summary>
            /// Press Combination keys
            /// </summary>
            /// <param name="combinationkey">Format: [Key]+[Key]+[Key]..., for example: Ctrl+Alt+D</param>
            public static void PressCombinationKeys(string combinationkey)
            {
                if (!string.IsNullOrEmpty(combinationkey))
                {
                    var ckeys = combinationkey.Split(new[] { '+' });
                    for(var i = 0; i < ckeys.Length - 1; i++)
                    {
                        KeyboardEmulate.KeyDown((Keys)Enum.Parse(typeof(Keys), ckeys[i]));
                    }
                    KeyboardEmulate.KeyPress((Keys)char.ToUpper(ckeys[ckeys.Length - 1][0]));
                    Thread.Sleep(500);

                    for (var i = 0; i < ckeys.Length - 1; i++)
                    {
                        KeyboardEmulate.KeyUp((Keys)Enum.Parse(typeof(Keys), ckeys[i]));
                    }
                }
            }

            /// <summary>
            /// Press short cut keys
            /// </summary>
            /// <param name="shortcut">Copy, Cut, Paste, SelectAll, Save, Open, New, Close, Print</param>
            public static void PressShortCutKeys(StandardShortcut shortcut)
            {
                KeyboardEmulate.SimulateStandardShortcut(shortcut);
            }
        }

        #region private functions
        /// <summary>
        /// Check pattern is supported for the automation element
        /// </summary>
        /// <param name="element">Automation element</param>
        /// <param name="pattern">pattern type</param>
        /// <returns>true or false</returns>
        private static bool IsSupportPattern(AutomationElement element, AutomationPattern pattern)
        {
            object obj;

            if (element.TryGetCurrentPattern(pattern, out obj))
                return true;
            throw new Exception(string.Format("Current element does not support {0}", pattern.ProgrammaticName));
        }

        private static DockPosition ParsePosition(PatternEnum.DockPosition position)
        {
            DockPosition dp;

            switch (position)
            {
                case PatternEnum.DockPosition.Top:
                    dp = DockPosition.Top;
                    break;
                case PatternEnum.DockPosition.Bottom:
                    dp = DockPosition.Bottom;
                    break;
                case PatternEnum.DockPosition.Left:
                    dp = DockPosition.Left;
                    break;
                case PatternEnum.DockPosition.Right:
                    dp = DockPosition.Right;
                    break;
                case PatternEnum.DockPosition.Fill:
                    dp = DockPosition.Fill;
                    break;
                default:
                    dp = DockPosition.None;
                    break;
            }

            return dp;
        }

        private static WindowVisualState ParseWindowVisualState(PatternEnum.WindowVisualState state)
        {
            WindowVisualState wvs;

            switch (state)
            {
                case PatternEnum.WindowVisualState.Minimized:
                    wvs = WindowVisualState.Minimized;
                    break;
                case PatternEnum.WindowVisualState.Maximized:
                    wvs = WindowVisualState.Maximized;
                    break;
                default:
                    wvs = WindowVisualState.Normal;
                    break;
            }

            return wvs;
        }

        private static ScrollAmount ParseScrollAmount(PatternEnum.ScrollAmount amount)
        {
            ScrollAmount sa;

            switch (amount)
            {
                case PatternEnum.ScrollAmount.LargeDecrement:
                    sa = ScrollAmount.LargeDecrement;
                    break;
                case PatternEnum.ScrollAmount.LargeIncrement:
                    sa = ScrollAmount.LargeIncrement;
                    break;
                case PatternEnum.ScrollAmount.SmallDecrement:
                    sa = ScrollAmount.SmallDecrement;
                    break;
                case PatternEnum.ScrollAmount.SmallIncrement:
                    sa = ScrollAmount.SmallIncrement;
                    break;
                default:
                    sa = ScrollAmount.NoAmount;
                    break;
            }

            return sa;
        }
        #endregion
    }
}
