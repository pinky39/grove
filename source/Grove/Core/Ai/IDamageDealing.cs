namespace Grove.Core.Ai
{
  public interface IDamageDealing
  {
    int PlayerDamage(Player player);
    int CreatureDamage(Card creature);
  }
}