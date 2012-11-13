namespace Grove.Tests.Unit
{
  using System;
  using System.Linq;
  using Core.Cards;
  using Xunit;

  public class CardTextFacts
  {
    [Fact]
    public void AbilityText()
    {
      CardText text = "{T}: Add {G}{B} to your mana pool.";
      Assert.Equal(9, text.AbilityTokens.Count());
    }

    [Fact]
    public void ParseNull()
    {
      CardText cardText = (string) null;
      Assert.Equal(String.Empty, cardText.Original);
    }

    [Fact]
    public void ParseText()
    {
      CardText cardText = "{T}: Add {G}{B} to your mana pool.";
      Assert.Equal(12, cardText.Tokens.Count);
    }
  }
}