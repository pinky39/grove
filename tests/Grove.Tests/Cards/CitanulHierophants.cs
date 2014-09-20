namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CitanulHierophants
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void EachCreatureYouControlCanAddOneGreenMana()
      {
        var hierophants = C("Citanul Hierophants");

        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Llanowar Elves");
        Hand(P1, hierophants);

        Exec(
          At(Step.FirstMain)
            .Cast(hierophants)
            .Verify(() => Equal(3, P1.GetAvailableConvertedMana()))
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void WhenHierophantsGoesToGraveyardCreaturesLooseManaAbility()
      {
        var shock = C("Shock");
        var hierophants = C("Citanul Hierophants");

        Hand(P1, shock);
        Battlefield(P2, hierophants, "Grizzly Bears");

        Exec(
          At(Step.FirstMain)
            .Verify(() => { True(P2.HasMana(2)); }),
          At(Step.DeclareAttackers)
            .Cast(shock, target: hierophants),
          At(Step.SecondMain)
            .Verify(() => { False(P2.HasMana(2)); })
          );
      }
    }
  }
}