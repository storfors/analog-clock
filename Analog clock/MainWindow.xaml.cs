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
using System.Windows.Threading;

namespace WPF_Playground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int _canvasWidth = 200;
        const int _secHandScale = 100;
        const int _minHandScale = 100 - 5;
        const int _hourHandScale = 100 - 30;
        DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(OnTimerTick);
            _timer.Start();
        }

        private void OnTimeChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextTime.Text = $"The time is: {SliderHours.Value}:{SliderMinutes.Value}:{SliderSeconds.Value}";
            var time = DateTime.Now;
            var seconds = (time.Second + SliderSeconds.Value) % 60;
            var minutes = (time.Minute + SliderMinutes.Value) % 60;
            var hours = (time.Hour + SliderHours.Value) % 24;
            var watch = new Watch((int)seconds, (int)minutes, (int)hours);
            var secondsAngle = watch.SecondsAngle();
            var minutesAngle = watch.MinutesAngle();
            var hoursAngle = watch.HoursAngle();
            DrawSecondsHand(secondsAngle);
            DrawMinutesHand(minutesAngle);
            DrawHoursHand(hoursAngle);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var time = DateTime.Now;
            var seconds = (time.Second + SliderSeconds.Value) % 60;
            var minutes = (time.Minute + SliderMinutes.Value) % 60;
            var hours = (time.Hour + SliderHours.Value) % 24;
            TextTime.Text = $"The time is: {hours}:{minutes}:{seconds}";
            var watch = new Watch((int)seconds, (int)minutes, (int)hours);
            var secondsAngle = watch.SecondsAngle();
            var minutesAngle = watch.MinutesAngle();
            var hoursAngle = watch.HoursAngle();
            DrawSecondsHand(secondsAngle);
            DrawMinutesHand(minutesAngle);
            DrawHoursHand(hoursAngle);
        }

        private void DrawSecondsHand(double angle)
        {
            LineSeconds.X2 = _secHandScale * Math.Cos(angle - Math.PI/2) + _canvasWidth / 2;
            LineSeconds.Y2 = _secHandScale * Math.Sin(angle - Math.PI/2) + _canvasWidth / 2;
        }

        private void DrawMinutesHand(double angle)
        {
            
            LineMinutes.X2 = _minHandScale * Math.Cos(angle - Math.PI/2) + _canvasWidth / 2;
            LineMinutes.Y2 = _minHandScale * Math.Sin(angle - Math.PI/2) + _canvasWidth / 2;
        }

        private void DrawHoursHand(double angle)
        {
            LineHours.X2 = _hourHandScale * Math.Cos(angle - Math.PI / 2) + _canvasWidth / 2;
            LineHours.Y2 = _hourHandScale * Math.Sin(angle - Math.PI / 2) + _canvasWidth / 2;
        }
    }

    public class Watch
    {
        int _seconds = 0;
        int _minutes = 0;
        int _hours = 0;
        double _secondsAngle = 0;
        double _minutesAngle = 0;
        double _hoursAngle = 0;

        bool _updateAngles = true;

        public Watch(int hours, int minutes, int seconds)
        {
            SetTime(hours, minutes, seconds);
        }

        public void SetTime(int seconds, int minutes, int hours)
        {
            if (seconds < 0 || seconds > 59) throw new ArgumentOutOfRangeException("Seconds can only be between 0-59.");
            if (minutes < 0 || minutes > 59) throw new ArgumentOutOfRangeException("Minutes can only be between 0-59.");
            if (hours   < 0 || hours   > 23) throw new ArgumentOutOfRangeException("Hours can only be between 0-23.");

            _seconds = seconds;
            _minutes = minutes;
            _hours = hours % 12;

            _updateAngles = true;
        }

        public double SecondsAngle()
        {
            if (_updateAngles)
                UpdateAngles();

            return _secondsAngle;
        }

        public double MinutesAngle()
        {
            if (_updateAngles)
                UpdateAngles();

            return _minutesAngle;
        }

        public double HoursAngle()
        {
            if (_updateAngles)
                UpdateAngles();

            return _hoursAngle;
        }

        private void UpdateAngles()
        {
            _secondsAngle = (2 * Math.PI * _seconds / 60);
            _minutesAngle = (2 * Math.PI * _minutes / 60) + _secondsAngle / 60;
            _hoursAngle = (2 * Math.PI * _hours / 12) + _minutesAngle / 12;

            _updateAngles = false;
        }
    }
}
