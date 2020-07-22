using System;
using System.Windows.Input;

namespace Clock
{

  static class ApplicationCommands
  {
    public static RoutedCommand Quit { get; }

    static ApplicationCommands()
    {
      var inputGestures = new InputGestureCollection
      {
        new KeyGesture(Key.Q, ModifierKeys.Control)
      };

      Quit = new RoutedUICommand("Quit", "Quit", typeof(ApplicationCommands), inputGestures);
    }
  }
}
