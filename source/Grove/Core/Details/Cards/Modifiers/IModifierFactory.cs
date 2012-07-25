namespace Grove.Core.Details.Cards.Modifiers
{
  using Targeting;

  public interface IModifierFactory
  {
    Modifier CreateModifier(Card modifierSource, ITarget modifierTarget, int? x = null);
  }
}