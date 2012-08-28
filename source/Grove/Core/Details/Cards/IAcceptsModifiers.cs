namespace Grove.Core.Details.Cards
{
  using Modifiers;

  public interface IAcceptsModifiers
  {
    void AddModifier(IModifier modifier);
    void RemoveModifier(IModifier modifier);
  }
}