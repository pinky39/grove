namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class StudentOfWarfare
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void LevelUpAndAttack()
      {
        var student = C("Student of Warfare");
        Battlefield(P1, student, "Plains", "Plains");

        RunGame(maxTurnCount: 1);

        Equal(2, C(student).Level);
        Equal(17, P2.Life);        
      }
    }
        
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void FirstStrike()
      {
        var student = C("Student of Warfare");
        var armodon = C("Trained Armodon");

        Battlefield(P1, student);
        Battlefield(P2, armodon);

        Exec(
          At(Step.FirstMain)
            .Activate(student)
            .Activate(student),
          At(Step.DeclareAttackers)
            .DeclareAttackers(student),
          At(Step.DeclareBlockers)
            .DeclareBlockers(student, armodon),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(armodon).Zone);
                Equal(Zone.Battlefield, C(student).Zone);
              })          
          );
      }      
      
      [Fact]
      public void LevelUp()
      {
        var student = C("Student of Warfare");

        Battlefield(P1, student);

        Exec(
          At(Step.FirstMain)
            .Activate(student)
            .Verify(() =>
            {
              Equal(1, C(student).Level);
              Equal(1, C(student).Power);
              Equal(1, C(student).Toughness);              
            })
            .Activate(student)
            .Verify(() =>
              {
                Equal(2, C(student).Level);
                Equal(3, C(student).Power);
                Equal(3, C(student).Toughness);
                True(C(student).Has().FirstStrike);
              })
            .Activate(student)
            .Activate(student)
            .Activate(student)
            .Activate(student)
            .Activate(student)
            .Verify(() =>
            {
              Equal(7, C(student).Level);
              Equal(4, C(student).Power);
              Equal(4, C(student).Toughness);
              False(C(student).Has().FirstStrike);
              True(C(student).Has().DoubleStrike);
            }));
      }
    }
  }
}