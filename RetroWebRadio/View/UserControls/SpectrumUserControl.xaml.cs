
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
using WinformsVisualization;

namespace RetroWebRadio.View.UserControls
{
    /// <summary>
    /// Logique d'interaction pour SpectrumUserControl.xaml
    /// </summary>
    public partial class SpectrumUserControl : UserControl
    {
        public SpectrumUserControl()
        {
            InitializeComponent();

            Form1 spectForm = new Form1();
            spectForm.TopLevel = false;
            spectForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            windowsFormsHost1.Child = spectForm;
            
        }
    }
}
