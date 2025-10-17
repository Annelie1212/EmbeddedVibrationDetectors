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
        }


    }
}
