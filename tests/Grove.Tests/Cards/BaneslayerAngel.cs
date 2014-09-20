namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BaneslayerAngel
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DragonCannotBlockAngel()
      {
        Battlefield(P1, "Baneslayer Angel");
        Battlefield(P2, "Shivan Dragon");

        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}