namespace Grove.Ui
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using System.Windows.Interactivity;
  using System.Windows.Interop;

  public class ResizeBehaviour : Behavior<Window>
  {
    private const Int32 SizeMaxhide = 0x0004;
    private const Int32 SizeMaximized = 0x0002;
    private const Int32 SizeMaxshow = 0x0003;
    private const Int32 SizeMinimized = 0x0001;
    private const Int32 SizeRestored = 0x0000;
    private const Int32 WmExitSizeMove = 0x0232;
    private const Int32 WmSize = 0x0005;
    private const Int32 WmSizing = 0x0214;
    private HwndSourceHook _hook;


    private Window Window
    {
      get { return AssociatedObject; }
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      
      Window.Loaded += (s, e) => WireUpWndProc();
      Window.KeyUp += ToggleFullScreen;

      Window.SizeChanged += delegate { OnResized(); };

      #if (DEBUG == FALSE)
      Window.Loaded += delegate { Window.WindowState = WindowState.Maximized; };
      #endif
    }

    protected override void OnDetaching()
    {
      RemoveWndProc();
      base.OnDetaching();
    }

    private void OnMaximized()
    {
      Window.WindowStyle = WindowStyle.None;
      Window.ResizeMode = ResizeMode.NoResize;  
      Window.Hide();
      Window.Show();          
    }

    private void OnResized()
    {
      var canvas = Window.FindDescendant<Border>("Canvas");

      var verticalBorderWidth = WindowDimensions.VerticalBorderWidth;
      var horizontalBorderWidth = WindowDimensions.HorizontalBorderWidth;

      if (Window.WindowStyle == WindowStyle.None)
      {
        verticalBorderWidth = 0;
        horizontalBorderWidth = 0;
      }


      var windowRatio = (Window.ActualWidth - verticalBorderWidth)/(Window.ActualHeight - horizontalBorderWidth);

      canvas.Width = WindowDimensions.CanvasWidth;
      canvas.Height = WindowDimensions.CanvasWidth/windowRatio;
    }

    private void OnRestored()
    {
      Window.WindowStyle = WindowStyle.ThreeDBorderWindow;
      Window.ResizeMode = ResizeMode.CanResize;            
    }

    private void RemoveWndProc()
    {
      var source = PresentationSource.FromVisual(AssociatedObject) as HwndSource;

      if (source != null)
      {
        source.RemoveHook(_hook);
      }
    }

    private void ToggleFullScreen(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.F)
      {
        Window.WindowState = Window.WindowState == WindowState.Normal
                               ? WindowState.Maximized
                               : WindowState.Normal;
      }
    }

    private void WireUpWndProc()
    {
      var source = PresentationSource.FromVisual(AssociatedObject) as HwndSource;

      if (source != null)
      {
        _hook = new HwndSourceHook(WndProc);
        source.AddHook(_hook);
      }
    }

    private IntPtr WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
    {
      var result = IntPtr.Zero;

      switch (msg)
      {
        case WmSizing: // sizing gets interactive resize          
          break;

        case WmSize: // size gets minimize/maximize as well as final size
          {
            var param = wParam.ToInt32();

            switch (param)
            {
              case SizeRestored:
                OnRestored();
                break;
              case SizeMinimized:
                break;
              case SizeMaximized:
                OnMaximized();
                break;
              case SizeMaxshow:
                break;
              case SizeMaxhide:
                break;
            }
          }
          break;

        case WmExitSizeMove:
          OnResized();
          break;
      }

      return result;
    }
  }
}