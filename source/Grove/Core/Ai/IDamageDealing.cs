namespace Grove.Core.Ai
{
  public interface IDamageDealing
  {
    int PlayerDamage(IPlayer player);
    int CreatureDamage(Card creature);
  }
}