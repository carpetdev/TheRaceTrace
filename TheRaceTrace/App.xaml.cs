using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace TheRaceTrace
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void LaunchApp(object sender, StartupEventArgs e)
        {
            ServiceCollection services = new();
            services.AddScoped<MainWindow>();
            services.AddScoped<ViewModel>();
            services.AddScoped<IErgastService, ErgastService>();
            services.AddScoped<IChartService, ChartService>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}
