namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class JaliraMasterPolymorphist
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutForceIntoPlay()
      {
        var force = C("Verdant Force");
        var forest = C("Forest");

        Library(P1, forest, force, "Swamp");
        Battlefield(P1, "Jalira, Master Polymorphist", C("Grizzly Bears").IsEnchantedWith("Pacifism"), "Island", "Island", "Island");

        Battlefield(P2, "Wall of Frost");

        P2.Life = 2;

        RunGame(1);

        Equal(Zone.Battlefield, C(force).Zone);
        Equal(Zone.Library, C(forest).Zone);
      }

      [Fact]
      public void PutAllOnBottomOfLibrary()
      {
        var legenadry = C("Jalira, Master Polymorphist");
        var forest = C("Forest");

        Library(P1, forest, legenadry, "Swamp");
        Battlefield(P1, "Jalira, Master Polymorphist", C("Grizzly Bears").IsEnchantedWith("Pacifism"), "Island", "Island", "Island");

        Battlefield(P2, "Wall of Frost");

        RunGame(1);

        Equal(Zone.Library, C(legenadry).Zone);
      }
    }
  }
}
