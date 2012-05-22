namespace Grove.Core.Modifiers
{
  using DamagePrevention;

  public class AddDamagePrevention : Modifier
  {
    private DamagePrevention _damagePrevention;
    private DamagePreventions _damagePreventions;
    public IDamagePreventionFactory Kind { get; set; }

    public override void Apply(DamagePreventions damagePreventions)
    {
      _damagePreventions = damagePreventions;
      _damagePrevention = Kind.Create(Target);
      damagePreventions.Add(_damagePrevention);
    }

    protected override void Unapply()
    {
      _damagePreventions.Remove(_damagePrevention);
    }
  }
}