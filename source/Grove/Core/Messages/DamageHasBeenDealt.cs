namespace Grove.Core.Messages
{
  public class DamageHasBeenDealt
  {
    public int Amount { get; set; }
    public Card Dealer { get; set; }
    public bool IsCombat { get; set; }
    public object Receiver { get; set; }
  }
}