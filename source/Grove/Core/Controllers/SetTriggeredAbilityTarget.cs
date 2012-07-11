namespace Grove.Core.Controllers
{
  using Details.Cards.Effects;
  using Results;
  using Targeting;
  using Zones;

  public abstract class SetTriggeredAbilityTarget : Decision<ChosenTargets>
  {
    public Effect Effect { get; set; }
    public Stack Stack { get; set; }
    public TargetSelectors TargetSelectors { get; set; }

    public override void ProcessResults()
    {
      if (Result.Targets == null)
        return;

      foreach (var target in Result.Targets.Effect())
      {
        Effect.AddTarget(target);
      }

      Stack.Push(Effect);
    }
  }
}