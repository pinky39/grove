namespace Grove.Events
{
  public class DamageHasBeenDealt
  {
    public readonly Damage Damage;
    public readonly object Receiver;

    public DamageHasBeenDealt(object receiver, Damage damage)
    {
      Receiver = receiver;
      Damage = damage;
    }

    public override string ToString()
    {
      return string.Format("{0} received {1} damage from {2}.", Receiver, Damage.Amount, Damage.Source);
    }
  }
}