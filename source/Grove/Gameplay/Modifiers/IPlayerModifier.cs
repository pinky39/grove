namespace Grove.Gameplay.Modifiers
{
  using Abilities;
  using Characteristics;

  public interface IPlayerModifier : IModifier
  {
    void Apply(LandLimit landLimit);
    void Apply(ContiniousEffects continiousEffects);
    void Apply(SkipSteps skipSteps);
  }
}