namespace Grove.UserInterface
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;

  /// <summary>
  ///     Interaction logic for RatingControl.xaml
  /// </summary>
  public partial class RatingControl : StackPanel
  {
    public static readonly DependencyProperty RatingProperty = DependencyProperty.Register(
      "Rating",
      typeof (Int32),
      typeof (RatingControl),
      new PropertyMetadata(0, RatingValueChanged));

    public RatingControl()
    {
      InitializeComponent();
    }

    public Int32 Rating
    {
      get { return (Int32) GetValue(RatingProperty); }
      set
      {
        if (value < 0)
        {
          SetValue(RatingProperty, 0);
        }
        else if (value > 5)
        {
          SetValue(RatingProperty, 5);
        }
        else
        {
          SetValue(RatingProperty, value);
        }
      }
    }

    private static void RatingValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var parent = sender as RatingControl;
      var ratingValue = (int) e.NewValue;
      var children = parent.Children;

      ToggleButton button = null;
      for (var i = 0; i < ratingValue; i++)
      {
        button = children[i] as ToggleButton;
        button.IsChecked = true;
      }

      for (var i = ratingValue; i < children.Count; i++)
      {
        button = children[i] as ToggleButton;
        button.IsChecked = false;
      }
    }

    private void RatingButtonClickEventHandler(Object sender, RoutedEventArgs e)
    {
      var button = sender as ToggleButton;
      Rating = Int32.Parse((String) button.Tag);
    }
  }
}