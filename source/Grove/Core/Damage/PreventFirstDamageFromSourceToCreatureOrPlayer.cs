namespace Grove
{
  using Infrastructure;

  public class PreventFirstDamageFromSourceToCreatureOrPlayer : DamagePrevention
  {
    private readonly object _creatureOrPlayer;

    private readonly Card _source;
    private readonly Trackable<bool> _isDepleted = new Trackable<bool>();

    private PreventFirstDamageFromSourceToCreatureOrPlayer() {}

    public PreventFirstDamageFromSourceToCreatureOrPlayer(Card source, object creatureOrPlayer)
    {
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
        base.CalculateHash(calc),
        calc.Calculate(_source),
        calc.Calculate(_creatureOrPlayer));
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (_isDepleted == false && p.Source == _source && p.Target == _creatureOrPlayer)
      {
        if (!p.QueryOnly)
        {
          _isDepleted.Value = true;
        }

        return p.Amount;
      }

      return 0;
    }
  }
}