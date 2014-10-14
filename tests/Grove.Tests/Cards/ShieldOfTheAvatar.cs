namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ShieldOfTheAvatar
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PreventDamageToEquipped()
      {
        Battlefield(P1, C("Grizzly Bears").IsEnchantedWith("Pacifism"), C("Grizzly Bears").IsEnchantedWith("Pacifism"), C("Juggernaut").IsEquipedWith("Shield Of The Avatar"));

        P2.Life = 3;
        Battlefield(P2, "Juggernaut", "Forest", "Forest", "Forest", "Forest");
        Library(P2, "Juggernaut");

        RunGame(3);

        Equal(3, P2.Life);
        Equal(3, P1.Battlefield.Creatures.Count());
        Equal(0, P2.Battlefield.Creatures.Count());
      }
    }
  }
}
