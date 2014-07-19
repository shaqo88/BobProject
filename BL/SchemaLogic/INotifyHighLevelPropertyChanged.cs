using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.SchemaLogic
{
    public interface INotifyHighLevelPropertyChanged
    {
        event PropertyChangedEventHandler HighLevelPropertyChanged;
    }
}
