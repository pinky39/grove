namespace Grove.Gameplay.Damage
{
  using Card;

  public class PreventReceived : DamagePrevention
  {
    private readonly bool _combatOnly;

    private PreventReceived() {}

    public PreventReceived(bool combatOnly)
    {
      _combatOnly = combatOnly;
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      if (_combatOnly && !isCombat)
        return amount;

      return 0;
    }

    public override void PreventReceivedDamage(Damage damage)
    {
      damage.Amount = EvaluateReceivedDamage(damage.Source, damage.Amount, damage.IsCombat);
    }
  }
}