namespace Grove.Core
{
  public interface IDamageable
  {
    void DealDamage(Card damageSource, int amount, bool isCombat);
  }
}