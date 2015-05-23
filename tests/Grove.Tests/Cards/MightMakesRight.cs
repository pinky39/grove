namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MightMakesRight
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainControlOfBearAndOfBothAmulets()
      {
        P1.Life = 2;
        var amulet1 = C("Avarice Amulet");
        var amulet2 = C("Avarice Amulet");

        Battlefield(P1, C("Runeclaw Bear").IsEquipedWith(amulet1),
          C("Grizzly Bears").IsEquipedWith(amulet2));

        Battlefield(P2, C("Shivan Dragon").IsEnchantedWith("Pacifism"), "Might Makes Right");
        RunGame(2);

        Equal(2, P1.Life);
        Equal(P2, C(amulet1).Controller);
        Equal(P2, C(amulet2).Controller);
      }

      [Fact]
      public void GetControlOfBear()
      {
        Battlefield(P1, "Might Makes Right", "Shivan Dragon");
        Battlefield(P2, C("Grizzly Bears").IsEquipedWith("Sword of Feast and Famine"));
        P2.Life = 9;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}