namespace Grove.Gameplay.Modifiers
{
  public interface IPlayerModifier : IModifier
  {
    void Apply(LandLimit landLimit);
    void Apply(ContiniousEffects continiousEffects);
    void Apply(SkipSteps skipSteps);
  }
}