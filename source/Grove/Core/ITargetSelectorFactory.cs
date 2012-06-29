namespace Grove.Core
{
  public interface ITargetSelectorFactory
  {
    TargetSelector Create(Card source);
  }
}