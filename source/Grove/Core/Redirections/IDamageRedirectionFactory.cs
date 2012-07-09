namespace Grove.Core.Redirections
{
  public interface IDamageRedirectionFactory
  {
    DamageRedirection Create(ITarget owner);
  }
}