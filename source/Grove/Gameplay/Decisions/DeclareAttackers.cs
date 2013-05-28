namespace Grove.Gameplay.Decisions
{
  using System;
  using Results;

  [Serializable]
  public abstract class DeclareAttackers : Decision<ChosenCards>
  {
    protected override bool ShouldExecuteQuery { get { return Controller.Battlefield.HasCreaturesThatCanAttack; } }

    public override void ProcessResults()
    {
      if (Result == null)
        return;

      foreach (var attacker in Result)
      {
        Game.Combat.DeclareAttacker(attacker);
      }
    }
  }
}