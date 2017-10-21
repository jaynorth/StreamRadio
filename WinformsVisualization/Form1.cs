using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.DSP;
using CSCore.SoundOut;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Streams.Effects;
using CSCore.CoreAudioAPI;
using WinformsVisualization.Visualization;

namespace WinformsVisualization
{
    public partial class Form1 : Form
    {
        private WasapiCapture _soundIn;
        private ISoundOut _soundOut;
        private IWaveSource _source;
        private PitchShifter _pitchShifter;
        private LineSpectrum _lineSpectrum;
        //private VoicePrint3DSpectrum _voicePrint3DSpectrum;

        private readonly Bitmap _bitmap = new Bitmap(2000, 600);
        //private int _xpos;

        public Form1()
        {
            InitializeComponent();
            DefaultDevice();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //empty implementation
        }

        
        private void DefaultDevice()
        {
          //  Stop();

            //open the default device 
            _soundIn = new WasapiLoopbackCapture(); 
            //Our loopback capture opens the default render device by default so the following is not needed
            //_soundIn.Device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Console);
            _soundIn.Initialize();

           

            var soundInSource = new SoundInSource(_soundIn);
            ISampleSource source = soundInSource.ToSampleSource().AppendSource(x => new PitchShifter(x), out _pitchShifter);

            SetupSampleSource(source);

            // We need to read from our source otherwise SingleBlockRead is never called and our spectrum provider is not populated
            byte[] buffer = new byte[_source.WaveFormat.BytesPerSecond /2];
            soundInSource.DataAvailable += (s, aEvent) =>
            {
                
                int read;
                while ((read = _source.Read(buffer, 0, buffer.Length)) > 0) ;
            };

            
            //play the audio
            _soundIn.Start();



            timer1.Start();

        }

      


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSampleSource"></param>
        private void SetupSampleSource(ISampleSource aSampleSource)
        {
            const FftSize fftSize = FftSize.Fft4096;
            //create a spectrum provider which provides fft data based on some input
            var spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels,
                aSampleSource.WaveFormat.SampleRate, fftSize);

            //linespectrum and voiceprint3dspectrum used for rendering some fft data
            //in oder to get some fft data, set the previously created spectrumprovider 
            _lineSpectrum = new LineSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = true,
                BarCount = 50,
                BarSpacing = 2,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt
            };
           

            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

            _source = notificationSource.ToWaveSource(16);

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Stop();
        }

        private void Stop()
        {
            timer1.Stop();

            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_soundIn != null)
            {
              _soundIn.Stop();
              _soundIn.Dispose();
              _soundIn = null;
            }
            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //render the spectrum
            GenerateLineSpectrum();

        }

        private void GenerateLineSpectrum()
        {
            Image image = pictureBoxTop.Image;
            var newImage = _lineSpectrum.CreateSpectrumLine(pictureBoxTop.Size, Color.Green, Color.Red, Color.Black, false);
            if (newImage != null)
            {
                pictureBoxTop.Image = newImage;
                if (image != null)
                    image.Dispose();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _soundIn.Stop();
            _bitmap.Dispose();
            _soundIn.Dispose();

            Close();

            Application.Exit();
        }
    }
}
