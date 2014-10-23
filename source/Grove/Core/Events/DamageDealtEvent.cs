namespace Grove.Events
{
  public class DamageDealtEvent
  {
    public readonly Damage Damage;
    public readonly object Receiver;
    public Card Source { get { return Damage.Source; } }

    public DamageDealtEvent(object receiver, Damage damage)
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