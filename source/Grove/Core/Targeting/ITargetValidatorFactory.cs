namespace Grove.Core.Targeting
{
  public interface ITargetValidatorFactory
  {
    TargetValidator Create(Card source, Game game);
  }
}