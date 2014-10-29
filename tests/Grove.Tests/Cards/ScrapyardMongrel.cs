namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ScrapyardMongrel
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void MongrelGets20AndTrample()
      {
        var mongrel = C("Scrapyard Mongrel");
        Battlefield(P1, mongrel, "Profane Memento");

        RunGame(1);

        Equal(5, C(mongrel).Power);
        True(C(mongrel).Has().Trample);
      }
    }
  }
}