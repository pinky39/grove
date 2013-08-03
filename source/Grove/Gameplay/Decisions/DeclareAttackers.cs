namespace Grove.Gameplay.Decisions
{
  using System.Linq;
  using Results;

  public abstract class DeclareAttackers : Decision<ChosenCards>
  {
    protected override bool ShouldExecuteQuery { get { return Controller.Battlefield.HasCreaturesThatCanAttack; } }

    protected override void SetResultNoQuery()
    {
      Result = new ChosenCards();
    }

    public override void ProcessResults()
    {
      AddCreaturesThatMustAttack();

      foreach (var attacker in Result)
      {
        Game.Combat.DeclareAttacker(attacker);
      }
    }

    private void AddCreaturesThatMustAttack()
    {
      var creaturesThatMustAttack = Controller.Battlefield
        .CreaturesThatCanAttack.Where(x => x.Has().AttacksEachTurnIfAble);

      foreach (var creature in creaturesThatMustAttack)
      {
        if (!Result.Contains(creature))
        {
          Result.Add(creature);
        }
      }
    }
  }
}