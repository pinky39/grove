namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public partial class EngineeredPlague
  {
    public class ObeliskOfUrd
    {
      public class Ai : AiScenario
      {
        [Fact]
        public void Confiscate()
        {
          var obelisk = C("Obelisk of Urd");

          Hand(P1, obelisk);
          Hand(P2, "Confiscate");

          var bear1 = C("Grizzly Bears");
          var bear2 = C("Grizzly Bears");

          Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp",
            bear1, "Grizzly Bears", "Grizzly Bears");
          
          Battlefield(P2, bear2, "Grizzly Bears", "Grizzly Bears", 
            "Island", "Island", "Island", "Island", "Island", "Island");

          RunGame(2);

          Equal(P2, C(obelisk).Controller);
          Equal(2, C(bear1).Power);
          Equal(4, C(bear2).Power);
        }
      }
    }
  }
}