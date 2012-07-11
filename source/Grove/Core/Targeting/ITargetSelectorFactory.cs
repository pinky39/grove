namespace Grove.Core.Targeting
{
  public interface ITargetSelectorFactory
  {
    TargetSelector Create(Card source);
  }
}