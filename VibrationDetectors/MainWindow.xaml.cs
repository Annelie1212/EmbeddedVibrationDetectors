using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using VibrationDetectors.Models;
using VibrationDetectors.ViewModels;
using VibrationDetectors.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace VibrationDetectors
{

    public partial class MainWindow : Window

    {
        private IHost _host;
        private string _logFilePath = "";
        private ObservableCollection<string>? _eventLog;

        MainWindowViewModel _vm;

        private VibrationSignalWorker _vibrationWorker; // keep as a field

        public MainWindow()
        {
            InitializeComponent();
            StartGrpcServer();
            InitializeFeatures();
            _vm = new MainWindowViewModel();

            _vibrationWorker = new VibrationSignalWorker();
            _vibrationWorker.Start();
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
        }

        public void InitializeFeatures()
        {
            _eventLog = [];
            LB_EventLog.ItemsSource = _eventLog;
        }


        public void Btn_Armed_Click(object sender, RoutedEventArgs e)
        {
            VibrationDetector.Btn_Armed();
            _vm.ButtonVM.UpdateButtonViewModel();
            UpdateView();

            Debug.WriteLine($"Worker: , DetectorLevel={VibrationDetector.VibrationLevel}");
        }

        public void Btn_TriggedState_Click(object sender, RoutedEventArgs e)
        {
            VibrationDetector.Btn_Trigged();
            _vm.ButtonVM.UpdateButtonViewModel();
            UpdateView();
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

        private void ThresholdChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //if (_skipSliderAction)
            //{
            //    _skipSliderAction = false;
            //    return; // Skip first event
            //}

            //var sliderValue = Slider_Threshold.Value;
            //SliderValues.Add(sliderValue);
            //_hasSliderChanged = true;

        }
    }
}