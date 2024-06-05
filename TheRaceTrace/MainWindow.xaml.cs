using System.Windows;

namespace TheRaceTrace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}