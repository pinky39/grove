namespace Grove.Ui
{
  using System;
  using System.Diagnostics;
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Interactivity;

  // Enables key bindings to be used in Caliburn micro based on idea found here:
  // http://www.felicepollano.com/2011/05/02/InputBindingKeyBindingWithCaliburnMicro.aspx
  public class InputBindingTrigger : TriggerBase<FrameworkElement>, ICommand
  {
    public static readonly DependencyProperty InputBindingProperty =
      DependencyProperty.Register("InputBinding", typeof (InputBinding)
        , typeof (InputBindingTrigger)
        , new UIPropertyMetadata(null));

    public InputBinding InputBinding { get { return (InputBinding) GetValue(InputBindingProperty); } set { SetValue(InputBindingProperty, value); } }

    public event EventHandler CanExecuteChanged = delegate { };

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      InvokeActions(parameter);
    }

    protected override void OnAttached()
    {
      if (InputBinding != null)
      {
        InputBinding.Command = this;
        Window window = null;
        AssociatedObject.Loaded += delegate
          {
            window = GetWindow(AssociatedObject);
            window.InputBindings.Add(InputBinding);
          };

        AssociatedObject.Unloaded += delegate
          {
            if (window != null)
              window.InputBindings.Remove(InputBinding);
          };
      }
      base.OnAttached();
    }

    private Window GetWindow(FrameworkElement frameworkElement)
    {
      if (frameworkElement is Window)
        return frameworkElement as Window;

      var parent = frameworkElement.Parent as FrameworkElement;
      Debug.Assert(parent != null);

      return GetWindow(parent);
    }
  }
}