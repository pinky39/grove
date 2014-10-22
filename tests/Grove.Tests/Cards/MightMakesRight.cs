namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MightMakesRight
  {
    public class Ai : AiScenario
    {
      [Fact(Skip = "Cannot change zones, when source zone and destination zone are the same.")]
      public void GetControlOnBear()
      {
        P1.Life = 2;
        Battlefield(P1, C("Runeclaw Bear").IsEquipedWith("Avarice Amulet"), C("Grizzly Bears").IsEquipedWith("Avarice Amulet"));

        Battlefield(P2, C("Shivan Dragon").IsEnchantedWith("Pacifism"), "Might Makes Right");

        RunGame(2);

        Equal(2, P1.Life);
        Equal(1, P1.Battlefield.Count); // Avarice Amulet
        Equal(4, P2.Battlefield.Count); // Avarice Amulet + 3 P2's permaments
        Equal(2, P1.Graveyard.Count);   // Both bears
      }
    }
  }
}
