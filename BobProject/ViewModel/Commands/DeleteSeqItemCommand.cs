using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.SchemaLogic.SchemaTypes;

namespace BobProject.ViewModel.Commands
{
    public class DeleteSeqItemCommand : ICommand
    {

        #region Constructor

        public DeleteSeqItemCommand()
        {
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            XmlSchemaWrapper scWr = (XmlSchemaWrapper)parameter;

            //check if parameter is XmlSchemaWrapper type
            if (scWr != null)
            {
                //check if parameter is XmlSchemaSequenceWrapper type
                if (scWr is XmlSchemaSequenceWrapper)
                {                   
                    XmlSchemaWrapper parent = scWr.Parent;

                    //check if parent is XmlSchemaSequenceArray
                    if (parent is XmlSchemaSequenceArray)
                        return true;
                }
            }
            return false;
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
            var scWr = (XmlSchemaSequenceWrapper)parameter;
            var parent = (XmlSchemaSequenceArray)scWr.Parent;

            //delete the selected item from array
            parent.RemoveAt(scWr.Index);           
        }

        #endregion
    }
}