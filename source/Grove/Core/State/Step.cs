namespace Grove
{
  public enum Step
  {
    Untap = 0,
    Upkeep = 1,
    Draw = 2,
    FirstMain = 3,
    BeginningOfCombat = 4,
    DeclareAttackers = 5,
    DeclareBlockers = 6,
    FirstStrikeCombatDamage = 7,
    CombatDamage = 8,
    EndOfCombat = 9,
    SecondMain = 10,
    EndOfTurn = 11,
    CleanUp = 12,
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