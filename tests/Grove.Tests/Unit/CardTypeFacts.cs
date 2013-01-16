namespace Grove.Tests.Unit
{
  using Core;
  using Xunit;

  public class CardTypeFacts
  {
    [Fact]
    public void Is()
    {
      var type = new CardType("Basic Land");
      Assert.True(type.BasicLand);
      Assert.True(type.Is("land"));
      Assert.True(type.Is("basic"));
      Assert.True(type.Is("basic land"));
    }

    [Fact]
    public void IsAny()
    {
      var type = new CardType("Legendary Creature - Vampire Shaman");
      Assert.True(type.IsAny(new[] {"creature", "land"}));
    }
  }
}