namespace Grove.UserInterface
{
  using System;
  using System.Windows;
  using System.Windows.Controls;

  public class SlotPanel : Panel
  {
    public static readonly DependencyProperty ChildHorizontalOffsetProperty =
      DependencyProperty.Register("ChildHorizontalOffset", typeof (int), typeof (SlotPanel));

    public static readonly DependencyProperty ChildVerticalOffsetProperty =
      DependencyProperty.Register("ChildVerticalOffset", typeof (int), typeof (SlotPanel));

    public int ChildHorizontalOffset { get { return (int) GetValue(ChildHorizontalOffsetProperty); } set { SetValue(ChildHorizontalOffsetProperty, value); } }

    public int ChildVerticalOffset { get { return (int) GetValue(ChildVerticalOffsetProperty); } set { SetValue(ChildVerticalOffsetProperty, value); } }

    protected override Size ArrangeOverride(Size finalSize)
    {
      for (var i = 0; i < Children.Count; i++)
      {
        var child = Children[i];
        child.Arrange(new Rect(i*ChildHorizontalOffset, i*ChildVerticalOffset,
          child.DesiredSize.Width, child.DesiredSize.Height));
      }

      return finalSize;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      var resultSize = new Size(0, 0);

      for (var i = 0; i < Children.Count; i++)
      {
        var child = Children[i];
        child.Measure(availableSize);

        resultSize.Height = Math.Max(resultSize.Height, child.DesiredSize.Height + i*ChildVerticalOffset);
        resultSize.Width = Math.Max(resultSize.Width, child.DesiredSize.Width + i*ChildHorizontalOffset);
      }

      resultSize.Width = double.IsPositiveInfinity(availableSize.Width)
        ? resultSize.Width
        : availableSize.Width;

      resultSize.Height = double.IsPositiveInfinity(availableSize.Height)
        ? resultSize.Height
        : availableSize.Height;

      return resultSize;
    }
  }
}