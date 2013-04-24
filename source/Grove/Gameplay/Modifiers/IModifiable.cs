namespace Grove.Gameplay.Modifiers
{
  public interface IModifiable
  {
    void Accept(IModifier modifier);
  }
}