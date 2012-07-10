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

    public override void PreventDamage(Damage damage)
    {
      if (damage.Source == Source)
      {
        if (OnlyOnce)
          EndOfLife.Raise();

        damage.PreventAll();
      }
    }

    public override int EvaluateHowMuchDamageCanBeDealt(Card source, int amount, bool isCombat)
    {
      return source == Source ? 0 : amount;
    }
  }
}