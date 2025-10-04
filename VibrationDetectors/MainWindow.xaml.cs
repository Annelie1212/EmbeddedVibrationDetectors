using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using VibrationDetectors.Services;

namespace VibrationDetectors
{
   
    public partial class MainWindow : Window
    {
        private string _logFilePath = "";
        private ObservableCollection<string>? _eventLog;
        public MainWindow()
        {
            InitializeComponent();
            InitializeFeatures();
        }

        public void InitializeFeatures()
        {
            _eventLog = [];
            LB_EventLog.ItemsSource = _eventLog;
        }
        public void Btn_Armed_Click(object sender, RoutedEventArgs e)
        {
            DeviceActions.ToggleArmedState();

            if (DeviceActions.GetArmedState())
            {
                Btn_OnOff.Content = "STOP";
                Btn_OnOff.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F40B0B")); // Red
                Btn_OnOff.BorderBrush =new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F40B0B")); // Red

                LogMessage(DeviceActions.GetDeviceName()+" has started.");
            }
            else
            {
                Btn_OnOff.Content = "START";
                Btn_OnOff.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF00")); // Green
                Btn_OnOff.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF00")); // Green

                LogMessage(DeviceActions.GetDeviceName() + " has stopped.");

                if (DeviceActions.GetTriggedState()==true)
                {

                    DeviceActions.ToggleTriggedState();

                    Btn_Trigged.Content = "TRIGGER ALARM";
                    Btn_Trigged.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6D825")); // Yellow
                    Btn_Trigged.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6D825")); // Yellow
                    LogMessage(DeviceActions.GetDeviceName() + " alarm has been reset.");
                }
            }


        }
        public void Btn_TriggedState_Click(object sender, RoutedEventArgs e) { 

            if (!DeviceActions.GetArmedState())
            {
                LogMessage("Cannot trigger alarm when device is not armed.");
                
            }
            else
            {
                DeviceActions.ToggleTriggedState();

                if (DeviceActions.GetTriggedState()==true)
                {
                    Btn_Trigged.Content = "RESET ALARM";
                    Btn_Trigged.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F40B0B")); // Red
                    Btn_Trigged.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F40B0B")); // Red
                    LogMessage(DeviceActions.GetDeviceName() + " has triggered the alarm!");
                }
                else
                {
                    Btn_Trigged.Content = "TRIGGER ALARM";
                    Btn_Trigged.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6D825")); // Yellow
                    Btn_Trigged.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6D825")); // Yellow
                    LogMessage(DeviceActions.GetDeviceName() + " alarm has been reset.");
                }
            }


        }

        private void Slider_Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
        private void LogMessage(string message)
        {
            var line = @$"{DateTime.Now:yyy-MM-dd HH:mm:ss} : {message}";
            _eventLog?.Add(line);

            try
            {
                //File.AppendAllText(_logFilePath, line + Environment.NewLine);
            }
            catch
            {

            }

            if (LB_EventLog.Items.Count > 0)
                LB_EventLog.ScrollIntoView(LB_EventLog.Items[^1]);

        }
    }
}