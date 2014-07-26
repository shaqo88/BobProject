using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace BobProject.UtilityClasses
{
    public static class TreeViewHelper
    {
        /// <summary>
        /// Expands all children of a TreeView
        /// </summary>
        /// <param name="treeView">The TreeView whose children will be expanded</param>
        public static void ExpandAll(this TreeView treeView)
        {
            ExpandSubContainers(treeView);
        }

        /// <summary>
        /// Collapse all children of a TreeView
        /// </summary>
        /// <param name="treeView">The TreeView whose children will be collapsed</param>
        public static void CollapseAll(this TreeView treeView)
        {
            SetTreeViewItems(treeView,false);
        }

        /// <summary>
        /// Collapse or Expand specific node from treeview
        /// </summary>
        /// <param name="item">node from treeview</param>
        /// <param name="isExpand">is it expand or collapse</param>
        public static void ExpandCollapseNode(this TreeViewItem item, bool isExpand)
        {
            SetTreeViewItems(item, isExpand);
        }


        /// <summary>
        /// Collapse or Expand specific node from treeview
        /// </summary>
        /// <param name="obj">object view to be collapse or expand </param>
        /// <param name="expand">is it expand or collapse</param>
        private static void SetTreeViewItems(object obj, bool expand)
        {
            if (obj is TreeViewItem)
            {
                ((TreeViewItem)obj).IsExpanded = expand;
                foreach (object obj2 in ((TreeViewItem)obj).Items)
                    SetTreeViewItems(obj2, expand);
            }
            else if (obj is ItemsControl)
            {
                foreach (object obj2 in ((ItemsControl)obj).Items)
                {
                    if (obj2 != null)
                    {
                        SetTreeViewItems(((ItemsControl)obj).ItemContainerGenerator.ContainerFromItem(obj2), expand);

                        TreeViewItem item = obj2 as TreeViewItem;
                        if (item != null)
                            item.IsExpanded = expand;
                    }
                }
            }
        }

        /// <summary>
        /// Expands all children of a TreeView or TreeViewItem
        /// </summary>
        /// <param name="parentContainer">The TreeView or TreeViewItem containing the children to expand</param>
        private static void ExpandSubContainers(ItemsControl parentContainer)
        {
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    //expand the item
                    currentContainer.IsExpanded = true;

                    //if the item's children are not generated, they must be expanded
                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        //store the event handler in a variable so we can remove it (in the handler itself)
                        EventHandler eh = null;
                        eh = new EventHandler(delegate
                        {
                            //once the children have been generated, expand those children's children then remove the event handler
                            if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                            {
                                ExpandSubContainers(currentContainer);
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        });

                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else //otherwise the children have already been generated, so we can now expand those children
                    {
                        ExpandSubContainers(currentContainer);
                    }
                }
            }
        }


        /// <summary>
        /// Searches a TreeView for the provided object and select it if found
        /// </summary>
        /// <param name="treeView">The TreeView containing the item</param>
        /// <param name="items">The item to search and select</param>
        public static bool SelectItem(this TreeView treeView, object item, Color selectedColor)
        {
           TreeViewItem treeViewItem = ExpandAndSelectItem(treeView, item, selectedColor);
           return treeViewItem != null;

        }


        /// <summary>
        /// Searches a TreeView for the provided objects and selects it if found
        /// </summary>
        /// <param name="treeView">The TreeView containing the item</param>
        /// <param name="items">The items to search and select</param>
        public static List<TreeViewItem> SelectItems(this TreeView treeView, object[] items, Color selectedColor)
        {
            List<TreeViewItem> treeviewItems = new List<TreeViewItem>();
            foreach (var item in items)
            {
                TreeViewItem treeViewItem = ExpandAndSelectItem(treeView, item, selectedColor);
                if (treeViewItem != null)
                    treeviewItems.Add(treeViewItem);
            }
            return treeviewItems;
            
        }

        /// <summary>
        /// Finds the provided object in an ItemsControl's children and selects it
        /// </summary>
        /// <param name="parentContainer">The parent container whose children will be searched for the selected item</param>
        /// <param name="itemToSelect">The item to select</param>
        /// <returns>True if the item is found and selected, false otherwise</returns>
        private static TreeViewItem ExpandAndSelectItem(ItemsControl parentContainer, object itemToSelect, Color selectedColor)
        {
            //check all items at the current level
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                //if the data item matches the item we want to select, set the corresponding
                //TreeViewItem IsSelected to true
                if (item == itemToSelect && currentContainer != null)
                {
                    currentContainer.Background = new SolidColorBrush(selectedColor);
                    /*currentContainer.IsSelected = true;                    
                    currentContainer.BringIntoView();
                    currentContainer.Focus();*/

                    //the item was found
                    return currentContainer;
                }
            }

            //if we get to this point, the selected item was not found at the current level, so we must check the children
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                //if children exist
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    //keep track of if the TreeViewItem was expanded or not
                    bool wasExpanded = currentContainer.IsExpanded;

                    //expand the current TreeViewItem so we can check its child TreeViewItems
                    currentContainer.IsExpanded = true;

                    //if the TreeViewItem child containers have not been generated, we must listen to
                    //the StatusChanged event until they are
                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        //store the event handler in a variable so we can remove it (in the handler itself)
                        EventHandler eh = null;
                        eh = new EventHandler(delegate
                        {
                            if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                            {
                                if (ExpandAndSelectItem(currentContainer, itemToSelect, selectedColor) ==  null)
                                {
                                    //The assumption is that code executing in this EventHandler is the result of the parent not
                                    //being expanded since the containers were not generated.
                                    //since the itemToSelect was not found in the children, collapse the parent since it was previously collapsed
                                    currentContainer.IsExpanded = false;
                                }

                                //remove the StatusChanged event handler since we just handled it (we only needed it once)
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        });
                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else //otherwise the containers have been generated, so look for item to select in the children
                    {
                        TreeViewItem treeViewItem = ExpandAndSelectItem(currentContainer, itemToSelect, selectedColor);
                        if (treeViewItem == null)
                        {
                            //restore the current TreeViewItem's expanded state
                            currentContainer.IsExpanded = wasExpanded;
                        }
                        else //otherwise the node was found and selected, so return true
                        {
                            return treeViewItem;
                        }
                    }
                }
            }

            //no item was found
            return null;
        }
    }
}
