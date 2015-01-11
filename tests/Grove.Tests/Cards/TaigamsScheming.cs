namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class TaigamsScheming
  {
    public class Ai : AiScenario
    {
      [Fact(Skip = "AI does not play the sorcery")]
      public void PutMountainOnTop()
      {
        Hand(P1, "Taigam's Scheming", "Shivan Raptor");
        Battlefield(P1, "Island", "Island");
        Library(P1, "Shivan Raptor", "Shivan Raptor", "Shivan Raptor", "Taigam's Scheming", "Mountain");

        RunGame(3);
        Equal(17, P2.Life);
      }
    }
  }
}
