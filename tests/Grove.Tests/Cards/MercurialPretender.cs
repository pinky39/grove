namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class MercurialPretender
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CopyArchangelAndGain10Life()
      {
        var pretender = C("Mercurial Pretender");
        
        Hand(P1, pretender);
        Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Resolute Archangel");

        P1.Life = 10;
        
        RunGame(1);
        
        Equal(2, P1.Battlefield.Count(x => x.Name == "Resolute Archangel"));
        Equal(20, P1.Life);
      }
    }
  }
}