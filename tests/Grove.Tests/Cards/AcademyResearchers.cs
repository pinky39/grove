namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class AcademyResearchers
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttachEmbrace()
      {
        var researchers = C("Academy Researchers");
        var embrace = C("Serra's Embrace");

        Hand(P1, researchers, embrace);
        Battlefield(P1, "Island", "Island", "Island");

        RunGame(1);

        Equal(4, C(researchers).Power);
      }

      [Fact]
      public void AttachDestructiveUrge()
      {
        var researchers = C("Academy Researchers");
        var urge = C("Destructive Urge");

        Hand(P1, researchers, urge);
        Battlefield(P1, "Island", "Island", "Island");
        Battlefield(P2, "Island");

        RunGame(3);

        Equal(0, P2.Battlefield.Lands.Count());
      }

      [Fact]
      public void DoNotAttachSicken()
      {
        var researchers = C("Academy Researchers");
        var sicken = C("Sicken");

        Hand(P1, researchers, sicken);
        Battlefield(P1, "Island", "Island", "Island");

        RunGame(1);

        Equal(2, C(researchers).Power);
      }
    }
  }
}