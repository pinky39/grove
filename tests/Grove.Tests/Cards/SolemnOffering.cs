namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SolemnOffering
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyStaff()
      {
        Hand(P1, "Solemn Offering");
        Battlefield(P1, "Plains", "Plains", "Plains");

        var staff = C("Staff of the Death Magus");
        Battlefield(P2, staff);

        RunGame(1);

        Equal(24, P1.Life);
        Equal(Zone.Graveyard, C(staff).Zone);
      }
    }
  }
}