namespace Grove.Gameplay.Modifiers
{
  using Costs;
  using DamageHandling;

  public interface IGameModifier : IModifier
  {
    void Apply(DamageRedirections damageRedirections);
    void Apply(DamagePreventions damagePreventions);
    void Apply(CostModifiers costModifiers);
  }
}