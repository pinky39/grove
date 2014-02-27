namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Pariah
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DiscipleReaverPariah()
      {
        var disciple = C("Disciple of Grace");
        var reaver = C("Flesh Reaver");
        var pariah = C("Pariah");

        Battlefield(P1, disciple, reaver, "Plains", "Plains", "Plains");
        Battlefield(P2, "Argothian Swine");
        Hand(P1, pariah);

        P2.Life = 4;
        RunGame(1);

        Equal(C(disciple), C(pariah).AttachedTo);
        Equal(3, P2.Life);
        Equal(20, P1.Life);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void RedirectToKillOpponentCreature()
      {
        var baloth = C("Ravenous Baloth");
        var pariah = C("Pariah");

        Battlefield(P2, baloth);
        Hand(P1, pariah);

        Exec(
          At(Step.SecondMain)
            .Cast(pariah, baloth),
          At(Step.DeclareAttackers, turn: 2)
            .DeclareAttackers(baloth),
          At(Step.SecondMain, turn: 2)
            .Verify(() =>
              {
                Equal(20, P1.Life);
                Equal(Zone.Graveyard, C(baloth).Zone);
                Equal(Zone.Graveyard, C(pariah).Zone);                
              }));
      }
    }
  }
}