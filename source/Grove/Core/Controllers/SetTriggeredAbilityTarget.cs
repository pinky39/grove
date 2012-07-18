namespace Grove.Core.Controllers
{
  using Details.Cards;
  using Details.Cards.Effects;
  using Results;
  using Targeting;

  public abstract class SetTriggeredAbilityTarget : Decision<ChosenTargets>
  {
    public IEffectSource Source { get; set; }
    public object TriggerMessage { get; set; }
    public IEffectFactory Factory { get; set; }
    public TargetSelectors TargetSelectors { get; set; }

    public override void ProcessResults()
    {
      if (Result.Targets == null)
        return;

      var effect = Factory.CreateEffect(
        new EffectParameters(
          source: Source,
          triggerMessage: TriggerMessage,
          targets: Result.Targets.Effect()));
      

      Game.Stack.Push(effect);
    }
  }
}