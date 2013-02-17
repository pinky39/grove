namespace Grove.Core.Modifiers
{
  using Effects;
  using Targeting;

  public class ModifierParameters
  {
    public Card SourceCard;
    public Effect SourceEffect;
    public ITarget Target;
    public int? X;    
  }
}