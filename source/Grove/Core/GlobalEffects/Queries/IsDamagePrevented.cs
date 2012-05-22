namespace Mtg.Core.GlobalEffects.Queries
{
  using Mtg.Core.Cards;

  public class IsDamagePrevented
  {
    public Card DamageDealer { get; set; }
    public Card DamageReceiver { get; set; }
  }
}