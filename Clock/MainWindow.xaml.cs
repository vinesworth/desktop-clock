using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Clock
{
  [StructLayout(LayoutKind.Sequential)]
  public struct WIN32Rectangle
  {
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
  }

  public partial class MainWindow : Window
  {
    public ClockViewModel ViewModel { get; } = new ClockViewModel();

    public MainWindow()
    {
      InitializeComponent();
      DataContext = ViewModel;
      Loaded += MainWindow_Loaded;
      MouseDown += MainWindow_MouseDown;
      SizeChanged += MainWindow_SizeChanged;

      ContextMenu.PlacementTarget = this;

      Focus();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      var helper = new WindowInteropHelper(this);
      var handle = helper.Handle;

      HwndSource.FromHwnd(handle).AddHook(HwndMessageHook);

      var exStyle = (int)WindowStylesInterop.GetWindowLong(handle, (int)WindowStylesInterop.GetWindowLongFields.GWL_EXSTYLE);
      exStyle |= (int)WindowStylesInterop.ExtendedWindowStyles.WS_EX_TOOLWINDOW;
      WindowStylesInterop.SetWindowLong(handle, (int)WindowStylesInterop.GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
    }

    private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton == MouseButton.Left)
        DragMove();
    }

    private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (Left < SystemParameters.VirtualScreenLeft)
        Left = SystemParameters.VirtualScreenLeft;

      if (Left > SystemParameters.VirtualScreenWidth - Width)
        Left = SystemParameters.VirtualScreenWidth - Width;

      if (Top < SystemParameters.VirtualScreenTop)
        Top = SystemParameters.VirtualScreenTop;

      if (Top > SystemParameters.VirtualScreenHeight - Height)
        Top = SystemParameters.VirtualScreenHeight - Height;
    }

    private IntPtr HwndMessageHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool bHandled)
    {
      const int WM_SIZING = 0x0214;
      const int WM_MOVING = 0x0216;

      switch (msg)
      {
        case WM_SIZING:
        case WM_MOVING:
          {
            var rect = (WIN32Rectangle)Marshal.PtrToStructure(lParam, typeof(WIN32Rectangle));
            var dpi = VisualTreeHelper.GetDpi(this);

            var width = (int)(Width * dpi.DpiScaleX);
            var minLeft = (int)(SystemParameters.VirtualScreenLeft * dpi.DpiScaleX);
            var maxLeft = (int)(SystemParameters.VirtualScreenWidth * dpi.DpiScaleX - width);

            var height = (int)(Height * dpi.DpiScaleY);
            var minTop = (int)(SystemParameters.VirtualScreenTop * dpi.DpiScaleY);
            var maxTop = (int)(SystemParameters.VirtualScreenHeight * dpi.DpiScaleY - height);

            if (rect.Left < minLeft)
            {
              rect.Left = minLeft;
              bHandled = true;
            }

            if (rect.Left > maxLeft)
            {
              rect.Left = maxLeft;
              bHandled = true;
            }

            if (rect.Top < minTop)
            {
              rect.Top = minTop;
              bHandled = true;
            }

            if (rect.Top > maxTop)
            {
              rect.Top = maxTop;
              bHandled = true;
            }

            if (bHandled)
            {
              rect.Right = rect.Left + width;
              rect.Bottom = rect.Top + height;
              Marshal.StructureToPtr(rect, lParam, true);
            }
          }
          break;
      }
      return IntPtr.Zero;
    }

    private void QuitCommandOnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void ToggleTopmostOnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      Properties.Settings.Default.Topmost = !Properties.Settings.Default.Topmost;
    }

    private void TaskbarIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
    {
      Activate();
    }

    private void TaskbarIcon_TrayRightMouseDown(object sender, RoutedEventArgs e)
    {
      ContextMenu.IsOpen = true;
      CommandManager.InvalidateRequerySuggested();
    }
  }
}
