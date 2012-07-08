namespace Grove.Core.Preventions
{
  using Infrastructure;

  public class PreventDamageFromSource : DamagePrevention
  {
    public Card Source { get; set; }
    public bool OnlyOnce { get; set; }

    protected override void Initialize()
    {
      EndOfLife = new TrackableEvent(this, Game.ChangeTracker);
    }

    public override int PreventDamage(Card damageDealer, int damageAmount, bool queryOnly)
    {
      if (damageDealer == Source)
      {
        if (!queryOnly && OnlyOnce)
          EndOfLife.Raise();

        return 0;
      }

      return damageAmount;
    }
  }
}