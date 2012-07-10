namespace Grove.Core.Modifiers
{
  using System;
  using Counters;
  using Preventions;
  using Redirections;

  public interface IModifier : IDisposable
  {
    void Apply(TriggeredAbilities abilities);
    void Apply(StaticAbilities abilities);
    void Apply(ActivatedAbilities abilities);
    void Apply(CardColors colors);
    void Apply(Power power);
    void Apply(Toughness toughness);
    void Apply(DamagePreventions damagePreventions);
    void Apply(Protections protections);
    void Apply(CardTypeCharacteristic cardType);
    void Apply(Counters counters);
    void Apply(Level level);
    void Apply(DamageRedirections damageRedirections);
    void Activate();
  }
}