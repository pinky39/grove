namespace Grove.Gameplay.Abilities
{
  using Modifiers;

  public class ContinuousEffectParameters
  {
    public ShouldApplyToCard CardFilter = delegate { return false; };
    public ModifierFactory Modifier;
    public ShouldApplyToPlayer PlayerFilter = delegate { return false; };
  }
}