namespace Grove.Core.Cards.Modifiers
{
  public interface IModifiable
  {
    void Accept(IModifier modifier);
  }
}