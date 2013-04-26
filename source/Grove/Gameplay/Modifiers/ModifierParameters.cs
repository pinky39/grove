namespace Grove.Gameplay.Modifiers
{
  using Card;
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