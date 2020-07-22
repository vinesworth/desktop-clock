using System;
using System.Windows.Threading;

using PropertyChanged;

namespace Clock
{
  [AddINotifyPropertyChangedInterface]
  public class ClockViewModel
  {
    public string TimeText { get; private set; }

    public ClockViewModel()
    {
      Timer.Interval = TimeSpan.FromSeconds(1);
      Timer.Tick += OnTimer;
      Timer.Start();
    }

    private void OnTimer(object sender, EventArgs e)
    {
      var now = DateTime.Now;
      var nowString = now.ToString("HH:mm");
      TimeText = (now.Second % 2 == 0) ? nowString + "." : nowString;
    }

    private DispatcherTimer Timer { get; } = new DispatcherTimer();
  }
}