using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace HP.LR.Test.UIOperation
{
    /// <summary>
    /// Public to Customized UIOperation with AutomationElement parameter
    /// </summary>
    public class ExtensionBase
    {
        /// <summary>
        /// Methods for Menu control
        /// </summary>
        public class Menu
        {
            private static AutomationElement GetMenuItem(AutomationElement element, string menuPath)
            {
                var currentElement = element;

                foreach (var menu in menuPath.Split(new[] { '|' }))
                {
                    var typePc = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem);
                    var namePc = new PropertyCondition(AutomationElement.NameProperty, menu);
                    var andCond = new AndCondition(typePc, namePc);

                    var selectedItem = currentElement.FindFirst(TreeScope.Children, andCond);

                    if (selectedItem == null)
                        throw new Exception(string.Format("Cannot find the menu item {0}.", menu));

                    object obj;

                    if (selectedItem.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out obj))
                        ClassicBase.ExpandCollapse.Expand(selectedItem);

                    currentElement = selectedItem;
                }

                return currentElement;
            }
            
            /// <summary>
            /// Select a menu item in a menu control, support hireachy.
            /// </summary>
            /// <param name="element">Automation element</param>
            /// <param name="menuPath">Menu path, format: Menu1|SubMenu|SubSubMenu</param>
            /// <remarks>Support type: Menu</remarks>
            public static void SelectMenuItem(AutomationElement element, string menuPath)
            {
                object obj;

                var menuItem = GetMenuItem(element, menuPath);                        

                if (menuItem.TryGetCurrentPattern(InvokePattern.Pattern, out obj))
                    ClassicBase.Invoke.Click(menuItem);
            }

            public static string[] GetAllSubItems(AutomationElement element, string menuPath)
            {
                var menuItem = GetMenuItem(element, menuPath);
                var subMenuItems = menuItem.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem));

                if (subMenuItems == null)
                    throw new Exception(string.Format("No Sub items under {0}", menuPath));

                var subitem = new string[subMenuItems.Count];

                for (var i = 0; i < subMenuItems.Count; i++)
                {
                    subitem[i] = subMenuItems[i].Current.Name;
                }

                return subitem;
            }
        }

        /// <summary>
        /// Methods for tree control
        /// </summary>
        public class Tree
        {
            /// <summary>
            /// Open tree item in a tree
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <param name="treeNavigator">tree item path, format: TreeItem|SubTreeItem|SubSubTreeItem</param>
            public static AutomationElement OpenTreeItem(AutomationElement element, string treeNavigator)
            {
                AutomationElement selectedElement = null;

                selectedElement = SelectTreeItem(element, treeNavigator);
                if (selectedElement != null)
                {
                    ClassicBase.Emulation.DoubleClick(selectedElement);
                }

                return selectedElement;
            }
            
            /// <summary>
            /// Select tree item in a tree
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <param name="treeNavigator">tree item path, format: TreeItem|SubTreeItem|SubSubTreeItem</param>
            public static AutomationElement SelectTreeItem(AutomationElement element, string treeNavigator)
            {
                AutomationElement selectedElement = null;
                var parentElement = element;

                foreach (var item in treeNavigator.Split(new[] { '|' }))
                {
                    var pc = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);

                    var items = parentElement.FindAll(TreeScope.Children, pc);
                    AutomationElement targetElement = null;

                    if (items == null)
                        throw new Exception("No tree items");

                    for (var i = 0; i < items.Count; i++)
                    {
                        var treeitem = items[i];
                        if (treeitem.Current.Name.ToLower() != item.ToLower())
                        {
                            pc = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
                            var tw = new TreeWalker(pc);
                            var selectedItemText = tw.GetFirstChild(treeitem);
                            if (selectedItemText != null)
                            {
                                if (selectedItemText.Current.Name.ToLower() == item.ToLower())
                                {
                                    targetElement = treeitem;
                                }
                            }
                        }
                        else
                        {
                            targetElement = treeitem;
                        }
                    }

                    if (targetElement == null)
                        throw new Exception(string.Format("Cannot find the tree view item {0}.", item));

                    object obj;
                    if (targetElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out obj))
                    {
                        ClassicBase.SelectionItem.SelectItem(targetElement);
                        selectedElement = targetElement;

                        if (targetElement.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out obj))
                        {
                            var expandcollapsePattern = (ExpandCollapsePattern)targetElement.GetCurrentPattern(ExpandCollapsePattern.Pattern);
                            ClassicBase.ExpandCollapse.Expand(targetElement);

                            parentElement = targetElement;
                            
                        }
                    }
                }

                return selectedElement;
            }

            /// <summary>
            /// Get Children items on the root of the tree.
            /// </summary>
            /// <param name="element">Tree element</param>
            /// <returns>element Collection</returns>
            public static AutomationElementCollection GetChildren(AutomationElement element)
            {
                var pc = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                return element.FindAll(TreeScope.Children, pc);
            }

            /// <summary>
            /// Get children items on the specified tree item
            /// </summary>
            /// <param name="element">Tree element</param>
            /// <param name="treeNavigator">tree item path, format: TreeItem|SubTreeItem|SubSubTreeItem</param>
            /// <returns>element Collection</returns>
            public static AutomationElementCollection GetItemChildren(AutomationElement element, string treeNavigator)
            {
                var selectedItem = SelectTreeItem(element, treeNavigator);
                return GetChildren(selectedItem);
            }
        }

        /// <summary>
        /// Methods for list control
        /// </summary>
        public class List
        {
            /// <summary>
            /// Select list item in a list
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <param name="itemName">list item name</param>
            /// <remarks>Support type: List</remarks>
            public static void SelectListItem(AutomationElement element, string itemName)
            {
                var itemElement = FindItemInList(element, itemName);

                if (itemElement != null)
                {
                    object obj;

                    if (itemElement.TryGetCurrentPattern(InvokePattern.Pattern, out obj))
                        ClassicBase.Invoke.Click(itemElement);
                    else if (itemElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out obj))
                        ClassicBase.SelectionItem.SelectItem(itemElement);
                    else
                        ClassicBase.Emulation.MouseClick(itemElement);
                }
                else
                {
                    throw new Exception("Cannot find the item in list.");
                }
            }

            /// <summary>
            /// Select list item in a list
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <param name="index">item index, start from 1</param>
            /// <remarks>Support type: List</remarks>
            public static void SelectListItem(AutomationElement element, int index)
            {
                var itemElement = FindItemInList(element, index);

                if (itemElement != null)
                {
                    object obj;

                    if (itemElement.TryGetCurrentPattern(InvokePattern.Pattern, out obj))
                        ClassicBase.Invoke.Click(itemElement);
                    else if (itemElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out obj))
                        ClassicBase.SelectionItem.SelectItem(itemElement);
                    else
                        ClassicBase.Emulation.MouseClick(itemElement);
                }
                else
                {
                    throw new Exception("Cannot find the item in list.");
                }
            }

            /// <summary>
            /// Select Multiple Items
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <param name="items">items</param>
            public static void SelectMultipleItems(AutomationElement element, params string[] items)
            {
                foreach (var item in items)
                {
                    var itemElement = FindItemInList(element, item);
                    if (itemElement != null)
                    {
                        ClassicBase.SelectionItem.AddItemToSelection(itemElement);
                    }
                }
            }

            /// <summary>
            /// UnSelect Multiple Items
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <param name="items">items</param>
            public static void UnSelectMultipleItems(AutomationElement element, params string[] items)
            {
                foreach (var item in items)
                {
                    var itemElement = FindItemInList(element, item);
                    if (itemElement != null)
                    {
                        ClassicBase.SelectionItem.RemoveItemFromSelection(itemElement);
                    }
                }
            }

            /// <summary>
            /// UnSelect Multiple Items
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <param name="itemName"></param>
            public static bool IsItemSelected(AutomationElement element, string itemName)
            {
                var isSelected = false;

                var itemElement = FindItemInList(element, itemName);
                if (itemElement != null)
                {
                    isSelected = ClassicBase.SelectionItem.IsItemSelected(itemElement);
                }

                return isSelected;
            }

            /// <summary>
            /// Get list items in a list
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <remarks>Support type: List</remarks>
            public static List<string> GetListItems(AutomationElement element)
            {
                var items = new List<string>();

                // Scan all list items by Vertical
                while (ClassicBase.Scroll.GetScrollPercentage(element, PatternEnum.ScrollAxis.Vertical) < 100.0)
                {
                    var aec = element.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
                    if (aec != null)
                    {
                        for (var i = 0; i < aec.Count; i++)
                        {
                            var item = aec[i];
                            if (string.IsNullOrEmpty(item.Current.Name))
                            {                                
                                var itemtext = item.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text));
                                Console.WriteLine("Item: {0}", itemtext.Current.Name);
                                if (items.Where(o => o == item.Current.Name).Count() == 0)
                                    items.Add(itemtext.Current.Name);
                            }
                            else
                            {
                                Console.WriteLine("Item: {0}", item.Current.Name);
                                if (items.Where(o => o == item.Current.Name).Count() == 0)
                                    items.Add(item.Current.Name);
                            }
                        }
                    }

                    if (ClassicBase.Scroll.GetScrollPercentage(element, PatternEnum.ScrollAxis.Vertical) < 0)
                        break;

                    // try to scroll a little
                    try
                    {
                        ClassicBase.Scroll.ScrollByAmount(element, PatternEnum.ScrollAmount.NoAmount, PatternEnum.ScrollAmount.SmallIncrement);
                    }
                    catch 
                    {
                        Console.WriteLine("Cannot scroll anymore.");
                        if (ClassicBase.Scroll.GetScrollPercentage(element, PatternEnum.ScrollAxis.Vertical) == 0)
                            break;
                    }                    
                }                

                return items;
            }

            private static AutomationElement FindItemInList(AutomationElement list, string itemName)
            {
                AutomationElement itemElement = null;

                var listitems = list.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));

                for (var i = 0; i < listitems.Count; i++)
                {
                    var item = listitems[i];
                    if (item.Current.Name.ToLower() == itemName.ToLower())
                    {
                        itemElement = item;
                        break;
                    }

                    var itemtext = item.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text));
                    if (itemtext != null)
                    {
                        if (itemtext.Current.Name.ToLower() == itemName.ToLower())
                        {
                            itemElement = item;
                            break;
                        }
                    }
                }

                return itemElement;
            }

            private static AutomationElement FindItemInList(AutomationElement list, int index)
            {
                var listitems = list.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));

                if (listitems.Count == 0)
                    throw new Exception("There is no list item.");

                if (index > listitems.Count)
                    throw new Exception("The index of searching item is out of the item count.");

                var itemElement = listitems[index - 1];

                return itemElement;
            }
        }

        /// <summary>
        /// Methods for combobox control
        /// </summary>
        public class ComboBox
        {
            /// <summary>
            /// Get the list items name in a dropdown list
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <returns>String[] - name list</returns>
            /// <remarks>Support type: Tree | List</remarks>
            public static string[] GetDropDownListItems(AutomationElement element)
            {
                string[] items = null;
                ClassicBase.ExpandCollapse.Expand(element);

                var aec = element.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
                if (aec != null)
                {
                    items = new string[aec.Count];
                    var i = 0;
                    foreach (AutomationElement item in aec)
                    {
                        items[i] = item.Current.Name;
                        i++;
                    }
                }

                return items;
            }
        }

        /// <summary>
        /// Methods for tab control
        /// </summary>
        public class Tab
        {
            /// <summary>
            /// Select tab item by the tab item name
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <param name="tabName">name of the tab item</param>
            /// <remarks>Support type: Tab</remarks>
            public static void SelectTabItem(AutomationElement element, string tabName)
            {
                AutomationElement itemElement = null;

                foreach (AutomationElement item in element.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem)))
                {
                    if (item.Current.Name == tabName)
                    {
                        itemElement = item;
                        break;
                    }
                }

                if (itemElement != null)
                {
                    object obj;

                    if (itemElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out obj))
                        ClassicBase.SelectionItem.SelectItem(itemElement);
                    else
                        ClassicBase.Emulation.MouseClick(itemElement);
                }
                else
                {
                    throw new Exception("Cannot find the tabItem in tab.");
                }
            }

            /// <summary>
            /// Get Tab items
            /// </summary>
            /// <param name="element">Automation Element</param>
            /// <remarks>Support type: Tab</remarks>
            public static string[] GetTabItems(AutomationElement element)
            {
                var tabItems = element.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem));
                string[] items = null;

                if (tabItems != null)
                {
                    items = new string[tabItems.Count];

                    for (var i = 0; i < tabItems.Count; i++)
                    {
                        items[i] = tabItems[i].Current.Name;
                    }
                }

                return items;
            }
        }


        /// <summary>
        /// Methods for HTML p control
        /// </summary>
        public class HTMLP
        {
            /// <summary>
            /// Get value of HTML P
            /// </summary>
            /// <param name="element">Automation Element</param>
            public static string GetValue(AutomationElement element)
            {
                string value = null;
                AutomationElement valueItem = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

                if (valueItem != null)
                {
                    object obj;

                    if (valueItem.TryGetCurrentPattern(ValuePattern.Pattern, out obj))
                        value = ClassicBase.Value.GetValue(valueItem);

                    if (string.IsNullOrEmpty(value))
                        value = valueItem.Current.Name;
                }
                else
                {
                    throw new Exception("Cannot find the edit control in HTMLP.");
                }

                return value;
            }
        }
    }
}
