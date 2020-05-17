namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class JaceTheLivingGuildpact
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void LookAtTop2Cards()
      {
        var jace = C("Jace, the Living Guildpact");
        var island = C("Island");
        
        Hand(P1, jace);
        Library(P1, island, "Centaur Courser");
        Battlefield(P1, "Forest", "Forest", "Island", "Island", "Island", "Island");

        RunGame(1);
        
        Equal(6, C(jace).Loyality);
        Equal(Zone.Graveyard, C(island).Zone);
      }
    }
  }
}