using System.Windows;

namespace Clock
{
  public partial class App : Application
  {
    public App()
    {
      Exit += Application_Exit;
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
      Clock.Properties.Settings.Default.Save();
    }
  }
}
