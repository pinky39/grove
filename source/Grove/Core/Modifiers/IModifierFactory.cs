namespace Grove.Core.Modifiers
{
  public interface IModifierFactory
  {
    Modifier CreateModifier(Card modifierSource, Card modifierTarget, int? x = null);
  }
}