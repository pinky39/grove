namespace Grove.Infrastructure
{
  using System.Collections;
  using System.Collections.Generic;
  using System.ComponentModel;

  public static class DictionaryEx
  {
    public static IDictionary ToArgDictionary(this object arguments)
    {
      IDictionary dic = new Dictionary<string, object>();
      var properties = TypeDescriptor.GetProperties(arguments);
      foreach (PropertyDescriptor property in properties)
      {
        dic.Add(property.Name, property.GetValue(arguments));
      }
      return dic;
    }
  }
}