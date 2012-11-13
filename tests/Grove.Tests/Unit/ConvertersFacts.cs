namespace Grove.Tests.Unit
{
  using Core.Mana;
  using Grove.Core;
  using Grove.Ui;
  using Xunit;
  using Xunit.Extensions;

  public class ConvertersFacts
  {
    [Fact]
    public void ConvertCardNameToCardImage()
    {
      var cardName = "Llanowar Elves: ";
      var converter = Converters.CardIllustrationNameToCardImage;

      var result = converter.GetCardImageName(cardName);
      const string expected = "llanowar elves";

      Assert.Equal(expected, result);
    }

    [Theory,
     InlineData(ManaColors.Green, Converters.CardColorToCardTemplateConverter.Green),
     InlineData(ManaColors.Red, Converters.CardColorToCardTemplateConverter.Red),
     InlineData(ManaColors.Colorless, Converters.CardColorToCardTemplateConverter.Artifact),
     InlineData(ManaColors.None, Converters.CardColorToCardTemplateConverter.Land),
    ]
    public void ConvertFromManaCostToCardTemplate(ManaColors colors, string cardTemplateName)
    {
      var converter = Converters.CardColorToCardTemplate;
      var result = converter.GetTemplateName(colors);

      Assert.Equal(cardTemplateName, result);
    }    
  }
}