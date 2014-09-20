#region

#endregion

namespace Grove
{
  using System;
  using System.Windows;

  /// <summary>
  ///   Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnExit(ExitEventArgs e)
    {
      base.OnExit(e);
      Environment.Exit(1);
    }

  }
}