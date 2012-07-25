namespace Grove.Core.Details.Cards.Modifiers
{
  using Targeting;

  public interface IModifierFactory
  {
    Modifier CreateModifier(Card modifierSource, Target modifierTarget, int? x = null);
  }
}