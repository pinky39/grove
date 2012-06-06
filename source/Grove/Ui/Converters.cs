namespace Grove.Ui
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Data;
  using System.Windows.Media;
  using Core;
  using Infrastructure;

  public static class Converters
  {
    public static AutoPassToImageConverter AutoPassToImage = new AutoPassToImageConverter();
    public static BooleanToVisibilityConverter BooleanToVisibility = new BooleanToVisibilityConverter();
    public static CardColorToCardTemplateConverter CardColorToCardTemplate = new CardColorToCardTemplateConverter();
    public static CardNameToCardImageConverter CardIllustrationNameToCardImage = new CardNameToCardImageConverter();
    public static CharacterCountToFontSizeConverter CharacterCountToFontSize = new CharacterCountToFontSizeConverter();
    public static LifeToColorConverter LifeToColor = new LifeToColorConverter();

    public static ManaCostToManaSymbolImagesConverter ManaCostToManaSymbolImages =
      new ManaCostToManaSymbolImagesConverter();

    public static MarkerBrushConverter MarkerBrush = new MarkerBrushConverter();

    public static NullToCollapsedConverter NullToCollapsed = new NullToCollapsedConverter();


    public class AutoPassToImageConverter : IValueConverter
    {
      private readonly Dictionary<Pass, string> _imageNames = new Dictionary<Pass, string>{
        {Pass.Always, "circle-transparent.png"},
        {Pass.Active, "circle-yellow.png"},
        {Pass.Passive, "circle-green.png"},
        {Pass.Never, "circle-red.png"},
      };

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var pass = (Pass) value;
        return MediaLibrary.GetImage(_imageNames[pass]);
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }

    public class CardColorToCardTemplateConverter : IValueConverter
    {
      public const string Artifact = "artifact-card";
      public const string Black = "black-card";
      public const string Blue = "blue-card";
      public const string Green = "green-card";
      public const string Land = "land-card";
      public const string Multi = "multi-card";
      public const string Red = "red-card";
      public const string White = "white-card";

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        return MediaLibrary.GetImage(
          GetTemplateName((ManaColors) value) + ".png");
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }

      public string GetTemplateName(ManaColors colors)
      {
        if (EnumEx.GetSetBitCount((long) colors) > 1)
          return Multi;

        var conversions = new Dictionary<ManaColors, string>{
          {ManaColors.White, White},
          {ManaColors.Blue, Blue},
          {ManaColors.Black, Black},
          {ManaColors.Red, Red},
          {ManaColors.Green, Green},
          {ManaColors.Colorless, Artifact},
          {ManaColors.None, Land},
        };

        return conversions[colors];
      }
    }

    public class CardNameToCardImageConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var cardName = (string) value;
        const string extension = ".jpg";
        return MediaLibrary.GetCardImage(GetCardImageName(cardName) + extension);
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }

      public string GetCardImageName(string cardName)
      {
        var invalidChars = Path.GetInvalidFileNameChars();

        var invalidCharsRemoved = cardName
          .Where(x => !invalidChars.Contains(x))
          .ToArray();

        return new string(invalidCharsRemoved)
          .Trim()
          .ToLower();
      }
    }

    public class CharacterCountToFontSizeConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var characterCount = (int) value;

        if (characterCount < 50)
          return 17;

        if (characterCount < 150)
          return 15;

        if (characterCount < 200)
          return 14;

        if (characterCount < 250)
          return 13;

        return 12;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }

    public class LifeToColorConverter : IValueConverter
    {
      private readonly SolidColorBrush _highLife = new SolidColorBrush(Color.FromArgb(0xff, 0xb1, 0xff, 0x00)); //#ffb1FF00
      private readonly SolidColorBrush _lowLife = new SolidColorBrush(Color.FromArgb(0xff, 0xe3, 0x00, 0x00)); // #ffE30000

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var life = int.Parse((string) value);

        return life > 9 ? _highLife : _lowLife;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }

    public class ManaCostToManaSymbolImagesConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var manaAmount = (IManaAmount) value;

        if (manaAmount == null)
          return new string[]{};

        return manaAmount.GetSymbolNames().Select(symbolName => MediaLibrary.GetImage(symbolName + ".png"));
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }

    public class MarkerBrushConverter : IValueConverter
    {
      private readonly SolidColorBrush[] _brushes = new[]{
        new SolidColorBrush(Color.FromArgb(0x66, 0xff, 0x00, 0x00)), // #ccFF0000
        new SolidColorBrush(Color.FromArgb(0x66, 0xff, 0xa5, 0x00)), // #ccFFA500
        new SolidColorBrush(Color.FromArgb(0x66, 0xff, 0x00, 0xff)), // #ccFF00FF
        new SolidColorBrush(Color.FromArgb(0x66, 0x0f, 0xff, 0xff)), // #cc0FFF00
        new SolidColorBrush(Color.FromArgb(0x66, 0xff, 0xef, 0xff)), // #ccFFEF00
        new SolidColorBrush(Color.FromArgb(0x66, 0x00, 0xff, 0xef)), // #cc00FFEF
        new SolidColorBrush(Color.FromArgb(0x66, 0x00, 0x7b, 0xff)), // #cc007BFF
      };

      private readonly SolidColorBrush _default = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var state = (int) value;

        if (state == 0)
          return _default;

        return _brushes[(state - 1)%_brushes.Length];
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }
  }

  public class NullToCollapsedConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value == null ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}