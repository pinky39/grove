namespace Grove.Core.Details.Cards.Modifiers
{
  using Targeting;

  public interface IModifierFactory
  {
    Modifier CreateModifier(Card source, ITarget target, int? x);
  }
}