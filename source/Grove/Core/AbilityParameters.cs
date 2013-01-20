namespace Grove.Core
{
  using Ai;
  using Effects;
  using Targeting;

  public class AbilityParameters
  {
    public EffectFactory EffectFactory;
    public MachinePlayRule[] Rules;
    public TargetSelector TargetSelector;
    public CardText Text;
    public bool UsesStack;
  }
}