namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class EvolvingWilds
  {
   public class Ai : AiScenario
   {
     [Fact]
     public void SearchForMountain()
     {
       var wilds = C("Evolving Wilds");
       var mountain = C("Mountain");
              
       Hand(P1, wilds);
       Battlefield(P1, "Forest");       
       Library(P1, "Forest", mountain);
       
       RunGame(1);

       Equal(Zone.Graveyard, C(wilds).Zone);
       Equal(Zone.Battlefield, C(mountain).Zone);
       
     }
   }
  }
}