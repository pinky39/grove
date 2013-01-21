namespace Grove.Core.Ai.TimingRules
{
  public class Counterspell : TimingRule
  {
    public int? CounterCost;

    public override bool ShouldPlay(ActivationContext context)
    {
      if (Stack.IsEmpty)
        return false;

      if (Stack.TopSpell.Controller == context.Card.Controller)
        return false;

      return !CounterCost.HasValue || !context.Card.Controller.Opponent.HasMana(CounterCost.Value);
    }
  }
}