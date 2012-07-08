namespace Grove.Core.Modifiers
{
  public interface IModifierFactory
  {
    Modifier CreateModifier(Card modifierSource, ITarget modifierTarget, int? x = null);
  }
}