using Models.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ViewModelsXML.Tools;
using ViewModelsXML.ViewModels.Helpers;

namespace ViewModelsXML.ViewModels
{
    public class RadioListsViewModel : BaseViewModel
    {
        public RadioListsViewModel(MainRadioViewModel mrvm)
        {


            //LoadXmlData(mrvm.xmlValidation);
        }

        public MainRadioViewModel mrvm { get; set; }

    }
}
