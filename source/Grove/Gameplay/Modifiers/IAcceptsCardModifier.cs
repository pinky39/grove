namespace Grove.Gameplay.Modifiers
{
  public interface IAcceptsCardModifier
  {
    void Accept(ICardModifier modifier);
  }
}