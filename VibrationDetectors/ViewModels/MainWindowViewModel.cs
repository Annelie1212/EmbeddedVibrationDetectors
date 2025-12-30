using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrationDetectors.Models;

namespace VibrationDetectors.ViewModels
{
    public class MainWindowViewModel
    {
        public ButtonViewModel ButtonVM { get; set; } = new ButtonViewModel();
        public LogViewModel LogVM { get; set; } = new LogViewModel();
        public SliderViewModel SliderVM { get; set; } = new SliderViewModel();

        public VibrationViewModel VibrationVM { get; set; } = new VibrationViewModel();

        public class ButtonViewModel
        {
            public string OnOffContent { get; set; }
            public string OnOffForeground { get; set; }
            public string OnOffBorderBrush { get; set; }

            public string TrigContent { get; set; }
            public string TrigForeground { get; set; }
            public string TrigBorderBrush { get; set; }

            public void UpdateButtonViewModel()
            {
                if (VibrationDetector.AlarmTriggered == false && VibrationDetector.AlarmArmed == false)
                {
                    TrigContent = "#E6D825"; // Yellow
                    TrigForeground = "#E6D825"; // Yellow
                    TrigBorderBrush = "#E6D825"; // Yellow

                    OnOffContent = "START";
                    OnOffForeground = "#00FF00"; // Green
                    OnOffBorderBrush = "#00FF00"; // Green
                }
                else if ( VibrationDetector.AlarmTriggered == false && VibrationDetector.AlarmArmed == true )
                {
                    TrigContent = "#E6D825"; // Yellow
                    TrigForeground = "#E6D825"; // Yellow
                    TrigBorderBrush = "#E6D825"; // Yellow

                    OnOffContent = "STOP";
                    OnOffForeground = "#F40B0B"; // Red
                    OnOffBorderBrush = "#F40B0B"; // Red
                }
                else
                {
                    TrigContent = "#F40B0B"; // Red
                    TrigForeground = "#F40B0B"; // Red
                    TrigBorderBrush = "#F40B0B"; // Red

                    OnOffContent = "STOP";
                    OnOffForeground = "#F40B0B"; // Red
                    OnOffBorderBrush = "#F40B0B"; // Red
                }


            }
        }

        public class LogViewModel
        {
        }

        public class SliderViewModel
        {
            public int SliderValue { get; set; }
            public double LinePositionSliderValue { get; set; }
            public void UpdateSliderViewModel()
            {
                SliderValue = VibrationDetector.VibrationLevelThreshold;
                LinePositionSliderValue = (double)(-10 * VibrationDetector.VibrationLevelThreshold) + 100;
            }
        }

        public class VibrationViewModel
        {
            public int VibrationLevel { get; set; }

            public double VibrationSpeedValue { get; set; }

            public double VectorScaleValue { get; set; }

            public void UpdateVibrationViewModel()
            {
                VibrationLevel = VibrationDetector.VibrationLevel;

                VibrationSpeedValue = -0.188 * (double)VibrationLevel + 2.0;
                VectorScaleValue = (double)VibrationLevel / 5 * 0.85;


                
            }
        }


    }
}
