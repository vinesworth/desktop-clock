using System;
using System.Runtime.InteropServices;


public static class WindowStylesInterop
{
  [Flags]
  public enum ExtendedWindowStyles
  {
    // ...
    WS_EX_TOOLWINDOW = 0x00000080,
    // ...
  }

  public enum GetWindowLongFields
  {
    // ...
    GWL_EXSTYLE = -20,
    // ...
  }

  [DllImport("user32.dll")]
  public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

  public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
  {
    // Win32 SetWindowLong() doesn't clear error on success
    SetLastError(0);

    var result = IntPtr.Size == 4
      ? new IntPtr(IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong)))
      : IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);

    var error = Marshal.GetLastWin32Error();
    if (result == IntPtr.Zero && error != 0)
      throw new System.ComponentModel.Win32Exception(error);

    return result;
  }

  #region Private

  private static int IntPtrToInt32(IntPtr intPtr)
  {
    return unchecked((int)intPtr.ToInt64());
  }

  [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
  private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

  [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
  private static extern int IntSetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

  [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
  private static extern void SetLastError(int dwErrorCode);

  #endregion
}