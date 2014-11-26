namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class IndulgentTormentor
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void OpponentSacrificesACreature()
      {
        var elves = C("Llanowar Elves");

        Battlefield(P1, "Indulgent Tormentor");
        Battlefield(P2, elves);

        P2.Life = 8;

        RunGame(1);

        Equal(Zone.Graveyard, C(elves).Zone);
      }

      [Fact]
      public void OpponentLoosesLife()
      {
        var angel = C("Deathless Angel");

        Battlefield(P1, "Indulgent Tormentor");
        Battlefield(P2, angel);

        P2.Life = 20;

        RunGame(1);

        Equal(Zone.Battlefield, C(angel).Zone);
        Equal(17, P2.Life);
      }

      [Fact]
      public void ControllerDrawsACard1()
      {
        var angel = C("Deathless Angel");

        Battlefield(P1, "Indulgent Tormentor");
        Battlefield(P2, angel);

        P2.Life = 3;

        RunGame(1);

        Equal(Zone.Battlefield, C(angel).Zone);
        Equal(3, P2.Life);
        Equal(1, P1.Hand.Count);
      }

      [Fact]
      public void ControllerDrawsACard2()
      {        
        Battlefield(P1, "Indulgent Tormentor");        
        P2.Life = 8;

        RunGame(1);
        
        Equal(3, P2.Life);
        Equal(1, P1.Hand.Count);
      }
    }
  }
}