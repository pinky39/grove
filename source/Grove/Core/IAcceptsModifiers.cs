namespace Grove.Core
{
  using Details.Cards.Modifiers;

  public interface IAcceptsModifiers
  {
    void AddModifier(IModifier modifier);
    void RemoveModifier(IModifier modifier);
  }
}