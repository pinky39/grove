namespace Grove.Core
{
  public interface IDamageable
  {
    int DealDamage(Card damageSource, int amount, bool isCombat);
  }  
}