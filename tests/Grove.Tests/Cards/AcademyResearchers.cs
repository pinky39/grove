namespace Grove.Tests.Cards
{
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