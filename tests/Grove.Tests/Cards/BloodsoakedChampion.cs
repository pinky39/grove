namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BloodsoakedChampion
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnChampionToBattlefield()
      {
        var champion = C("Bloodsoaked Champion");

        Battlefield(P1, "Grizzly Bears", "Swamp", "Mountain");
        Graveyard(P1, champion);

        RunGame(1);

        Equal(Zone.Battlefield, C(champion).Zone);
      }      
    }
  }
}
