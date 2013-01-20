namespace Grove.Core.Ai.TimingRules
{
  using Decisions.Results;

  public class Counterspell : TimingRule
  {
    public int? CounterCost;
    
    public override bool ShouldPlay(Playable playable, ActivationPrerequisites prerequisites)
    {
      if (Stack.IsEmpty)
        return false;
      
      if (Stack.TopSpell.Controller == playable.Controller)
        return false;

      return !CounterCost.HasValue || !playable.Controller.Opponent.HasMana(CounterCost.Value);      
    }
  }
}