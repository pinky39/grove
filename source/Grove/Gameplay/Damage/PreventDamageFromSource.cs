namespace Grove.Gameplay.Damage
{
  using System;

  [Serializable]
  public class PreventDamageFromSource : DamagePrevention
  {
    private readonly bool _onlyOnce;
    private readonly Card _source;

    private PreventDamageFromSource() {}

    public PreventDamageFromSource(Card source, bool onlyOnce = true)
    {
      _onlyOnce = onlyOnce;
      _source = source;
    }

    public override void PreventReceivedDamage(Damage damage)
    {
      if (damage.Source == _source)
      {
        if (_onlyOnce)
          EndOfLife.Raise();

        damage.PreventAll();
      }
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      return source == _source ? 0 : amount;
    }
  }
}