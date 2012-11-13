namespace Grove.Core.Cards.Preventions
{
  using Grove.Infrastructure;

  public class PreventDamageFromSource : DamagePrevention
  {
    public Card Source { get; set; }
    public bool OnlyOnce { get; set; }

    protected override void Initialize()
    {
      EndOfLife = new TrackableEvent(this, Game.ChangeTracker);
    }

    public override void PreventReceivedDamage(Damage damage)
    {
      if (damage.Source == Source)
      {
        if (OnlyOnce)
          EndOfLife.Raise();

        damage.PreventAll();
      }
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      return source == Source ? 0 : amount;
    }
  }
}