namespace Grove.Core.Effects
{
  using Zones;

  public class CounterTargetSpell : Effect
  {
    public int? ControllersLifeloss;
    public IManaAmount DoNotCounterCost;

    public override void Resolve()
    {
      var targetSpellController = Target.Effect().Controller;

      if (DoNotCounterCost != null && targetSpellController.HasMana(DoNotCounterCost))
      {
        Decisions.EnqueueConsiderPayingLifeOrMana(
          player: targetSpellController,
          effect: Target.Effect(),
          mana: DoNotCounterCost,
          handler: (args) => {
            if (args.Answer)
            {
              args.Player.Consume(DoNotCounterCost);
              return;
            }

            Counter(args.Player, args.Effect, args.Stack);
            args.Stack.Counter(args.Effect);
          });
        return;
      }

      Counter(targetSpellController, Target.Effect(), Game.Stack);
    }

    private void Counter(Player targetSpellController, Effect spell, Stack stack)
    {
      if (ControllersLifeloss.HasValue)
      {
        targetSpellController.Life -= ControllersLifeloss.Value;
      }

      stack.Counter(spell);
    }
  }
}