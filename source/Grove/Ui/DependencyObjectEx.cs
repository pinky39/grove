namespace Grove.Ui
{
  using System.Windows;
  using Caliburn.Micro;

  public static class DependencyObjectEx
  {
    public static T FindDescendant<T>(this DependencyObject parent, string childName)
      where T : DependencyObject
    {
      var elements = BindingScope.GetNamedElements(parent);
      return elements.FindName(childName) as T;
    }
  }
}