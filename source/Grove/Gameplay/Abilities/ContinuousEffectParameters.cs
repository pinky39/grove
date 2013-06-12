namespace Grove.Gameplay.Abilities
{
  using Modifiers;

  public class ContinuousEffectParameters
  {
    public ShouldApplyToCard CardFilter = delegate { return false; };
    public CardModifierFactory Modifier;    
  }
}