namespace Grove.Core
{
  using Ai;
  using Targeting;

  public class ActivationPrerequisites
  {
    public Card Card;
    public int Index;
    public CardText Description;
    public bool DistributeDamage;
    public MachinePlayRule[] Rules;
    public int? MaxX;
    public TargetSelector Selector;
  }
}