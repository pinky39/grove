namespace Grove.Core.Messages
{
  public class DamageHasBeenDealt
  {
    public Damage Damage { get; set; }
    public object Receiver { get; set; }
  }
}