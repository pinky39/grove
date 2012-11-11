namespace Grove.Core.Details.Cards.Redirections
{
  using Targeting;

  public interface IDamageRedirectionFactory
  {
    DamageRedirection Create(ITarget owner, Game game);
  }
}