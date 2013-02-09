namespace Grove.Core.Modifiers
{
  using Preventions;

  public class AddDamagePrevention : Modifier
  {
    private readonly DamagePrevention _damagePrevention;
    private DamagePreventions _damagePreventions;

    private AddDamagePrevention() {}

    public AddDamagePrevention(DamagePrevention damagePrevention)
    {
      _damagePrevention = damagePrevention;
    }


    public override void Apply(DamagePreventions damagePreventions)
    {
      _damagePreventions = damagePreventions;
      _damagePrevention.Initialize(Target, Game);            

      damagePreventions.AddPrevention(_damagePrevention);
    }

    protected override void Unapply()
    {
      _damagePreventions.Remove(_damagePrevention);
    }
  }
}