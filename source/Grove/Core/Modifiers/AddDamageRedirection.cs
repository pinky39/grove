namespace Grove.Core.Modifiers
{
  using Redirections;

  public class AddDamageRedirection : Modifier
  {
    private DamageRedirection _damageRedirection;
    private DamageRedirections _damageRedirections;

    public AddDamageRedirection(DamageRedirection damageRedirection)
    {
      _damageRedirection = damageRedirection;
    }

    public override void Apply(DamageRedirections damageRedirections)
    {
      _damageRedirections = damageRedirections;
      _damageRedirection.Initialize(this, Game);
            
      damageRedirections.Add(_damageRedirection);
    }

    protected override void Unapply()
    {
      _damageRedirections.Remove(_damageRedirection);
    }
  }
}