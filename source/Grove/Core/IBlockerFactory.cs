namespace Grove.Core
{
  public interface IBlockerFactory {
    Blocker Create(Card cardBlocker, Attacker attacker);
  }
}