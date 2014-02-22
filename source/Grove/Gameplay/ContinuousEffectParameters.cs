namespace Grove.Gameplay
{
  using Grove.Gameplay.Modifiers;

  public class ContinuousEffectParameters
  {
    public ShouldApplyToCard CardFilter = delegate { return false; };
    public CardModifierFactory Modifier;    
  }
}