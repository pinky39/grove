namespace Grove.Core.Effects
{
  public interface IDamageDealing
  {
    int PlayerDamage(Player player);
    int CreatureDamage(Card creature);
  }
}