using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
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
            services.AddScoped<ErgastService>();
            services.AddScoped<ChartService>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}
