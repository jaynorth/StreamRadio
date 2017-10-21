using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsXML.ViewModels.Helpers;

namespace ViewModelsXML.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel(MainRadioViewModel mrvm)
        {
           //_currentRadioName = "Shit";
           // mrvm.CurrentStation.Name = "what???";
        }

        public DashboardViewModel()
        {
            
        }



        private string _currentRadioName;

        public string CurrentRadioName
        {
            get { return _currentRadioName; }
            set { _currentRadioName = value;

                OnPropertyChanged();
            }
        }

    }
}
