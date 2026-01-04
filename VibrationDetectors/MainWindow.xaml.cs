using AlarmDatabaseLibrary.Context;
using AlarmDatabaseLibrary.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VibrationDetectors.Models;
using VibrationDetectors.Services;
using VibrationDetectors.ViewModels;
using static VibrationDetectors.Models.Enumerators;

namespace VibrationDetectors
{

    public partial class MainWindow : Window

    {
        //Should be set based on dropdown list later maybe!
        //private int _deviceId = 1;
        //TODO make sure that the static VD does not include nulls if we cannot read
        //from device.

        private IHost _host;
        private string _logFilePath = "";
        private DoubleAnimation _animation;
        private ObservableCollection<string>? _eventLog;

        private double _vibrationSpeedValue = 0;
        private double _linePositionSlider_Value = 0;
        private DispatcherTimer _timer;

        public List<DeviceLog> DeviceLogs { get; set; } = new List<DeviceLog>();

        public DeviceLog DeviceLog { get; set; } = new DeviceLog();

        //public List<double> SliderValues { get; set; } = new List<double>();
        //double _previousSliderValues = -1;
        //bool _hasSliderChanged = false;

        bool _skipSliderAction = true;

        MainWindowViewModel _vm;

        private VibrationSignalWorker _vibrationWorker; // keep as a field

        //------SLIDER CHANGE HANDLING VARS------
        private DispatcherTimer _sliderDebounceTimer;
        private double _pendingSliderValue;

        private DbLogService _dbLogService;

        private readonly AlarmDbContext _context;

        private DateTime LatestDateTimeStamp;

        public MainWindow(DbLogService dbLogService)
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            StartGrpcServer();
            
            InitializeFeatures();

            _dbLogService = dbLogService;

            StartWorker();

            _vm = new MainWindowViewModel();

            _vibrationWorker = new VibrationSignalWorker();
            _vibrationWorker.Start();

            _sliderDebounceTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _sliderDebounceTimer.Tick += SliderDebounceTimer_Tick;

            //Get status from database. I want to read using my DbLogServices now!
            DeviceLog dvtest = _dbLogService.ReadOne(VibrationDetector.DeviceId);
            try
            {
                //TODO: check why the oldvalue becomes 0 when error reading from database!!!
                Debug.WriteLine($"error:{dvtest.ErrorMessage} oldvalue{dvtest.OldUserValue}");
            }
            catch (Exception ex)
            {
                
                Debug.WriteLine($"Error reading from database!");
            }

            DeviceLog = dvtest;

            SaveDbLogInVibrationDetectorCache(dvtest);

            DoWork();
        }


        private void StartWorker()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => DoWork();
            _timer.Start();
        }
        //public async void PostSliderValue()
        //{
        //    //Check length of SliderValues list
        //    if ((SliderValues.Count == _previousSliderValues) && (_hasSliderChanged == true))
        //    {

        //        Console.WriteLine("Slider has not changed for 1 second -> posting new value to API.");

        //        var sliderValue = SliderValues.Last();

        //        string logMessage = DeviceActions.SetThresholdLevel(sliderValue);
        //        LogMessage(logMessage);

        //        SyncUserPanelWPF();

        //        //double testvalueSpeed = _vibrationSpeedValue;

        //        SliderValues.Clear();
        //        _previousSliderValues = -1;
        //        _hasSliderChanged = false;
        //    }
        //    else
        //    {
        //        _previousSliderValues = SliderValues.Count;
        //        Console.WriteLine("Slider has changed, waiting for it to stabilize...");
        //    }
        //}
        
        //public void SaveSliderValueInCache()
        //{
        //    //Check length of SliderValues list
        //    if ((SliderValues.Count == _previousSliderValues) && (_hasSliderChanged == true))
        //    {

        //        Console.WriteLine("Slider has not changed for 1 second -> posting new value to API.");

        //        var sliderValue = SliderValues.Last();

        //        string logMessage = DeviceActions.SetThresholdLevel(sliderValue);
        //        LogMessage(logMessage);

        //        SyncUserPanelWPF();

        //        //double testvalueSpeed = _vibrationSpeedValue;

        //        SliderValues.Clear();
        //        _previousSliderValues = -1;
        //        _hasSliderChanged = false;
        //    }
        //    else
        //    {
        //        _previousSliderValues = SliderValues.Count;
        //        Console.WriteLine("Slider has changed, waiting for it to stabilize...");
        //    }
        //}

        public async void SyncUserPanelWPF()
        {
            //1. Hämtar status från modellen via API:et
            //VDFetchStatusResponse statusResponse = await VDClientService.FetchStatusVDAsync();

            //2. Updaterar vår lokala model VibrationDetector
            //VibrationDetector.Update(statusResponse);

            DeviceLog dl = new DeviceLog();
            //dl.PopulateDeviceLog(statusResponse);
            //3 . Uppdaterar lokal vyn och ui
            //UpdateUserPanelView(dl);
        }

        //Gammal UPDATE-metod som inte längre används
        //public void UpdateUserPanelView(DeviceLog dl)
        //{
        //    //Animation update utifrån slider value + threshold + vibration level

        //    //Vibration level
        //    //ScaleY_VibrationLevelChanged(VibrationDetector.VibrationLevel);
        //    _vibrationSpeedValue = (double)VibrationDetector.VibrationLevel;
        //    VibrationSpeed_ValueChanged();

        //    //Armknapp
        //    Btn_OnOff.Content = DeviceActions.GetArmedState() ? "STOP" : "START";
        //    Btn_OnOff.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DeviceActions.GetArmedState() ? "#F40B0B" : "#00FF00")); // Red or Green
        //    Btn_OnOff.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DeviceActions.GetArmedState() ? "#F40B0B" : "#00FF00")); // Red or Green

        //    //Triggknapp
        //    BellImage.Source = ChangeColor(BellImage.Source, DeviceActions.GetTriggedState() ? "#F40B0B" : "#E6D825");
        //    Btn_Trigged.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DeviceActions.GetTriggedState() ? "#F40B0B" : "#E6D825")); // Yellow

        //    //THRESHOLD 0-10
        //    //Slider Röd linje och threshold.
        //    //Tex Threshold = 7;
        //    //AnimationBox.ActualHeight = 200;
        //    //Röd linje = 140 (7/10 * 200) pixlar från toppen.
        //    Slider_Threshold.Value = VibrationDetector.VibrationLevelThreshold;
        //    _linePositionSlider_Value = (double)(-10 * VibrationDetector.VibrationLevelThreshold) + 100;
        //    ThresholdLine.Y1 = _linePositionSlider_Value;


        //    //Loggning
        //    LogMessage(dl.LogMessage);
        //}
        
        private void VibrationSpeed_ValueChanged()
        {
            // Update speed by restarting the animation
            StartAnimation();
        }
        
        //This worker is only working with getting vibration signals from outside world.
        private void DoWork()
        {
            //0 till 100
            //HorizontalLine.Y1 = 100;

            //--------------TILLBAKA SEN---------------
            //PostSliderValue();
            //SaveSliderValueInCache();
            

            SaveVibrationLevelInCache();

            // Runs every second
            //Console.WriteLine($"Tick doing work at {DateTime.Now}");

            //--------------TILLBAKA SEN---------------
            SyncUserPanelWPF();

            //Update View and viewModel
            UpdateViewModels();
            UpdateView();
        }

        public void SaveVibrationLevelInCache()
        {
            VibrationDetector.VibrationLevel = (int)_vibrationWorker.SignalValue;
        }

        public void SaveDbLogInVibrationDetectorCache(DeviceLog dl)
        {
            VibrationDetector.DeviceId = dl.DeviceId;
            VibrationDetector.UserId = dl.UserId;
            VibrationDetector.DeviceName = dl.DeviceName;
            VibrationDetector.Location = dl.Location;
            VibrationDetector.AlarmArmed = dl.AlarmArmed;   
            VibrationDetector.AlarmTriggered = dl.AlarmTriggered;
            VibrationDetector.VibrationLevel = dl.VibrationLevel;
            VibrationDetector.VibrationLevelThreshold = dl.VibrationLevelThreshold;


        }


        //This methods takes everything from the ViewModel and updates the view accordingly.
        public void UpdateView()
        {
            Btn_OnOff.Content = _vm.ButtonVM.OnOffContent;
            Btn_OnOff.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_vm.ButtonVM.OnOffForeground));
            Btn_OnOff.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_vm.ButtonVM.OnOffBorderBrush));
            //Btn_Trigged.Content = _vm.ButtonVM.TrigContent;
            BellImage.Source = ChangeColor(BellImage.Source, _vm.ButtonVM.TrigContent);
            Btn_Trigged.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_vm.ButtonVM.TrigForeground));
            Btn_Trigged.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_vm.ButtonVM.TrigBorderBrush));

            ThresholdLine.Y1 = _vm.SliderVM.LinePositionSliderValue;

            //Update vibrationinput view
            ScaleY_VibrationLevelChanged();
            VibrationSpeed_ValueChanged();

            //Update slider position - not allowed to be new slider solution.
            //Slider_Threshold.Value = _vm.SliderVM.SliderValue;

        }

        public void UpdateViewModels()
        {
            _vm.ButtonVM.UpdateButtonViewModel();
            _vm.SliderVM.UpdateSliderViewModel();
            _vm.VibrationVM.UpdateVibrationViewModel();
        }

        

        //-----------------------------------INITIALIZATION-----------------------------------------
        //---------------------------------------------------------------------------------------

        private void StartAnimation()
        {
            // Ensure layout is ready
            AnimationBox.UpdateLayout();
            MovingVectorContainer.UpdateLayout();

            double containerWidth = MovingVectorContainer.ActualWidth;

            if (containerWidth <= 0)
            {
                MovingVectorContainer.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                containerWidth = MovingVectorContainer.DesiredSize.Width;
            }


            //double convertedSpeedValue = -0.188 * _vibrationSpeedValue + 2.0;
            double convertedSpeedValue = _vm.VibrationVM.VibrationSpeedValue;

            // Get duration from slider (default fallback = 3 sec)
            double durationSeconds = convertedSpeedValue;
            //
            //double durationSeconds = 2;

            double from = 20;
            double to = -containerWidth / 2 + 20;

            VectorTransform.Y = 24;

            _animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(durationSeconds),
                RepeatBehavior = RepeatBehavior.Forever
            };

            VectorTransform.BeginAnimation(TranslateTransform.XProperty, _animation);
        }

        private void InitializeSliderFromModel()
        {
            _skipSliderAction = true;

            Slider_Threshold.Value = VibrationDetector.VibrationLevelThreshold;

            _skipSliderAction = false;

            // Force visual update
            UpdateViewModels();
            UpdateView();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Hook up slider events
            //ScaleYSlider.ValueChanged += ScaleYSlider_ValueChanged;
            //SpeedSlider.ValueChanged += VibrationSpeed_ValueChanged;

            this.Dispatcher.InvokeAsync(() =>
            {
                // Ensure layout is measured so ActualHeight is valid
                InitializeSliderFromModel();
                AnimationBox.UpdateLayout();
                if (AnimationBox.ActualHeight > 0)
                    _linePositionSlider_Value = AnimationBox.ActualHeight / 2;
            });

            StartAnimation();
        }

        public void InitializeFeatures()
        {
            _eventLog = [];
            LB_EventLog.ItemsSource = _eventLog;



        }

        private void StartGrpcServer()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ListenLocalhost(5001, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                    });

                    webBuilder.ConfigureServices(services =>
                    {
                        services.AddGrpc();
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGrpcService<VDStatusHandlerService>();
                        });
                    });
                })
                .Build();

            _host.Start();
            Console.WriteLine("gRPC server started on http://localhost:5001 with HTTP/2");

        }

        //-----------------------------------GRAPHICS WINDOW-----------------------------------------
        //---------------------------------------------------------------------------------------

        private void ScaleY_VibrationLevelChanged()
        {
            //double newValue = (double)newValueInt / 5 * 0.85;

            // Update ScaleY on both vectors
            //VectorScale1.ScaleY = newValue;
            //VectorScale2.ScaleY = newValue;

            VectorScale1.ScaleY = _vm.VibrationVM.VectorScaleValue;
            VectorScale2.ScaleY = _vm.VibrationVM.VectorScaleValue;


        }

        //-----------------------------------VIBRATION WORKER-----------------------------------------
        //---------------------------------------------------------------------------------------

        //-----------------------------------SLIDER-----------------------------------------
        //---------------------------------------------------------------------------------------
        private void Slider_Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ThresholdChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LatestDateTimeStamp = DateTime.Now;


            if (_skipSliderAction)
            {
                _skipSliderAction = false;
                return;
            }

            // Store latest value
            _pendingSliderValue = Slider_Threshold.Value;

            // Restart debounce timer
            _sliderDebounceTimer.Stop();
            _sliderDebounceTimer.Start();
        }
        // Save ONCE after 1s inactivity
        private void SliderDebounceTimer_Tick(object? sender, EventArgs e)
        {

            //0. Stop the timer
            _sliderDebounceTimer.Stop();

            //1. Get old user value before it is overwritten.
            var oldUserValue = VibrationDetector.VibrationLevelThreshold;

            //2. Set threshold level, nothing else!
            DeviceActions.SetThresholdLevel(_pendingSliderValue);

            //3 Get new user value.
            var newUserValue = VibrationDetector.VibrationLevelThreshold;

            //3.1. Create success log messsage
            string logMessage = DeviceLog.BuildLogMessage(LatestDateTimeStamp,
                                                          oldUserValue, 
                                                          newUserValue,
                                                          DeviceAction.SetThreshold,
                                                          StatusAndErrorType.Success);

            //4. add logmessage to eventlog view.
            _eventLog?.Add(logMessage);

            //5. Scroll to the latest log entry.
            if (LB_EventLog.Items.Count > 0)
                LB_EventLog.ScrollIntoView(LB_EventLog.Items[^1]);

            //6. Update devicelog object.
            DeviceLog.UpdateDeviceLog(LatestDateTimeStamp,
                            DeviceAction.SetThreshold,
                            oldUserValue,
                            newUserValue,
                            VibrationDetector.UserId,
                            VibrationDetector.DeviceId,
                            VibrationDetector.DeviceName,
                            VibrationDetector.Location,
                            StatusAndErrorType.Success,
                            VibrationDetector.AlarmArmed,
                            VibrationDetector.AlarmTriggered,
                            VibrationDetector.VibrationLevel,
                            VibrationDetector.VibrationLevelThreshold,
                            logMessage
                            );

            //7. add devicelog to list.
            DeviceLogs.Add(DeviceLog);

            //8. add devicelog to database.
            _dbLogService.CreateOne(DeviceLog);

            //9. Update viewmodels and view.
            UpdateViewModels();

            //10. Update view.
            UpdateView();

            //11. Debug output
            Debug.WriteLine($"Slider committed value: {_pendingSliderValue}");
        }

        public void CompleteLogAction(bool oldUserValue, bool newUserValue, DeviceAction deviceAction)
        {
            //3.1. Create success log messsage
            string logMessage = DeviceLog.BuildLogMessage(LatestDateTimeStamp,
                                                          (oldUserValue == true) ? 1 : 0,
                                                          (newUserValue == true) ? 1 : 0,
                                                          deviceAction,
                                                          StatusAndErrorType.Success);

            //4. add logmessage to eventlog view.
            _eventLog?.Add(logMessage);

            //5. Scroll to the latest log entry.
            if (LB_EventLog.Items.Count > 0)
                LB_EventLog.ScrollIntoView(LB_EventLog.Items[^1]);

            //6. Update devicelog object.
            DeviceLog.UpdateDeviceLog(LatestDateTimeStamp,
                            deviceAction,
                            (oldUserValue == true) ? 1 : 0,
                            (newUserValue == true) ? 1 : 0,
                            VibrationDetector.UserId,
                            VibrationDetector.DeviceId,
                            VibrationDetector.DeviceName,
                            VibrationDetector.Location,
                            StatusAndErrorType.Success,
                            VibrationDetector.AlarmArmed,
                            VibrationDetector.AlarmTriggered,
                            VibrationDetector.VibrationLevel,
                            VibrationDetector.VibrationLevelThreshold,
                            logMessage
                            );

            //7. add devicelog to list.
            DeviceLogs.Add(DeviceLog);

            //8. add devicelog to database.
            _dbLogService.CreateOne(DeviceLog);
        }

        //-----------------------------------BUTTONS-----------------------------------------
        //---------------------------------------------------------------------------------------
        public void Btn_Armed_Click(object sender, RoutedEventArgs e)
        {
            //int logMessageCount = 1;
            //if ( VibrationDetector.AlarmArmed == true && VibrationDetector.AlarmTriggered == true)
            //{

            //}

            //1. Get old user value before it is overwritten.
            var oldUserValue = VibrationDetector.AlarmArmed;

            //2. Set threshold armstate, nothing else!
            DeviceAction deviceAction = DeviceActions.Btn_Armed();
            
            //3 Get new user value.
            var newUserValue = VibrationDetector.AlarmArmed;

            CompleteLogAction(oldUserValue, newUserValue, deviceAction);

            if (VibrationDetector.AlarmArmed == true && VibrationDetector.AlarmTriggered == true)
            {
                //1. Get old user value before it is overwritten.
                oldUserValue = VibrationDetector.AlarmTriggered;

                deviceAction = DeviceActions.Btn_Trigged();

                //3 Get new user value.
                newUserValue = VibrationDetector.AlarmTriggered;

                CompleteLogAction(oldUserValue, newUserValue, deviceAction);
            }



            //old2. Get log messages from action
            //List<string> logList = DeviceActions.Btn_Armed();



            //9. Update viewmodels and view.
            UpdateViewModels();

            //10. Update view.
            UpdateView();

            //11. Debug output
            Debug.WriteLine($"Worker: , DetectorLevel={VibrationDetector.VibrationLevel}");

            //foreach (var logMessage in logList)
            //{
            //    //LogMessage(logMessage, DeviceAction.Error); //Change later to real
            //    //Buildlogmessage
            //}

            //_vm.ButtonVM.UpdateButtonViewModel();
            //UpdateView();





        }

        public void Btn_TriggedState_Click(object sender, RoutedEventArgs e)
        {
            DeviceAction logMessage = DeviceActions.Btn_Trigged();
            
            //LogMessage(logMessage,DeviceAction.Error);
            //Buildlogmessage

            //DeviceActions.Btn_Trigged();
            //_vm.ButtonVM.UpdateButtonViewModel();
            //UpdateView();
        }

        public ImageSource ChangeColor(ImageSource source, string color)
        {
            // 1. Get the original DrawingImage from the Image
            var originalDrawingImage = (DrawingImage)BellImage.Source;

            // 2. Clone it so it is modifiable (unfrozen)
            var modifiableDrawingImage = originalDrawingImage.Clone();

            // 3. Access the GeometryDrawing inside the cloned DrawingImage
            var geometryDrawing = (GeometryDrawing)modifiableDrawingImage.Drawing;

            // 4. Set the new color using ColorConverter
            geometryDrawing.Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));

            // 5. Assign the modified DrawingImage back to the Image
            return modifiableDrawingImage;

        }

        //-----------------------------------LOG MESSAGE-----------------------------------------
        //---------------------------------------------------------------------------------------

        //moved to DeviceLog class

        //This has the purpose of a dictionary between enum and user inputted values.
        public int ActionToValue(DeviceAction action)
        {
            switch(action)
            {
                case DeviceAction.ArmDevice:
                    return VibrationDetector.AlarmArmed ? 1 : 0;
                case DeviceAction.DisarmDevice:
                    return VibrationDetector.AlarmArmed ? 1 : 0;
                case DeviceAction.TriggerDevice:
                    return VibrationDetector.AlarmTriggered ? 1 : 0;
                case DeviceAction.ResetDevice:
                    return VibrationDetector.AlarmTriggered ? 1 : 0;
                case DeviceAction.SetThreshold:
                    return VibrationDetector.VibrationLevelThreshold;
                case DeviceAction.TriggerFailure:
                    return VibrationDetector.AlarmTriggered ? 1 : 0;
                default:
                    return -1; // Unknown action

            }


        }

        //private void ThresholdChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{

        //    if (_skipSliderAction)
        //    {
        //        _skipSliderAction = false;
        //        return; // Skip first event
        //    }

        //    var sliderValue = Slider_Threshold.Value;
        //    SliderValues.Add(sliderValue);
        //    _hasSliderChanged = true;

        //    DeviceActions.SetThresholdLevel(sliderValue);

        //    UpdateViewModels();

        //    UpdateView();

        //}







    }
}