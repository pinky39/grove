namespace Grove.Gameplay.Modifiers
{
  using System;
  using Card.Abilities;
  using Card.Characteristics;
  using Card.Counters;
  using Damage;
  using Player;

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
    void Apply(ControllerCharacteristic controller);
    void Apply(ContiniousEffects continiousEffects);
    void Apply(LandLimit landLimit);
  }
}