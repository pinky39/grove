namespace Grove.Gameplay.Decisions.Scenario
{
  using System;
  using Card.Characteristics;
  using Common;
  using Effects;
  using Infrastructure;
  using Modifiers;
  using Targeting;

  public class ScenarioEffect : ITarget, IHasColors
  {
    public Func<Effect> Effect { get; set; }

    public bool HasColor(CardColor color)
    {
      return Effect().HasColor(color);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return Effect().CalculateHash(calc);
    }

    public void AddModifier(IModifier modifier) {}

    public void RemoveModifier(IModifier modifier) {}
  }
}