using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace To_do_list_WinUI3.Class
{
    public class BlockTime : INotifyPropertyChanged
    
     {
        int _Minutes = default;
        int _Hours = default;
        int _Seconds = default;

        public event PropertyChangedEventHandler PropertyChanged;
        private  void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public int Minutes { get { return _Minutes; } set { _Minutes = value; NotifyPropertyChanged(nameof(Minutes));  } }
        public int Hours { get { return _Hours; } set { _Hours = value; NotifyPropertyChanged(nameof(Hours));   } }
        public int Seconds  
        {
            get { return _Seconds;  }
            set { _Seconds = value; NotifyPropertyChanged(nameof(Seconds)); }
        }

    }
}
