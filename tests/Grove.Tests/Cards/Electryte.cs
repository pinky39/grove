namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Electryte
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DealDamageToBlockers()
      {
        var armodon1 = C("Trained Armodon");
        var armodon2 = C("Trained Armodon");
        var bear = C("Grizzly Bears");
        var electryte = C("Electryte");

        Battlefield(P1, electryte, bear);
        Battlefield(P2, armodon1, armodon2);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(electryte, bear),
          At(Step.DeclareBlockers)
            .DeclareBlockers(bear, armodon1, bear, armodon2),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(armodon1).Zone);
                Equal(Zone.Graveyard, C(armodon2).Zone);
              })
          );
      }
    }
  }
}