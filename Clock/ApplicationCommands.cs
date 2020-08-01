using System;
using System.Windows.Input;

namespace Clock
{

  static class ApplicationCommands
  {
    public static RoutedCommand Quit { get; }
    public static RoutedCommand ToggleTopmost { get; }

    static ApplicationCommands()
    {
      Quit = new RoutedUICommand("Quit", nameof(Quit), typeof(ApplicationCommands),
        new InputGestureCollection
        {
          new KeyGesture(Key.Q, ModifierKeys.Control)
        });

      ToggleTopmost = new RoutedUICommand("Topmost window", nameof(ToggleTopmost), typeof(ApplicationCommands),
        new InputGestureCollection
        {
          new KeyGesture(Key.T, ModifierKeys.Control)
        });
    }
  }
}
