namespace Grove.Tests.Cards
{
  using Core;
  using Infrastructure;
  using Xunit;

  public class LivingDestiny
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainLife()
      {
        var destiny = C("Living Destiny");
        var engine = C("Wurmcoil Engine");

        Hand(P2, destiny, engine);
        Battlefield(P2, "Forest", "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(26, P2.Life);
      }
    }
    
    
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void GainLife()
      {
        var destiny = C("Living Destiny");
        var engine = C("Wurmcoil Engine");

        Hand(P1, destiny, engine);
        
        Exec(
          At(Step.FirstMain)
            .Cast(p =>
              {
                p.Card = destiny;
                p.CostTargets(C(engine));
              })
            .Verify(() => Equal(26, P1.Life))
          );
      }
    }
  }
}