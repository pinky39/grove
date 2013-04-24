namespace Grove.Tests.Cards
{
  using Core;
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class Bravado
  {
    public class Predefined : PredefinedAiScenario
    {
      [Fact]
      public void Gets22()
      {
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");
        var bear3 = C("Grizzly Bears");
        var bravado = C("Bravado");

        Hand(P1, bravado, bear3);
        Battlefield(P1, bear1, bear2);

        Exec(
          At(Step.FirstMain)
            .Cast(bravado, target: bear1)
            .Verify(() => Equal(3, C(bear1).Power)),
          At(Step.SecondMain)
            .Cast(bear3)
            .Verify(() => Equal(4, C(bear1).Power))
          );
      }

      [Fact]
      public void Confiscate1()
      {
        var bear1 = C("Grizzly Bears");
        var confiscate = C("Confiscate");
        var bravado = C("Bravado");

        Hand(P1, confiscate);
        Battlefield(P2, bear1.IsEnchantedWith(bravado), "Grizzly Bears");

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate, bear1)
            .Verify(() => Equal(3, C(bear1).Power))
        );
        
      } 
      
      [Fact]
      public void Confiscate2()
      {
        var bear1 = C("Grizzly Bears");
        var confiscate = C("Confiscate");
        var bravado = C("Bravado");

        Hand(P1, confiscate);
        Battlefield(P2, bear1.IsEnchantedWith(bravado), "Grizzly Bears");

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate, bravado)
            .Verify(() =>
              {
                Equal(P1, C(bravado).Controller);
                Equal(2, C(bear1).Power);
              })
        );
        
      }       
    }
  }
}