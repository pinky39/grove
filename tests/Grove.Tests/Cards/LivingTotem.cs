namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class LivingTotem
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastLivingTotemWithCounter()
      {
        var totem = C("Living Totem");
        var wall = C("Wall of Frost");
        
        Hand(P1, totem);        
        Battlefield(P1, "Forest", "Forest", "Forest", wall, "Grizzly Bears");

        P2.Life = 3;

        RunGame(1);

        Equal(Zone.Battlefield, C(totem).Zone);          
        Equal(0, P2.Life);
      }

      [Fact]
      public void CastLivingTotemWithoutCounter()
      {
        var totem = C("Living Totem");
        Hand(P1, totem);
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P1.Hand.Count);
        Equal(2, C(totem).Power);
      }

      [Fact]
      public void CastLivingTotemWithoutMana()
      {
        // Convoke has to order cards correctly: [green][colorless]...

        Hand(P1, "Living Totem");
        Battlefield(P1, "Ornithopter", "Ornithopter", "Ornithopter", "Ornithopter", "Grizzly Bears");

        RunGame(1);

        Equal(0, P1.Hand.Count);
      }
    }
  }
}
