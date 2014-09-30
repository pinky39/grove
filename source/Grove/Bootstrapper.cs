namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Threading.Tasks;
  using System.Windows;
  using Caliburn.Micro;
  using Infrastructure;
  using UserInterface;
  using UserInterface.Shell;
  using log4net.Config;

  public class Bootstrapper : Bootstrapper<IShell>
  {
    public static IoC Container;

    protected override void Configure()
    {                        
      AppDomain.CurrentDomain.UnhandledException +=
        (s, a) =>
          {
            var message = a.ExceptionObject.ToString();
            
            LogFile.Error(message);

            MessageBox.Show(
              message,
              "An error has occured",
              MessageBoxButton.OK,
              MessageBoxImage.Error);
            
            Application.Current.Shutdown();
          };

      ConfigureLogger();
      Container = new IoC(IoC.Configuration.Ui);
      ConfigureCaliburn();
    }

    protected override void DisplayRootView()
    {
      Ui.Shell = Container.Resolve<Shell>();
      Ui.Dialogs = Container.Resolve<Dialogs>();
      Ui.Configuration = Configuration.Default;

      Ui.Shell.ChangeScreen(Ui.Dialogs.LoadScreen.Create());
      new WindowManager().ShowWindow(Ui.Shell);
    }


    protected override IEnumerable<object> GetAllInstances(Type service)
    {
      return Container.ResolveAll(service).Cast<object>();
    }

    protected override object GetInstance(Type service, string key)
    {
      return String.IsNullOrEmpty(key)
        ? Container.Resolve(service)
        : Container.Resolve(key, service);
    }

    private static void ConfigureCaliburn()
    {
      ConfigureViewLocator();
    }

    private static void ConfigureLogger()
    {
#if DEBUG
      var configName = "Grove.LogDebug.xml";
#else
      var configName = "Grove.LogRelease.xml";
#endif

      var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(configName);
      XmlConfigurator.Configure(stream);
    }

    private static void ConfigureViewLocator()
    {
      ViewLocator.LocateForModelType = (presenter, displayLocation, context) =>
        {
          if (presenter.Name.Contains("Proxy"))
          {
            presenter = presenter.BaseType;
          }

          var viewType = context == null
            ? Assembly.GetExecutingAssembly().GetType(presenter.Namespace + ".View")
            : Assembly.GetExecutingAssembly().GetType(presenter.Namespace + "." + context.ToString());

          Asrt.True(viewType != null, String.Format("Could not find View for ViewModel: {0}.", presenter));

          return ViewLocator.GetOrCreateViewType(viewType);
        };
    }
  }
}