namespace Grove.Core.Modifiers
{
  public class AddStaticAbility : Modifier
  {
    private StaticAbilities _abilities;
    public StaticAbility StaticAbility { get; set; }

    public override void Apply(StaticAbilities abilities)
    {
      _abilities = abilities;
      _abilities.Add(StaticAbility);
    }

    protected override void Unapply()
    {
      _abilities.Remove(StaticAbility);
    }    
  }
}