namespace Grove.Infrastructure
{
  using System;
  using System.Linq;
  using System.Xml.Linq;

  public static class XElementEx
  {
    public static string GetAttributeValue(this XElement element, string name)
    {
      var attr = element.Attribute(name);

      if (attr == null)
        throw new InvalidOperationException(
          String.Format("Attribute {0} does not exist.", name));

      return attr.Value;
    }

    public static XElement GetElement(this XElement ancestor, string name)
    {
      var element = ancestor.Descendants(name).FirstOrDefault();

      if (element == null)
        throw new InvalidOperationException(
          String.Format("Element {0} is not descendant of {1}.", name, ancestor.Value));

      return element;
    }

    public static string GetOptionalAttributeValue(this XElement element, string name)
    {
      var attr = element.Attribute(name);
      return attr == null ? String.Empty : attr.Value;
    }

    public static XElement GetOptionalElement(this XElement ancestor, string name)
    {
      return ancestor.Descendants(name).FirstOrDefault();
    }

    public static int? ParseOptionalAttributeValue(this XElement element, string name)
    {
      var stringValue = GetOptionalAttributeValue(element, name);
      return stringValue == String.Empty
        ? null
        : (int?) Int32.Parse(stringValue);
    }

    public static int ParseOptionalAttributeValue(this XElement element, string name, int defaultValue)
    {
      var nullable = element.ParseOptionalAttributeValue(name);

      if (nullable.HasValue)
        return nullable.Value;

      return defaultValue;
    }
  }
}