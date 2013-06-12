namespace Grove.Gameplay.Modifiers
{
  public interface IAcceptsGameModifier
  {
    void Accept(IGameModifier modifier);
  }
}