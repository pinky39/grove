namespace Grove.Modifiers
{
  public interface IAcceptsGameModifier
  {
    void Accept(IGameModifier modifier);
  }
}