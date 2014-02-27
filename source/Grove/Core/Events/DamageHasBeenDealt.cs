namespace Grove.Events
{
  public class DamageHasBeenDealt
  {
    public DamageHasBeenDealt(object receiver, Damage damage)
    {
      Receiver = receiver;
      Damage = damage;
    }

    public Damage Damage { get; private set; }
    public object Receiver { get; private set; }

    public override string ToString()
    {
      return string.Format("{0} received {1} damage from {2}.", Receiver, Damage.Amount, Damage.Source);
    }
  }
}