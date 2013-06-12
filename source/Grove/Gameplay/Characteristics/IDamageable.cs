namespace Grove.Gameplay.Characteristics
{
  using DamageHandling;

  public interface IDamageable
  {
    void ReceiveDamage(Damage damage);
  }
}