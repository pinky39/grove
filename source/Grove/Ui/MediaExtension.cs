namespace Grove.Ui
{
  using System;
  using System.Globalization;
  using System.Windows.Data;
  using System.Windows.Markup;

  public class MediaExtension : MarkupExtension, IValueConverter
  {
    private readonly string _name;

    public MediaExtension(string name)
    {
      _name = name;
    }

    public MediaExtension() :this(null)
    {
      
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return CreateAsset((string) value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      // if the assetName is not provided it will be provided via binding
      if (String.IsNullOrEmpty(_name))
        return this;

      return CreateAsset(_name);
    }

    private static object CreateAsset(string name)
    {
      return MediaLibrary.GetImage(name);
    }
  }
}