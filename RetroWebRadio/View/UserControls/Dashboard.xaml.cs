using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using ViewModelsXML.ViewModels;

namespace RetroWebRadio.View.UserControls
{
    /// <summary>
    /// Logique d'interaction pour Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {

        
        bool pause = false;
        DispatcherTimer dt = new DispatcherTimer();
        public Dashboard()
        {
            InitializeComponent();

            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 1);
            //Buffering
            Player.BufferingStarted += Player_BufferingStarted;
            Player.BufferingEnded += Player_BufferingEnded;

            Player.MediaEnded += Player_MediaEnded;
            Player.MediaOpened += Player_MediaOpened;
            Player.MediaFailed += Player_MediaFailed;

        }


        private void Player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            displayBox.Text = "MediaFailed";
        }

        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
           
            displayBox.Text = "Opened ..";
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
           
            displayBox.Text = "MediaEnded";
        }

        private void Player_BufferingEnded(object sender, RoutedEventArgs e)
        {
            
            string s = "buffer ended";
            //BufferProgress.Value = Player.BufferingProgress * 100;
            double v = Player.BufferingProgress * 100;
            displayBox.Text = s + " " + v.ToString();

            string b = (Player.BufferingProgress * 100).ToString();
            displayBox.Text = "Buffer Progress " + b + " %";
            dt.Stop();
           // displayBox.Text = "Playing ...";

 
            // displayBox.Text += " " + Player.NaturalDuration;
        }

        private void dt_Tick(object sender, EventArgs e)
        {

            string b = (Player.BufferingProgress * 100).ToString();
            displayBox.Text = "Buffer Progress " + b + " %";

        }

        private void Player_BufferingStarted(object sender, RoutedEventArgs e)
        {
            
            dt.Start();
            string s = "buffer started";
            displayBox.Text = s;
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            play_button.Foreground = new SolidColorBrush(Colors.Orange);
            play_button.FontWeight = FontWeights.Bold;
           
            
            stop_button.Foreground = new SolidColorBrush(Colors.Black);

            Player.Play();
            displayBox.Text = "Attempt to play Stream";
            //RadioOff.IsChecked = true;
        
            pause_button.IsEnabled = true;
            pause = false;

        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            Player.Stop();
            stop_button.Foreground = new SolidColorBrush(Colors.Orange);
            stop_button.FontWeight = FontWeights.Bold;
            play_button.Foreground = new SolidColorBrush(Colors.Black);
            pause_button.Foreground = new SolidColorBrush(Colors.Black);
            displayBox.Text = "Stopped";
            //RadioOff.IsChecked = false;

            pause_button.IsEnabled = false;
        }

        private void pause_button_Click(object sender, RoutedEventArgs e)
        {
            if (pause)
            {
                pause_button.Foreground = new SolidColorBrush(Colors.Black);
                Player.Play();
                pause = false;
                displayBox.Text = "Playing ..";
            }
            else
            {
                Player.Pause();
                pause = true;
                pause_button.Foreground = new SolidColorBrush(Colors.Orange);
 
           
                displayBox.Text = "Paused";

            }

        }



        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double vol;
            double v = volumeSlider.Value;
            double decibel = Math.Log(10, v) * -0.01;

            if (v > 0 && Double.IsNaN(decibel))
            {
                decibel = 1d;
                vol = 1d;
            }

            else if (decibel < 0 || Double.IsNaN(decibel))
            {
                vol = 0;
            }
            else
            {
                vol = decibel;
            }


            Player.Volume = vol;
        }

      

        private void TargetEventhandler(object sender, DataTransferEventArgs e)
        {
            
            
            //
            

        }

        
    }
}
