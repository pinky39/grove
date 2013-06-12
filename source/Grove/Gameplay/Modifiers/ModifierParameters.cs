namespace Grove.Gameplay.Modifiers
{
  using Effects;

  public class ModifierParameters
  {
    public bool IsStatic;
    public Card SourceCard;
    public Effect SourceEffect;
    public IModifiable Owner;
    public int? X;
  }
}