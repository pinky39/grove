namespace Grove.Core.Decisions
{
  using Cards;
  using Cards.Effects;
  using Grove.Core.Targeting;
  using Results;

  public abstract class SetTriggeredAbilityTarget : Decision<ChosenTargets>
  {
    public IEffectSource Source { get; set; }
    public object Trigger { get; set; }
    public IEffectFactory Factory { get; set; }
    public TargetSelector TargetSelector { get; set; }

    public override void Init()
    {
      TargetSelector.SetTrigger(Trigger);
    }

    public override void ProcessResults()
    {
      if (!Result.HasTargets)
        return;

      var effect = Factory.CreateEffect(
        new EffectParameters(
          source: Source,
          triggerMessage: Trigger,
          targets: Result.Targets), Game);
      

      Game.Stack.Push(effect);
    }
  }
}