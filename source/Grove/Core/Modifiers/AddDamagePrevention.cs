namespace Grove.Modifiers
{
  using System;

  public class AddDamagePrevention : Modifier, IGameModifier
  {
    private readonly Func<Modifier, DamagePrevention> _damagePreventionFactory;
    private DamagePrevention _damagePrevention;
    private DamagePreventions _damagePreventions;

    private AddDamagePrevention() {}

    public AddDamagePrevention(DamagePrevention damagePrevention)
    {
      _damagePrevention = damagePrevention;
    }

    public AddDamagePrevention(Func<Modifier, DamagePrevention> damagePreventionFactory)
    {
      _damagePreventionFactory = damagePreventionFactory;
    }

    public override void Apply(DamagePreventions damagePreventions)
    {
      _damagePreventions = damagePreventions;
      _damagePrevention = _damagePrevention ?? _damagePreventionFactory(this);
      _damagePrevention.Initialize(this, Game);

      damagePreventions.AddPrevention(_damagePrevention);
    }

    protected override void Unapply()
    {
      _damagePreventions.Remove(_damagePrevention);
    }
  }
}