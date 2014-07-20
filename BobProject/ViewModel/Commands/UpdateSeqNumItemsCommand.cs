using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BL.SchemaLogic.SchemaTypes;

namespace BobProject.ViewModel.Commands
{
    public class UpdateSeqNumItemsCommand : ICommand
    {
        #region Constructor

        public UpdateSeqNumItemsCommand()
        {
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            var selectedItem = parameter as XmlSchemaWrapper;
            if (selectedItem == null)
                return false;
            return (selectedItem.NodeType == NodeType.Sequence);
        }

        /// <summary>
        /// Fires when the CanExecute status of this command changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Invokes this command to perform its intended task.
        /// </summary>
        public void Execute(object parameter)
        {
            var item = parameter as XmlSchemaSequenceArray;
            int currNumItems = item.Count;
            int chosenNum = MainWindow.Instance.SequenceSize.Value - currNumItems;

            if (chosenNum != 0)
            {
                if (chosenNum > 0)
                {
                    for (int i = 0; i < chosenNum; i++)
                    {
                        item.AddNewWrapper();
                        item.DrillOnce();
                    }
                }
                else //chosenNum < 0
                {
                    MessageBoxResult result = MessageBox.Show("Would you like to delete " + (Math.Abs(chosenNum)).ToString()  +" last items?", "Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        for (int i = currNumItems - 1; i >= currNumItems + chosenNum; i--)
                            item.RemoveAt(i);
                    }

                }
            }
                        
        }

        #endregion
    }
}
