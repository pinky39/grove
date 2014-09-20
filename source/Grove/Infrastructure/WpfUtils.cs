namespace Grove.Infrastructure
{
  using System;
  using System.Windows;
  using Caliburn.Micro;

  public static class WpfUtils
  {
    public static T FindDescendant<T>(this DependencyObject parent, string childName)
      where T : DependencyObject
    {
      var elements = BindingScope.GetNamedElements(parent);
      return elements.FindName(childName) as T;
    }
  }

  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = true)]
  public class UpdatesAttribute : Attribute
  {
    public UpdatesAttribute(params string[] propertyNames)
    {
      PropertyNames = propertyNames;
    }

    public string[] PropertyNames { get; set; }
  }
}