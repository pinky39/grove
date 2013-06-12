namespace Grove.Gameplay.Modifiers
{
  public interface IAcceptsPlayerModifier
  {
    void Accept(IPlayerModifier modifier);    
  }
}