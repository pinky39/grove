namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class KarmicGuide
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BringBackTitan()
      {
        var titan = C("Grave Titan");
        
        Hand(P1, "Karmic Guide");
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains");
        
        Graveyard(P1, titan);

        RunGame(1);

        Equal(Zone.Battlefield, C(titan).Zone);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DoNotBringBackTitan()
      {
        var titan = C("Grave Titan");
        var guide = C("Karmic Guide");
        var beetles = C("Carrion Beetles");
        
        Hand(P1, guide);
        
        Battlefield(P2, beetles);                
        Graveyard(P1, titan);

        Exec(
          At(Step.FirstMain)
            .Cast(guide)
            .Target(titan)
            .Activate(beetles, target: titan, stackShouldBeEmpty: false),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Exile, C(titan).Zone))
        );
      }

      [Fact]
      public void NothingToBringBack()
      {
         var titan = C("Grave Titan");
        var guide = C("Karmic Guide");
        var beetles = C("Carrion Beetles");
        
        Hand(P1, guide);
        
        Battlefield(P2, beetles);                
        Graveyard(P1, titan);

        Exec(
          At(Step.FirstMain)
            .Cast(guide)
            .Activate(beetles, target: titan, stackShouldBeEmpty: false),                        
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Exile, C(titan).Zone)));
      }
    }
  }
}