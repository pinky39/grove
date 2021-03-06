﻿namespace Grove.Tests.Unit
{
  using System.Linq;
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
    public void ParseText()
    {
      CardText cardText = "{T}: Add {G}{B} to your mana pool.";
      Assert.Equal(12, cardText.Tokens.Count);
    }
  }
}