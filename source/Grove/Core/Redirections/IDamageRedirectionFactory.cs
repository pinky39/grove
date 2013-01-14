namespace Grove.Core.Redirections
{
  using Grove.Core.Targeting;

  public interface IDamageRedirectionFactory
  {
    DamageRedirection Create(ITarget owner, Game game);
  }
}