namespace Grove.Core.Modifiers
{
  using Preventions;

  public class AddDamagePrevention : Modifier
  {
    private DamagePrevention _damagePrevention;
    private DamagePreventions _damagePreventions;
    public IDamagePreventionFactory Prevention { get; set; }

    public override void Apply(DamagePreventions damagePreventions)
    {
      _damagePreventions = damagePreventions;
      _damagePrevention = Prevention.Create(Target, Game);

      AddLifetime(
        Builder.Lifetime<DependantLifetime>(
          l => l.LifetimeDependency = _damagePrevention));

      damagePreventions.AddPrevention(_damagePrevention);
    }

    protected override void Unapply()
    {
      _damagePreventions.Remove(_damagePrevention);
    }
  }
}