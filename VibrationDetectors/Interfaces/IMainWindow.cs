using System.Windows;

namespace VibrationDetectors.Interfaces
{
    public interface IMainWindow
    {
        void Btn_Armed();
        void Btn_Armed_Click(object sender, RoutedEventArgs e);
        void Btn_TriggedState_Click(object sender, RoutedEventArgs e);
        void InitializeComponent();
        void InitializeFeatures();
    }
}