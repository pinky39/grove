namespace Grove
{
  using Grove.Infrastructure;

  public class PreventAllDamageFromSourceToTarget : DamagePrevention
  {
    private readonly object _creatureOrPlayer;
    private readonly bool _onlyOnce;
    private readonly Card _source;
    private readonly Trackable<bool> _isDepleted = new Trackable<bool>();

    private PreventAllDamageFromSourceToTarget() {}

    public PreventAllDamageFromSourceToTarget(Card source, object creatureOrPlayer, bool onlyOnce)
    {
      _onlyOnce = onlyOnce;
      _source = source;
      _creatureOrPlayer = creatureOrPlayer;
    }

    protected override void Initialize()
    {
      _isDepleted.Initialize(ChangeTracker);
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(_source),
        calc.Calculate((IHashable) _creatureOrPlayer));
    }

    public override int PreventDamage(PreventDamageParameters p)
    {            
      if (_isDepleted == false && p.Source == _source && p.Target == _creatureOrPlayer)
      {
        if (_onlyOnce && !p.QueryOnly)
        {
          _isDepleted.Value = true;
        }

        return p.Amount;
      }

      return 0;
    }
  }
}