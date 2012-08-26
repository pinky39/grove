namespace Grove.Tests.Cards
{
  using Core;
  using Core.Details.Mana;
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
            .Verify(()=> Equal(3, P1.GetConvertedMana(ManaUsage.Any)))
          );
      }
    }
  }
}