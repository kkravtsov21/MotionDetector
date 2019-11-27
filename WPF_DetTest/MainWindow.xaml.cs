using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;
using System.Timers;
using System.Windows.Threading;

namespace AForge.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow_Loaded : Window
    {

       #region Public properties

        public ObservableCollection<FilterInfo> VideoDevices { get; set; }

        //public FilterInfo CurrentDevice
        //{
        //    get { return _currentDevice; }
        //    set { _currentDevice = value; this.OnPropertyChanged("CurrentDevice"); }
        //}
       // private FilterInfo _currentDevice;

        #endregion

        public Bitmap GetCurrentVideoFrame;
        MotionDetector Detector;
        private FilterInfoCollection Devices;
        private VideoCaptureDevice VidSource;
        private DispatcherTimer timer;
        private MotionAreaHighlighting MotionAreaHighlighting;
         









       #region Private fields

        private IVideoSource _videoSource;
        private double DetLevel;

        #endregion

        public MainWindow_Loaded()
        {
            InitializeComponent();
            Detector = new MotionDetector(new TwoFramesDifferenceDetector(), new MotionBorderHighlighting());
            DetLevel = 0;

         Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo x in Devices)
            {
                comboBox.Items.Add(x.Name);
            }
            comboBox.SelectedIndex = 0;

        }
     
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (VidSource.IsRunning == true)
            {
                VidSource.Stop();
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            VidSource = new VideoCaptureDevice(Devices[comboBox.SelectedIndex].MonikerString);
            videoSourcePlayer1.VideoSource = VidSource;
          
            videoSourcePlayer1.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            videoSourcePlayer1.SignalToStop();
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
        private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            DetLevel = Detector.ProcessFrame(image);
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
      
        }
    }
}
