namespace Grove.Core.Details.Cards.Modifiers
{
  using Redirections;

  public class AddDamageRedirection : Modifier
  {
    private DamageRedirection _damageRedirection;
    private DamageRedirections _damageRedirections;

    public IDamageRedirectionFactory Redirection { get; set; }

    public override void Apply(DamageRedirections damageRedirections)
    {
      _damageRedirections = damageRedirections;
      _damageRedirection = Redirection.Create(Target);

      damageRedirections.Add(_damageRedirection);
    }

    protected override void Unapply()
    {
      _damageRedirections.Remove(_damageRedirection);
    }
  }
}