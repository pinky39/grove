namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BlooshotCyclops
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackAndSacrificeToKill()
      {
        Battlefield(P1, "Bloodshot Cyclops", "Ravenous Skirge"); 
        Battlefield(P2, "Wall of Junk");
        P2.Life = 6;

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void KillSkirge()
      {
        var skirge = C("Ravenous Skirge");

        Battlefield(P1, "Bloodshot Cyclops", "Llanowar Elves");        
        Battlefield(P2, "Wall of Junk", "Wall of Junk", skirge);

        P1.Life = 3;
        
        RunGame(2);

        Equal(Zone.Graveyard, C(skirge).Zone);


      }
    }
  }
}