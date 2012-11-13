namespace Grove.Core.Cards.Modifiers
{
  using Grove.Core.Targeting;

  public interface IModifierFactory
  {
    Modifier CreateModifier(Card source, ITarget target, int? x, Game game);
  }
}