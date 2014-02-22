namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class SerraAvatar
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PowerToughnessInHand()
      {
        var avatar = C("Serra Avatar");
        
        P1.Life = 15;        
        Hand(P1, avatar);
        
        Equal(15, C(avatar).Power);
        Equal(15, C(avatar).Toughness);
      }

      [Fact]
      public void PowerToughnessOnBattlefield()
      {
        var avatar = C("Serra Avatar");

        P1.Life = 15;
        Battlefield(P1, avatar);

        Equal(15, C(avatar).Power);
        Equal(15, C(avatar).Toughness);
      }

      [Fact]
      public void ControllerChange()
      {
        var avatar = C("Serra Avatar");
        var confiscate = C("Confiscate");
                
        P1.Life = 10;
        
        Battlefield(P2, avatar);
        Hand(P1, confiscate);

        Exec(
          At(Step.FirstMain)
            .Cast(confiscate, target: avatar)
            .Verify( () =>
              {
                Equal(10, C(avatar).Power);
                Equal(10, C(avatar).Toughness);
              })            
          );
      }
    }
  }
}