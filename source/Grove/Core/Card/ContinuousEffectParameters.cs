namespace Grove
{
  public class ContinuousEffectParameters
  {
    public ShouldApplyToCard CardFilter = delegate { return false; };
    public CardModifierFactory Modifier;    
  }
}