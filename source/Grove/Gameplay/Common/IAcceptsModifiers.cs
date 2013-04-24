namespace Grove.Core
{
  using Modifiers;

  public interface IAcceptsModifiers
  {
    void AddModifier(IModifier modifier);
    void RemoveModifier(IModifier modifier);
  }
}