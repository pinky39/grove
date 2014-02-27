namespace Grove.Tests.Unit
{
  using Xunit;

  public class CardTypeFacts
  {
    [Fact]
    public void Is()
    {
      var type = new CardType("Basic Land - Island");
      Assert.True(type.BasicLand);
      Assert.True(type.Is("land"));      
      Assert.True(type.Is("basic land"));
      Assert.True(type.Is("island"));
    }

    [Fact]
    public void IsAny()
    {
      var type = new CardType("Legendary Creature - Vampire Shaman");
      Assert.True(type.IsAny(new[] {"creature", "land"}));
    }
  }
}