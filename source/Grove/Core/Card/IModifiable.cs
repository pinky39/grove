namespace Grove
{
  using Modifiers;

  public interface IModifiable
  {
    void RemoveModifier(IModifier modifier);
  }
}