namespace Grove.Core
{
  using Modifiers;

  public class ContinuousEffectParameters
  {
    public ModifierFactory Modifier;
    public ShouldApplyToCard CardFilter = delegate { return false; };
    public ShouldApplyToPlayer PlayerFilter = delegate { return false; };
  }
}