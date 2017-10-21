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
using ViewModelsXML.ViewModels;

namespace RetroWebRadio.View.UserControls
{
    /// <summary>
    /// Logique d'interaction pour ProcessXML.xaml
    /// </summary>
    public partial class ProcessXML : UserControl
    {
        

        public ProcessXML()
        {
            InitializeComponent();

            //this.FontSize = 11;
            //this.Foreground = Brushes.White;


        }

        private void dropfiles(object sender, DragEventArgs e)
        {


            string[] droppedFiles = null;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                droppedFiles = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }

            if ((null == droppedFiles) || (!droppedFiles.Any())) { return; }

            var myVM = DataContext as MainRadioViewModel;

            myVM.DoFileDrop(droppedFiles);

        }
    }
}
