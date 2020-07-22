using System.Windows.Data;

namespace Clock
{
  public class SettingsExtension : Binding
  {
    public SettingsExtension()
    {
      Initialize();
    }

    public SettingsExtension(string path)
        : base(path)
    {
      Initialize();
    }

    private void Initialize()
    {
      Source = Properties.Settings.Default;
      Mode = BindingMode.TwoWay;
    }
  }
}