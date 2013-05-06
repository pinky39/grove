namespace Grove.UserInterface
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using System.Windows.Interactivity;

  public class FocusBehavior : Behavior<Control>
  {
    public static readonly DependencyProperty IsFocusedProperty =
      DependencyProperty.Register(
        "IsFocused",
        typeof (bool),
        typeof (FocusBehavior),
        new PropertyMetadata(false, (d, e) => { if ((bool) e.NewValue) ((FocusBehavior) d).AssociatedObject.Focus(); }));

    public static readonly DependencyProperty HasInitialFocusProperty =
      DependencyProperty.Register(
        "HasInitialFocus",
        typeof (bool),
        typeof (FocusBehavior),
        new PropertyMetadata(false, null));

    public bool IsFocused { get { return (bool) GetValue(IsFocusedProperty); } set { SetValue(IsFocusedProperty, value); } }

    public bool HasInitialFocus { get { return (bool) GetValue(HasInitialFocusProperty); } set { SetValue(HasInitialFocusProperty, value); } }

    protected override void OnAttached()
    {
      AssociatedObject.GotFocus += (sender, args) => IsFocused = true;
      AssociatedObject.LostFocus += (sender, a) => IsFocused = false;
      AssociatedObject.Loaded += (o, a) =>
        {
          if (HasInitialFocus || IsFocused)
          {
            Keyboard.Focus(AssociatedObject);
            AssociatedObject.Focus();
          }
        };

      base.OnAttached();
    }
  }
}