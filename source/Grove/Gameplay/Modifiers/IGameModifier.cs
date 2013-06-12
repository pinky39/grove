namespace Grove.Gameplay.Modifiers
{
  using DamageHandling;

  public interface IGameModifier : IModifier
  {
    void Apply(DamageRedirections damageRedirections);
    void Apply(DamagePreventions damagePreventions);
  }
}