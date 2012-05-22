namespace Grove.Tests.Cards
{
  using Grove.Core;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class Dread
  {
    public class Predefined : PredifinedScenario
    {
      [Fact]
      public void KillAttackers()
      {
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");
        
        Battlefield(P1, bear1, bear2);
        Battlefield(P2, "Dread");

        Exec(
            At(Step.DeclareAttackers)
              .DeclareAttackers(bear1, bear2),
            At(Step.SecondMain)
              .Verify(() => {
                Equal(Zone.Graveyard, C(bear1).Zone);
                Equal(Zone.Graveyard, C(bear2).Zone);
              })
          );
      }

      [Fact]
      public void KillRumblingSlum()
      {
        var slum = C("Rumbling Slum");

        Battlefield(P1, slum);
        Battlefield(P2, "Dread");

        Exec(
          At(Step.FirstMain)
          .Verify(() => Equal(Zone.Graveyard, C(slum).Zone))
          );
      }      
    }
  }
}