using System.Configuration;
using System.Data;
using System.Windows;
using TestApp.Presenters;
using TestApp.Views;

namespace TestApp;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var w = new MainWindow
        {
            DataContext = new List<object>
            {
                new Page1Presenter(),
                new Page2Presenter(),
                new Page3Presenter(),
                new Page4Presenter(),
                new Page5Presenter(),
                new Page5View(),
                new Page6PresenterFactory(),
                new Page7View(),
                new SecretPageView()
            }
        };

        w.ShowDialog();
    }
}

