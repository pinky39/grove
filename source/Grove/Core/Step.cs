namespace Grove.Core
{
  public enum Step
  {       
    Untap = 0,
    Upkeep,
    Draw,
    FirstMain,
    BeginningOfCombat,
    DeclareAttackers,
    DeclareBlockers,
    CombatDamage,
    EndOfCombat,
    SecondMain,
    EndOfTurn,  
    CleanUp,
    GameStart,
    Mulligan
  }

  public static class StepEx
  {
    public static bool IsMain(this Step step)
    {
      return step == Step.FirstMain || step == Step.SecondMain;
    }
  }
}