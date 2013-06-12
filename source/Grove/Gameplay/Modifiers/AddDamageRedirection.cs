namespace Grove.Gameplay.Modifiers
{
  using System;
  using DamageHandling;

  public class AddDamageRedirection : Modifier, IGameModifier
  {
    private readonly Func<Modifier, DamageRedirection> _damageRedirectionFactory;
    private DamageRedirections _damageRedirections;
    private DamageRedirection _damageRedirection;

    private AddDamageRedirection() {}

    public AddDamageRedirection(Func<Modifier, DamageRedirection> damageRedirectionFactory)
    {
      _damageRedirectionFactory = damageRedirectionFactory;
    }

    public override void Apply(DamageRedirections damageRedirections)
    {
      _damageRedirections = damageRedirections;
      _damageRedirection = _damageRedirectionFactory(this);
      _damageRedirection.Initialize(this, Game);

      damageRedirections.Add(_damageRedirection);
    }

    protected override void Unapply()
    {
      _damageRedirections.Remove(_damageRedirection);
    }
  }
}