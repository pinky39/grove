namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MultanisDecree
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyAllGain4Life()
      {
        Hand(P1, "Multani's Decree");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Worship", C("Grizzly Bears").IsEnchantedWith("Rancor"));

        RunGame(1);


        Equal(24, P1.Life);
      }  
    }            
  }
}