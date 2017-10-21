using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using RetroWebRadio.Properties;
using ViewModelsXML.ViewModels;
using Models.Model;

namespace RetroWebRadio.View.UserControls
{
    /// <summary>
    /// Logique d'interaction pour RadioLists.xaml
    /// </summary>
    public partial class RadioLists : UserControl
    {

        

        public RadioLists()
        {
            InitializeComponent();

           
           

        }

        public delegate void PassSelectedStation(RadioStation CurrentStation);

        public event PassSelectedStation StationSelectedEvent;

        private void datagridRadioList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridRadioList.SelectedItem!=null)
            {
                StationSelectedEvent((RadioStation)datagridRadioList.SelectedItem);
            }
            
        }


    }
}
