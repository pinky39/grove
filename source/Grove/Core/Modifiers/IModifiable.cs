namespace Grove.Core.Modifiers
{
  public interface IModifiable
  {
    void Accept(IModifier modifier);
  }
}