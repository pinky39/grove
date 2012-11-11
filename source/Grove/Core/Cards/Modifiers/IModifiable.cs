namespace Grove.Core.Details.Cards.Modifiers
{
  public interface IModifiable
  {
    void Accept(IModifier modifier);
  }
}