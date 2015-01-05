using System;
using System.Windows.Automation;
using System.Threading;
using UIEngine;
using System.Collections.Generic;
using System.Drawing;
using AutomationHelper;

namespace HP.LR.Test.UIOperation
{
    /// <summary>
    /// Base class for UIOperation, includes some properties methods
    /// </summary>
    public class Basic
    {
        /// <summary>
        /// Indicates whether the control is in enabled status.
        /// </summary>
        /// <returns>Boolean - True or False</returns>
        public static bool IsEnabled(UIControl control)
        {
            return ClassicBase.Properties.IsEnabled((AutomationElement)control.Element);
        }

        /// <summary>
        /// Indicates whether the control is password control.
        /// </summary>
        /// <returns>Boolean - True or False</returns>
        public static bool IsPassword(UIControl control)
        {
            return ClassicBase.Properties.IsPassword((AutomationElement)control.Element);
        }

        /// <summary>
        /// Indicates whether the control is out of screen.
        /// </summary>        
        /// <returns>Boolean - True or False</returns>
        public static bool IsOffScreen(UIControl control)
        {
            return ClassicBase.Properties.IsOffScreen((AutomationElement)control.Element);
        }

        /// <summary>
        /// Check whether the control is required control in current form.
        /// </summary>        
        /// <returns>Boolean - True or False</returns>
        public static bool IsRequiredForForm(UIControl control)
        {
            return ClassicBase.Properties.IsRequiredforForm((AutomationElement)control.Element);
        }

        /// <summary>
        /// Indicates whether the entered or selected value is valid for the form rule associated.
        /// </summary>        
        /// <returns>Boolean - True or False</returns>
        //public static bool IsDataValidForForm(UIControl control)
        //{
        //    return ClassicBase.Properties.IsDataValidForForm((AutomationElement)control.Element);
        //}

        /// <summary>
        /// Indicates whether the keyboard is focusable for the control.
        /// </summary>        
        /// <returns>True or False</returns>
        public static bool IsKeyboardFocusable(UIControl control)
        {
            return ClassicBase.Properties.IsKeyboardFocusable((AutomationElement)control.Element);
        }

        /// <summary>
        /// Return Name property value for the control.
        /// </summary>        
        /// <returns>String - Control Name</returns>
        public static string GetName(UIControl control)
        {
            return ClassicBase.Properties.GetName((AutomationElement)control.Element);
        }

        /// <summary>
        /// Return the runtime id for the control.
        /// </summary>        
        /// <returns>String - Runtime ID</returns>
        public static string GetRuntimeId(UIControl control)
        {
            return ClassicBase.Properties.GetRuntimeId((AutomationElement)control.Element);
        }

        /// <summary>
        /// Return the runtime id for the control.
        /// </summary>
        /// <returns>String - Runtime ID</returns>
        public static string GetFrameworkId(UIControl control)
        {
            return ClassicBase.Properties.GetFrameworkId((AutomationElement)control.Element);
        }

        /// <summary>
        /// Return the access key for the control.
        /// </summary>
        /// <returns>String - Access Key</returns>
        public static string GetAccessKey(UIControl control)
        {
            return ClassicBase.Properties.GetAccessKey((AutomationElement)control.Element);
        }

        /// <summary>
        /// Return the help text for the control.
        /// </summary>        
        /// <returns>String - Help Text</returns>
        public static string GetHelpText(UIControl control)
        {
            return ClassicBase.Properties.GetHelpText((AutomationElement)control.Element);
        }

        /// <summary>
        /// Get Width of the control
        /// </summary>        
        /// <returns>Width</returns>
        public static  double GetWidth(UIControl control)
        {
            double width = 0;
            
            var rect = ClassicBase.Properties.GetBoundingRectangle((AutomationElement)control.Element);
            if (rect != null)
            {
                width = rect.Width;
            }

            return width;
        }

        /// <summary>
        /// Get height of the control
        /// </summary>        
        /// <returns>Width</returns>
        public static double GetHeight(UIControl control)
        {
            double height = 0;

            var rect = ClassicBase.Properties.GetBoundingRectangle((AutomationElement)control.Element);
            if (rect != null)
            {
                height = rect.Height;
            }

            return height;
        }

        /// <summary>
        /// Get top of the control
        /// </summary>        
        /// <returns>Width</returns>
        public static double GetTopPosition(UIControl control)
        {
            double top = 0;

            var rect = ClassicBase.Properties.GetBoundingRectangle((AutomationElement)control.Element);
            if (rect != null)
            {
                top = rect.Top;
            }

            return top;
        }

        /// <summary>
        /// Get left of the control
        /// </summary>        
        /// <returns>Width</returns>
        public static double GetLeftPosition(UIControl control)
        {
            double left = 0;

            var rect = ClassicBase.Properties.GetBoundingRectangle((AutomationElement)control.Element);
            if (rect != null)
            {
                left = rect.Left;
            }

            return left;
        }

        /// <summary>
        /// Move control to specified position
        /// </summary>
        /// <param name="control"></param>
        /// <param name="x">X axis of position</param>
        /// <param name="y">Y axis of position</param>
        /// <returns>void</returns>
        public static void Move(UIControl control, double x, double y)
        {
            ClassicBase.Transform.Move((AutomationElement)control.Element, x, y);
        }

        /// <summary>
        /// Resize control to specified position
        /// </summary>
        /// <param name="control"></param>
        /// <param name="x">X axis of position</param>
        /// <param name="y">Y axis of position</param>
        /// <returns>void</returns>
        public static void Resize(UIControl control, double x, double y)
        {
            ClassicBase.Transform.Resize((AutomationElement)control.Element, x, y);
        }

        /// <summary>
        /// Rotate control to specified position
        /// </summary>
        /// <param name="control"></param>
        /// <param name="degrees">degrees of the rotation</param>
        /// <returns>void</returns>
        public static void Rotate(UIControl control, double degrees)
        {
            ClassicBase.Transform.Rotate((AutomationElement)control.Element, degrees);
        }

        /// <summary>
        /// Indicates if the control can be moved
        /// </summary>        
        /// <returns>Boolean - True or false</returns>
        public static bool IsCanMove(UIControl control)
        {
            return ClassicBase.Transform.CanMove((AutomationElement)control.Element);
        }

        /// <summary>
        /// Indicates if the control can be resized
        /// </summary>        
        /// <returns>Boolean - True or false</returns>
        public static bool IsCanResize(UIControl control)
        {
            return ClassicBase.Transform.CanResize((AutomationElement)control.Element);
        }

        /// <summary>
        /// Indicates if the control can be rotated
        /// </summary>        
        /// <returns>Boolean - True or false</returns>
        public static bool IsCanRotate(UIControl control)
        {
            return ClassicBase.Transform.CanRotate((AutomationElement)control.Element);
        }

        /// <summary>
        /// Send value to control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value">value which to be sent</param>
        /// <returns>void</returns>
        /// <remarks>Support Control Type: Edit</remarks>
        public static void SendKeys(UIControl control, string value)
        {
            ClassicBase.Emulation.SendValue((AutomationElement)control.Element, value);
        }

        /// <summary>
        /// Mouse click control
        /// </summary>
        /// <returns>void</returns>
        /// <remarks>Support Control Type: All</remarks>
        public static void MouseClick(UIControl control)
        {
            ClassicBase.Emulation.MouseClick((AutomationElement)control.Element);
        }        

        /// <summary>
        /// Mouse click control
        /// </summary>
        /// <returns>void</returns>
        /// <remarks>Support Control Type: All</remarks>
        public static void MouseClick(UIControl control, int xOffset, int yOffset)
        {
            ClassicBase.Emulation.MouseClick((AutomationElement)control.Element, xOffset, yOffset);
        }

        /// <summary>
        /// Mouse right click control
        /// </summary>
        /// <returns>void</returns>
        public static void MouseRightClick(UIControl control)
        {
            ClassicBase.Emulation.RightClick((AutomationElement)control.Element);
        }

        /// <summary>
        /// Mouse right click control with offset
        /// </summary>
        /// <param name="control"></param>
        /// <param name="xOffset">offset in X axis</param>
        /// <param name="yOffset">offset in Y axis</param>
        public static void MouseRightClick(UIControl control, int xOffset, int yOffset)
        {
            ClassicBase.Emulation.RightClick((AutomationElement)control.Element, xOffset, yOffset);
        }

        /// <summary>
        /// Mouse click control
        /// </summary>
        /// <returns>void</returns>
        public static void MouseDoubleClick(UIControl control)
        {
            ClassicBase.Emulation.DoubleClick((AutomationElement)control.Element);
        }

        /// <summary>
        /// Mouse click control with offset
        /// </summary>
        /// <returns>void</returns>
        /// <param name="control"></param>
        /// <param name="xOffset">offset in X axis</param>
        /// <param name="yOffset">offset in Y axis</param>
        public static void MouseDoubleClick(UIControl control, int xOffset, int yOffset)
        {
            ClassicBase.Emulation.DoubleClick((AutomationElement)control.Element, xOffset, yOffset);
        }

        /// <summary>
        /// Wheeling the mouse
        /// </summary>
        /// <param name="control"></param>
        /// <param name="delta">Delta of 120 wheels up once normally, -120 wheels down once normally</param>
        public static void MouseWheeling(UIControl control, int delta)
        {
            ClassicBase.Emulation.Wheeling((AutomationElement)control.Element, delta);
        }

        /// <summary>
        /// Mouse over control
        /// </summary>
        /// <returns>void</returns>
        /// <remarks>Support Control Type: All</remarks>
        public static void MouseOver(UIControl control)
        {
            ClassicBase.Emulation.MouseOver((AutomationElement)control.Element);
        }

        /// <summary>
        /// Mouse over control
        /// </summary>
        /// <returns>void</returns>
        /// <remarks>Support Control Type: All</remarks>
        public static void MouseOver(UIControl control, int xOffset, int yOffset)
        {
            ClassicBase.Emulation.MouseOver((AutomationElement)control.Element, xOffset, yOffset);
        }

        /// <summary>
        /// Drag and drop something
        /// </summary>
        /// <param name="control"></param>
        /// <param name="fromXOffset"></param>
        /// <param name="fromYOffset"></param>
        /// <param name="toXOffset"></param>
        /// <param name="toYOffset"></param>
        public static void DragDrop(UIControl control, int fromXOffset, int fromYOffset, int toXOffset, int toYOffset)
        {
            ClassicBase.Emulation.DragDrop((AutomationElement)control.Element, fromXOffset, fromYOffset, toXOffset, toYOffset);
        }

        /// <summary>
        /// Enter some text by keyboard with axis.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="text"></param>
        public static void KeyBoardEnter(UIControl control, int xOffset, int yOffset, string text)
        {
            MouseClick(control, xOffset, yOffset);
            Thread.Sleep(1000);
            ClassicBase.Emulation.KeyBoardEnter(text);
        }

        /// <summary>
        /// enter some text by keyboard
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        public static void KeyBoardEnter(UIControl control, string text)
        {
            MouseClick(control);
            Thread.Sleep(500);
            ClassicBase.Emulation.KeyBoardEnter(text);
        }

        /// <summary>
        /// take snapshot of control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="path">PNG file, for example: C:\Temp\test.png</param>
        public static void TakeSnapshot(UIControl control, string path)
        {
            System.Drawing.Bitmap bmp = ClassicBase.Properties.GetSnapshot((AutomationElement)control.Element);

            bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    /// <summary>
    /// Button type control UIOperation methods.
    /// </summary>
    public class Button : Basic
    {
        /// <summary>
        /// Invoke the control (Click).
        /// </summary>
        public static void Click(UIControl control)
        {
            ClassicBase.Invoke.Click((AutomationElement)control.Element);
        }

        /// <summary>
        /// Expand the control.
        /// </summary>
        public static void Expand(UIControl control)
        {
            ClassicBase.ExpandCollapse.Expand((AutomationElement)control.Element);
        }

        /// <summary>
        /// Collapse the control.
        /// </summary>
        public static void Collapse(UIControl control)
        {
            ClassicBase.ExpandCollapse.Collapse((AutomationElement)control.Element);
        }
    }

    /// <summary>
    /// Menu type control UIOperation methods.
    /// </summary>
    public class Menu : Basic
    {
        /// <summary>
        /// Expand the control.
        /// </summary>
        public static void Expand(UIControl control)
        {
            ClassicBase.ExpandCollapse.Expand((AutomationElement)control.Element);
        }

        /// <summary>
        /// Collapse the control.
        /// </summary>
        public static void Collapse(UIControl control)
        {
            ClassicBase.ExpandCollapse.Collapse((AutomationElement)control.Element);
        }

        /// <summary>
        /// Select a menu item in a menu control, support hireachy.
        /// </summary>
        /// <param name="control">UIControl</param>
        /// <param name="menuPath">Menu path, format: Menu1|SubMenu|SubSubMenu</param>
        public static void Select(UIControl control, string menuPath)
        {
            ExtensionBase.Menu.SelectMenuItem((AutomationElement)control.Element, menuPath);
        }

        /// <summary>
        /// Get all sub menu items under the specified menu item.
        /// </summary>
        /// <param name="control">UIControl</param>
        /// <param name="menuPath">Menu path, format: Menu1|SubMenu|SubSubMenu</param>
        /// <returns>Menu items name array</returns>
        public static string[] GetAllSubItems(UIControl control, string menuPath)
        {
            return ExtensionBase.Menu.GetAllSubItems((AutomationElement)control.Element, menuPath);
        }
    }

    /// <summary>
    /// Window type control UIOperation methods.
    /// </summary>
    public class Window : Basic
    {
        /// <summary>
        /// Close Window control
        /// </summary>        
        /// <returns>void</returns>
        public static void CloseWindow(UIControl control)
        {
            ClassicBase.Window.CloseWindow((AutomationElement)control.Element);
        }

        /// <summary>
        /// Set visual state of the window control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="windowState">State: Minimized | Maximize | Normal</param>
        /// <returns>void</returns>
        public static void SetWindowVisualState(UIControl control, PatternEnum.WindowVisualState windowState)
        {
            ClassicBase.Window.SetWindowVisualState((AutomationElement)control.Element, windowState);
        }

        /// <summary>
        /// Indicates whether the window control is timeout for waiting input
        /// </summary>
        /// <param name="control"></param>
        /// <param name="seconds">Seconds for the timeout</param>
        /// <returns>Boolean - True or False</returns>
        public static bool IsWindowWaitForInputIdleTimeout(UIControl control, int seconds)
        {
            return ClassicBase.Window.IsWindowWaitForInputIdleTimeout((AutomationElement)control.Element, seconds);
        }

        /// <summary>
        /// Get visual state of the window control
        /// </summary>        
        /// <returns>String - State: Minimized | Maximize | Normal</returns>
        public static string GetWindowVisualState(UIControl control)
        {
            return ClassicBase.Window.GetWindowVisualState((AutomationElement)control.Element);
        }

        /// <summary>
        /// Get interaction state of the window control
        /// </summary>        
        /// <returns>String - State: BlockedByModalWindow | Closing | NotResponding | ReadyForUserInteraction | Running</returns>
        public static string GetWindowInteractionState(UIControl control)
        {
            return ClassicBase.Window.GetWindowInteractionState((AutomationElement)control.Element);
        }

        /// <summary>
        /// Indicates whether the control is topmost
        /// </summary>        
        /// <returns>Boolean - True or false</returns>
        public static bool IsWindowTopmost(UIControl control)
        {
            return ClassicBase.Window.IsWindowTopmost((AutomationElement)control.Element);
        }

        /// <summary>
        /// Indicates whether the control is maximize
        /// </summary>        
        /// <returns>Boolean - True or false</returns>
        public static bool IsWindowCanMaximize(UIControl control)
        {
            return ClassicBase.Window.IsWindowCanMaximize((AutomationElement)control.Element);
        }

        /// <summary>
        /// Indicates whether the control is minimize
        /// </summary>        
        /// <returns>Boolean - True or false</returns>
        public static bool IsWindowCanMinimize(UIControl control)
        {
            return ClassicBase.Window.IsWindowCanMinimize((AutomationElement)control.Element);
        }

        /// <summary>
        /// Indicates whether the control is modal window
        /// </summary>        
        /// <returns>Boolean - True or false</returns>
        public static bool IsWindowModal(UIControl control)
        {
            return ClassicBase.Window.IsWindowModal((AutomationElement)control.Element);
        }
    }

    /// <summary>
    /// CheckBox type control UIOperation methods
    /// </summary>
    public class CheckBox : Basic
    {
        /// <summary>
        /// Toggle the control (Check / UnCheck).
        /// </summary>
        /// <param name="control"></param>
        /// <param name="state">ToggleOn | ToggleOff</param>
        /// <returns>void</returns>
        public static void Check(UIControl control, PatternEnum.ToggleState state)
        {
            ClassicBase.Toggle.Check((AutomationElement)control.Element, state);
        }

        /// <summary>
        /// Get status of the control.
        /// </summary>        
        /// <returns>Toggled | Non-Toggled | NA</returns>
        public static bool? GetStatus(UIControl control)
        {
            return ClassicBase.Toggle.GetToggleStatus((AutomationElement)control.Element);
        }
    }

    /// <summary>
    /// EditBox type control UIOperation methods
    /// </summary>
    public class EditBox : Basic
    {
        /// <summary>
        /// Set value on the control
        /// </summary>        
        /// <param name="value">String - value</param>
        /// <param name="control">UIControl</param>
        /// <returns>AutomationElement - Parent control</returns>
        public static void SetValue(UIControl control, string value)
        {
            ClassicBase.Value.SetValue((AutomationElement)control.Element, value);
        }

        /// <summary>
        /// Get value on current control
        /// </summary>        
        /// <param name="control">UIControl</param>
        /// <returns>String - value in the control</returns>
        public static string GetValue(UIControl control)
        {
            return ClassicBase.Value.GetValue((AutomationElement)control.Element);
        }        
    }

    /// <summary>
    /// ComboBox type control UIOperation methods
    /// </summary>
    public class ComboBox : Basic
    {
        /// <summary>
        /// Expand the control.
        /// </summary>
        public static void Expand(UIControl control)
        {
            ClassicBase.ExpandCollapse.Expand((AutomationElement)control.Element);
        }

        /// <summary>
        /// Collapse the control.
        /// </summary>
        public static void Collapse(UIControl control)
        {
            ClassicBase.ExpandCollapse.Collapse((AutomationElement)control.Element);
        }

        /// <summary>
        /// Select item in the combobox
        /// </summary>
        /// <param name="control"></param>
        /// <param name="itemName">Item name</param>
        public static void SelectItem(UIControl control, string itemName)
        {
            ExtensionBase.List.SelectListItem((AutomationElement)control.Element, itemName);
        }

        /// <summary>
        /// Get all items name in the combobox
        /// </summary>
        /// <param name="control"></param>
        /// <returns>item name array</returns>
        public static string[] GetItems(UIControl control)
        {
            return ExtensionBase.ComboBox.GetDropDownListItems((AutomationElement)control.Element);
        }

        /// <summary>
        /// Set value on the control
        /// </summary>        
        /// <param name="value">String - value</param>
        /// <param name="control">UIControl</param>
        /// <returns>AutomationElement - Parent control</returns>
        public static void SetValue(UIControl control, string value)
        {
            ClassicBase.Value.SetValue((AutomationElement)control.Element, value);
        }
    }    

    /// <summary>
    /// HyperLink type control UIOperation methods
    /// </summary>
    public class HyperLink : Basic
    {
        /// <summary>
        /// Invoke the control (Click).
        /// </summary>
        public static void Click(UIControl control)
        {
            ClassicBase.Invoke.Click((AutomationElement)control.Element);
        }
    }    

    /// <summary>
    /// List type control UIOperation methods
    /// </summary>
    public class List : Basic
    {
        /// <summary>
        /// Select item in the list
        /// </summary>
        /// <param name="control"></param>
        /// <param name="itemName">Item name</param>
        public static void SelectItem(UIControl control, string itemName)
        {
            ExtensionBase.List.SelectListItem((AutomationElement)control.Element, itemName);
        }

        /// <summary>
        /// Select item in the list
        /// </summary>
        /// <param name="control"></param>
        /// <param name="index">Item index, start from 1</param>
        public static void SelectItem(UIControl control, int index)
        {
            ExtensionBase.List.SelectListItem((AutomationElement)control.Element, index);
        }

        /// <summary>
        /// Indicates whether require some selection for the control
        /// </summary>        
        /// <returns>Bool - True or False</returns>
        /// <remarks>Support Control Type: List | Tree | Menu</remarks>
        public static bool IsSelectionRequired(UIControl control)
        {
            return ClassicBase.Selection.IsSelectionRequired((AutomationElement)control.Element);
        }

        /// <summary>
        /// Select Multiple items
        /// </summary>
        /// <param name="control"></param>
        /// <param name="items"></param>
        public static void SelectMultipleItems(UIControl control, params string[] items)
        {
            ExtensionBase.List.SelectMultipleItems((AutomationElement)control.Element, items);
        }

        /// <summary>
        /// UnSelect Multiple items
        /// </summary>
        /// <param name="control"></param>
        /// <param name="items"></param>
        public static void UnSelectMultipleItems(UIControl control, params string[] items)
        {
            ExtensionBase.List.UnSelectMultipleItems((AutomationElement)control.Element, items);
        }

        /// <summary>
        /// Select Multiple items
        /// </summary>
        /// <param name="control"></param>
        /// <param name="itemName"></param>
        public static void IsSelected(UIControl control, string itemName)
        {
            ExtensionBase.List.IsItemSelected((AutomationElement)control.Element, itemName);
        }

        /// <summary>
        /// Indicates whether support multiple selection for the control
        /// </summary>        
        /// <returns>Bool - True or False</returns>
        /// <remarks>Support Control Type: List | Tree | Menu</remarks>
        public static bool IsSupportMultipleSelection(UIControl control)
        {
            return ClassicBase.Selection.IsSelectionCanSelectMultiple((AutomationElement)control.Element);
        }

        /// <summary>
        /// Get list Items
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static List<string> GetListItems(UIControl control)
        {
            return ExtensionBase.List.GetListItems((AutomationElement)control.Element);
        }
    }

    /// <summary>
    /// RaddioButton type control UIOperation methods
    /// </summary>
    public class RadioButton : Basic
    {
        /// <summary>
        /// Toggle the control (Check / UnCheck).
        /// </summary>
        /// <param name="control"></param>
        /// <param name="state">ToggleOn | ToggleOff</param>
        /// <returns>void</returns>
        public static void Check(UIControl control, PatternEnum.ToggleState state)
        {
            ClassicBase.Toggle.Check((AutomationElement)control.Element, state);
        }

        /// <summary>
        /// Get status of the control.
        /// </summary>        
        /// <returns>Toggled | Non-Toggled | NA</returns>
        public static bool? GetStatus(UIControl control)
        {
            return ClassicBase.Toggle.GetToggleStatus((AutomationElement)control.Element);
        }

        ///<summary>
        ///</summary>
        ///<param name="control"></param>
        public static void Select(UIControl control)
        {
            ClassicBase.SelectionItem.SelectItem((AutomationElement)control.Element);
        }
    }

    /// <summary>
    /// ScrollBar type control UIOperation methods
    /// </summary>
    public class ScrollBar : Basic
    {
        /// <summary>
        /// Scroll the control by amount
        /// </summary>
        /// <param name="control"></param>
        /// <param name="scrollHorizontalAmount">Horizontal Amount: LargeDecrement | LargeIncrement | SmallDecrement | SmallIncrement | NoAmount</param>
        /// <param name="scrollVerticalAmount">Vertical Amount: LargeDecrement | LargeIncrement | SmallDecrement | SmallIncrement | NoAmount</param>
        /// <returns>void</returns>
        public static void ScrollByAmount(UIControl control, PatternEnum.ScrollAmount scrollHorizontalAmount, PatternEnum.ScrollAmount scrollVerticalAmount)
        {
            ClassicBase.Scroll.ScrollByAmount((AutomationElement)control.Element, scrollHorizontalAmount, scrollVerticalAmount);
        }

        /// <summary>
        /// Scroll the control by percentage
        /// </summary>
        /// <param name="control"></param>
        /// <param name="scrollHorizontalPercentage"> Horizontal percentage, double type</param>
        /// <param name="scrollVerticalPercentage">Vertical percentage, double type</param>
        /// <returns>void</returns>
        public static void ScrollByPercentage(UIControl control, double scrollHorizontalPercentage, double scrollVerticalPercentage)
        {
            ClassicBase.Scroll.ScrollByPercentage((AutomationElement)control.Element, scrollHorizontalPercentage, scrollVerticalPercentage);
        }

        /// <summary>
        /// Indicates whether the controll support horizontally scroll
        /// </summary>
        /// <param name="control"></param>
        /// <param name="scrollAxis">PatternEnum.ScrollAxis: Horizontal or Veritical</param>
        /// <returns>Boolean - True or false</returns>
        public static bool IsScrollable(UIControl control, PatternEnum.ScrollAxis scrollAxis)
        {
            return ClassicBase.Scroll.IsScrollable((AutomationElement)control.Element, scrollAxis);
        }

        /// <summary>
        /// Get Scroll view size
        /// </summary>
        /// <param name="control"></param>
        /// <param name="scrollAxis">PatternEnum.ScrollAxis: Horizontal or Veritical</param>
        /// <returns>Double - size of scroll view</returns>
        public static double GetScrollViewSize(UIControl control, PatternEnum.ScrollAxis scrollAxis)
        {
            return ClassicBase.Scroll.GetScrollViewSize((AutomationElement)control.Element, scrollAxis);
        }

        /// <summary>
        /// Get Scroll percentage
        /// </summary>
        /// <param name="control"></param>
        /// <param name="scrollAxis">PatternEnum.ScrollAxis: Horizontal or Veritical</param>
        /// <returns>Double - scroll percentage</returns>
        public static double GetScrollPercentage( UIControl control, PatternEnum.ScrollAxis scrollAxis)
        {
            return ClassicBase.Scroll.GetScrollPercentage((AutomationElement)control.Element, scrollAxis);
        }

        /// <summary>
        /// Scroll specified control to current view 
        /// </summary>        
        /// <returns>void</returns>
        public static void ScrollItemIntoView(UIControl control)
        {
            ClassicBase.ScrollItem.ScrollItemIntoView((AutomationElement)control.Element);
        }
    }

    /// <summary>
    /// Dock type control UIOperation methods
    /// </summary>
    public class Dock : Basic
    {
        /// <summary>
        /// Set Dock control position
        /// </summary>
        /// <param name="control"></param>
        /// <param name="dockPosition">dock Position: Fill | Bottom | Top | Left | Right | None</param>
        /// <returns>void</returns>
        public static void SetDockPosition(UIControl control, PatternEnum.DockPosition dockPosition)
        {
            ClassicBase.Dock.SetDockPosition((AutomationElement)control.Element, dockPosition);
        }

        /// <summary>
        /// Get Dock control position
        /// </summary>        
        /// <returns>String - dock Position: Fill | Bottom | Top | Left | Right | None</returns>
        public static string GetDockPosition(UIControl control)
        {
            return ClassicBase.Dock.GetDockPosition((AutomationElement)control.Element);
        }
    }

    /// <summary>
    /// Tab type control UIOperation methods
    /// </summary>
    public class Tab : Basic
    {
        /// <summary>
        /// Select tab item
        /// </summary>
        /// <param name="control"></param>
        /// <param name="tabName">tab name</param>
        public static void SelectItem(UIControl control, string tabName)
        {
            ExtensionBase.Tab.SelectTabItem((AutomationElement)control.Element, tabName);
        }

        /// <summary>
        /// Select tab items
        /// </summary>
        /// <param name="control"></param>
        public static string[] GetItems(UIControl control)
        {
            return ExtensionBase.Tab.GetTabItems((AutomationElement)control.Element);
        }
    }

    /// <summary>
    /// DataGrid type control UIOperation methods
    /// </summary>
    public class DataGrid : Basic
    {
        /// <summary>
        /// Get count of item in grid
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gridAxis">Indicates whether calculate in row or column</param>
        /// <returns>Int - item count</returns>
        public static int GetItemCount(UIControl control, PatternEnum.Axis gridAxis)
        {
            return ClassicBase.Grid.GetGridItemCount((AutomationElement)control.Element, gridAxis);
        }

        /// <summary>
        /// Get item index on row/column in grid
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gridAxis">Indicates whether calculate in row or column</param>
        /// <returns>Int - item count</returns>
        public static int GetGridRowColumnIndex(UIControl control, PatternEnum.Axis gridAxis)
        {
            return ClassicBase.GridItem.GetGridRowColumnIndex((AutomationElement)control.Element, gridAxis);
        }

        /// <summary>
        /// Get row/column span in grid
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gridAxis">Indicates whether calculate in row or column</param>
        /// <returns>Int - item count</returns>
        public static int GetGridRowColumnSpan(UIControl control, PatternEnum.Axis gridAxis)
        {
            return ClassicBase.GridItem.GetGridRowColumnSpan((AutomationElement)control.Element, gridAxis);
        }
    }

    /// <summary>
    /// Table type control UIOperation methods
    /// </summary>
    public class Table : Basic
    {
        /// <summary>
        /// Get count of item in table
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gridAxis">Indicates whether calculate in row or column</param>
        /// <returns>Int - item count</returns>
        public static int GetItemCount(UIControl control, PatternEnum.Axis gridAxis)
        {
            return ClassicBase.Table.GetTableItemCount((AutomationElement)control.Element, gridAxis);
        }

        /// <summary>
        /// Get headers name of row/column from table
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gridAxis">Indicates whether calculate in row or column</param>
        /// <returns>String[] - header names</returns>
        public static string[] GetHeaderNames(UIControl control, PatternEnum.Axis gridAxis)
        {
            return ClassicBase.Table.GetTableHeadersName((AutomationElement)control.Element, gridAxis);
        }

        /// <summary>
        /// Get major of row/column from table
        /// </summary>
        /// <returns>String - Major name</returns>
        public static string GetRowColumnMajor(UIControl control)
        {
            return ClassicBase.Table.GetTableRowColumnMajor((AutomationElement)control.Element);
        }

        /// <summary>
        /// Get index of row/column from table item
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gridAxis">Indicates whether calculate in row or column</param>
        /// <returns>Int - item count</returns>
        public static int GetItemRowColumnIndex(UIControl control, PatternEnum.Axis gridAxis)
        {
            return ClassicBase.TableItem.GetTableItemRowColumnIndex((AutomationElement)control.Element, gridAxis);
        }

        /// <summary>
        /// Get row/column span in table item
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gridAxis">Indicates whether calculate in row or column</param>
        /// <returns>Int - item count</returns>
        public static int GetItemRowColumnSpan(UIControl control, PatternEnum.Axis gridAxis)
        {
            return ClassicBase.TableItem.GetTableItemRowColumnSpan((AutomationElement)control.Element, gridAxis);
        }

        /// <summary>
        /// Get headers of row/column from table item
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gridAxis">Indicates whether calculate in row or column</param>
        /// <returns>String[] - header names</returns>
        public static string[] GetItemHeaderNames(UIControl control, PatternEnum.Axis gridAxis)
        {
            return ClassicBase.TableItem.GetTableItemHeadersName((AutomationElement)control.Element, gridAxis);
        }
    }

    /// <summary>
    /// Text type control UIOperation methods
    /// </summary>
    public class Text : Basic
    {
        /// <summary>
        /// Get selected text
        /// </summary>
        /// <returns>String - selected text</returns>
        public static string GetSupportedTextSelection(UIControl control)
        {
            return ClassicBase.Text.GetSupportedTextSelection((AutomationElement)control.Element);
        }

        /// <summary>
        /// Get value of the text control
        /// </summary>
        public static string GetValue(UIControl control)
        {
            return ClassicBase.Properties.GetName((AutomationElement)control.Element);
        }
    }

    /// <summary>
    /// Tree type control UIOperation methods
    /// </summary>
    public class Tree : Basic
    {
        /// <summary>
        /// Select item in the tree
        /// </summary>
        /// <param name="control"></param>
        /// <param name="treePath">tree Path, format: TreeNode|SubTreeNode|SubSubTreeNode</param>
        public static void SelectItem(UIControl control, string treePath)
        {
            ExtensionBase.Tree.SelectTreeItem((AutomationElement)control.Element, treePath);
        }

        /// <summary>
        /// Open item in the tree
        /// </summary>
        /// <param name="control"></param>
        /// <param name="treePath">tree Path, format: TreeNode|SubTreeNode|SubSubTreeNode</param>
        public static void OpenItem(UIControl control, string treePath)
        {
            ExtensionBase.Tree.OpenTreeItem((AutomationElement)control.Element, treePath);
        }
    }

    /// <summary>
    /// ToolBar type control UIOperation methods
    /// </summary>
    public class ToolBar : Basic
    {
    }

    /// <summary>
    /// Pane type control UIOperation methods
    /// </summary>
    public class Pane : Basic
    {
    }

    /// <summary>
    /// Image type control UIOperation methods
    /// </summary>
    public class Image : Basic
    {
        /// <summary>
        /// Take a screenshot for the control and save it to the specified path
        /// </summary>
        /// <param name="control"></param>
        /// <param name="savePath">The path to save the control screenshot</param>
        public static void GetScreenshot(UIControl control, string savePath)
        {
            try
            {
                var rect = (control.Element as AutomationElement).Current.BoundingRectangle;

                if (!rect.IsEmpty)
                {
                    var size = new Size(Convert.ToInt32(rect.Width), Convert.ToInt32(rect.Height));

                    var bmp = ImageHelper.TakeSnapshot(Convert.ToInt32(rect.Width), Convert.ToInt32(rect.Height), Convert.ToInt32(rect.X), Convert.ToInt32(rect.Y), size);
                    bmp.Save(savePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        /// <summary>
        /// Compare the control picture with target picture
        /// </summary>
        /// <param name="control"></param>
        /// <param name="targetPicture">Target picture to compare with</param>
        /// <param name="tolerance">tolerance of the comparision</param>
        /// <returns></returns>
        public static bool IsEquals(UIControl control, string targetPicture, int tolerance)
        {
            const bool issame = false;


            return issame;
        }

        /// <summary>
        /// Compare the pictures of 2 controls, source and target control
        /// </summary>
        /// <param name="sourceControl">Source control</param>
        /// <param name="targetControl">Target control</param>
        /// <param name="tolerance">tolerance of the comparision</param>
        /// <returns></returns>
        public static bool IsEquals(UIControl sourceControl, UIControl targetControl, int tolerance)
        {
            const bool issame = false;


            return issame;
        }
    }

    /// <summary>
    /// Customer type control UIOperation methods
    /// </summary>
    public class Customer : Basic
    {
        /// <summary>
        /// Get value on current control
        /// </summary>        
        /// <param name="control">UIControl</param>
        /// <returns>String - value in the control</returns>
        public static string GetValue(UIControl control)
        {
            return ClassicBase.Value.GetValue((AutomationElement)control.Element);
        }

        public static string GetHTMLPValue(UIControl control)
        {
            return ExtensionBase.HTMLP.GetValue((AutomationElement)control.Element);
        }
    }
}
