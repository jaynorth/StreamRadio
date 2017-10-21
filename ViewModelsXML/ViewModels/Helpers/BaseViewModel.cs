using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViewModelsXML.ViewModels.Helpers
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            string MainXmlFilePath = "../../../ViewModelsXML/XML/Main/RadioStations.xml";

            doc = XDocument.Load(MainXmlFilePath);

        }

        protected XDocument doc;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
