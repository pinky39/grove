namespace Grove.Core.Details.Combat
{
  public interface IBlockerFactory
  {
    Blocker Create(Card cardBlocker, Attacker attacker);
  }
}