namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;
  using Mana;
  using Targeting;
  using Zones;

  public class CounterTargetSpell : Effect
  {
    public int? ControllersLifeloss;
    public IManaAmount DoNotCounterCost;

    protected override void ResolveEffect()
    {
      var targetSpellController = Target().Effect().Controller;

      if (DoNotCounterCost != null && targetSpellController.HasMana(DoNotCounterCost))
      {
        Decisions.Enqueue<ConsiderPayingLifeOrMana>(
          controller: targetSpellController,
          init: p =>
            {
              p.Context = Target().Effect();
              p.Mana = DoNotCounterCost;
              p.Handler = (args) =>
                {
                  if (args.Answer)
                  {
                    args.Player.Consume(DoNotCounterCost);
                    return;
                  }

                  Counter(args.Player, args.Ctx<Effect>(), args.Game.Stack);
                };
            });
      }

      Counter(targetSpellController, Target().Effect(), Game.Stack);
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